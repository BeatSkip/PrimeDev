using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimeWeb.Utility;
using PrimeWeb.Calculator;

namespace PrimeWeb.Utility
{
	public static class MessageUtils
	{
		public static class Debug
        {
			private static byte getsequenceNumber(int sendcounter)
			{
				return (byte)((sendcounter % 252) + 2);

			}

			public static void TestSequence()
			{
				List<byte> tmp = new List<byte>();

                for (int i = 0; i < 1280; i++)
                {
					tmp.Add(getsequenceNumber(i));
				}

				Console.WriteLine("debugging sequence counter!");
				DbgTools.PrintPacket(tmp.ToArray());
			}
		}
		internal static class Misc
        {
			internal static (byte id, byte[] data) GetPacketRequestScreen(ScreenFormat format)
			{
				byte[] content = { 0x01, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, (byte)PrimeCMD.RECV_SCREEN, 0x08 , 0x00, 0x00, 0x00, 0x01, 0x03 };
				return (0, content);
			}

			internal static (byte id, byte[] data) GetPacketSetProtocolV2()
			{
				byte[] content = { 0xFF, 0xEC, 0, 0, 0, 0, 0, 0 };
				return (0, content);
			}

			internal static (byte id, byte[] data) GetPacketSetProtocolV3()
			{
				byte[] content = { 0xFF, 0xEC, 0, 0, 0, 0, 0, 1 };
				return (0, content);
			}

			internal static (byte id, byte[] data) GetPacketStatusRequest()
			{
				byte[] content = { 0x00, 0xFA, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
				return (0, content);
			}

			internal static (byte id, byte[] data) GetPacketRequestSettings()
			{
				
				byte[] content = { 0x01, 0x01, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0xfd, 0x01, 0x00, 0x00, 0x00, 0x01, 0x03 };
				return (0, content);
			}

			internal static (byte id, byte[] data) GetPacketAck(long packet)
			{
				byte[] content = { 0xFE, (byte)ResponseStatus.ACK, 0, 0, 0, 0, 0, 0 };
				return (0, content);
			}

			internal static (byte id, byte[] data) GetPacketInfoRequest()
			{
				byte[] content = { 0x00, (byte)PrimeCMD.GET_INFOS, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
				return (0, content);
			}
		}
	}
}
