using System.Globalization;
using System.Runtime.InteropServices;

namespace PrimeWeb.HpInternal
{

	public struct Packet_HP_Real
	{
	}

	public struct HP_Real
	{
		public ushort EmbeddedRefCount { get; set; }

		public byte TypeFlags { get; set; }

		public byte Type
		{
			get { return (byte)(TypeFlags & 0x0F); }
			set { TypeFlags = (byte)((TypeFlags & 0xF0) & ((byte)(value & 0x0F))); }
		}

		public byte Flags
		{
			get { return (byte)((TypeFlags & 0xF0) >> 4); }
			set { TypeFlags = (byte)((TypeFlags & 0x0F) & ((byte)((value << 4) & 0xF0))); }
		}

		public Sign sign { get; set; }
		public uint Exponent { get; set; }

		public Mantissa mantissa { get; set; }

		public enum Sign : sbyte
		{
			Pos = 1,
			Neg = -1,
			NaN = 0,
			PInf = 2,
			NInf = -2
		}

	}

	[StructLayout(LayoutKind.Explicit)]
	public struct Mantissa
	{
		[FieldOffset(0)]
		public uint m0;

		[FieldOffset(4)]
		public uint m1;

		[FieldOffset(0)]
		public ulong M;
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


			var e = Conversion.GetLittleEndianBytes((uint)exponent - 1).SubArray(0,3);
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
	}
}
