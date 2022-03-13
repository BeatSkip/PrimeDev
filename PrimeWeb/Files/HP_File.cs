using System.Diagnostics;
namespace PrimeWeb.Files
{
	internal class HP_File
	{

		//byte[] data;

		public PrimeDataType Type { get; set; }
		public uint Length { get; set; }
		public string Name { get; set; }
		public byte[] CRC { get; set; }
		public byte[] Content { get; set; }

		public HP_File(byte[] data)
		{
			HandleContents(HandleCompression(data));
		}

		private static byte[] HandleCompression(byte[] payload)
		{

			bool iscompressed = (payload[8] == 0x00 && payload[9] == 0x00 && payload[10] == 0x78);

			byte[] data;

			if (iscompressed)
			{
				data = HpFileParser.DecompressFileStream(payload.SubArray(10));
				//DbgTools.PrintPacket(data, title: "raw uncompressed dump");
			}
			else
				data = payload;


			return data.SubArray(2);
		}

		private void HandleContents(byte[] data)
		{
			Console.WriteLine($"lenght: {data.Length}");
			using (var ms = new MemoryStream(data))
			using (var reader = new BinaryReader(ms))
			{

				var length = Conversion.ReadBigEndianBytes(reader);
				Console.WriteLine($"lenght: {data.Length} - parsed len: {length}");
				this.Type = reader.ReadByte();
				var namelen = (int)reader.ReadByte();

				this.CRC = reader.ReadBytes(2);
				if (namelen > 0)
					this.Name = reader.ReadUnicodeString(namelen);

				Content = reader.ReadBytes((int)length - 4 - namelen);

				Console.WriteLine($"[HP_FILE] Created file: {Name}. Content type: {Type.ToString()}");
			}
		}
	}

	internal static class HpFileReader
	{

	} 
}
