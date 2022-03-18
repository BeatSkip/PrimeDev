__int64 __fastcall fUnpack(unsigned __int64 a1, __int64 a2)
{
  if ( (unsigned int)dcbDigit(a1, 15LL) )
  {
    if ( (unsigned int)dcbDigit(a1, 15LL) == 9 )
      *(_BYTE *)(a2 + 3) = -1;
    else
      *(_BYTE *)(a2 + 3) = dcbDigit(a1, 15LL) - 4;
  }
  else
  {
    *(_BYTE *)(a2 + 3) = 1;
  }
  *(_BYTE *)(a2 + 2) = *(_BYTE *)(a2 + 2) & 0xF | 0x10;
  *(_BYTE *)(a2 + 2) &= 0xF0u;
  *(_QWORD *)(a2 + 8) = a1 & 0xFFFFFFFFFFFF000LL;
  *(_DWORD *)(a2 + 4) = a1 & 0xFFF;
  *(_DWORD *)(a2 + 4) <<= 20;
  *(int *)(a2 + 4) >>= 20;
  return a2;
}


__int64 __fastcall LoadStructIsValidFile(unsigned int a1, const wchar_t *a2, __int64 a3)
{
  char v7; // [xsp+3Fh] [xbp-21h]
  int v8; // [xsp+40h] [xbp-20h]
  int v9; // [xsp+44h] [xbp-1Ch]
  unsigned __int16 v10; // [xsp+48h] [xbp-18h]
  unsigned __int16 v11; // [xsp+4Ah] [xbp-16h]
  __int64 v12[2]; // [xsp+50h] [xbp-10h] BYREF

  v12[1] = *(_QWORD *)(_ReadStatusReg(ARM64_SYSREG(3, 3, 13, 0, 2)) + 40);
  if ( (wcsIsEmpty(a2) & 1) != 0 )
  {
    v7 = 0;
  }
  else if ( (OpenFile(a1, a2, v12, 0LL, 1LL) & 1) != 0 )
  {
    if ( Fread() == 12
      && v8 == -1299553924
      && v9 == *(_DWORD *)a3
      && v11 <= (int)*(unsigned __int16 *)(a3 + 8)
      && v10 <= (int)*(unsigned __int16 *)(a3 + 10) )
    {
      Fclose(v12[0]);
      v7 = 1;
    }
    else
    {
      Fclose(v12[0]);
      v7 = 0;
    }
  }
  else
  {
    v7 = 0;
  }
  _ReadStatusReg(ARM64_SYSREG(3, 3, 13, 0, 2));
  return v7 & 1;
}

__int64 __fastcall TPersistedTypeBase::GetPersistedMember(
        TPersistedTypeBase *this,
        TPersistedTypeBase::TPersistedMember *a2,
        TPersistedTypeBase::TPersistedMember *a3)
{
  __int64 Members; // x0
  __int128 v4; // q0
  int v6; // [xsp+10h] [xbp-60h]
  int v7; // [xsp+18h] [xbp-58h]
  int v8; // [xsp+28h] [xbp-48h]
  int v9; // [xsp+30h] [xbp-40h]
  int v10; // [xsp+3Ch] [xbp-34h]
  __int64 v12; // [xsp+48h] [xbp-28h]

  v12 = *((_QWORD *)this + 6);
  while ( 1 )
  {
    if ( (--v12 & 0x8000000000000000LL) != 0 )
      return 0;
    v10 = *(_DWORD *)a2 >> 22;
    if ( v10 == *(_DWORD *)TPersistedTypeBase::GetMembers(this, v12) >> 22 )
    {
      if ( ((*(_DWORD *)TPersistedTypeBase::GetMembers(this, v12) >> 7) & 0x7FFF) == 0
        && ((*(_DWORD *)a2 >> 7) & 0x7FFF) == (*(_DWORD *)this & 0x7FFF) )
      {
        break;
      }
      if ( ((*(_DWORD *)TPersistedTypeBase::GetMembers(this, v12) >> 7) & 0x7FFF) != 0 )
      {
        v9 = (*(_DWORD *)a2 >> 7) & 0x7FFF;
        if ( v9 == ((*(_DWORD *)TPersistedTypeBase::GetMembers(this, v12) >> 7) & 0x7FFF) )
          break;
      }
    }
  }
  v8 = *(_DWORD *)a2 & 0xF;
  if ( v8 == (*(_DWORD *)TPersistedTypeBase::GetMembers(this, v12) & 0xF)
    || (*(_DWORD *)a2 & 0xFu) <= 0xA
    && (*(_DWORD *)TPersistedTypeBase::GetMembers(this, v12) & 0xFu) <= 0xA
    && ((*(_QWORD *)TPersistedTypeBase::GetMembers(this, v12) & 0x2000000000000000LL) != 0
     && (v7 = *(_DWORD *)a2 & 0xF, v7 < (*(_DWORD *)TPersistedTypeBase::GetMembers(this, v12) & 0xF))
     || (*(_QWORD *)TPersistedTypeBase::GetMembers(this, v12) & 0x1000000000000000LL) != 0
     && (v6 = *(_DWORD *)a2 & 0xF, v6 > (*(_DWORD *)TPersistedTypeBase::GetMembers(this, v12) & 0xF))) )
  {
    Members = TPersistedTypeBase::GetMembers(this, v12);
    v4 = *(_OWORD *)Members;
    *((_QWORD *)a3 + 2) = *(_QWORD *)(Members + 16);
    *(_OWORD *)a3 = v4;
    return 1;
  }
  else
  {
    return 0;
  }
}