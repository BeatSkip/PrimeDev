using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;


namespace PrimeWeb.Utility
{
	public static class Conversion
	{

		public static string DecodeTextData(byte[] data)
		{
			return BytesToUnicodeString(data);
		}

		public static string DecodeTextData(BinaryReader reader, int length)
		{
			return BytesToUnicodeString(reader.ReadBytes(length));
		}

		public static string DecompressAndDecodeTextData(byte[] data)
		{
			var decompressed = decompress(data);
			return DecodeTextData(decompressed);
		}

		public static byte[] EncodeTextData(string data)
		{
			return UnicodeStringToBytes(data);
		}

		public static byte[] EncodeAndCompressTextData(string data)
		{
			var unicodebytes = EncodeTextData(data);


			return compress(unicodebytes);

		}

		private static byte[] UnicodeStringToBytes(string message)
		{
			UnicodeEncoding unicode = new UnicodeEncoding();
			var Preamble = unicode.Preamble.ToArray();

			if (Preamble[0] != 0xFF || Preamble[1] != 0xFE)
			{
				Console.WriteLine("!!-> TEXT ENCODING ERROR, PREAMBLE NOT CORRECT! <-!!");
			}

			return unicode.GetBytes(message);


		}

		private static string BytesToUnicodeString(byte[] message)
		{
			UnicodeEncoding unicode = new UnicodeEncoding();
			var Preamble = unicode.Preamble.ToArray();

			if (Preamble[0] != 0xFF || Preamble[1] != 0xFE)
			{
				Console.WriteLine("!!-> TEXT ENCODING ERROR, PREAMBLE NOT CORRECT! <-!!");
			}

			return unicode.GetString(message);
		}

		

		public static byte[] compress(byte[] input)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (ZLibStream zls = new ZLibStream(ms, CompressionLevel.Fastest))
				{
					zls.Write(input, 0, input.Length);
				}
				return ms.ToArray();
			}
		}

		public static byte[] decompress(byte[] input)
		{
			using (MemoryStream compressed = new MemoryStream())
			{
				compressed.Write(input, 0, input.Length);
				using (MemoryStream decompressedFileStream = new MemoryStream())
				{
					using (ZLibStream decompressionStream = new ZLibStream(compressed, CompressionMode.Decompress))
					{
						
						decompressionStream.CopyTo(decompressedFileStream);
						return decompressedFileStream.ToArray();
					}
				}
			}
		}

		public static byte[] GetLittleEndianBytes(int number)
		{
			return GetLittleEndianBytes((uint)number);
		}

		public static byte[] GetLittleEndianBytes(uint number)
		{
			byte[] data = new byte[4];
			data[0] = (byte)(number & (uint)0x000000FF);
			data[1] = (byte)((number & (uint)0x0000FF00) >> 8);
			data[2] = (byte)((number & (uint)0x00FF0000) >> 16);
			data[3] = (byte)((number & (uint)0xFF000000) >> 24);

			return data;
		}

		public static byte[] GetBigEndianBytes(int number)
		{
			return GetBigEndianBytes((uint)number);
		}

		public static byte[] GetBigEndianBytes(uint number)
		{
			byte[] data = new byte[4];
			data[3] = (byte)(number & (uint)0x000000FF);
			data[2] = (byte)((number & (uint)0x0000FF00) >> 8);
			data[1] = (byte)((number & (uint)0x00FF0000) >> 16);
			data[0] = (byte)((number & (uint)0xFF000000) >> 24);

			return data;
		}

		public static uint ReadLittleEndianBytes(byte[] number, int startindex = 0)
		{
			return ((uint)number[startindex + 3]) << 24 | ((uint)number[startindex + 2]) << 16 | ((uint)number[startindex + 1]) << 8 | (uint)number[startindex];
		}

		public static uint ReadBigEndianBytes(byte[] number, int startindex = 0)
		{
			return ((uint)number[startindex]) << 24 | ((uint)number[startindex + 1]) << 16 | ((uint)number[startindex + 2]) << 8 | (uint)number[startindex + 3];
		}
		public static uint ReadBigEndianBytes(BinaryReader reader)
		{
			return ReadBigEndianBytes(reader.ReadBytes(4));
		}

		public static ulong ReadLittleEndianULong(byte[] number, int startindex = 0)
		{
			return ((ulong)number[startindex + 7]) << 56 | ((ulong)number[startindex + 6]) << 48 | ((ulong)number[startindex + 5]) << 40 | ((ulong)number[startindex + 4]) << 32 | ((ulong)number[startindex + 3]) << 24 | ((ulong)number[startindex + 2]) << 16 | ((ulong)number[startindex + 1]) << 8 | (ulong)number[startindex];
		}

		public static ulong ReadBigEndianULong(byte[] number, int startindex = 0)
		{
			return ((ulong)number[startindex]) << 56 | ((ulong)number[startindex + 1]) << 48 | ((ulong)number[startindex + 2]) << 40 | ((ulong)number[startindex + 3]) << 32 | ((ulong)number[startindex + 4]) << 24 | ((ulong)number[startindex + 5]) << 16 | ((ulong)number[startindex + 6]) << 8 | (ulong)number[startindex + 7];
		}

		

		public static uint ReadLittleEndianBytes(BinaryReader reader)
		{
			return ReadLittleEndianBytes(reader.ReadBytes(4));
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
