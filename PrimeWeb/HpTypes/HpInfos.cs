namespace PrimeWeb.Types
{
	public struct HpInfos
	{
		public byte[] Data { get; private set; } = new byte[0];

		public string Serial { get; private set; } = "";

		public string Version { get; private set; } = "";

		public int Build { get; private set; } = 0;

		public string Product { get; set; } = "";

		public ushort ProductID { get; set; } = 0;

		public HpInfos() { }

		public static HpInfos FromBytes(byte[] data)
		{
			
			int index_serial = data.Length - 16;
			int index_version = index_serial - 16;
			int index_build = index_version - 4;

			byte[] BytesSerial = data.SubArray(index_serial, 10);
			byte[] BytesVersion = data.SubArray(index_version, 10);
			int version = data[index_build + 1] << 8 | data[index_build];

			return new HpInfos()
			{
				ProductID = 0,
				Serial = Encoding.UTF8.GetString(BytesSerial),
				Version = Encoding.UTF8.GetString(BytesVersion),
				Build = version,
				Product = "",
				Data = data,
			};
		}

		public void SetProductId(ushort? pid)
		{
			ProductID = pid ?? 0;
			switch (pid)
			{
				case (0x0441):
				case (0x1541):
					Product = "HP Prime G1";
					break;
				case (0x2441):
					Product = "HP Prime G2";
					break;
				default:
					Product = "Unrecognized Calculator";
					break;

			}

		}
	}

}
