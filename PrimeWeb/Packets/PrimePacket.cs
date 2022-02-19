using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimeWeb.Protocol;
using PrimeWeb.Types;

namespace PrimeWeb.Packets
{
	public interface IPacketPayload
	{
		public void ReversePayload(byte[] payload);
		public byte[] GeneratePayload();
		public Type Type { get; }

	}

	public class PrimePacket : IDisposable
	{
		public PrimePacket(IPacketPayload parent, TransferType dir = TransferType.Tx) {

			payload = parent;
			this.Direction = dir;

		}

		public IPacketPayload GetPayload()
		{
			return this.payload;
		}

		private IPacketPayload payload;

		public TransferType Direction { get; set; }

		public bool Initialized { get; protected set; }

		public uint MessageNumber { get { return Frames[1].IOMessageCounter; } }

		public uint MessageSize { get { return Frames[1].IOMessageSize; } }

		protected int sequencecounter = 1;

		protected uint BlockPosition = 0;

		protected bool TransmissionComplete = false;

		protected int bytestogo = 0;

		public Dictionary<uint, ContentFrame> Frames { get; protected set; } = new Dictionary<uint, ContentFrame>();

		public void Initialize(uint Messagenumber)
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

		

		protected void GenerateFrames(uint messagenumber)
		{
			Frames = new Dictionary<uint, ContentFrame>();

			var datad = payload.GeneratePayload();

			var blocks = SplitDataBlocks(datad);

			int sequencenumber = 1;

			foreach (var block in blocks)
			{
				var frame = new ContentFrame(sequencenumber, block.block, messagenumber);
				sequencenumber++;

				if (sequencenumber == 254)
					sequencenumber = 2;

				Frames.Add((uint)block.sequence, frame);
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

			if (!Frames.ContainsKey((uint)sequencecounter))
				throw new Exception("Error frame to resend is not available in Packet!");

			await worker.SendFrame(Frames[(uint)sequencecounter]);

			sequencecounter++;

			if(Frames.Count < sequencecounter)
			{
				Console.WriteLine("Last Frame is transmitted!");
				//TODO: handle transmission done event handling
				//TransmissionComplete = true;
				return true;
			}

			return false;
		}

		#endregion

		#region Reception

		

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

			payload.ReversePayload(completebuffer);

			PayloadCompleted(this.payload, ConversionStatus.Success);
		}

		protected AckFrame ReceiveContentFrame(ContentFrame frame)
		{
			if (sequencecounter < frame.Sequence)
			{
				return GenerateAck(sequencecounter, BlockPosition, ack: false);
			}
			else
			{
				this.Frames.Add((uint)sequencecounter, frame);
				var ackpkt = GenerateAck(sequencecounter, BlockPosition);
				BlockPosition += (uint)frame.BlockLength;

				if (frame.IsStartFrame)
				{
					if (frame.IOMessageSize < 1015)
						bytestogo = 0;
					else
						bytestogo = bytestogo = (int)frame.IOMessageSize - 1015;
				}
				else
				{
					if (bytestogo < 1023)
						bytestogo = 0;
					else
						bytestogo -= 1023;
				}

				sequencecounter++;

				Console.WriteLine($"added content to packet! bytestogo: {bytestogo}");

				if (bytestogo == 0)
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
				this.ReverseFrames();

		}
		#endregion

		#region Events


		public event EventHandler<TransmissionEventArgs> OnPayloadCompleted;

		protected virtual void PayloadCompleted(IPacketPayload data, ConversionStatus stat )
		{

			var handler = OnPayloadCompleted;
			if (handler != null) handler(this, new TransmissionEventArgs() { result = data, status = stat });
		}


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

			if (!Frames.ContainsKey((uint)sequence))
				throw new Exception("Error frame to resend is not available in Packet!");

			await worker.SendFrame(Frames[(uint)sequence]);

		}

		public void Dispose()
		{

		}

		#endregion
	}

	public class TransmissionEventArgs : EventArgs
	{
		public ConversionStatus status { get; set; }
		public IPacketPayload result { get; set; }
	}

	public enum ConversionStatus
	{
		Success,
		CompressionFail,
		DecompressionFail,
		Fail

	}
}
