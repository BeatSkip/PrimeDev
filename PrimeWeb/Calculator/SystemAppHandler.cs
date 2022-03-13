using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.Calculator
{
	public static class SystemAppHandler
	{
		public static void ParseAPP(byte[] data)
		{
			using(var ms = new MemoryStream(data))
			using(var reader = new BinaryReader(ms))
			{

			}
		}
	}

//	public static class AppItem
//	{
//
//		public byte TypeFlags
//		{
//			get { return Source[2]; }
//			set { Source[2] = value; }
//		}
//
//		public ObjTag Type
//		{
//			get { return (byte)(TypeFlags & 0x0F); }
//			set { TypeFlags = (byte)((TypeFlags & 0xF0) & ((byte)(value & 0x0F))); }
//		}
//
//		public byte Flags
//		{
//			get { return (byte)((TypeFlags & 0xF0) >> 4); }
//			set { TypeFlags = (byte)((TypeFlags & 0x0F) & ((byte)((value << 4) & 0xF0))); }
//		}
//
//		u32 Type: 4, // Matches one of the TTypes above
//
//TypeModifiers: 2, // See modifiers above
//Compressed: 1, // set if data compressed
//TypeId: 15, // Either matches the current structure TypeId, OR matches a sub-structure thereof. A sub structure will be loaded and placed as needed.
//			// Can also be set to 0 for facility. At this point, it is assumed to be current structure TypeId
//MemberId: 10;
//	}
//

}
