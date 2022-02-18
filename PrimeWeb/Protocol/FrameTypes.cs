﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PrimeWeb.Packets;
using PrimeWeb.Types;

namespace PrimeWeb.Protocol
{
	public enum FrameType
	{
		Ack,
		Content,
		Legacy,
		OutOfBand,
		Error
	}

	public interface IFrame
	{
		public byte[] GetBytes();
		public bool IsValid { get; }
		public FrameType Type { get; }
	}

	public class AckFrame : IFrame
	{
		byte Byte254; // Packet ID: out of bounds: 254
		byte NewProtocolCommand;// command at 0: nack, 1: ack
		byte SequenceToResend;  // no meaning for ack...
		byte unused_0;
		uint BlockPosition;   // Position associated with first bit in this packet
		uint IOMessageID;   // id of the IO Message beeing acked here...

		public FrameType Type { get { return FrameType.Ack; } }
		public bool IsValid { get { return (Byte254 == 0xFE && unused_0 == 0x00 && SequenceToResend != 0xFF && IOMessageID != 0); } }

		public AckFrame(byte[] data)
		{
			Byte254 = data[0];
			NewProtocolCommand = data[1];
			SequenceToResend = data[2];
			unused_0 = data[3];
			BlockPosition = (uint)data[4] | (uint)data[5] << 8 | (uint)data[6] << 16 | (uint)data[7] << 24;
			IOMessageID = (uint)data[8] | (uint)data[9] << 8 | (uint)data[10] << 16 | (uint)data[11] << 24;
		}

		public AckFrame(bool ack, int sequence, int messagenumber, int blockposition)
		{
			Byte254 = 0xFE;
			NewProtocolCommand = ack ? (byte)0x01 : (byte)0x00;
			SequenceToResend = (byte)sequence;
			unused_0 = (byte)0x00;
			BlockPosition = (uint)blockposition;
			IOMessageID = (uint)messagenumber;
		}

		public byte[] GetBytes()
		{
			var data = new byte[12];
			data[0] = Byte254;
			data[1] = NewProtocolCommand;
			data[2] = SequenceToResend;
			data[3] = 0x00;

			data[4] = (byte)(BlockPosition & (uint)0x000000FF);
			data[5] = (byte)((BlockPosition & (uint)0x0000FF00) >> 8);
			data[6] = (byte)((BlockPosition & (uint)0x00FF0000) >> 16);
			data[7] = (byte)((BlockPosition & (uint)0xFF000000) >> 24);

			data[8] = (byte)(IOMessageID & (uint)0x000000FF);
			data[9] = (byte)((IOMessageID & (uint)0x0000FF00) >> 8);
			data[10] = (byte)((IOMessageID & (uint)0x00FF0000) >> 16);
			data[11] = (byte)((IOMessageID & (uint)0xFF000000) >> 24);
			return data;
		}

	}

	public class ContentFrame : IFrame
	{
		public bool IsStartOfMessage { get { return (Sequence == 0x01); } }

		public int BlockLength { get { return Data.Length; } }

		byte Sequence; // Packet ID: out of bounds: 254

		uint IOMessageCounter;   // Position associated with first bit in this packet

		uint IOMessageSize;   // id of the IO Message beeing acked here...

		byte[] Data;

		public FrameType Type { get { return FrameType.Content; } }

		public bool IsValid { get { return (Sequence != 0xFE && IsStartOfMessage ? (IOMessageSize < ((240 * 1023) - 8) && IOMessageCounter < 25000000 && IOMessageCounter != 0) : (Sequence > 0x00 && Sequence < 0xFE)); } }

		public ContentFrame(byte[] data)
		{
			Sequence = data[0];

			if (Sequence == 0x01)
			{
				IOMessageCounter = (uint)data[1] | (uint)data[2] << 8 | (uint)data[3] << 16 | (uint)data[4] << 24;
				IOMessageSize = (uint)data[5] | (uint)data[6] << 8 | (uint)data[7] << 16 | (uint)data[8] << 24;

				Data = data.SubArray(9);
			}
			else
			{
				IOMessageCounter = 0;
				IOMessageSize = 0;
				Data = data.SubArray(1);
			}
		}

		public ContentFrame(int sequence, byte[] data, int messagenumber = 0)
		{
			Sequence = (byte)sequence;

			if (Sequence == 0x01)
			{
				IOMessageCounter = (uint)data[1] | (uint)data[2] << 8 | (uint)data[3] << 16 | (uint)data[4] << 24;
				IOMessageSize = (uint)data[5] | (uint)data[6] << 8 | (uint)data[7] << 16 | (uint)data[8] << 24;

				Data = data;
			}
			else
			{
				IOMessageCounter = 0;
				IOMessageSize = 0;
				Data = data.SubArray(1);
			}
		}

		public byte[] GetBytes()
		{
			if (this.IsStartOfMessage)
				return BytesLargeHeader();
			else
				return BytesSmallHeader();
		}

		private byte[] BytesLargeHeader()
		{
			var payload = new byte[9 + Data.Length];
			payload[0] = Sequence;
			payload[1] = (byte)(IOMessageCounter & (uint)0x000000FF);
			payload[2] = (byte)((IOMessageCounter & (uint)0x0000FF00) >> 8);
			payload[3] = (byte)((IOMessageCounter & (uint)0x00FF0000) >> 16);
			payload[4] = (byte)((IOMessageCounter & (uint)0xFF000000) >> 24);
			payload[5] = (byte)(IOMessageSize & (uint)0x000000FF);
			payload[6] = (byte)((IOMessageSize & (uint)0x0000FF00) >> 8);
			payload[7] = (byte)((IOMessageSize & (uint)0x00FF0000) >> 16);
			payload[8] = (byte)((IOMessageSize & (uint)0xFF000000) >> 24);

			Array.Copy(Data, 0, payload, 9, Data.Length);
			return payload;
		}

		private byte[] BytesSmallHeader()
		{
			var payload = new byte[1 + Data.Length];
			payload[0] = Sequence;
		
			Array.Copy(Data, 0, payload, 1, Data.Length);
			return payload;
		}

	}

	public class LegacyFrame : IFrame
	{
		public FrameType Type { get { return FrameType.Legacy; } }
		public bool IsValid { get { return true; } }

		private byte[] Data;
		
		public LegacyFrame(byte[] data)
		{
			Data = data;
		}

		public byte[] GetBytes()
		{
			return new byte[0];
		}
	}


}