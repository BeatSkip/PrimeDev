namespace PrimeWeb.Types
{
	public struct HpInfos
	{
		public byte[] Data { get; init; }

		public string Serial { get; init; }

		public string Version { get; init; }

		public int Build { get; init; }

		public string Product { get; set; }

		public ushort ProductID { get; set; }
		public HpInfos(byte[] data)
		{
			this.Data = data;
			ProductID = 0;
			int index_serial = Data.Length - 16;
			int index_version = index_serial - 16;
			int index_build = index_version - 4;

			byte[] BytesSerial = Data.SubArray(index_serial, 10);
			byte[] BytesVersion = Data.SubArray(index_version, 10);
			int version = Data[index_build + 1] << 8 | Data[index_build];

			Serial = Encoding.UTF8.GetString(BytesSerial);
			Version = Encoding.UTF8.GetString(BytesVersion);
			Build = version;
			Product = "";
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
					Product =  "HP Prime G2";
					break;
				default:
					Product = "Unrecognized Calculator";
					break;

			}

		}
	}

}
