using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimeWeb.Protocol;
using PrimeWeb.Types;

namespace PrimeWeb.Packets
{

	public abstract class PrimePacket : IDisposable
	{
		public TransferType Direction { get; protected set; }

		public bool Initialized { get; protected set; }

		protected uint MessageNumber { get { return Frames[1].IOMessageCounter; } }

		protected uint MessageSize { get { return Frames[1].IOMessageSize; } }

		protected int sequencecounter = 1;

		protected uint BlockPosition = 0;

		protected bool TransmissionComplete = false;

		public Dictionary<int, ContentFrame> Frames { get; protected set; }

		public void Initialize(int Messagenumber)
		{
			if(this.Direction == TransferType.Tx)
			{
				GenerateFrames(Messagenumber);
			}
			else
			{
				Console.WriteLine("Initialization for RX not needed, automatically done with reception of first packet");
			}	
		}

		#region Transmission

		protected abstract byte[] GeneratePayload();

		protected void GenerateFrames(int messagenumber)
		{
			Frames = new Dictionary<int, ContentFrame>();

			var payload = GeneratePayload();

			var blocks = SplitDataBlocks(payload);

			int sequencenumber = 1;

			foreach (var block in blocks)
			{
				var frame = new ContentFrame(sequencenumber, block.block, messagenumber);
				sequencenumber++;

				if (sequencenumber == 254)
					sequencenumber = 2;

				Frames.Add(block.sequence, frame);
			}
		}

		protected List<(int sequence, int blockposition, byte[] block)> SplitDataBlocks(byte[] data)
		{
			int sequence = 1;
			int blockindex = 0;
			int bytesToGo = data.Length;
			bool done = false;

			List<(int sequence, int blockposition, byte[] block)> result = new List<(int sequence, int blockposition, byte[] block)>();


			do
			{
				var blocklength = (sequence == 1) ? (bytesToGo > 1015 ? 1015 : bytesToGo) : (bytesToGo > 1023 ? 1023 : bytesToGo);
				var framedata = data.SubArray(blockindex, blocklength);
				result.Add(new(sequence, blockindex, framedata));

				bytesToGo -= blocklength;
				blockindex += blocklength;
				sequence++;

			} while (bytesToGo > 0);

			return result;
		}

		public async Task<bool> TransmitNextFrame(FrameWorker worker)
		{

			if (!Frames.ContainsKey(sequencecounter))
				throw new Exception("Error frame to resend is not available in Packet!");

			await worker.SendFrame(Frames[sequencecounter]);

			sequencecounter++;

			if(Frames.Count < sequencecounter)
			{
				Console.WriteLine("Last Frame is transmitted!");
				//TODO: handle transmission done event handling
				//TransmissionComplete = true;

			}

			return TransmissionComplete;
		}

		#endregion

		#region Reception

		protected abstract void ReversePayload(byte[] payload);

		protected void ReverseFrames()
		{
			var firstframe = Frames[1] as ContentFrame;

			if (firstframe == null)
				Console.WriteLine("ERROR while reversing Frames!");

			var buffersize = firstframe.PayloadSize;

			var bufferpos = 0;

			var completebuffer = new byte[buffersize];

			foreach (var block in Frames)
			{
				var slice = block.Value.GetFrameBytes();
				Array.Copy(slice, 0, completebuffer, bufferpos, slice.Length);
				bufferpos += slice.Length;
			}

			ReversePayload(completebuffer);
		}

		protected AckFrame ReceiveContentFrame(ContentFrame frame)
		{
			if (sequencecounter < frame.Sequence)
			{
				return GenerateAck(sequencecounter, BlockPosition, ack: false);
			}
			else
			{
				this.Frames.Add(sequencecounter, frame);
				var ackpkt = GenerateAck(sequencecounter, BlockPosition);
				BlockPosition += (uint)frame.BlockLength;
				sequencecounter++;

				if (BlockPosition == this.MessageSize)
				{
					Console.WriteLine("Message Transfer done!");
					//TODO: message transfer done event handling!
					TransmissionComplete = true;
				}
				return ackpkt;
			}
		}


		public async Task ReceiveNextFrame(FrameWorker worker, IFrame packet)
		{
			if (!packet.IsValid)
				return;

			if (packet.Type == FrameType.Content)
			{
				var ack = ReceiveContentFrame(packet as ContentFrame);
				await worker.SendFrame(ack);

				
			}
			else
			{
				var ack = packet as AckFrame;

				if (!ack.IsValid)
					return;

				if (!ack.IsAck)
				{
					await ResendFrame(worker, ack.SequenceToResend, ack.BlockPosition);
				}
				else
				{
					if (ack.SequenceToResend == Frames.Count)
						this.TransmissionComplete = true;
				}

			}

			if (TransmissionComplete)
				await Convert();
		}
		#endregion

		#region Events

		private async Task Convert()
		{
			this.ReverseFrames();
			await OnTransferFinalized();
		}

		internal event EventHandler OnRemovalFromBufferPossible;


		protected abstract Task OnTransferFinalized();

		#endregion


		#region Utility

		protected AckFrame GenerateAck(int sequence, uint blockpos, bool ack = true)
		{
			return new AckFrame(ack, sequence, this.MessageNumber, blockpos);
		}

		protected async Task ResendFrame(FrameWorker worker, int sequence, uint blockpos)
		{
			this.sequencecounter = sequence + 1;
			this.BlockPosition = blockpos;

			if (!Frames.ContainsKey(sequence))
				throw new Exception("Error frame to resend is not available in Packet!");

			await worker.SendFrame(Frames[sequence]);

		}

		public void Dispose()
		{

		}

		#endregion
	}

	public class TransmissionEventArgs : EventArgs
	{
		public PrimePacket result { get; set; }
	}

	public enum ConversionStatus
	{
		Success,
		CompressionFail,
		DecompressionFail,
		Fail

	}
}
