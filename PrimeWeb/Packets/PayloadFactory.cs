using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimeWeb.Protocol;
using PrimeWeb.Types;

namespace PrimeWeb.Packets
{
	public static class PayloadFactory
	{

		public static PrimePacket CreateFromFrame(ContentFrame frame, out IPacketPayload payload)
		{
			switch ((PrimeCMD)frame.Data[0])
			{
				case (PrimeCMD.RECV_BACKUP):
					var backup = new PrimeBackupPayload();
					payload = backup;
					return new PrimePacket(backup, TransferType.Rx);
				case (PrimeCMD.TRANSFER_CHAT):
					var chat = new PrimeChatPayload();
					payload = chat;
					return new PrimePacket(chat, TransferType.Rx);
				case (PrimeCMD.RECV_FILE):
					var file = new PrimeFilePayload();
					payload = file;
					return new PrimePacket(file, TransferType.Rx);

			}

			throw new Exception("no suitable payload type found!");

		}

	}
}
