//#define DBG_ACK

using Blazm.Hid;
using PrimeWeb.Packets;
using PrimeWeb.Types;
using PrimeWeb.Utility;
using System.Text;

namespace PrimeWeb.Protocol
{



	public class FrameWorker
	{
		private bool ProtocolNegotiated = false;

		private IHidDevice device;

		public uint MessageCount { get; private set; } = 1;

		public ProtocolVersion Protocol { get; private set; } = ProtocolVersion.Unknown;

		private PrimePacket TransmissionPacket { get; set; }
		private PrimePacket ReceivePacket { get; set; }

		private PayloadFactory factory;

		private uint MessageID_Backup = uint.MaxValue;

		public FrameWorker(IHidDevice hid, PayloadFactory pfactory)
		{
			factory = pfactory;
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

		public async Task SendFrame(IFrame frame)
		{
			var bytes = frame.GetFrameBytes();
			await device.SendReportAsync(0x00, bytes);
			//Console.WriteLine("-- sent data ----");
			//DbgTools.PrintPacket(bytes, maxlines: 5);
			//Console.WriteLine(" -- -- -- --- -- ");
		}

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

		public async Task HandleReport_Content(byte[] data)
		{
			var frame = new ContentFrame(data);
			
			if (frame.IsStartFrame)
			{
				ReceivePacket = new PrimePacket(factory);

				logpacket($"Created new Packet! of type: {(PrimeCMD)frame.Data[0]}");
			}

			await ReceivePacket.ReceiveNextFrame(this, frame);

			
		}

		public async Task HandleReport_Ack(byte[] data)
		{
			var frame = new AckFrame(data);

			//Console.WriteLine("Received ACK!");
			//DbgTools.PrintPacket(data);

			if (!frame.IsValid)
			{
				Console.WriteLine("Received Ack Frame not valid!");
			}
			else
			{

				if(frame.IOMessageID == MessageID_Backup)
				{
					this.factory.StartBackup();
				}

#if (DBG_ACK)
				frame.printdebug();
				Console.WriteLine("Received ACK!");
#endif
				//DbgTools.PrintPacket(data);
				//await TransmissionPacket.ReceiveNextFrame(this, frame);

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
				Console.WriteLine("Received Old protocol message while not in old protocol mode!");
				Console.WriteLine("Switching back to old protocol mode....");
			}

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

		public static PacketType IdentifyPacket(ContentFrame frame)
		{
			if (frame.IsStartFrame)
				throw new Exception("Can't identify packet if frame is not the primary frame!");

			return (PacketType)frame.GetContentBytes()[0];
		}


		#endregion

		#region Packet input

		public async Task Send(IPayloadGenerator data,bool isbackup)
		{
			await this.Send(new PrimePacket(data),isbackup);
		}

		public async Task Send(IPayloadGenerator data)
		{
			await this.Send(new PrimePacket(data));
		}

		public async Task Send(PrimePacket packet, bool isbackup = false)
		{
			packet.Direction = TransferType.Tx;

			if (isbackup)
			{
				this.MessageID_Backup = MessageCount;
			}
				

			packet.Initialize(MessageCount++);

			TransmissionPacket = packet;

			bool allsent = false;
			do
			{
				allsent = await TransmissionPacket.TransmitNextFrame(this);
			} while (!allsent);
		}

#endregion

#region legacy

		private async Task ProcessHpInfos(byte[] data)
		{
			var result = new HpInfos(data);
			result.Product = ProductIdToProductString(device.ProductId);
			Console.WriteLine($"Info | {result.Product} | Serialnumber: {result.Serial} | Version: {result.Version} | Build: {result.Build} ");

			if (result.Build < 10591)
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

			this.Protocol = ProtocolVersion.V2;
			OnInfoReceived(result);
		}

		private string ProductIdToProductString(ushort? pid)
		{
			switch (pid)
			{
				case (0x0441):
				case (0x1541):
					return "HP Prime G1";
				case (0x2441):
					return "HP Prime G2";
				default:
					return "Unrecognized Calculator";

			}
		}


#endregion

#region Events

		/// <summary>
		/// Event to indicate Description
		/// </summary>
		public event EventHandler<FilePacketEventArgs> ContentReceived;


		/// <summary>
		/// Called to signal to subscribers that Description
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnContentReceived(FilePacketEventArgs e)
		{
			var handler = ContentReceived;
			if (handler != null) handler(this, e);
		}




		/// <summary>
		/// Error event triggered when an unsupported calculator is connected.
		/// </summary>
		public event EventHandler UnsupportedCalcConnected;

		protected virtual void OnUnsupportedCalcConnected()
		{

			var handler = UnsupportedCalcConnected;
			if (handler != null) handler(this, EventArgs.Empty);
		}



		/// <summary>
		/// event indicating the calculator communication has been 
		/// initialized, calculator info has been received and 
		/// protocol has been negotiated
		/// </summary>
		public event EventHandler<CommsInitEventArgs> CalcInitialized;



		protected virtual void OnInfoReceived(HpInfos info)
		{

			var handler = CalcInitialized;
			if (handler != null) handler(this, new CommsInitEventArgs() { Info = info, Version = ProtocolVersion.V2 });
		}



#endregion

#region Legacy code

		/*
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
			if (data[1] == 0x00)
			{
				Console.WriteLine($"NACK Received! Sequence: {data[2]}");
				if (CurrentMessageOut != null)
					CurrentMessageOut.NAck(data[2]);

				return;
			}

			if (data[1] == 0x01 && data[2] == 0xFF)
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

		public event EventHandler<V2MessageEventArgs> V2MessageReceived;

		protected virtual void OnV2MessageReceived(V2MessageEventArgs e)
		{

			var handler = V2MessageReceived;
			if (handler != null) handler(this, e);
		}
		*/
#endregion

		private void logpacket(string line)
		{
#if (DBG_PACKETS)
			Console.WriteLine($"[PacketWorker - packets] {line}");
#endif
		}


	}


	public class CommsInitEventArgs : EventArgs
	{
		public ProtocolVersion Version { get; set; }
		public HpInfos Info { get; set; }
	}

	



}
