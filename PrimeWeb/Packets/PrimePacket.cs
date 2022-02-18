using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimeWeb.Protocol;

namespace PrimeWeb.Packets
{
	public interface IPacket
	{
		Task TransmitNextFrameAsync(FrameWorker worker);
		

		

	}

	public abstract class PrimePacket : IDisposable
	{

		public Dictionary<int, IFrame> Frames { get; protected set; }

		#region Transmission

		protected abstract byte[] GeneratePayload();

		protected void GenerateFrames(int messagenumber)
		{
			var payload = GeneratePayload();

			var blocks = SplitDataBlocks(payload);
			int sequencenumber = 1;

			foreach (var block in blocks)
			{

			}
		}

		protected List<(int sequence, int blockposition, byte[] block)> SplitDataBlocks(byte[] data)
		{
			int sequence = 1;
			int blockindex = 0;
			int bytesToGo = data.Length;
			bool done = false;

			List<(int sequence,int blockposition, byte[] block)> result = new List<(int sequence, int blockposition, byte[] block)>();


			do
			{
				var blocklength = (sequence == 1) ? (bytesToGo > 1015 ? 1015:bytesToGo) : (bytesToGo > 1023 ? 1023 : bytesToGo);
				var framedata = data.SubArray(blockindex, blocklength);
				result.Add(new(sequence, blockindex, framedata));

				bytesToGo -= blocklength;
				blockindex += blocklength;
				sequence++;

			} while (bytesToGo > 0);
	
			return result;		
		}

		#endregion

		#region Reception
		public abstract void CombineFrames();

		public abstract Task ReceiveNextFrameAsync(FrameWorker worker, IFrame packet);
		#endregion

		#region Events

		public event EventHandler<TransmissionEventArgs> TransferDone;

		#endregion


		#region Utility

		public void Dispose()
		{

		}

		#endregion
	}

	public class TransmissionEventArgs : EventArgs
	{
		public PrimePacket result { get; set; }
	}

	public enum TransmissionStatus
	{
		TSuccess,

	}
}
