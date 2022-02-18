using Blazm.Hid;
using PrimeWeb.Types;
using PrimeWeb.Packets;
using System.Text;
using PrimeWeb.Calculator;
using PrimeWeb.Utility;

namespace PrimeWeb.Protocol
{
	public class FrameWorker
	{
		private bool ProtocolNegotiated = false;

		private IHidDevice device;

		private PrimeCalculator prime;

		public uint MessageCount { get; private set; } = 1;

		public ProtocolVersion Protocol { get; private set; } = ProtocolVersion.Unknown;

		private V2MessageIn CurrentMessageIn { get; set; }
		private V2MessageOut CurrentMessageOut { get; set; }


		private FrameReceiver Receiver;
		
		private FrameTransmitter Sender;



		public FrameWorker(IHidDevice hid)
		{
			this.device = hid;
			this.device.ReportReceived += Prime_ReportReceived;
			this.device.Connected += Device_Connected;
		}

		#region initialization

		private async void Device_Connected(object? sender, EventArgs e)
		{
			Console.WriteLine("[PacketWorker] - Prime is connected!");
			Console.WriteLine("Sending status packet!");

			var pkt_status = MessageUtils.Misc.GetPacketInfoRequest();
			await device.SendReportAsync(pkt_status.id, pkt_status.data);
		}

		public async Task ConnectAsync()
		{
			if (device.Opened)
				return;

			await device.OpenAsync();
		}

		#endregion

		#region USB Input/Output

		public async Task SendFrame(IFrame payload)
		{
			await device.SendReportAsync(0x00, payload.GetBytes());
		}

		private void Prime_ReportReceived(object? sender, OnInputReportArgs e)
		{
			var data = e.Data;

			switch (IdentifyReport(data.AsSpan().Slice(0, 13))){
				case (FrameType.Legacy):
					HandleReport_Legacy(data);
					break;
				case (FrameType.Content):
					HandleReport_Content(data);
					break;
				case (FrameType.Ack):
					HandleReport_Ack(data);
					break;
				case (FrameType.OutOfBand):
					HandleReport_OutOfBand(data);
					break;
				case (FrameType.Error):
					Console.WriteLine("[PacketWorker] - Received report with unknown header!");
					DbgTools.PrintPacket(data);
					break;
			}
		}


		#endregion


		#region Protocol Handling

		

		public void HandleReport_Legacy(byte[] data)
		{
			if(this.Protocol != ProtocolVersion.Old)
			{
				this.Protocol = ProtocolVersion.Old;
				Console.WriteLine("Received Old protocol message while not in old protocol mode!");
				Console.WriteLine("Switching back to old protocol mode....");
			}

			var Frame = new LegacyFrame(data);

				

		}

		public void HandleReport_Content(byte[] data)
		{
			var frame = new ContentFrame(data);

			if (!frame.IsValid)
			{
				Console.WriteLine("Received Content Frame not valid!");
			}

		}

		public void HandleReport_Ack(byte[] data)
		{
			var frame = new AckFrame(data);

			if (!frame.IsValid)
			{
				Console.WriteLine("Received Ack Frame not valid!");
			}



		}

