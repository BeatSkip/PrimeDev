using MiniBinaryParser;
using System.Globalization;
using System.Runtime.InteropServices;

namespace PrimeWeb.Files
{

	public class HP_Real : HP_Obj
	{

		private uint marker;

		public HP_Real(byte[] data) : base(data) { }
		public HP_Real() : base(12) { base.Type = Tags.REAL; }

		public HP_Real(BinaryReader reader) : base(12)
		{
			this.Source = reader.ReadBytes(12);
			
			Parse();
			Console.WriteLine($"read HP_Real with value: {this.Value}");
		
			
		}

		public byte[] unpacked { get; private set; }

		public sbyte Sign { get; set; }
		public int Exponent { get; set; }
		public ulong mantissa { get; set; }
		public double Value { get; set; }

		public static HP_Real FromBytes(byte[] bytes)
		{
			var result = new HP_Real();
			
			return result;
		}

		private void ParseExpanded()
		{
			var srcexp = new byte[4] { base.Source[7], base.Source[6], base.Source[5], base.Source[4] };
			this.Exponent = BitConverter.ToInt32(srcexp);
			ulong data = Conversion.ReadLittleEndianULong(base.Source, 8);
			var bcd = Binary.GetBCD(data);
			var realbcd = Binary.HpBCD(data);
			//this.Exponent = (uint)(data & (ulong)0x0000000000000FFF);
			this.Sign = (sbyte)(data >> 60 & (ulong)0x000000000000000F);

			Value = realbcd * Math.Pow(10, ((int)Exponent) * Sign);
			if (bcd[0] != '0')
				Console.WriteLine($"bcd: {bcd}\texponent: {Exponent}\tsign: {Sign.ToString("X")}\tRef: {base.Flags.ToString("X2")}\tnumber: {base.RefCount}\tValue: {realbcd.ToString()}");

		}

		private void Parse()
		{
			ulong data = Conversion.ReadLittleEndianULong(base.Source, 4);
			var bcd = Binary.GetBCD(data);
			var realbcd = Binary.HpBCD(data);
			this.Exponent = (int)(data & (ulong)0x0000000000000FFF);
			this.Sign = (sbyte)(data >> 60 & (ulong)0x000000000000000F);

			Value = realbcd * Math.Pow(10, ((int)Exponent)*Sign);
			if(bcd[0] != '0')
				Console.WriteLine($"bcd: {bcd}\texponent: {Exponent}\tsign: {Sign.ToString("X")}\tRef: {base.Flags.ToString("X2")}\tnumber: {base.RefCount}\tValue: {realbcd.ToString()}");

		}

	}

	public static class hpmath
	{
		public enum Sign : sbyte
		{
			Pos = 1,
			Neg = -1,
			NaN = 0,
			PInf = 2,
			NInf = -2
		}

		public static byte[] stringtoHpReal(string input, ushort reference)
		{
			byte flags = 0x0A;
			byte type = 0x0B;

			var abs = input.Replace("-", "");
			var mantissa = abs.Replace(".", "");
			var exponent = abs.IndexOf(".");
			var sign = input.Contains("-") ? hpmath.Sign.Neg : Sign.Pos;


			Console.WriteLine("decoded real:");
			Console.WriteLine($"sign: {sign.ToString()}");

			byte TypeFlags = 0x00;

			TypeFlags = (byte)((TypeFlags & 0xF0) & ((byte)(flags & 0x0F))); //flags
			TypeFlags = (byte)((TypeFlags & 0x0F) & ((byte)((type << 4) & 0xF0)));

			bool up = false;
			int byteindex = 0;
			byte[] m = new byte[8];

			m[0] = 0x00;


			string result = "";
			string mtmp = "";


			var e = Conversion.GetLittleEndianBytes((uint)exponent - 1).SubArray(0, 3);
			result += BitConverter.ToString(e).Replace("-", "");

			switch (sign)
			{
				case Sign.Pos:
					mtmp += "0";
					break;
				case Sign.Neg:
					mtmp += "9";
					break;
				default:
					mtmp += (((int)sign) + 4).ToString();
					break;
			}

			for (int i = 0; i < 11; i++)
			{

				up = !up;
				mtmp += (i < mantissa.Length ? ((int)mantissa[i] - 48) : 0).ToString();
			}
			Console.WriteLine("");



			var tmp = ConvertHexStringToByteArray(mtmp);
			result += BitConverter.ToString(tmp.Reverse().ToArray()).Replace("-", "");
			var interim = ConvertHexStringToByteArray(result);
			Console.WriteLine($"BCD: { BitConverter.ToString(interim)}");


			return interim;

		}

		private static byte[] ConvertHexStringToByteArray(string hexString)
		{
			if (hexString.Length % 2 != 0)
			{
				throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
			}

			byte[] data = new byte[hexString.Length / 2];
			for (int index = 0; index < data.Length; index++)
			{
				string byteValue = hexString.Substring(index * 2, 2);
				data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			}

			return data;
		}

		private static void ParseBytes(byte[] data)
		{
			
		}
	}
}
