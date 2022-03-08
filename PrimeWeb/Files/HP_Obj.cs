namespace PrimeWeb.Files
{
	public class HP_Obj
	{
		public HP_Obj(byte[] src)
		{
			Source = src;
		}

		public HP_Obj(int bytecount)
		{
			Source = new byte[bytecount];
		}
		public byte[] Source { get; set; }

		public ushort RefCount 
		{ 
			get { return (byte)((Source[1] << 8) | Source[0]); }
			set { Source[0] = (byte) (value & 0x00FF); Source[1] = (byte)((value & 0xFF00) >> 8);}
		}

		public byte TypeFlags
		{
			get { return Source[2]; }
			set { Source[2] = value; }
		}

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

		public static class Tags
		{
			public const byte REAL = 0;
			public const byte INT = 1;
			public const byte STRING = 2;
			public const byte COMPLEX = 3;
			public const byte MATRIX = 4;
			public const byte ERROR = 5;
			public const byte LIST = 6;
			public const byte IDENT = 7;
			public const byte FUNC_CALL = 8;
			public const byte UNIT = 9;
			public const byte INSTRUCTION_SEQUENCE = 10; // Just a list with special semantics
			public const byte USERFUNC = 11;
			public const byte LIST_PROCESSOR = 12;
			public const byte EVALUATOR_REQUEST = 13;
			public const byte GEN = 14;

			public static Dictionary<PrimeCommand, string> AsString = new Dictionary<PrimeCommand, string>
			{
				{REAL					  , "REAL"},
				{INT					  , "INT"},
				{STRING					  , "STRING"},
				{COMPLEX				  , "COMPLEX"},
				{MATRIX					  , "MATRIX"},
				{ERROR					  , "ERROR"},
				{LIST					  , "LIST"},
				{IDENT					  , "IDENT"},
				{FUNC_CALL				  , "FUNC_CALL"},
				{UNIT					  , "UNIT"},
				{INSTRUCTION_SEQUENCE	  , "INSTRUCTION_SEQUENCE"},
				{USERFUNC				  , "USERFUNC"},
				{LIST_PROCESSOR			  , "LIST_PROCESSOR"},
				{EVALUATOR_REQUEST		  , "EVALUATOR_REQUEST"},
				{GEN                      , "GEN"}
			};

		}
	}

	public readonly struct ObjTag
	{
		private readonly byte value;

		public ObjTag(byte val)
		{
			this.value = val;
		}

		public static implicit operator byte(ObjTag d) => d.value;
		public static explicit operator ObjTag(byte b) => new ObjTag(b);
		public static implicit operator ObjTag(int b) => new ObjTag((byte)b);

		public override string ToString() => $"{HP_Obj.Tags.AsString[this.value]}";
	}


	

	public enum Sign : sbyte
	{
		Pos = 1,
		Neg = -1,
		NaN = 0,
		PInf = 2,
		NInf = -2
	}

}