		public void HandleReport_OutOfBand(byte[] data)
		{

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



		#region legacy

		public async Task SendPayload(byte[] Data)
		{
			CurrentMessageOut = new V2MessageOut(MessageCount++, Data);
			CurrentMessageOut.GeneratePackets();
			bool isdone;
			var blk = 1;
			bool firstsent = false;

			foreach(var pkt in CurrentMessageOut.Packets)
			{
				if (firstsent)
				{
					while (CurrentMessageOut.HasNacks())
					{
						Console.WriteLine("Detected Nack! resending...");
						var nackpkt = CurrentMessageOut.GetNextNACK();
						await device.SendReportAsync(0x00, nackpkt);
					}
				}
				

				await device.SendReportAsync(0x00, pkt.Value);
				Console.WriteLine($"Sent message block {pkt.Key}");
				firstsent = true;
				DbgTools.PrintPacket(pkt.Value, maxlines: 10);
			}
			



		}


		private void ParseReportV2Protocol(byte[] data)
		{

			var kind = data[0] == 254 ? NewPacketType.OutOfBounds : (data[0] == 1 ? NewPacketType.MessageStart : NewPacketType.Message);

			//Console.WriteLine($"Packet kind: {kind.ToString()}");

			switch (kind)
			{
				case (NewPacketType.OutOfBounds):
					HandleOutOfBounds(data);
					break;
				case (NewPacketType.MessageStart):
					var report = new V2ReportStart(data);
					report.Print();
					CurrentMessageIn = new V2MessageIn(report);

					if (CurrentMessageIn.Completed)
					{
						OnV2MessageReceived(new V2MessageEventArgs() { Data = CurrentMessageIn.GetData() });
						device.SendReportAsync(0x00, CurrentMessageIn.GetAckMessage());
					}
						

					break;
				case (NewPacketType.Message):
					if (CurrentMessageIn == null)
						return;
					var status = CurrentMessageIn.AddSlice(data);
					device.SendReportAsync(0x00, CurrentMessageIn.GetAckMessage());
					if (CurrentMessageIn.Completed)
					{
						OnV2MessageReceived(new V2MessageEventArgs() { Data = CurrentMessageIn.GetData() });
						
					}
						

					Console.WriteLine($"Added slice {status.slicenumber} to message {CurrentMessageIn.MessageNumber}! {status.BytesToGo} Bytes to go");
					break;
				default:
					break;
			}
		}


		private void HandleOutOfBounds(byte[] data)
		{
			if(data[1] == 0x00)
			{
				Console.WriteLine($"NACK Received! Sequence: {data[2]}");
				if (CurrentMessageOut != null)
					CurrentMessageOut.NAck(data[2]);

				return;
			}

			if(data[1] == 0x01 && data[2] == 0xFF)
			{
				Console.WriteLine("heartbeat..");
				return;
			}

			if (data[1] == 0x01 && data[2] != 0xFF)
			{
				Console.WriteLine("ACK Received! Sequence: {data[2]}");
				if (CurrentMessageOut != null)
				{
					var messagecomplete = CurrentMessageOut.Ack(data[2]);

					if (messagecomplete)
					{
						//TODO: Sent message complete handler
					}

				}
					

				return;
			}


		}

		private void ParseReportOldProtocol(byte[] data)
		{
			var cmd = (PrimeCMD)data[1];
			var BodyLength = BitConverter.ToInt32(data.SubArray(3, 4).Reverse().ToArray());
			var Body = data.SubArray(7, BodyLength);

			var Bodyhex = BitConverter.ToString(Body).Replace("-", " ");
			var Bodystr = Encoding.UTF8.GetString(Body);	

			switch (cmd)
			{
				case (PrimeCMD.GET_INFOS):
					ProcessHpInfos(Body);
					return;
		
				default:
					break;
			}

			Console.WriteLine("### Unknown V1 Packet ###");
			Console.WriteLine($"Command: {cmd.ToString()}  - {data[1].ToString("X2")}");
			Console.WriteLine($"Body length: {BodyLength} Bytes");
			Console.WriteLine($"Body [HEX]:\n {Bodyhex}");
			Console.WriteLine($"Body [UTF8]:\n {Bodystr}");
			Console.WriteLine("### END ###");
			Console.WriteLine("");

		}

		private async Task ProcessHpInfos(byte[] data)
		{
			prime.DeviceInfo = new HpInfos(data);

			Console.WriteLine($"Prime Info | Serialnumber: {prime.DeviceInfo.Serial} | Version: {prime.DeviceInfo.Version} | Build: {prime.DeviceInfo.Build} ");

			if (prime.DeviceInfo.Build < 10591)
			{
				Console.WriteLine("Sorry Calculator not supported! please update!");
				OnUnsupportedCalcConnected();
				return;
			}

			if (ProtocolNegotiated)
				return;

			var pkt_prot = MessageUtils.Misc.GetPacketSetProtocolV2();
			await device.SendReportAsync(pkt_prot.id, pkt_prot.data);

			Console.WriteLine("Sent protocol V2 request");

			ProtocolNegotiated = true;
			
			prime.InfoChanged();
			prime.ConnectionInitDone();
		}

		

		#endregion

		

		#region Events

		public event EventHandler UnsupportedCalcConnected;

		protected virtual void OnUnsupportedCalcConnected()
		{

			var handler = UnsupportedCalcConnected;
			if (handler != null) handler(this, EventArgs.Empty);
		}


		public event EventHandler<V2MessageEventArgs> V2MessageReceived;

		protected virtual void OnV2MessageReceived(V2MessageEventArgs e)
		{

			var handler = V2MessageReceived;
			if (handler != null) handler(this, e);
		}

		#endregion
	}


	public class V2MessageEventArgs : EventArgs
	{
		public byte[] Data { get; set; }
	}



}
