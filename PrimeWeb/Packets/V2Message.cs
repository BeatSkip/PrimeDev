using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Packets
{
	public class V2Message : IDisposable
	{
		public MsgDir Direction { get; init; }
		public long MessageNumber { get; private set; }
		public long MessageSize { get; set; }
		public List<byte> Data { get; set; }

		public long TransferredBytes
		{
			get
			{
				if (Direction == MsgDir.IN)
					return Data.Count;
				else
					return MessageSize - Data.Count;
			}
		}

		public V2Message(MsgDir dir,long msgNumber, long msgSize, byte[] data)
		{

		}


		public void Dispose()
		{

		}

	}

	public enum MsgDir
	{
		IN,
		OUT
	}


}
