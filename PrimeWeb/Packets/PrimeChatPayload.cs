using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimeWeb.Utility;
using PrimeWeb.Calculator;
using PrimeWeb.Types;

namespace PrimeWeb.Packets
{
	public class PrimeChatPayload : IPayloadGenerator,IPayloadParser
	{
		public Type Type { get { return typeof(PrimeChatPayload);} }
		public string Message { get; set; }

		private bool usecompression = false;
		private bool _autocompress = false;


		public PrimeChatPayload(){}

		public PrimeChatPayload(string message, bool autocompress = false)
		{
			this.Message = message;

			_autocompress = autocompress;
			
		}

		#region Packet Conversion

		public void ParsePayload(byte[] content)
		{
			Console.WriteLine("PrimeChatPacket - Reversing content!");

			bool iscompressed = (content[8] == 0x00 && content[9] == 0x00 && content[10] == 0x78);

			if(iscompressed)
				Message = Conversion.DecompressAndDecodeTextData(content.SubArray(10));
			else
				Message = Conversion.DecodeTextData(content.SubArray(6));

			OnTransferFinalized();
		}

		public byte[] Generate()
		{
			if (Message.Length > 100 && _autocompress)
			{
				usecompression = true;
			}

			List<byte> data = new List<byte>() { (byte)PrimeCMD.TRANSFER_CHAT, 0x03 };

			var content = Conversion.EncodeTextData(Message);

			if (usecompression)
			{
				byte[] bytescontent = Conversion.compress(content);
				Console.WriteLine("content length: " + bytescontent.Length + "original " + content.Length + " message: " + Message.Length);
				content = bytescontent;
			}
				
			
				

			var length = BitConverter.GetBytes(usecompression ? (uint)content.Length + 4 : (uint)content.Length).Reverse();

			data.AddRange(length);

			var crc = new byte[] { 0x00, 0x00 };

			if (usecompression)
			{
				data.AddRange(crc);
				data.AddRange(new byte[] { 0x00, 0x00 });
			}

			data.AddRange(content);
			return data.ToArray();
		}


		async Task OnTransferFinalized()
		{
			Console.WriteLine("PrimeChatPacket - Received!!");

			Console.WriteLine($"Content: {Message}");
		}


		#endregion
	}

	public class ChatPacketEventArgs : EventArgs
	{
		public ConversionStatus status { get; set; }

		public string Message { get; set; }
	}

}
