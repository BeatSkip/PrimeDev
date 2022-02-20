using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimeWeb.Types;
using PrimeWeb.Utility;

namespace PrimeWeb.Packets
{
	public class PrimeBackupPayload : IPacketPayload
	{
		public Type Type { get { return typeof(PrimeBackupPayload); } }

		public string Message { get; set; }

		private bool usecompression = false;

		public PrimeBackupPayload() { }

		public PrimeBackupPayload(string message, bool autocompress = true)
		{
			this.Message = message;

			if (Message.Length > 100 && autocompress)
			{
				usecompression = true;
			}
		}

		#region Packet Conversion

		public void ReversePayload(byte[] content)
		{
			Console.WriteLine("Backup Received!");

			OnTransferFinalized();
		}

		public byte[] GeneratePayload()
		{
			//var content = Conversion.EncodeTextData("calc.hpsettings");
			//var size = BitConverter.GetBytes((uint)(10 - 6 + content.Length));
			var payload = new List<byte> { (byte)PrimeCMD.RECV_BACKUP, 0x03 };

			return payload.ToArray();
		}




		async Task OnTransferFinalized()
		{
			Console.WriteLine("PrimeBackupPacket - Received!!");

			Console.WriteLine($"Content: {Message}");
		}


		#endregion
	}

	public class BackupPacketEventArgs : EventArgs
	{
		public ConversionStatus status { get; set; }

		//public string Message { get; set; }
	}
}
