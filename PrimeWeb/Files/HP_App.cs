using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Files
{
	public class HP_App : HP_File
	{
		public string Name { get; set; }
		public BaseHpApp BaseApp { get; set; }
		public bool IsSystemApp { get; set; }
		public byte[] HpAppcontent { get; set; }

		public string Note { get; set; }
		public int Appsize { get; private set; }
		public Dictionary<string, byte[]> Files { get; set; } = new Dictionary<string, byte[]>();
		public Dictionary<string, HP_Obj> Parameters { get; set; } = new Dictionary<string, HP_Obj>();

		public string SvgIcon { get; private set; }

		public HP_App(byte[] data) : base(data)
		{
			ParseByteData(base.Content as byte[]);
		}

		private void ParseByteData(byte[] sourcedata)
		{

			using (var ms = new MemoryStream(sourcedata))
			using (var reader = new BinaryReader(ms))
			{
				//var hpapplengtha = (int)Conversion.ReadBigEndianBytes(reader.ReadBytes(4));
				var hpapplength = (int)Conversion.ReadBigEndianBytes(reader.ReadBytes(4));
				Console.WriteLine($"ParsedHpapplength: {hpapplength}");
				this.HpAppcontent = reader.ReadBytes(hpapplength);

				//var testID = new StructId(reader.ReadBytes(4));

				//var hpnotelength = (int)Conversion.ReadBigEndianBytes(reader.ReadBytes(4));
				//if (hpnotelength > 1)
					//this.Note = Conversion.DecodeTextData(reader.ReadBytes(hpnotelength));

				//var splitterlength = (int)Conversion.ReadBigEndianBytes(reader.ReadBytes(4));
				//var splittercontent = reader.ReadBytes(splitterlength);

			}

			BaseApp = (BaseHpApp)HpAppcontent[20];
			int i = 0;
			if (BaseApp == BaseHpApp.Finance)
			{
				//Console.WriteLine($"spaghetti method finance!");
				//using (var ms = new MemoryStream(HpAppcontent))
				//using (var reader = new BinaryReader(ms))
				//{
				//	while (reader.BaseStream.Position < reader.BaseStream.Length - 1)
				//	{
				//		var testID = new StructId(reader.ReadBytes(4));
				//		
				//		//if (testID.Member != 0)
				//		//	Console.WriteLine($"TestID\t{reader.BaseStream.Position}\ttype:\t{testID.Type}\ttypemodifiers:\t{testID.TypeModifiers}\tcompressed:\t{testID.Compressed.ToString()}\tTypeId\t{testID.TypeId}\tmember:\t{testID.Member}");
				//		
				//		i++;
				//	}
				//}
				//Console.WriteLine($"spaghetti end \n");
			}
		} 
	}

	public class StructId {
		public uint Contents { get; private set; }

		public byte Type { get { return (byte)((Contents & 0xF)); } }
		public byte TypeModifiers { get { return (byte)((Contents & 0x30) >> 4); } }
		public bool Compressed { get { return ((Contents & 0x40) >> 6) != 0; } }
		public ushort TypeId { get { return (ushort)((Contents & 0xffc00000) >> 22); } }
		public ushort Member { get { return (ushort)((Contents & 0x3fff80) >> 7); } }

		public StructId(byte[] data) {
			Contents = Conversion.ReadLittleEndianBytes(data);

		}
	}
}
