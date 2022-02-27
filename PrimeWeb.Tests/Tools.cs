using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Tests
{
	internal static class Tools
	{
		internal static int Search(byte[] src, byte[] pattern)
		{
			int maxFirstCharSlot = src.Length - pattern.Length + 1;
			for (int i = 0; i < maxFirstCharSlot; i++)
			{
				if (src[i] != pattern[0]) // compare only first byte
					continue;

				// found a match on first byte, now try to match rest of the pattern
				for (int j = pattern.Length - 1; j >= 1; j--)
				{
					if (src[i + j] != pattern[j]) break;
					if (j == 1) return i;
				}
			}
			return -1;
		}

		internal static void PrintPacket(ReadOnlySpan<byte> data, int linesize = 32, int maxlines = int.MaxValue, int msg = -1)
		{
			int index = 0;
			int linecounter = 0;

			string id = msg > 0 ? $"message id: {msg}" : $"";

			Console.WriteLine($"# -- # {id}");
			while (index < data.Length && linecounter < maxlines)
			{

				ReadOnlySpan<byte> buffer;
				string line = "";
				if (index + linesize < data.Length)
					buffer = data.Slice(index, linesize);
				else
					buffer = data.Slice(index);

				line = Convert.ToHexString(buffer);
				line += "\t|\t";
				line += Encoding.UTF8.GetString(buffer).Replace("\r", " ").Replace("\n", " ");
				index += linesize;
				Console.WriteLine(line);
				linecounter++;
			}
			Console.WriteLine(" -- ");
		}

		public static uint ReadLittleEndianBytes(byte[] number, int startindex = 0)
		{
			return ((uint)number[startindex + 3]) << 24 | ((uint)number[startindex + 2]) << 16 | ((uint)number[startindex + 1]) << 8 | (uint)number[startindex];
		}

		public static uint ReadBigEndianBytes(byte[] number, int startindex = 0)
		{
			return ((uint)number[startindex]) << 24 | ((uint)number[startindex + 1]) << 16 | ((uint)number[startindex + 2]) << 8 | (uint)number[startindex + 3];
		}

		public static uint ReadBytesAsUint(byte[] number, Endianness type = Endianness.Big, int startindex = 0)
		{
			if (type == Endianness.Big)
				return ReadBigEndianBytes(number, startindex);
			else
				return ReadLittleEndianBytes(number, startindex);
		}

		public enum Endianness
		{
			Big,
			Little
		}

	}
}
