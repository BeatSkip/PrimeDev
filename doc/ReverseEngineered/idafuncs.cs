//C++ TO C# CONVERTER NOTE: _fastcall is not available in C#:
//ORIGINAL LINE: long __fastcall fUnpack(ulong a1, long a2)
private long fUnpack(ulong a1, long a2)
{
  if (((uint)dcbDigit(a1, 15L)) != 0)
  {
	if ((uint)dcbDigit(a1, 15L) == 9)
	{
	  (_BYTE)(a2 + 3) = -1;
	}
	else
	{
	  (_BYTE)(a2 + 3) = dcbDigit(a1, 15L) - 4;
	}
  }
  else
  {
	(_BYTE)(a2 + 3) = 1;
  }
  (_BYTE)(a2 + 2) = (_BYTE)(a2 + 2) & 0xF | 0x10;
  (_BYTE)(a2 + 2) &= 0xF0u;
  (_QWORD)(a2 + 8) = a1 & 0xFFFFFFFFFFFF000L;
  (_DWORD)(a2 + 4) = a1 & 0xFFF;
  (_DWORD)(a2 + 4) <<= 20;
  (int)(a2 + 4) >>= 20;
  return a2;
}
