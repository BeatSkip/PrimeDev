using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Calculator
{



	public class PrimePacketold
	{
		private byte[] data;

		public PacketType Type;

		public PrimePacketold(byte Id,byte[] src, bool isReceived = true)
		{
			data = src;

			if (isReceived)
				ParsePacket();

		}


		private void ParsePacket()
		{

		}

	}

	public enum PacketType : byte
	{
		Chat = 0xF2,
		FileRequest = 0xF8,
		FileTransfer = 0xF7,
		SendKey = 0xEC,
		SetDateTime = 0xE7,
		TransferBackup = 0xF9,
		CheckReady = 0xFF,
		unkown = 0
	}

	public struct PrimeChunk
	{
		public byte ReportId { get; init; }
		public PacketType Type { get; init; }
		public byte[] Body { get; init; }
		public int ItemCount { get; init; }
		public int BodyLength { get; init; }
		public PrimeChunk(int reportId, byte[] data)
		{
			ReportId = (byte)((reportId << 24) >> 24);
			Type = (PacketType)data[1];
			ItemCount = ((int)data[2]);
			BodyLength = BitConverter.ToInt32(data.SubArray(3,4).Reverse().ToArray());
			Body = data.SubArray(7, BodyLength);
		}

		public void Print(int identifier = -1)
		{
			if(identifier == -1)
				Console.WriteLine("## Packet ##");
			else
				Console.WriteLine($"## Packet {identifier} ##");

			Console.WriteLine($"Report ID: {(int)this.ReportId}");
			Console.WriteLine($"Packet Type: {this.Type.ToString()} | {((byte)this.Type).ToString()}");
			Console.WriteLine($"Body length: {BodyLength}");
			Console.WriteLine($"Body:\n{ System.Text.Encoding.Unicode.GetString(Body)}");
		}

		private string DebugString(int identifier = -1)
		{
			StringBuilder str = new StringBuilder();
			if (identifier == -1)
				str.AppendLine("## Packet ##");
			else
				str.AppendLine($"## Packet {identifier} ##");

			str.AppendLine($"Report ID: {(int)this.ReportId}");
			str.AppendLine($"Packet Type: {this.Type.ToString()} | {((byte)this.Type).ToString()}");
			str.AppendLine($"Body length: {BodyLength}");
			str.AppendLine($"Body:\n{ System.Text.Encoding.Unicode.GetString(Body)}");

			return str.ToString();
		}

		public override string ToString()
		{
			switch (Type)
			{
				case (PacketType.Chat):
					return System.Text.Encoding.Unicode.GetString(Body);
				default:
					return DebugString();

			}
		}
	}
}
