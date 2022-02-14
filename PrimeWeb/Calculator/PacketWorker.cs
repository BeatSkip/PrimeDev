using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazm.Hid;
using PrimeWeb.Utility;
using PrimeWeb.HpTypes;

namespace PrimeWeb.Calculator
{
	public class PacketWorker
	{
		private HidDevice calc;
		private PrimeCalculator prime;

		public long MessageCount { get; private set; }

		public ProtocolVersion MaxProtocolVersion { get; private set; }

		public PacketWorker(PrimeCalculator parent, HidDevice device)
		{
			calc = device;
			calc.Notification += Prime_Notification;
			this.prime = parent;
			prime.Connected += Prime_Connected;

		}

		private async void Prime_Connected(object? sender, EventArgs e)
		{
			Console.WriteLine("[PacketWorker] - Prime is connected!");
			Console.WriteLine("Sending status packet!");

			var pkt_status = GetPacketStatusRequest();
			await calc.SendReportAsync(pkt_status.id, pkt_status.data);

			var pkt_prot = GetPacketSetProtocolV2();
			await calc.SendReportAsync(pkt_prot.id, pkt_prot.data);

			


		}

		private void Prime_Notification(object? sender, OnInputReportArgs e)
		{
			var data = e.Data;
			Console.WriteLine("[PacketWorker] - Report Received!");

			var version = GetReportProtocol(data);

			switch (version)
			{
				case (ProtocolVersion.Old):
					ParseReportOldProtocol(data);
					break;
				case (ProtocolVersion.New):
					ParseReportNewProtocol(data);
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

		private void ParseReportNewProtocol(byte[] data)
		{
			Console.WriteLine("### V2 Packet ###");
			
			var kind = data[0] == 254 ? NewPacketType.OutOfBounds : (data[0] == 1 ? NewPacketType.IOMessageStart : NewPacketType.IOMessage);

			Console.WriteLine($"Packet kind: {kind.ToString()}");


			byte[] command = data.SubArray(1, 2);
			var cmdstr = BitConverter.ToString(command).Replace("-", " ");
			Console.WriteLine($"Command: {cmdstr}");
			var BodyLength = BitConverter.ToInt32(data.SubArray(3, 4).Reverse().ToArray());
			Console.WriteLine($"Body length: {BodyLength} Bytes");
			var Body = data.SubArray(7, BodyLength);
			var Bodystr = BitConverter.ToString(Body).Replace("-", " ");
			Console.WriteLine("Body:");
			Console.WriteLine(Bodystr);
			Console.WriteLine("### END ###");
			Console.WriteLine("");
		}

	
		private ProtocolVersion GetReportProtocol(byte[] data)
		{
			if (data[0] == 0x00)
				return ProtocolVersion.Old;

			if (data[0] == 255)
			{
				if (data[7] == 0)
					return ProtocolVersion.New;

				if (data[7] == 1)
					return ProtocolVersion.New;
			}

			if (data[0] == 254)
				return ProtocolVersion.New;

			return ProtocolVersion.Unknown;
		}

		#region packet parsers

		private void ProcessHpInfos(byte[] data)
		{
			var infos = new HpInfos(data);

			Console.WriteLine($"Prime Info | Serialnumber: {infos.Serial} | Version: {infos.Version} | Build: {infos.Build} ");
			prime.DeviceInfo = infos;
			prime.InfoChanged();
			
		}

		

		#endregion

		#region Packet Generators

		private (byte id, byte[] data) GetPacketSetProtocolV2()
		{
			byte[] content = { 0xFF, 0xEC, 0, 0, 0, 0, 0, 0 };
			return (0, content);
		}

		private (byte id, byte[] data) GetPacketStatusRequest()
		{
			byte[] content = { 0x00, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			return (0, content);
		}

		private (byte id, byte[] data) GetPacketAck(long packet)
		{
			byte[] content = { 0xFE, (byte)ResponseStatus.ACK, 0, 0, 0, 0, 0, 0 };
			return (0, content);
		}

		private (byte id, byte[] data) GetPacketInfoRequest()
		{
			byte[] content = { 0x00, (byte)PrimeCMD.GET_INFOS, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			return (0, content);
		}

		#endregion
	}

	public enum ProtocolVersion : byte
	{
		Unknown = 254,
		Old = 0,
		New = 1,
	}

	public enum NewPacketType : byte
	{
		IOMessageStart,
		IOMessage,
		OutOfBounds
	}

	public enum ResponseStatus : byte
	{
		ACK = 0x01,
		NACK = 0x00
	}

	public enum PrimeCMD : byte
	{
		//CMD_PRIME_CHECK_READY (0xFF)
		CHECK_READY = 0xFF,
		//CMD_PRIME_GET_INFOS (0xFA)
		GET_INFOS = 0xFA,
		//CMD_PRIME_RECV_SCREEN (0xFC)
		RECV_SCREEN = 0xFC,
		//CMD_PRIME_RECV_BACKUP (0xF9)
		RECV_BACKUP = 0xF9,
		//CMD_PRIME_REQ_FILE (0xF8)
		REQ_FILE = 0xF8,
		//CMD_PRIME_RECV_FILE (0xF7)
		RECV_FILE = 0xF7,
		//CMD_PRIME_SEND_CHAT (0xF2)
		TRANSFER_CHAT = 0xF2,
		//CMD_PRIME_SEND_KEY (0xEC)
		SEND_KEY = 0xEC,
		//CMD_PRIME_SET_DATE_TIME (0xE7)
		SET_DATETIME = 0xE7,
	}
}
