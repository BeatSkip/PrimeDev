﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Types
{
	public struct HpInfos
	{
		public byte[] Data { get; init; }

		public string Serial { get; init; }

		public string Version { get; init; }

		public int Build { get; init; }

		public string Product { get; set; }

		public HpInfos(byte[] data)
		{
			this.Data = data;

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
	}

}