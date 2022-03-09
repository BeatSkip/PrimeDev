namespace PrimeWeb.Types
{
	public static class PrimeCommands
	{
		public const byte CHECK_READY		= 0xFF;
		public const byte INFOS				= 0xFA;
		public const byte SCREEN			= 0xFC;
		public const byte BACKUP			= 0xF9;
		public const byte REQ_FILE			= 0xF8;
		public const byte RECV_FILE			= 0xF7;
		public const byte FILE				= 0xF7;
		public const byte CHAT				= 0xF2;
		public const byte TRANSFER_CHAT		= 0xF2;
		public const byte SENDKEY			= 0xEC;
		public const byte SET_DATETIME		= 0xE7;
		public const byte MAX_PROT_VER		= 0xFD;
		public const byte UNKNOWN			= 0x00;

		internal static List<PrimeCommand> ValidRequests = new List<PrimeCommand>()
		{
			INFOS,
			BACKUP,
			CHECK_READY
		};

		internal static Dictionary<PrimeCommand, byte> DefaultParams = new Dictionary<PrimeCommand, byte>
		{
			{CHECK_READY	   , 0x01},
			{INFOS			   , 0x01},
			{SCREEN			   , 0x08},
			{BACKUP			   , 0x01},
			{REQ_FILE		   , 0x01},
			{FILE			   , 0x03},
			{CHAT			   , 0x03},
			{SENDKEY		   , 0x03},
			{SET_DATETIME	   , 0x03},
			{MAX_PROT_VER	   , 0x03},
			{UNKNOWN           , 0x00},
		};

		internal static Dictionary<PrimeCommand, string> AsString = new Dictionary<PrimeCommand, string>
		{
			{CHECK_READY       ,"CHECK_READY"},
			{INFOS             ,"INFOS"},
			{SCREEN            ,"SCREEN"},
			{BACKUP            ,"BACKUP"},
			{REQ_FILE          ,"REQ_FILE"},
			{FILE              ,"FILE"},
			{CHAT              ,"CHAT"},
			{SENDKEY           ,"SENDKEY"},
			{SET_DATETIME      ,"SET_DATETIME"},
			{MAX_PROT_VER      ,"MAX_PROT_VER"},
			{UNKNOWN           ,"UNKNOWN"},
		};

	}

	public readonly struct PrimeCommand
	{
		private readonly byte digit;

		public PrimeCommand(byte digit)
		{
			this.digit = digit;
		}

		public static implicit operator byte(PrimeCommand d) => d.digit;
		public static explicit operator PrimeCommand(byte b) => new PrimeCommand(b);
		public static implicit operator PrimeCommand(int b) => new PrimeCommand((byte)b);

		public override string ToString() => $"{PrimeCommands.AsString[this.digit]}";
	}



}
