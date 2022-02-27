namespace PrimeWeb.Types;

public class HpApp
{
	private bool IsSystemApp;

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
	public HpApp(byte[] data)
	{
		rawdata = data;
		ParseByteData(data);
		//DbgTools.PrintPacket(HpAppcontent);
		this.SvgIcon = HpIcons.GetIcon(HpAppcontent[20]);
	}

	private void ParseByteData(byte[] sourcedata)
	{
		var header = HpFileParser.ParseFileHeader(sourcedata);
		this.contents = header.Contents;
		this.Name = header.Name;
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



}

