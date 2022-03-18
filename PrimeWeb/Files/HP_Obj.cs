namespace PrimeWeb.Files
{
	public class HP_Obj
	{
		public HP_Obj(byte[] src)
		{
			Source = src;
		}

		public HP_Obj(BinaryReader reader)
		{
			var src = new List<byte>();
			if(reader.BytesToGo() < 4)
			{
				Console.WriteLine($"wanted to read object, but only {reader.BytesToGo()} bytes left!");
				return;
			}
			src.AddRange(reader.ReadBytes(4));
			Source = src.ToArray();
			Console.WriteLine($"loaded HP_Obj with reader! type: {(this.Type)}");
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

		public ObjTag Type
		{
			get { return (byte)(TypeFlags & 0x0F); }
			set { TypeFlags = (byte)((TypeFlags & 0xF0) & ((byte)(value & 0x0F))); }
		}

		public byte Flags
		{
			get { return (byte)((TypeFlags & 0xF0) >> 4); }
			set { TypeFlags = (byte)((TypeFlags & 0x0F) & ((byte)((value << 4) & 0xF0))); }
		}

		public static HP_Obj ReadObject(byte[] data)
		{
			using(var ms = new MemoryStream(data))
			using(var reader = new BinaryReader(ms))
			{
				return ReadObject(reader);
			}

		}


		public static HP_Obj ReadObject(BinaryReader reader)
		{
			var obj = new HP_Obj(reader);
			Console.WriteLine($"read");
			switch ((byte)obj.Type)
			{
				case Tags.LIST:
					return new HP_List(obj.Source, reader);
					
				case Tags.REAL:
					
					return new HP_Real(obj.Source, reader);
					
				default:
					throw new Exception($"Objectreader type note expected! {obj.Type}");
					return null;
			}
			
		}

		//public static HP_List ReadList(byte[] data)
		//{
		//	using (var ms = new MemoryStream(data))
		//	using (var reader = new BinaryReader(ms))
		//	{
		//		var obj = new HP_Obj(reader);
		//		return new HP_List(obj.Source, reader);
		//	}
		//
		//}





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


