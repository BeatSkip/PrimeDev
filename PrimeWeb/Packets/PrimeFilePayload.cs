using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimeWeb.Packets;
using PrimeWeb.Utility;
using PrimeWeb.Types;

namespace PrimeWeb.Packets
{

	public class PrimeFilePayload : IPacketPayload
	{
		public Type Type { get { return typeof(PrimeFilePayload); } }

		public string Message { get; set; }

		private bool usecompression = false;

		public PrimeFilePayload() { }

		public PrimeFilePayload(string Filename, bool autocompress = true)
		{
			

			if (Message.Length > 100 && autocompress)
			{
				usecompression = true;
			}
		}

		#region Packet Conversion

		public void ReversePayload(byte[] content)
		{
			Console.WriteLine("File Received!");

			OnTransferFinalized();
		}

		public byte[] GeneratePayload()
		{
			//var content = Conversion.EncodeTextData("calc.hpsettings");
			//var size = BitConverter.GetBytes((uint)(10 - 6 + content.Length));
			var payload = new List<byte> { (byte)PrimeCMD.REQ_FILE, 0x03 };

			return payload.ToArray();
		}




		async Task OnTransferFinalized()
		{
			Console.WriteLine("PrimeBackupPacket - Received!!");

			Console.WriteLine($"Content: {Message}");
		}


		#endregion
	}

	public class FilePacketEventArgs : EventArgs
	{
		public ConversionStatus status { get; set; }

		//public string Message { get; set; }
	}
}
