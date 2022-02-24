using PrimeWeb.Types;
using PrimeWeb.Utility;

namespace PrimeWeb.Packets
{
	//TODO: find alternate implementation with new HpApp format etc
	public class PrimeFilePayload : IPayloadParser, IPayloadGenerator
	{

		public uint FileSize { get; private set; }

		public PrimeFileType FileType { get; private set; }

		public string Filename { get; private set; }

		public byte[] PacketContent { get; set; }

		public PrimeFilePayload() { }



		#region Packet Conversion

		public void ParsePayload(byte[] content)
		{
			Console.WriteLine("File Received!");
			FileSize = ((uint)content[2]) << 24 | ((uint)content[3]) << 16 | ((uint)content[4]) << 8 | (uint)content[5];
			int namelength = content[7];
			this.FileType = (PrimeFileType)content[6];

			Filename = Conversion.DecodeTextData(content.SubArray(10, namelength));

			PacketContent = content.SubArray(10 + namelength);

			OnTransferFinalized();
		}

		public byte[] Generate()
		{
			//var content = Conversion.EncodeTextData("calc.hpsettings");
			//var size = BitConverter.GetBytes((uint)(10 - 6 + content.Length));
			var payload = new List<byte> { (byte)PrimeCMD.REQ_FILE, 0x03 };

			return payload.ToArray();
		}




		async Task OnTransferFinalized()
		{
			Console.WriteLine("Prime file - Received!!");

			OnContentReceived(FileSize, FileType, Filename, PacketContent);
			//Console.WriteLine($"Content: {Message}");
		}


		/// <summary>
		/// Event to indicate Description
		/// </summary>
		public event EventHandler<FilePacketEventArgs> ContentReceived;


		/// <summary>
		/// Called to signal to subscribers that Description
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnContentReceived(uint size, PrimeFileType type, string name, byte[] content)
		{
			var handler = ContentReceived;
			if (handler != null) handler(this, new FilePacketEventArgs() 
			{
				status = ConversionStatus.Success,
				FileSize = size,
				FileType = type,
				Filename = name,
				PacketContent = content
			});
		}

		#endregion
	}

	public class RawContentEventArgs : EventArgs
	{
		public PrimeFileType Type { get; set; }
		public byte[] RawData { get; set; }
	}

	public class FilePacketEventArgs : EventArgs
	{
		public ConversionStatus status { get; set; }

		public uint FileSize { get; set; }

		public PrimeFileType FileType { get; set; }

		public string Filename { get; set; }

		public byte[] PacketContent { get; set; }
	}
}
