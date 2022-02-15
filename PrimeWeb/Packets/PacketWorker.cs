using Blazm.Hid;
using PrimeWeb.HpTypes;
using PrimeWeb.Packets;
using System.Text;
using PrimeWeb.Calculator;

namespace PrimeWeb.Packets
{
	public class PacketWorker
	{
		private HidDevice device;

		private PrimeCalculator prime;

		public uint MessageCount { get; private set; } = 0;

		public ProtocolVersion Protocol { get; private set; } = ProtocolVersion.Unknown;

		private V2MessageIn CurrentMessageIn	{ get; set; }
		private V2MessageOut CurrentMessageOut	{ get; set; }

		public PacketWorker(PrimeCalculator parent, HidDevice hid)
		{
			this.device = hid;
			this.prime = parent;
			this.device.ReportReceived += Prime_ReportReceived;
            this.device.Connected += Device_Connected;
			
		}

        private async void Device_Connected(object? sender, EventArgs e)
        {
			Console.WriteLine("[PacketWorker] - Prime is connected!");
			Console.WriteLine("Sending status packet!");

			var pkt_status = MessageUtils.Misc.GetPacketInfoRequest();
			await device.SendReportAsync(pkt_status.id, pkt_status.data);
		}


        #region Usb Communication

        public async Task ConnectAsync()
        {
			if (device.Opened)
				return;

			await device.OpenAsync();
		}

		#endregion



		public async Task Send(byte[] Data)
		{
			var msg = new V2MessageOut(MessageCount++, Data);

		}

	

		private void Prime_ReportReceived(object? sender, OnInputReportArgs e)
		{
			var data = e.Data;
			//Console.WriteLine("[PacketWorker] - Report Received!");

			var version = GetReportProtocol(data);

			switch (version)
			{
				case (ProtocolVersion.Old):
					ParseReportOldProtocol(data);
					break;
				case (ProtocolVersion.V2):
					ParseReportV2Protocol(data);
					break;
				default:
					break;

			}
			//DbgTools.PrintPacket(data);
		}


		private void ParseReportOldProtocol(byte[] data)
		{
			Console.WriteLine("### V1 Packet ###");

			byte[] command = { data[1] };

			var cmdstr = BitConverter.ToString(command).Replace("-", " ");
			var cmd = (PrimeCMD)data[1];
			Console.WriteLine($"Command: {cmdstr} | {cmd.ToString()}");
			var BodyLength = BitConverter.ToInt32(data.SubArray(3, 4).Reverse().ToArray());
			Console.WriteLine($"Body length: {BodyLength} Bytes");
			var Body = data.SubArray(7, BodyLength);
			var Bodyhex = BitConverter.ToString(Body).Replace("-", " ");
			var Bodystr = Encoding.UTF8.GetString(Body);
			switch (cmd)
			{
				case (PrimeCMD.GET_INFOS):
					ProcessHpInfos(Body);
					break;
				default:
					Console.WriteLine($"Body [HEX]:\n {Bodyhex}");
					Console.WriteLine($"Body [UTF8]:\n {Bodystr}");
					break;
			}
			Console.WriteLine("### END ###");
			Console.WriteLine("");
		}

		private void ParseReportV2Protocol(byte[] data)
		{


			var kind = data[0] == 254 ? NewPacketType.OutOfBounds : (data[0] == 1 ? NewPacketType.MessageStart : NewPacketType.Message);

			//Console.WriteLine($"Packet kind: {kind.ToString()}");

			switch (kind)
			{
				case (NewPacketType.OutOfBounds):
					//Console.WriteLine("Out of bounds packet!");
					break;
				case (NewPacketType.MessageStart):
					var report = new V2ReportStart(data);
					report.Print();
					CurrentMessageIn = new V2MessageIn(report);
					break;
				case (NewPacketType.Message):
					if (CurrentMessageIn == null)
						return;

					var status = CurrentMessageIn.AddSlice(data);

					if (CurrentMessageIn.Completed)
						OnV2MessageReceived(new V2MessageEventArgs() { Data = CurrentMessageIn.GetData() });

					Console.WriteLine($"Added slice {status.slicenumber} to message {CurrentMessageIn.MessageNumber}! {status.BytesToGo} Bytes to go");
					break;
				default:
					break;

			}

		}


		private ProtocolVersion GetReportProtocol(byte[] data)
		{
			if (data[0] == 0x00)
				return ProtocolVersion.Old;

			return ProtocolVersion.V2;
		}

		#region packet parsers

		private async Task ProcessHpInfos(byte[] data)
		{
			var infos = new HpInfos(data);

			Console.WriteLine($"Prime Info | Serialnumber: {infos.Serial} | Version: {infos.Version} | Build: {infos.Build} ");
			prime.DeviceInfo = infos;


			if (infos.Build < 10591)
            {
				Console.WriteLine("Sorry Calculator not supported! please update!");
				OnUnsupportedCalcConnected();
				return;
			}

			var pkt_prot = MessageUtils.Misc.GetPacketSetProtocolV3();
			await device.SendReportAsync(pkt_prot.id, pkt_prot.data);

			Console.WriteLine("Sent protocol V3 request");

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
