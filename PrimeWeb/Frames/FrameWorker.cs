#define DBG_ACK

using Blazm.Hid;
using PrimeWeb.Packets;
using PrimeWeb.Types;
using PrimeWeb.Utility;
using System.Text;

namespace PrimeWeb.Frames
{

	public class FrameWorker
	{
		static readonly byte[] PacketReqInfo = { 0x00, PrimeCommands.INFOS, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
		private bool ProtocolNegotiated = false;
		private IHidDevice device;
		public uint MessageCount { get; private set; } = 1;
		public ProtocolVersion Protocol { get; private set; } = ProtocolVersion.Unknown;
		private uint MessageID_Backup = uint.MaxValue;

		public FrameWorker(IHidDevice hid)
		{
			this.device = hid;
			this.device.ReportReceived += Prime_ReportReceived;
			this.device.Connected += Device_Connected;
		}

		#region initialization

		private async void Device_Connected(object? sender, EventArgs e)
		{
			await device.SendReportAsync(0x00, PacketReqInfo);
		}

		public async Task ConnectAsync()
		{
			if (device.Opened)
				return;

			await device.OpenAsync();
		}

		#endregion

		#region internal usb TX

		private Action<byte[]> AckReceived;

		private void NotifyAckReceived(byte[] data) => AckReceived?.Invoke(data);

		public void StartBackup()
		{
			this.MessageID_Backup = MessageCount;
		}

		public async Task TransmitRawData(byte [] data)
		{
			await device.SendReportAsync(0x00, data);
		}

		private async Task<AckFrame> TransmitMessageAsync(byte[] data)
		{
			Dictionary<int, byte[]> sentmessages = new Dictionary<int, byte[]>();
			AckFrame acknack;
			var ackmsg = WaitForAckResponse();

			foreach (var item in SplitBlocks((int)MessageCount, data))
			{
				sentmessages.Add((int)item[0], item);

				await device.SendReportAsync(0x00, item);

				if (ackmsg.IsCompletedSuccessfully)
				{
					acknack = new AckFrame(ackmsg.Result);
					if (!acknack.IsAck && acknack.SequenceToResend != 0xFF)
						Console.WriteLine("resending sequence!");
					{
						for (int i = acknack.SequenceToResend; i < (int)item[0]; i++)
						{
							await device.SendReportAsync(0x00, sentmessages[i]);
						}
					}
					ackmsg = WaitForAckResponse();
				}
			}




			MessageCount++;

			return await ackmsg.ContinueWith(x => new AckFrame(x.Result));
		}

		private Task<byte[]> WaitForAckResponse()
		{
			Console.WriteLine("received async ACK!");
			var tcsAck = new TaskCompletionSource<byte[]>();

			Action<byte[]> callback = null;
			callback = (e) =>
			{
				this.AckReceived -= callback;
				tcsAck.SetResult(e);
			};

			this.AckReceived += callback;
			return tcsAck.Task;
		}

		private IEnumerable<byte[]> SplitBlocks(int messagenumber, byte[] data)
		{
			byte sequence = 1;
			int bytestogo = data.Length;
			int bytestotake = data.Length <= 1015 ? data.Length : 1015;
			using (var ms = new MemoryStream(data))
			using (var reader = new BinaryReader(ms))
			{

				List<byte> packet = new List<byte>();
				packet.AddRange(GetMessageHeader(messagenumber, data.Length));
				packet.AddRange(reader.ReadBytes(bytestotake));
				bytestogo -= bytestotake;
				sequence++;
				yield return packet.ToArray();

				while (bytestogo > 0)
				{
					packet.Clear();
					packet.Add(sequence);
					sequence++;
					bytestotake = bytestogo <= 1023 ? bytestogo : 1023;

					packet.AddRange(reader.ReadBytes(bytestotake));
					bytestogo -= bytestotake;
					yield return packet.ToArray();
				}
			}
		}

		#endregion

		#region USB Input/Output

		private async void Prime_ReportReceived(object? sender, OnInputReportArgs e)
		{
			var data = e.Data;
			var type = IdentifyReport(data.SubArray(0, 13));
			logpacket($"Report received! type: {type}");

			switch (type)
			{
				case (FrameType.Legacy):
					HandleReport_Legacy(data);
					break;
				case (FrameType.Content):
					await HandleReport_Content(data);
					break;
				case (FrameType.Ack):
					await HandleReport_Ack(data);
					break;
				case (FrameType.OutOfBand):
					HandleReport_OutOfBand(data);
					break;
				case (FrameType.Error):
					Console.WriteLine("[PacketWorker] - Received report with unknown header!");
					DbgTools.PrintPacket(data);
					break;
				default:
					Console.WriteLine("Unkown report error!");
					break;
			}
		}

		#endregion

		#region Protocol Handling	

		int IncomingToGo;
		int BytesToGo;
		int IncomingMessageID;
		int IncomingSequence;
		MemoryStream IncomingBytes;
		BinaryWriter receiveWriter;


		public async Task HandleReport_Content(byte[] data)
		{
			var frame = new ContentFrame(data);
			var bytestoread = BytesToGo <= 1023 ? BytesToGo : 1023;
			if (frame.IsStartFrame)
			{
				IncomingBytes = new MemoryStream();
				receiveWriter = new BinaryWriter(IncomingBytes);
				IncomingToGo = (int)frame.IOMessageSize;
				BytesToGo = IncomingToGo;
				IncomingMessageID = (int)frame.IOMessageCounter;
				IncomingSequence = 1;
				bytestoread = BytesToGo <= 1015 ? BytesToGo : 1015;

				logpacket($"Created new Packet! of type: {((PrimeCommand)frame.Data[0]).ToString()}");
			}

			if ((int)frame.Sequence > IncomingSequence)
			{
				Console.WriteLine("error! missed sequence!");
			}

			receiveWriter.Write(frame.GetContentBytes(bytestoread));
			BytesToGo -= bytestoread;

			if (IncomingBytes.Position > IncomingToGo -2)
			{
				var acker = new AckFrame(true,frame.Sequence,(uint)IncomingMessageID, (uint) IncomingToGo);
				await TransmitRawData(acker.GetFrameBytes());
				NotifyMessageReceived(IncomingBytes.ToArray());
			}

			IncomingSequence++;

		}

		public async Task HandleReport_Ack(byte[] data)
		{
			var frame = new AckFrame(data);
			if (frame.IsValid)
			{
				if(frame.IOMessageID == MessageID_Backup)
				{
					NotifyBackupStarted();
				}
				NotifyAckReceived(data);
			}
		}

		public void HandleReport_OutOfBand(byte[] data)
		{

		}

		public void HandleReport_Legacy(byte[] data)
		{
			if (this.Protocol != ProtocolVersion.Old)
			{
				this.Protocol = ProtocolVersion.Old;
			}

			PrimeCommand cmd = data[1];
			var BodyLength = BitConverter.ToInt32(data.SubArray(3, 4).Reverse().ToArray());
			var Body = data.SubArray(7, BodyLength);

			if(cmd == PrimeCommands.INFOS)
				finalizeprotocolchange(Body);
	
		}

		public FrameType IdentifyReport(ReadOnlySpan<byte> data)
		{
			if (data[0] == 0x00)
				return FrameType.Legacy;

			if (data[0] > 0x00 && data[0] < 0xFE && Protocol == ProtocolVersion.V2)
				return FrameType.Content;

			if (data[0] > 0x00 && data[0] < 0xFE && Protocol == ProtocolVersion.Old)
				return FrameType.Legacy;

			if (data[0] == 0xFE && data[2] > 0x00 && data[2] < 0xFE)
				return FrameType.Ack;

			if (data[0] == 0xFE && data[2] == 0xFF)
				return FrameType.OutOfBand;

			return FrameType.Error;
		}


		#endregion

		#region Packet Sending

		public async Task Send(IPayloadGenerator data)
		{
			await TransmitMessageAsync(data.Generate());
		}
		public async Task Send(byte[] data)
		{
			await TransmitMessageAsync(data);
		}

		#endregion

		#region legacy

		private async Task finalizeprotocolchange(byte[] data)
		{
			

			var result = HpInfos.FromBytes(data);
			result.SetProductId(device.ProductId);
			NotifyCalcInfoReceived(result);

			if (result.Build < 10591)
			{
				Console.WriteLine("Sorry Calculator not supported! please update!");
				throw new Exception("Sorry Calculator not supported! please update!");
				return;
			}

			if (ProtocolNegotiated)
				return;

			byte[] content = { 0xFF, 0xEC, 0, 0, 0, 0, 0, 0 };
			await device.SendReportAsync(0x00, content);
			Console.WriteLine("Sent protocol V2 request");
			ProtocolNegotiated = true;
			this.Protocol = ProtocolVersion.V2;
			NotifyCommunicationInitialized();


		}
		#endregion

		#region Events

		public Action BackupStarted { get; set; }
		private void NotifyBackupStarted() => BackupStarted?.Invoke();


		public Action<byte[]> MessageReceived { get; set; }
		private void NotifyMessageReceived(byte[] data) => MessageReceived?.Invoke(data);

		public Action CommunicationInitialized { get; set; }
		private void NotifyCommunicationInitialized() => CommunicationInitialized?.Invoke();

		public Action<HpInfos> CalcInfoReceived { get; set; }
		private void NotifyCalcInfoReceived(HpInfos data) => CalcInfoReceived?.Invoke(data);

		#endregion

		#region utility

		private static byte[] GetMessageHeader(int messagenumber, int messagesize)
		{
			byte[] header = new byte[9];

			header[0] = (byte)0x01;
			header[1] = (byte)((uint)messagenumber & (uint)0x000000FF);
			header[2] = (byte)(((uint)messagenumber & (uint)0x0000FF00) >> 8);
			header[3] = (byte)(((uint)messagenumber & (uint)0x00FF0000) >> 16);
			header[4] = (byte)(((uint)messagenumber & (uint)0xFF000000) >> 24);

			header[5] = (byte)((uint)messagesize & (uint)0x000000FF);
			header[6] = (byte)(((uint)messagesize & (uint)0x0000FF00) >> 8);
			header[7] = (byte)(((uint)messagesize & (uint)0x00FF0000) >> 16);
			header[8] = (byte)(((uint)messagesize & (uint)0xFF000000) >> 24);

			return header;
		}

		private void logpacket(string line)
		{
#if (DBG_PACKETS)
			Console.WriteLine($"[PacketWorker - packets] {line}");
#endif
		}

		#endregion






	}



}
