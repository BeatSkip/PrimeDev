using BinaryExtensions;
using NetCoreEx.BinaryExtensions;
using System;
using System.Collections.Generic;
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

				Console.WriteLine($"List - {name} - {lstlength} Bytes");

				return new HP_List();
			}
		}

	}
}
