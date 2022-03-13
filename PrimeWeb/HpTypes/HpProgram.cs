using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.HpTypes
{
	public class HpProgram
	{
		//TODO: Type implementation - OTPrograms
		private byte[] content;

		public HpProgram(byte[] data)
		{
			this.content = data;
			parseData(data);
		}


		private void parseData(byte[] data)
		{
			using(var ms = new MemoryStream(data))
			using(var reader = new BinaryReader(ms))
			{
				reader.ReadBytes(2);
				var totallen = Conversion.ReadBigEndianBytes(reader.ReadBytes(4));
				var type = reader.ReadByte();
				var namelength = (int)reader.ReadByte();
				var crc = reader.ReadBytes(2);
				var name = Conversion.DecodeTextData(reader.ReadBytes(namelength));
			}
			//DbgTools.PrintPacket(data, title: "hpprogram");
		}
	}
}
