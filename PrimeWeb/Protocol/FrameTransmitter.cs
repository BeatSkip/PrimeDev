using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimeWeb.Packets;

namespace PrimeWeb.Protocol
{
	public class FrameTransmitter
	{
		public int PacketCount { get; private set; } = 1;

		public Dictionary<int, PrimePacket> Packets { get; set; }

		public FrameTransmitter()
		{
			
		}

		public async Task SendPacket(PrimePacket packet)
		{

		}
	}
}
