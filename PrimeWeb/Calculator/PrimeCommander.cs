using ComponentAce.Compression.Libs.zlib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.Zip.Compression;


namespace PrimeWeb.Calculator
{
	internal static class PrimeCommander
	{

		#region Packet Creation

		public static byte[] GetPayloadMessage(string message)
		{
			var content = UnicodeStringToBytes(message);
			var compressed = compress(content);
			var size = BitConverter.GetBytes((uint)compressed.Length);

			var crc = GetCRC(content);

			Console.WriteLine($"CRC: {crc[1].ToString("X2")} {crc[0].ToString("X2")}");
			var payload = new List<byte> { 0xf2, 0x01, size[3], size[2], size[1], size[0], 0xe8, 0x23, 0x00, 0x00 };

			payload.AddRange(compressed);
			return payload.ToArray();
		}

		public static void TestCRC()
		{
			var data = "7801d559db8ee3360cd573bf221f60f41f8a5e8005daed168bf65d493c332e623b1bcbc17e7ea54352222579b64f05ba0bcc641c89a2c8c3c38b7f75ab7bb8d1cdeee42677779bdbf1f91a9fdff0dd293e9b5c88bf7dfc668c9f86f8f912bf5be23763fc14f074c75a1f77263953fcee127f2eee353e1da3ac24e3fbf8f9aff8d7163f4fee1cf7dcf8bc2dee7cc59a80bd27f712d7ed78e671fa2dfebec427e9bb2756fab822adf3f1e985e56d38e32324dfa0f1297e4efb03b449ebaef1c952c9b4bbe47eaff1c9589d37baafb867ab7392f1295ac1e386239f718715037624db5c602b6be381effb808597bce202ab6e7cebbff13bc41d24e935ae4ce726ff90ff16e8f0475c979e7f61ed47e844ba247949ca80bfe8fee57bb2546d79cf3e7fb8375845bc40ab9fc6bb9fe21a8fdbdfe05bf2968f7b27fe8b2c9b9eddb06f831f76b60b59f1036c30461d46a06ac6ea11964808f38d545abbc1ce24e333beb9c39e57b6d3987d3ec1761eb7bbe04633fcb960c598b53901db498335eb9ba4ff08391b56f8784ac8fe0e2c3f61226164c55f2bfb8322e4c4165b61f9137f227f096ae8e664ad3d629bb45d202960dfa0d611b64768b242ab37a022c94b785c59ef7ef479785322e6026d4472415ddaff0334ff023fccb0ce4bfc3ff18d85032e159b880d7d8c9da4d59ea3857032b2bf3566c9963b3499d8ce1ac1a47100062ed89bee4cde7e4077c1641d0f13bc9cf4a03b26748cff12cb779cfa840ccf91ab9969848f7b5cf3c2983be7f8a3bfdef0a9304fcd6727dc64cbbbc8ba358a67a0ed064f9f70d38963cf72ef84b89959726baf13c7fc0c6e5b7284b46c91e4fd1e3f5f70b28dc6052bd66cf13be330a85b8de61cd2f6cabfef9046de9f724ccc8805c2c9c0a76cd04890f780c52778433c2668fb2efeff00cff79876e2a82cd95073bebe5dbaf72ff874e1bbadf0ce889d6db6d47e269622648fc05f3a8f728fc493ac78859d9fd0cc3363bce25e9e7120ec70cd915e6e23b19764ff8653cef093ceeb1e2c9f344ff2ced092f020d9a79fe3890b85474666a525f3513fa20b263d50b3e79cbb305f240facb8cddab0c5c0b19524cc58452b768e8ea443d27338883ecb62da7f74ee0be42cc87a12892ba35bfb87e2c9dad15afb735c71659609cc2f7bdefb96d1e9ab28231bb60c5357262f1cd19bb2d6c0d21715679bba9b669e82b1a1739a447b3945df2969401152a2b77840e7cd217361bd87ea27a98a42ce836201caae3bd07f5659a5e5c596ff8b5708f71e1a8dcc143aba741e294f4ba45a3eb316d416a931d9561357ac185c5db5da1a4b73449b518b65eafc5ba2ea880bb4577a39c01b89438530a9086bdb8f90ab4faa77da2aaaadef3dc7c7906397228e7038361296cc142dc35abfebe8260cae394fb5dc301ca0f10adfcccc93177e3ee4bb4fb8ed02afcb5aca321fab6c7e01a74cd03be9f79362c23df7180bafd0faf86ff07caf33bbc37a01d82db597ad033ee69bd556b65cd68bc39631a89e9a39436c9943eaf8ecf726fd8e833204e5b9d0c59bae8bdeab6c65f7c41d9f64f8f7ebcfb77c6bafaa0fc9a281cf0e1c3994f747c6b1ed1c166597c29a7537f3c23e2cd6b3b56fdddf589ccd1d9ed5f582f8a8cd403d1fe9f8792adb25e61e1cf534d40b09475d59be9c5ceae1e3deb19c3bf1bd832b95f651fd5dd7bd1bf3705baff66e56583b34362559f341174fd179535668bbe792477cc5d2135747bd9e43f27e7abae6eeb28dbb967526e69e1d98b49960e68c44f5cecf406a5d61b69c6ca3bf8e552bd7ce23bcb1d1b3138b833bee319f4054ca3577d74e7beae9409bd7f58a7a6ad2af62fb5c545bb8f6a3edc48e2bc76030bb713cd15c8c90b21f4c9c5a8e2ff14792dbdaf60c09b437bc3bfb78e2bc3b2ceda165c9c3bdee44e6379635de3be1981b24bf494d410839a10a0bf8291809070c6babcd7602f6be5e2dd67bb14835c6c099e2cc79582a9afa89dc41e76bf1edff734294f28a660bca033737b2ddc517b66ab093ccde84af873c9d5bf47c8d30709cd3a55ab428b51dde310efaf739ee5b03fb96aa423b59a9a3e2cf5c6bd92cabbb0dcfdc5b7342e19ad1856f54273323cb3b3d3beaa3563a565d9bfb3c49b2bd4acf4bb613ec652639a154e87bae73444bebdf1621c57ac4163d9e9bb97eae7b20c1433bc128ba0f86dfec1c5ef7f97a56b221d349ad56b3baeebd49e76f4f50dba90ecdec53d4496dd2d6157d2e9c0edf1ad8de7503fbd014cdf632523dd51542adc1e0f45c46f7d06fdcab056629fdd66470ba8712a6acfba1daae832b355d2f1f975cf5b542de2d47e171dc48ad65d1d876c83dd6e9f9453303759a758dd47b474453c882df7ab25777708573ec89539eb9d90964dba149e4d0c96d2eb4f6900a9750ddde88e6a3bd2e46b051ce1a91a165bd646f99571dbf89b0731ddb1f0eae9e140867d8fec156bd7bd65de2b2d4fb1fb8421174e8c963dd67dae9c28373a9cc6fdeab7205d705c5652244b15ccfe9da9e37f9c6ce107ab30b5b01f72750a52e26248686e1d2ea27747f70af65a70d473d8266003d51af71da4740bf0e2636d1efa44966cbf9650e51a607f66daeedac6dd772e3783cb36f29df5067ef730f7d73a5efaa35e84570411065dd7ef62589940d7e75fff57bf9b6ab92bc5a625838cb22b0f581be2f754965bed99b04ea7c5e2a0179d3a5e718bad3eb5b517b4fbf15e8cdc7edfbffdebcc1f6567422cda68ea7a23a63b5738e1ac9ba0a3eee9e8aa6751cdb6c5fb845571dedc4b4ce79ba132d759c64823a07d87c51e7c1c06f61ca1bae9a9128ee09456bc699f0afe576e1585bc1d5dc54df47d7717534da3ed2735dd276f196aff4dc54572893c2aab6796f8edfabe4a65c0d94f758f41dd5db0f8ec6b6dab2eff282d9abe749e9df3fdae39c6f";
			var bytes = Convert.FromHexString(data.ToUpper());
			var lorem = decompress(bytes);

			Console.WriteLine(BytesToUnicodeString(lorem));
			Console.WriteLine("CRC TESING!");
			//Console.WriteLine($"CRC: {crc[1].ToString("X2")} {crc[0].ToString("X2")}");

		}
		public static byte[] StringToByteArray(string hex)
		{
			return Enumerable.Range(0, hex.Length)
							 .Where(x => x % 2 == 0)
							 .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
							 .ToArray();
		}

