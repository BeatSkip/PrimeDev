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

	public enum BaseHpApp: byte
	{
		Function = 0x00,
		Spreadsheet = 0x01,
		Statistics1Var = 0x02,
		Statistics2Var = 0x03,
		Inference = 0x04,
		Parametric = 0x05,
		Polar = 0x06,
		Sequence = 0x07,
		Finance = 0x08,
		LinearSolver = 0x09,
		TriangleSolver = 0x0A,
		DataStreamer = 0x0E,
		Geometry = 0x0F,
		Solve = 0x10,
		AdvancedGraphing = 0x11,
		Graph3D = 0x12,
		Explorer = 0x13,
		Python = 0x15,
		Unknown,
	}
}
