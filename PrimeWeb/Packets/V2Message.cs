using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Packets
{
	public class V2Message : IDisposable
	{
		public MsgDir Direction { get; protected set; }
		public bool Completed { get; protected set; } = false;
		public uint MessageNumber { get; protected set; }
		public uint MessageSize { get; protected set; }

		protected List<byte> Data;

		public Dictionary<int, byte[]> Packets { get; protected set; }
		public Dictionary<int, bool>  ACKS { get; protected set; }
		public Dictionary<int, bool>  NACKS { get; protected set; }

		public int completedAck { get; protected set; }

		protected long bytesleft;

		public virtual int BytesInNextPacket { get; }

		public V2Message()
		{
		}


		public void Dispose()
		{

		}

	}

	public class V2MessageIn : V2Message
	{
		private int lastpacket = 1;

		public V2MessageIn(V2ReportStart first)
		{
			base.Direction = MsgDir.IN;
			MessageNumber = (uint)first.MessageNumber;
			MessageSize = (uint)first.MessageSize;
			Data = new List<byte>(first.Data);
			bytesleft = MessageSize - Data.Count;

			if (bytesleft == 0)
				Completed = true;
		}


		public (int slicenumber, uint BytesToGo) AddSlice(byte[] data)
		{
			var takeamount = bytesleft < 1023 ? bytesleft : 1023;
			Data.AddRange(data.SubArray(1, (int)takeamount));
			bytesleft -= takeamount;

			if (bytesleft == 0)
				Completed = true;

			lastpacket = data[0];

			return ((int)data[0], (uint)bytesleft);
		}

		public byte[] GetData()
		{
			return Data.ToArray();
		}

		public byte[] GetAckMessage()
		{
			return new byte[] { 0xFE, 0x01, (byte)lastpacket };
		}
		
	}

	public class V2MessageOut : V2Message
	{
		private int sendcounter;

		public V2MessageOut(uint msgNumber, byte[] data, int blockSize = 1024)
		{
			base.Direction = MsgDir.OUT;
			Data = new List<byte>(data);
			MessageNumber = msgNumber;
			MessageSize = (uint)data.Length;
			sendcounter = 0;
		}

		public void GeneratePackets()
		{
			this.Packets = new Dictionary<int, byte[]>();
			this.ACKS = new Dictionary<int, bool>();
			this.NACKS = new Dictionary<int, bool>();
			bool isdone;
			var blk = 0;
			do
			{
				var pkt = this.GetNextPacket(out isdone);
				Packets.Add((int)pkt.sequence, pkt.Data);
				ACKS.Add((int)pkt.sequence, false);
				NACKS.Add((int)pkt.sequence, false);
				blk++;
				if (isdone)
					completedAck = (int)pkt.sequence;
			} while (!isdone);


		}

		public byte[] GetNextNACK()
		{
			bool isdone = true;
			foreach (var item in NACKS)
			{
				if (item.Value)
				{
					NACKS[item.Key] = false;
					return Packets[item.Key];
				}
					
			}
			return null;
		}

		public bool HasNacks()
		{
			bool isdone = false;
			foreach (var item in NACKS)
			{
				if (item.Value)
					isdone = true;
			}
			return isdone;
		}

		public bool IsAcknowledged()
		{
			bool isdone = true;
			foreach (var item in ACKS)
			{
				if (!item.Value)
					isdone = false;
			}
			return isdone;
		}

		public bool Ack(int packet)
		{
			this.ACKS[packet] = true;

			if(packet == completedAck)
			{
				Completed = true;
				Console.WriteLine("Sending packet Complete!");
				return true;
			}
			return false;
		}

		public void NAck(int packet)
		{
			this.NACKS[packet] = true;
		}

		private (byte sequence, byte[] Data) GetNextPacket(out bool FinalPacket)
		{
			byte[] data;
			byte sequence = 0;

			if(sendcounter == 0)
            {
				data = GetFirstMessage();
				sequence = 0x01;

			}
            else
            {
				data = GetNextMessage();
				sequence = getsequenceNumber();

			}

			sendcounter++;

			if(Data.Count == 0)
				FinalPacket = true;
			else
				FinalPacket = false;

			return (sequence, data);

		}
		

		private byte[] GetFirstMessage()
		{
			var bytes_msgnr = BitConverter.GetBytes(MessageNumber);
			var bytes_msglen = BitConverter.GetBytes(MessageSize);

			var msg = new List<byte>(new byte[] { 0x01, bytes_msgnr[0], bytes_msgnr[1], bytes_msgnr[2], bytes_msgnr[3], bytes_msglen[0], bytes_msglen[1], bytes_msglen[2], bytes_msglen[3] });

			var takecount = Data.Count < 1015 ? (int)Data.Count : 1015;

			msg.AddRange(Data.Take(takecount));
			Data.RemoveRange(0, takecount);

			return msg.ToArray();

		}

		private byte[] GetNextMessage()
        {
			var msg = new List<byte>(new byte[] { getsequenceNumber() });

			var takecount = Data.Count < 1015 ? (int)Data.Count : 1015;

			msg.AddRange(Data.Take(takecount));
			Data.RemoveRange(0, takecount);

			return msg.ToArray();
		}

		private byte getsequenceNumber()
        {
			return (byte)((sendcounter % 252) + 1);

		}

		public void TestSequence()
        {

        }




	}

	public struct V2ReportStart
	{
		public uint MessageNumber { get;  init; }
		public uint MessageSize { get; init; }
		public byte[] Data { get; init; }

		private byte[] raw;

		public V2ReportStart(byte[] data)
		{
			raw = data;
			MessageNumber = (uint)data[1] | (uint)data[2] << 8 | (uint)data[3] << 16 | (uint)data[4] << 24;
			MessageSize = (uint)data[5] | (uint)data[6] << 8 | (uint)data[7] << 16 | (uint)data[8] << 24;
			Data = data.SubArray(9, MessageSize < 1015 ? (int)MessageSize : 1015);
		}

		public void Print(int linesize = 16)
		{
			Console.WriteLine("### V2 report ###");
			Console.WriteLine($"Message Number: {MessageNumber}");
			Console.WriteLine($"Message Length: {MessageSize}");
			int index = 0;
			while (index < raw.Length)
			{

				byte[] buffer;
				string line = "";
				if (index + linesize < raw.Length)
					buffer = raw.SubArray(index, linesize);
				else
					buffer = raw.SubArray(index);

				line = BitConverter.ToString(buffer).Replace("-", " ");
				line += "\t|\t";
				line += System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
				index += linesize;
				Console.WriteLine(line);
			}
		}
	}

	public enum MsgDir
	{
		IN,
		OUT
	}

}
