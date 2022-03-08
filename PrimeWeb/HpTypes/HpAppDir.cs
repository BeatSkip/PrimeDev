namespace PrimeWeb.Types;

public class HpAppDir : IPayloadGenerator
{
	public bool IsSystemApp { get; set; }

	public BaseHpApp BaseApp {get; set;}

	public byte[] HpAppcontent { get; set; }
	public string HpAppnote { get; set; }

	public string Name { get; set; }
	public int Appsize { get; private set; }
	public Dictionary<string, byte[]> Files { get; set; } = new Dictionary<string, byte[]>();
	public List<byte[]> sections { get; set; } = new List<byte[]>();

	private byte[] contents;

	public string SvgIcon { get; private set; }

	public byte[] Content { get { return this.contents; } set { this.contents = value; } }

	private byte[] rawdata;

	private byte[] headerbytes;

	public HpAppDir(byte[] data)
	{
		rawdata = data;
		ParseByteData(data);
		//DbgTools.PrintPacket(HpAppcontent);
		this.SvgIcon = HpIcons.GetIcon(HpAppcontent[20]);
		this.BaseApp = (BaseHpApp)HpAppcontent[20];
	}

	private void ParseByteData(byte[] sourcedata)
	{
		var header = HpFileParser.ParseFileHeader(sourcedata);
		this.contents = header.Contents;
		this.Name = header.Name;

		if(this.Name[0] == '&')
		{
			this.IsSystemApp = true;
			Name = Name.Substring(1);
		}

		this.Appsize = (int)header.Size;


		using (var ms = new MemoryStream(contents))
		using (var reader = new BinaryReader(ms))
		{
			var hpapplength = (int)Conversion.ReadBigEndianBytes(reader.ReadBytes(4));
			this.HpAppcontent = reader.ReadBytes(hpapplength);

			var hpnotelength = (int)Conversion.ReadBigEndianBytes(reader.ReadBytes(4));
			if (hpnotelength > 1)
				this.HpAppnote = Conversion.DecodeTextData(reader.ReadBytes(hpnotelength));

			var splitterlength = (int)Conversion.ReadBigEndianBytes(reader.ReadBytes(4));
			var splittercontent = reader.ReadBytes(splitterlength);



			while (reader.BaseStream.Position != reader.BaseStream.Length)
			{
				var sectionlength = Conversion.ReadBigEndianBytes(reader.ReadBytes(4));
				var section = reader.ReadBytes((int)sectionlength);
				if (!(sectionlength == 2 && section.Length == 2 && section[0] == 0x00 && section[1] == 0x00))//the rest of the program is just regular files
					sections.Add(section);
			}

		}

		if ((BaseHpApp)HpAppcontent[20] == BaseHpApp.Finance)
		{
			Console.WriteLine("Finance app dump!");
			//DbgTools.PrintPacket(contents);
		}

		Console.WriteLine($"finalized {this.Name} app files, reading files");
		var tmpfiles = ParseSections(sections);

		foreach (var item in tmpfiles)
		{
			if (this.Files.ContainsKey(item.filename))
				this.Files[item.filename] = item.content;
			else
				this.Files.Add(item.filename, item.content);
		}

		Console.WriteLine($"{this.Name} parsing done! result is: {Files.Count}");

		if (this.Files.Count == 0)
		{
			Console.WriteLine($"section count is: {sections.Count}");
			for (int i = 0; i < sections.Count; i++)
			{
				Console.WriteLine($"[{this.Name}] - dumping section: {i}");
				//DbgTools.PrintPacket(sections[i]);
			}
			if (sections.Count == 0)
				Console.WriteLine($"Lost bytes: {(this.contents.Length - this.HpAppcontent.Length)}");
		}

	}

	
	private static List<(string filename, byte[] content)> ParseSections(List<byte[]> sections)
	{
		var files = new List<(string filename, byte[] content)>();

		foreach (var item in sections)
		{
			var section = convertbinaryfile(item);
			files.Add((section.filename, section.data));
		}

		return files;
	}

	private static (string filename, byte[] data) convertbinaryfile(byte[] content)
	{
		var length = Search(content, new byte[] { 0x00, 0x00 });
		string name = Conversion.DecodeTextData(content.SubArray(0, length + 1));
		var contents = content.SubArray(length + (content.Length > length + 3 ? 3 : 1));

		Console.WriteLine($"parsed filename: {name}");

		return (name, contents);
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


	public byte[] Generate()
	{
		return createPayloadData();
	}
	public byte[] createPayloadData()
	{
		
		var crc = new byte[] { 0x00, 0x00 };
		var appdata = PackageAppData();
		var newlenght = Conversion.GetBigEndianBytes((uint)(appdata.Length-4));
		appdata[0] = newlenght[0];
		appdata[1] = newlenght[1];
		appdata[2] = newlenght[2];
		appdata[3] = newlenght[3];

		//DbgTools.PrintPacket(appdata, title: "created packet!");

		var compressed = Conversion.compress(appdata);
		var length = Conversion.GetBigEndianBytes((uint)compressed.Length + 4);

		using (var ms = new MemoryStream())
		using (var writer = new BinaryWriter(ms))
		{
			writer.Write((byte)0xF7);
			writer.Write((byte)0x03);
			//writer.Write(cmdheader);
			writer.Write(length);
			writer.Write(crc);
			writer.Write(crc);
			writer.Write(compressed);
			//writer.Write(appdata);

			var tocheck = ms.ToArray();
			var crcdata = CrcChecker.crc16_block(tocheck, (uint)(compressed.Length + 4));

			tocheck[6] = (byte)crcdata;
			tocheck[7] = (byte)(crcdata >> 8 & 0xFF);

			return tocheck;
		}
		Console.WriteLine("app payload has been created!");

	}

	private byte[] PackageAppData()
	{
		using(var ms = new MemoryStream())
		using(var writer = new BinaryWriter(ms))
		{
			writer.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 });
			writer.Write((byte)PrimeFileType.APP);
			var namebytes = Conversion.EncodeTextData(this.IsSystemApp ? $"&{this.Name}" : this.Name);
			writer.Write((byte)namebytes.Length);
			writer.Write(namebytes);
			var headerlengthbytes = Conversion.GetBigEndianBytes((uint)HpAppcontent.Length);
			writer.Write(headerlengthbytes);
			writer.Write(HpAppcontent);

			var notedata = Conversion.EncodeTextData(HpAppnote);
			var notelengthbytes = Conversion.GetBigEndianBytes((uint)notedata.Length);
			writer.Write(notelengthbytes);
			writer.Write(notedata);

			var filescontent = packageAppFiles();
			writer.Write(filescontent);
			return ms.ToArray();
		}
	}

	private byte[] packageAppFiles()
	{
		var filesstart = new byte[] { 0x00, 0x00, 0x00, 0x02, 0x00, 0x00 };
		var splitter = new byte[] { 0x00, 0x00 };
		using (var ms = new MemoryStream())
		using (var writer = new BinaryWriter(ms))
		{
			writer.Write(filesstart);
			foreach (var file in Files)
			{
				var namebytes = Conversion.EncodeTextData(file.Key);

				var length = Conversion.GetBigEndianBytes((uint)(namebytes.Length + 2 + file.Value.Length));
				writer.Write(length);
				writer.Write(namebytes);
				writer.Write(splitter);
				writer.Write(file.Value);
			}

			return ms.ToArray();
		}
	}


}

