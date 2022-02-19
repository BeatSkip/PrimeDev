using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Types
{
	public enum ProtocolVersion : byte
	{
		Unknown = 254,
		Old = 0,
		V2 = 1,
		V3 = 2,
	}

	public enum NewPacketType : byte
	{
		MessageStart,
		Message,
		OutOfBounds
	}

	public enum PrimeCMD : byte
	{
		//CMD_PRIME_CHECK_READY (0xFF)
		CHECK_READY = 0xFF,
		//CMD_PRIME_GET_INFOS (0xFA)
		GET_INFOS = 0xFA,
		//CMD_PRIME_RECV_SCREEN (0xFC)
		RECV_SCREEN = 0xFC,
		//CMD_PRIME_RECV_BACKUP (0xF9)
		RECV_BACKUP = 0xF9,
		//CMD_PRIME_REQ_FILE (0xF8)
		REQ_FILE = 0xF8,
		//CMD_PRIME_RECV_FILE (0xF7)
		RECV_FILE = 0xF7,
		//CMD_PRIME_SEND_CHAT (0xF2)
		TRANSFER_CHAT = 0xF2,
		//CMD_PRIME_SEND_KEY (0xEC)
		SEND_KEY = 0xEC,
		//CMD_PRIME_SET_DATE_TIME (0xE7)
		SET_DATETIME = 0xE7,
	}

	public enum ScreenFormat : byte
	{
		//CALC_SCREENSHOT_FORMAT_PRIME_PNG_320x240x16 = 8,
		PNG_320px_240px_16bit = 0x08,
		//CALC_SCREENSHOT_FORMAT_PRIME_PNG_320x240x4 = 9,
		PNG_320px_240px_4bit = 0x09,
		//CALC_SCREENSHOT_FORMAT_PRIME_PNG_160x120x16 = 10,
		PNG_160px_120px_16bit = 0x0A,
		//CALC_SCREENSHOT_FORMAT_PRIME_PNG_160x120x4 = 11,
		PNG_160px_120px_4bit = 0x0B,

		UNKNOWN_A = 0x0D
	}

	public enum ReportType
	{
		OldProtocol,
		V2Slice,
		AckSlice,
		OutOfBound,
		Error,
	}

	public enum PacketType : byte
	{
		Chat = 0xF2,
		FileRequest = 0xF8,
		FileTransfer = 0xF7,
		SendKey = 0xEC,
		SetDateTime = 0xE7,
		TransferBackup = 0xF9,
		CheckReady = 0xFF,
		unkown = 0
	}

	public enum TransferType
	{
		Tx,
		Rx,
	}
}
