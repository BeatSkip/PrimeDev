using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimeWeb.Calculator;
using PrimeWeb.Packets;
using PrimeWeb.Protocol;
using PrimeWeb.Types;
using PrimeWeb.Utility;

namespace PrimeWeb.Packets
{
	public class PrimeKeyPayload : IPacketPayload
	{
		public uint Key { get; set; }
		public Type Type { get { return typeof(PrimeKeyPayload); } }

		public PrimeKeyPayload()
		{

		}

		public void ReversePayload(byte[] payload)
		{
			var bts = payload.SubArray(6, payload[5]).ToList();

			for (int i = 0; i < 4-bts.Count; i++)
			{
				bts.Add(0x00);
			}

			Key = BitConverter.ToUInt32(bts.ToArray());
		}

		public byte[] GeneratePayload()
		{
			return new byte[] { (byte)PrimeCMD.SEND_KEY, 0x03, 0x00, 0x00, 0x00, 0x02, (byte)Key, (byte)(Key >> 8) };

		}
	}
}
