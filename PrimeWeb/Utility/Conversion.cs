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
				using (ZLibStream zls = new ZLibStream(ms, CompressionLevel.NoCompression))
				{
					zls.Write(input, 0, input.Length);
				}
				return ms.ToArray();
			}
		}


		public static byte[] decompress_old(byte[] input)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (ZLibStream zls = new ZLibStream(ms, CompressionMode.Decompress))
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

		public static byte[] GetLittleEndianBytes(uint number)
		{
			byte[] data = new byte[4];
			data[0] = (byte)(number & (uint)0x000000FF);
			data[1] = (byte)((number & (uint)0x0000FF00) >> 8);
			data[2] = (byte)((number & (uint)0x00FF0000) >> 16);
			data[3] = (byte)((number & (uint)0xFF000000) >> 24);

			return data;
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
	}
}
