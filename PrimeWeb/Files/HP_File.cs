using System.Diagnostics;
namespace PrimeWeb.Files
{
	public class HP_File
	{

		//byte[] data;

		public PrimeDataType Type { get; set; }
		public uint Length { get { return (uint)((Content as byte[]).Length + 4 + Name.Length); } }
		public string Name { get; set; }
		public byte[] CRC { get; set; }
		public object Content { get; set; }

		protected HP_File() { }

		public HP_File(byte[] data)
		{
			HandleContents(HandleCompression(data));
		}

		protected byte[] Serialize()
		{
			using (var ms = new MemoryStream())
			using (var writer = new BinaryWriter(ms))
			{
				writer.Write(Conversion.GetBigEndianBytes(Length));

				writer.Write((byte)Type);
				writer.Write((byte)(Name.Length * 2));
				writer.Write(CRC);
				writer.Write(Conversion.EncodeTextData(Name));
				writer.Write(Content as byte[]);
				return ms.ToArray();
			}
		}

		public static implicit operator byte[](HP_File d) => d.Serialize();

		private static byte[] HandleCompression(byte[] payload)
		{
			var pattern = new byte[] { 0x00, 0x00, 0x78 };
			bool hasf7 = payload[0] == 0xF7;

			bool iscompressed = Utilities.Search(payload, pattern, 12) != -1;

			byte[] data;

			if (iscompressed)
			{
				data = HpFileParser.DecompressFileContent(payload.SubArray(hasf7 ? 10 : 8));
			}
			else
				data = hasf7 ? payload.SubArray(2) : payload;

			return data;
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
}