		#endregion


		#region data conversion

		private static byte[] UnicodeStringToBytes(string message)
		{
			UnicodeEncoding unicode = new UnicodeEncoding();
			var Preamble = unicode.Preamble.ToArray();

			if (Preamble[0] != 0xFF || Preamble[1] != 0xFE)
			{
				Console.WriteLine("!!-> TEXT ENCODING ERROR, PREAMBLE NOT CORRECT! <-!!");
			}

			return unicode.GetBytes(message);


		}

		private static string BytesToUnicodeString(byte[] message)
		{
			UnicodeEncoding unicode = new UnicodeEncoding();
			var Preamble = unicode.Preamble.ToArray();

			if (Preamble[0] != 0xFF || Preamble[1] != 0xFE)
			{
				Console.WriteLine("!!-> TEXT ENCODING ERROR, PREAMBLE NOT CORRECT! <-!!");
			}

			return unicode.GetString(message);


		}

		private static byte[] compress(byte[] input)
		{
			var deflate = new Deflater(Deflater.NO_COMPRESSION, false);
			deflate.SetInput(input);
			var output = new byte[input.Length];
			deflate.Deflate(output);

			return output;
		}


		private static byte[] decompress(byte[] input)
		{
			var outputStream = new MemoryStream();
			using (var compressedStream = new MemoryStream(input))
			using (var inputStream = new InflaterInputStream(compressedStream))
			{
				inputStream.CopyTo(outputStream);
				outputStream.Position = 0;
				return outputStream.ToArray();
			}
		}

