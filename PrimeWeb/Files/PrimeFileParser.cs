using BinaryExtensions;
using NetCoreEx.BinaryExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Files
{
	internal static class PrimeFileParser
	{
		public static HP_List Parse_List(byte[] data)
		{
			using(var ms = new MemoryStream(data))
			using(var reader = new BinaryReader(ms))
			{
				reader.ReadBytes(2);//dump start

				var lstlength = reader.ReadInt32();


				PrimeDataType type = reader.ReadByte();
				int namelength = reader.ReadByte();
				var crc = reader.ReadBytes(2);
				string name = reader.ReadUnicodeString(namelength);

				

				var MagicString = reader.ReadBytes(4);

				if (MagicString[0] != 0xFE || MagicString[1] != 0xFF || MagicString[2] != 0x16 || MagicString[3] != 0x00)
				{
					DbgTools.PrintPacket(MagicString, title: "error list!");
					throw new Exception("Sorry trying to parse a list that isn't a list!");
				}
					

				int itemcnt = reader.ReadByte();

				Console.WriteLine($"List - {name} - {lstlength} Bytes - {itemcnt} items");

				return new HP_List();
			}
		}

	}
}
