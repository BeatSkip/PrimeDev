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
		public bool Completed { get { return (bytesleft == 0); } }
		public uint MessageNumber { get; protected set; }
		public uint MessageSize { get; protected set; }

		protected List<byte> Data;

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

		public V2MessageIn(V2ReportStart first)
		{
			base.Direction = MsgDir.IN;
			MessageNumber = (uint)first.MessageNumber;
			MessageSize = (uint)first.MessageSize;
			Data = new List<byte>(first.Data);
			bytesleft = MessageSize - Data.Count;
		}


		public (int slicenumber, uint BytesToGo) AddSlice(byte[] data)
		{
			var takeamount = bytesleft < 1023 ? bytesleft : 1023;
			Data.AddRange(data.SubArray(1, (int)takeamount));
			bytesleft -= takeamount;
			return ((int)data[0], (uint)bytesleft);
		}

		public byte[] GetData()
		{
			return Data.ToArray();
		}
		
	}

	public class V2MessageOut : V2Message
	{
		public V2MessageOut(uint msgNumber, byte[] data, int blockSize = 1024)
		{
			base.Direction = MsgDir.OUT;
			Data = new List<byte>(data);
			MessageNumber = msgNumber;
			MessageSize = (uint)data.Length;
		}

		private byte[] GetHeader(int index)
		{
				return GetFirstHeader();


		}

		private byte[] GetFirstHeader()
		{
			var bytes_msgnr = BitConverter.GetBytes(MessageNumber);
			var bytes_msglen = BitConverter.GetBytes(MessageSize);

			return new byte[] { 0x01, bytes_msgnr[0], bytes_msgnr[1], bytes_msgnr[2], bytes_msgnr[3], bytes_msglen[0], bytes_msglen[1], bytes_msglen[2], bytes_msglen[3] };
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

	public struct AckPacket
	{


	}

	public enum MsgDir
	{
		IN,
		OUT
	}


}
