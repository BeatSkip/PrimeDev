bool __fastcall AppFilesPacketToFiles(unsigned __int64 a1, const void *a2, __int64 a3)
{
  __int64 v4; // [xsp+38h] [xbp-88h]
  unsigned __int64 v5; // [xsp+40h] [xbp-80h]
  __int64 v6; // [xsp+40h] [xbp-80h]
  __int64 v7; // [xsp+48h] [xbp-78h]
  unsigned __int64 v10; // [xsp+60h] [xbp-60h]
  bool v11; // [xsp+6Fh] [xbp-51h]
  __int64 v12[4]; // [xsp+70h] [xbp-50h] BYREF
  __int64 v13[2]; // [xsp+90h] [xbp-30h] BYREF
  __int64 v14; // [xsp+A0h] [xbp-20h]
  __int64 v15; // [xsp+A8h] [xbp-18h]
  const void *v16[2]; // [xsp+B0h] [xbp-10h] BYREF

  v16[1] = *(const void **)(_ReadStatusReg(ARM64_SYSREG(3, 3, 13, 0, 2)) + 40);
  v16[0] = a2;
  v7 = 3LL;
  while ( (--v7 & 0x8000000000000000LL) == 0 )
  {
    v10 = a1 - 4;
    if ( (v10 & 0x8000000000000000LL) != 0 )
    {
      v11 = 0;
      goto LABEL_18;
    }
    v13[0] = 0LL;
    v13[1] = 0LL;
    v14 = (unsigned int)Readu32BigEndian(v16[0]);
    v15 = (__int64)v16[0] + 4;
    a1 = v10 - v14;
    if ( (a1 & 0x8000000000000000LL) != 0 )
    {
      v11 = 0;
      goto LABEL_18;
    }
    v16[0] = (char *)v16[0] + v14 + 4;
    THPBList<TAppFile,&(void DoNothingWithParam_HPList<TAppFile>(TAppFile &)),&(void DoNothingWithParam_HPList<CHPROList<TAppFile>>(TAppFile &)),(THPListCopy)0>::add(
      a3,
      v13);
  }
  while ( a1 >= 5 )
  {
    v5 = (unsigned int)Readu32BigEndianInc<unsigned char>(v16);
    a1 -= v5 + 4;
    if ( (a1 & 0x8000000000000000LL) != 0 )
    {
      v11 = 0;
      goto LABEL_18;
    }
    v4 = 0LL;
    while ( v5 >= 2 )
    {
      ++v4;
      v16[0] = (char *)v16[0] + 2;
      if ( !*((_BYTE *)v16[0] - 1) && !*((_BYTE *)v16[0] - 2) )
        break;
    }
    v6 = v5 - 2 * v4;
    v12[0] = (__int64)v16[0] - 2 * v4;
    v12[1] = v4 - 1;
    v12[2] = v6;
    v12[3] = (__int64)v16[0];
    THPBList<TAppFile,&(void DoNothingWithParam_HPList<TAppFile>(TAppFile &)),&(void DoNothingWithParam_HPList<CHPROList<TAppFile>>(TAppFile &)),(THPListCopy)0>::add(
      a3,
      v12);
    v16[0] = (char *)v16[0] + v6;
  }
  v11 = a1 == 0;
LABEL_18:
  _ReadStatusReg(ARM64_SYSREG(3, 3, 13, 0, 2));
  return v11;
}