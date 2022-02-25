using System.IO.Compression;

namespace PrimeWeb.Utility
{
	public static class HpFileParser
	{
		public static (string Name, PrimeFileType Type, uint Size, byte[] Contents) ParseFileHeader(byte[] sourcedata)
		{
			uint FileSize = ((uint)sourcedata[2]) << 24 | ((uint)sourcedata[3]) << 16 | ((uint)sourcedata[4]) << 8 | (uint)sourcedata[5];
			int namelength = sourcedata[7];
			var FileType = (PrimeFileType)sourcedata[6];

			var Filename = Conversion.DecodeTextData(sourcedata.SubArray(10, namelength));

			var PacketContent = sourcedata.SubArray(10 + namelength);

			return (Filename, FileType, FileSize, PacketContent);
		}

		public static void ParseSettingsFile(byte[] data)
		{
			Console.WriteLine("Settings data:");
			DbgTools.PrintPacket(data);
		}

		public static byte[] DecompressFileStream(byte[] compressed)
		{
			using var from = new MemoryStream(compressed);
			using var to = new MemoryStream();
			using var zLibStream = new ZLibStream(from, CompressionMode.Decompress);
			zLibStream.CopyTo(to);
			List<byte> result = new List<byte>(new byte[] { 0xF7, 0x03 });
			var ds = to.ToArray();

			result.AddRange(Conversion.GetBigEndianBytes((uint)ds.Length));
			result.AddRange(to.ToArray());
			return result.ToArray();
		}


		public static List<(uint length, bool isfile, byte[] content)> SplitHpAppDir(byte[] data)
		{
			int index = 0;
			var split = new List<(uint length, bool isfile, byte[] content)>();

			uint sectionlength;
			byte[] sectionContent;
			uint sectionType;
			bool headerdone = false;

			using (var ms = new MemoryStream(data))
			using (var reader = new BinaryReader(ms))
			{
				while (reader.BaseStream.Position != reader.BaseStream.Length)
				{
					sectionlength = Conversion.ReadBigEndianBytes(reader.ReadBytes(4));
					sectionContent = reader.ReadBytes((int)sectionlength);
					


					split.Add((sectionlength, headerdone, sectionContent));

					if (sectionlength == 2 && sectionContent[0] == 0x00 && sectionContent[1] == 0x00)
						headerdone = true;
				}
			}

			return split;

		}

		public static (string filename, byte[] data, bool istext) parseHpAppSubfile(byte[] content)
		{
			var length = Search(content, new byte[]{ 0x00, 0x00 });
			string name = Conversion.DecodeTextData(content.SubArray(0, length+1));
			var contents = content.SubArray(length + (content.Length > length + 3 ? 3 : 1));

			Console.WriteLine($"parsed filename: {name}");
			if (name.ToLower().EndsWith(".py") || name.ToLower().EndsWith(".txt") || name.ToLower().EndsWith(".ppl"))
				return (name, contents, true);
			else
				return (name, contents, false);
		} 


		private static int Search(byte[] src, byte[] pattern)
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

		public enum contentids : uint
		{
			HpApp = 0x7C61,
			unknown,
		}

	}
}
