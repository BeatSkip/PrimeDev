namespace PrimeWeb.Utility
{
	public static class HpSettingsBuilder
	{
		public static (string name, int bytesread, int length) GetFileName(byte[] data)
		{
			string name = "";
			using (var ms = new MemoryStream(data))
			using (var reader = new BinaryReader(ms))
			{
				reader.ReadBytes(2);
				var length = Conversion.ReadBigEndianBytes(reader);
				reader.ReadByte();
				var namelength = (int)reader.ReadByte();

				var crc = reader.ReadBytes(2);
				return (Conversion.DecodeTextData(reader, namelength),(int)reader.BaseStream.Position, (int)length);
			}
		}

		



	}

	public enum HpSettingsType
	{
		CalcSettings,
		CasSettings,
		MainSettings,
		HpVars
	}
}
