using System.IO.Compression;

namespace PrimeWeb.Utility
{
	public static class HpFileParser
	{
		public static (string Name, PrimeDataType Type, uint Size, byte[] Contents) ParseFileHeader(byte[] sourcedata)
		{
			uint FileSize = ((uint)sourcedata[2]) << 24 | ((uint)sourcedata[3]) << 16 | ((uint)sourcedata[4]) << 8 | (uint)sourcedata[5];
			int namelength = sourcedata[7];
			var FileType = (PrimeDataType)sourcedata[6];

			var Filename = Conversion.DecodeTextData(sourcedata.SubArray(10, namelength));

			var PacketContent = sourcedata.SubArray(10 + namelength);

			return (Filename, FileType, FileSize, PacketContent);
		}

		public static byte[] GenerateFileHeader(string Name, PrimeDataType Type, uint Size, byte[] Contents)
		{
			var crc = new byte[] { 0x00, 0x00 };
			using (var ms = new MemoryStream())
			using (var writer = new BinaryWriter(ms))
			{
				//writer.Write(cmdheader);
				//writer.Write(length);
				//writer.Write(crc);
				//writer.Write(compressed);
				//return ms.ToArray();
			}

			//uint FileSize = ((uint)sourcedata[2]) << 24 | ((uint)sourcedata[3]) << 16 | ((uint)sourcedata[4]) << 8 | (uint)sourcedata[5];
			//int namelength = sourcedata[7];
			//var FileType = (PrimeFileType)sourcedata[6];
			//
			//var Filename = Conversion.DecodeTextData(sourcedata.SubArray(10, namelength));
			//
			//var PacketContent = sourcedata.SubArray(10 + namelength);
			//
			//return (Filename, FileType, FileSize, PacketContent);
			return new byte[] { 0x00, 0x00 };
		}

		public static void ParseSettingsFile(byte[] data)
		{
			Console.WriteLine("Settings data:");
			//DbgTools.PrintPacket(data);
		}

		public static byte[] DecompressFileStream(byte[] compressed)
		{
			using var from = new MemoryStream(compressed);
			using var to = new MemoryStream();
			using var zLibStream = new ZLibStream(from, CompressionMode.Decompress);
			zLibStream.CopyTo(to);
			List<byte> result = new List<byte>(new byte[] { 0xF7, 0x03 });
			//List<byte> result = new List<byte>( );
			var ds = to.ToArray();

			result.AddRange(Conversion.GetBigEndianBytes((uint)ds.Length));
			result.AddRange(to.ToArray());
			return result.ToArray();
		}

		


		private static int Search(byte[] src, byte[] pattern, int startindex = 0)
		{
			int maxFirstCharSlot = src.Length - pattern.Length + 1;
			for (int i = startindex; i < maxFirstCharSlot; i++)
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

		public enum contentids : uint
		{
			HpApp = 0x7C61,
			unknown,
		}

	}
}
