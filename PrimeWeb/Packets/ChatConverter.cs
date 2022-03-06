using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Packets
{
	public static class ChatConverter
	{
		public static byte[] CreateChat(string content)
		{
			var messagebytes = Conversion.EncodeTextData(content);
			var compress = false;

			//if (messagebytes.Length > 1000)
			//	compress = true;

			using(var ms = new MemoryStream())
			using(var writer = new BinaryWriter(ms))
			{
				writer.Write((byte)0xF2);
				writer.Write((byte)0x03);
				if (compress)//compression currently disabled
				{
					writer.Write((byte)0x00);
					writer.Write((byte)0x00);
					writer.Write((byte)0x00);//CRC
					writer.Write((byte)0x00);//CRC
					writer.Write(Conversion.GetLittleEndianBytes(messagebytes.Length));
					writer.Write(Conversion.compress(messagebytes));
				}
				else
				{
					writer.Write(Conversion.GetBigEndianBytes(messagebytes.Length));
					writer.Write(messagebytes);
				}
				
				return ms.ToArray();
			}
		}

	}
}
