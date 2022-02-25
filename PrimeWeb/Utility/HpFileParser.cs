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


		public static List<(uint length, uint type, byte[] content)> SplitHpAppDir(byte[] data)
		{
			int index = 0;
			var split = new List<(uint length, uint type, byte[] content)>();

			uint sectionlength;
			byte[] sectionContent;
			uint sectionType;

			using (var ms = new MemoryStream(data))
			using (var reader = new BinaryReader(ms))
			{
				while (reader.BaseStream.Position != reader.BaseStream.Length)
				{
					sectionlength = Conversion.ReadBigEndianBytes(reader.ReadBytes(4));
					sectionContent = reader.ReadBytes((int)sectionlength);
					if (sectionContent.Length > 2)
						sectionType = (uint)sectionContent[0] << 8 | (uint)sectionContent[1];
					else
						sectionType = 0;

					split.Add((sectionlength, sectionType, sectionContent));
				}
			}

			return split;

		}



		public enum contentids : uint
		{
			HpApp = 0x7C61,
			unknown,
		}

	}
}
