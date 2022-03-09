namespace PrimeWeb.Calculator;

public static class PrimeDataTypes
{
	public const byte SETTINGS = 0x00;
	public const byte APP = 0x02;
	public const byte LIST = 0x03;
	public const byte MATRIX = 0x04;
	public const byte NOTE = 0x05;
	public const byte PRGM = 0x06;
	public const byte APPNOTE = 0x07;
	public const byte APPPRGM = 0x08;
	public const byte COMPLEX = 0x09;
	public const byte REAL = 0x0A;
	public const byte TESTMODECONFIG = 0x0B;
	public const byte UNKNOWN = 0xFF;

	internal static Dictionary<PrimeDataType, string> AsString = new Dictionary<PrimeDataType, string>
	{
		{SETTINGS ,"SETTINGS"},
		{APP      ,"APP"},
		{LIST     ,"LIST"},
		{MATRIX   ,"MATRIX"},
		{NOTE     ,"NOTE"},
		{PRGM     ,"PRGM"},
		{APPNOTE  ,"APPNOTE"},
		{APPPRGM  ,"APPPRGM"},
		{COMPLEX  ,"COMPLEX"},
		{REAL     ,"REAL"},
		{TESTMODECONFIG,"TESTMODECONFIG"},
		{UNKNOWN,"UNKNOWN"},
	};

}

public readonly struct PrimeDataType
{
	private readonly byte digit;

	public PrimeDataType(byte digit)
	{
		this.digit = digit;
	}

	public static implicit operator byte(PrimeDataType d) => d.digit;
	public static explicit operator PrimeDataType(byte b) => new PrimeDataType(b);
	public static implicit operator PrimeDataType(int b) => new PrimeDataType((byte)b);

	public override string ToString() => $"{PrimeDataTypes.AsString[this.digit]}";
}