		private static byte[] GetCRC(byte[] data)
		{
			var len = data.Length;
			var index = 0;
			var itbl = 0;
			ushort crc = 0x0000;

			ushort[] crctable =
			{
				0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50a5, 0x60c6, 0x70e7,
				0x8108, 0x9129, 0xa14a, 0xb16b, 0xc18c, 0xd1ad, 0xe1ce, 0xf1ef,
				0x1231, 0x0210, 0x3273, 0x2252, 0x52b5, 0x4294, 0x72f7, 0x62d6,
				0x9339, 0x8318, 0xb37b, 0xa35a, 0xd3bd, 0xc39c, 0xf3ff, 0xe3de,
				0x2462, 0x3443, 0x0420, 0x1401, 0x64e6, 0x74c7, 0x44a4, 0x5485,
				0xa56a, 0xb54b, 0x8528, 0x9509, 0xe5ee, 0xf5cf, 0xc5ac, 0xd58d,
				0x3653, 0x2672, 0x1611, 0x0630, 0x76d7, 0x66f6, 0x5695, 0x46b4,
				0xb75b, 0xa77a, 0x9719, 0x8738, 0xf7df, 0xe7fe, 0xd79d, 0xc7bc,
				0x48c4, 0x58e5, 0x6886, 0x78a7, 0x0840, 0x1861, 0x2802, 0x3823,
				0xc9cc, 0xd9ed, 0xe98e, 0xf9af, 0x8948, 0x9969, 0xa90a, 0xb92b,
				0x5af5, 0x4ad4, 0x7ab7, 0x6a96, 0x1a71, 0x0a50, 0x3a33, 0x2a12,
				0xdbfd, 0xcbdc, 0xfbbf, 0xeb9e, 0x9b79, 0x8b58, 0xbb3b, 0xab1a,
				0x6ca6, 0x7c87, 0x4ce4, 0x5cc5, 0x2c22, 0x3c03, 0x0c60, 0x1c41,
				0xedae, 0xfd8f, 0xcdec, 0xddcd, 0xad2a, 0xbd0b, 0x8d68, 0x9d49,
				0x7e97, 0x6eb6, 0x5ed5, 0x4ef4, 0x3e13, 0x2e32, 0x1e51, 0x0e70,
				0xff9f, 0xefbe, 0xdfdd, 0xcffc, 0xbf1b, 0xaf3a, 0x9f59, 0x8f78,
				0x9188, 0x81a9, 0xb1ca, 0xa1eb, 0xd10c, 0xc12d, 0xf14e, 0xe16f,
				0x1080, 0x00a1, 0x30c2, 0x20e3, 0x5004, 0x4025, 0x7046, 0x6067,
				0x83b9, 0x9398, 0xa3fb, 0xb3da, 0xc33d, 0xd31c, 0xe37f, 0xf35e,
				0x02b1, 0x1290, 0x22f3, 0x32d2, 0x4235, 0x5214, 0x6277, 0x7256,
				0xb5ea, 0xa5cb, 0x95a8, 0x8589, 0xf56e, 0xe54f, 0xd52c, 0xc50d,
				0x34e2, 0x24c3, 0x14a0, 0x0481, 0x7466, 0x6447, 0x5424, 0x4405,
				0xa7db, 0xb7fa, 0x8799, 0x97b8, 0xe75f, 0xf77e, 0xc71d, 0xd73c,
				0x26d3, 0x36f2, 0x0691, 0x16b0, 0x6657, 0x7676, 0x4615, 0x5634,
				0xd94c, 0xc96d, 0xf90e, 0xe92f, 0x99c8, 0x89e9, 0xb98a, 0xa9ab,
				0x5844, 0x4865, 0x7806, 0x6827, 0x18c0, 0x08e1, 0x3882, 0x28a3,
				0xcb7d, 0xdb5c, 0xeb3f, 0xfb1e, 0x8bf9, 0x9bd8, 0xabbb, 0xbb9a,
				0x4a75, 0x5a54, 0x6a37, 0x7a16, 0x0af1, 0x1ad0, 0x2ab3, 0x3a92,
				0xfd2e, 0xed0f, 0xdd6c, 0xcd4d, 0xbdaa, 0xad8b, 0x9de8, 0x8dc9,
				0x7c26, 0x6c07, 0x5c64, 0x4c45, 0x3ca2, 0x2c83, 0x1ce0, 0x0cc1,
				0xef1f, 0xff3e, 0xcf5d, 0xdf7c, 0xaf9b, 0xbfba, 0x8fd9, 0x9ff8,
				0x6e17, 0x7e36, 0x4e55, 0x5e74, 0x2e93, 0x3eb2, 0x0ed1, 0x1ef0
			};

			while (len-- > 0)
			{
				itbl = ((byte)(crc >> 8) ^ data[index++]);
				crc = (ushort)(((uint)crctable[(int)itbl]) ^ ((uint)((ushort)(crc << 8))));
			}

			return new byte[] { (byte)(crc >> 8), (byte)crc };

		}


		#endregion
	}
}
