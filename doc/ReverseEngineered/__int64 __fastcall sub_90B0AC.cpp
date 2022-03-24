__int64 __fastcall sub_90B0AC(__int64 a1, __int64 a2, __int64 a3, __int64 a4)
{
  __int64 v4; // x9
  __int64 v5; // x0
  __int64 v6; // x1
  __int64 v7; // x2
  int v8; // w0
  __int64 v9; // x0
  __int64 v10; // x1
  __int64 *v11; // x0
  __int64 *v12; // x0
  unsigned int v13; // w9
  __int64 *v14; // x0
  __int64 *v15; // x0
  __int64 v16; // x0
  unsigned __int8 **v17; // x2
  __int64 *v18; // x0
  __int64 v19; // x8
  THPObj **v20; // x0
  __int64 v21; // x0
  __int64 v22; // x0
  __int64 *v23; // x0
  __int64 *v24; // x0
  int v25; // w9
  __int64 v26; // x0
  unsigned int v28; // [xsp+4Ch] [xbp-584h]
  char *v29; // [xsp+58h] [xbp-578h]
  bool v30; // [xsp+70h] [xbp-560h]
  __int64 v31; // [xsp+B8h] [xbp-518h]
  giac::gen *v32; // [xsp+D0h] [xbp-500h]
  giac::gen *v33; // [xsp+D8h] [xbp-4F8h]
  wchar_t *v34; // [xsp+168h] [xbp-468h]
  unsigned __int8 *v35; // [xsp+170h] [xbp-460h]
  unsigned int v36; // [xsp+2A4h] [xbp-32Ch]
  __int64 v37; // [xsp+2A8h] [xbp-328h]
  unsigned int v38; // [xsp+2B4h] [xbp-31Ch]
  __int64 *v39; // [xsp+2B8h] [xbp-318h]
  _DWORD *SubFromId; // [xsp+2C0h] [xbp-310h]
  int v41; // [xsp+2CCh] [xbp-304h]
  int v42; // [xsp+2CCh] [xbp-304h]
  __int64 j; // [xsp+2D0h] [xbp-300h]
  __int64 *v44; // [xsp+2D8h] [xbp-2F8h]
  __int64 v45; // [xsp+2E0h] [xbp-2F0h]
  __int64 k; // [xsp+2E8h] [xbp-2E8h]
  giac::gen **v47; // [xsp+2F0h] [xbp-2E0h]
  _BYTE *v48; // [xsp+2F8h] [xbp-2D8h]
  __int64 v49; // [xsp+300h] [xbp-2D0h]
  __int64 v50; // [xsp+308h] [xbp-2C8h]
  _QWORD *v51; // [xsp+310h] [xbp-2C0h]
  __int64 v52; // [xsp+318h] [xbp-2B8h]
  __int64 v53; // [xsp+320h] [xbp-2B0h]
  __int64 v54; // [xsp+328h] [xbp-2A8h]
  unsigned __int16 *v55; // [xsp+330h] [xbp-2A0h]
  unsigned __int16 *i; // [xsp+330h] [xbp-2A0h]
  wchar_t *v57; // [xsp+338h] [xbp-298h]
  __int64 v58; // [xsp+360h] [xbp-270h]
  char *v59; // [xsp+368h] [xbp-268h]
  unsigned __int8 *v60; // [xsp+370h] [xbp-260h]
  __int64 v61; // [xsp+378h] [xbp-258h]
  __int64 v62; // [xsp+388h] [xbp-248h]
  __int64 v63; // [xsp+390h] [xbp-240h]
  __int64 *v64; // [xsp+398h] [xbp-238h]
  int v68; // [xsp+3C0h] [xbp-210h] BYREF
  const void *v69; // [xsp+3C8h] [xbp-208h] BYREF
  __int64 v70; // [xsp+3D0h] [xbp-200h]
  int v71; // [xsp+3D8h] [xbp-1F8h] BYREF
  _QWORD v72[2]; // [xsp+3E0h] [xbp-1F0h] BYREF
  char *v73; // [xsp+3F0h] [xbp-1E0h] BYREF
  __int64 v74; // [xsp+3F8h] [xbp-1D8h] BYREF
  __int64 v75; // [xsp+400h] [xbp-1D0h] BYREF
  char v76[8]; // [xsp+408h] [xbp-1C8h] BYREF
  __int64 v77; // [xsp+410h] [xbp-1C0h] BYREF
  void *v78; // [xsp+418h] [xbp-1B8h] BYREF
  char v79[16]; // [xsp+420h] [xbp-1B0h] BYREF
  __int64 v80; // [xsp+430h] [xbp-1A0h] BYREF
  THPObj *v81; // [xsp+438h] [xbp-198h] BYREF
  __int64 v82; // [xsp+440h] [xbp-190h] BYREF
  const void *v83; // [xsp+448h] [xbp-188h] BYREF
  __int64 v84; // [xsp+450h] [xbp-180h] BYREF
  __int64 v85; // [xsp+458h] [xbp-178h] BYREF
  __int64 v86; // [xsp+460h] [xbp-170h] BYREF
  __int128 v87; // [xsp+468h] [xbp-168h] BYREF
  __int128 v88; // [xsp+478h] [xbp-158h] BYREF
  __int128 dest; // [xsp+488h] [xbp-148h] BYREF
  __int64 v90; // [xsp+498h] [xbp-138h] BYREF
  __int64 v91; // [xsp+4A0h] [xbp-130h] BYREF
  int v92; // [xsp+4A8h] [xbp-128h]
  int v93; // [xsp+4B0h] [xbp-120h] BYREF
  unsigned int v94; // [xsp+4B8h] [xbp-118h]
  unsigned __int8 *v95; // [xsp+4C0h] [xbp-110h] BYREF
  char v96[4]; // [xsp+4CCh] [xbp-104h] BYREF
  __int64 v97; // [xsp+4D0h] [xbp-100h] BYREF
  unsigned int v98; // [xsp+4DCh] [xbp-F4h] BYREF
  unsigned int v99; // [xsp+4E0h] [xbp-F0h]
  unsigned int v100; // [xsp+4E8h] [xbp-E8h]
  unsigned int v101; // [xsp+4F0h] [xbp-E0h] BYREF
  __int64 v102; // [xsp+4F8h] [xbp-D8h] BYREF
  unsigned int v103; // [xsp+500h] [xbp-D0h]
  char v104[40]; // [xsp+508h] [xbp-C8h] BYREF
  char v105[40]; // [xsp+530h] [xbp-A0h] BYREF
  char v106[32]; // [xsp+558h] [xbp-78h] BYREF
  __int64 *v107; // [xsp+578h] [xbp-58h] BYREF
  signed __int64 v108; // [xsp+580h] [xbp-50h] BYREF
  int v109; // [xsp+588h] [xbp-48h]
  _QWORD v110[4]; // [xsp+5A0h] [xbp-30h] BYREF

  v110[3] = *(_QWORD *)(_ReadStatusReg(ARM64_SYSREG(3, 3, 13, 0, 2)) + 40);
  v102 = a4;
  TPersistedTypeBase::TLoadCode::TLoadCode((TPersistedTypeBase::TLoadCode *)&v101);
  if ( *(_QWORD *)(a2 + 40) )
  {
    v100 = (*(__int64 (__fastcall **)(__int64, _QWORD, __int64, __int64, _QWORD, _QWORD, _QWORD, _QWORD, __int64 *))(a2 + 40))(
             a1,
             0LL,
             a2,
             a3,
             0LL,
             0LL,
             0LL,
             0LL,
             &v102);
    v101 = v100;
  }
  if ( (TPersistedTypeBase::TLoadCode::IsError((TPersistedTypeBase::TLoadCode *)&v101) & 1) != 0 )
  {
    v103 = v101;
    goto LABEL_220;
  }
  if ( (TPersistedTypeBase::TLoadCode::IsDefault((TPersistedTypeBase::TLoadCode *)&v101) & 1) != 0
    && ((*(_BYTE *)(a2 + 12) & 1) == 0 || !*(_QWORD *)a3) )
  {
    v4 = *(unsigned int *)(a2 + 4);
    *(_QWORD *)(a3 + 8) = v4;
    v5 = sub_908B54(v4);
    *(_QWORD *)a3 = v5;
    if ( !v5 )
    {
LABEL_216:
      v103 = TPersistedTypeBase::TLoadCode::Error((TPersistedTypeBase::TLoadCode *)&v101);
      goto LABEL_220;
    }
    if ( (*(_BYTE *)(a2 + 12) & 2) == 0 )
    {
      if ( *(_QWORD *)(a2 + 16) )
        memcpy(*(void **)a3, *(const void **)(a2 + 16), *(_DWORD *)(a2 + 4));
      else
        memset(*(void **)a3, 0, *(_DWORD *)(a2 + 4));
    }
  }
  if ( *(_QWORD *)(a2 + 40) )
  {
    v99 = (*(__int64 (__fastcall **)(__int64, __int64, __int64, __int64, _QWORD, _QWORD, _QWORD, _QWORD, __int64 *))(a2 + 40))(
            a1,
            1LL,
            a2,
            a3,
            0LL,
            0LL,
            0LL,
            0LL,
            &v102);
    v101 = v99;
  }
  if ( (TPersistedTypeBase::TLoadCode::IsError((TPersistedTypeBase::TLoadCode *)&v101) & 1) != 0 )
  {
    v103 = v101;
    goto LABEL_220;
  }
  while ( ((*(__int64 (__fastcall **)(__int64, unsigned int *, __int64))(*(_QWORD *)a1 + 8LL))(a1, &v98, 4LL) & 1) != 0 )
  {
    v97 = v98 - 4LL;
    if ( (v97 & 0x8000000000000000LL) != 0 )
      goto LABEL_216;
    if ( ((*(__int64 (__fastcall **)(__int64, _QWORD *, __int64))(*(_QWORD *)a1 + 8LL))(a1, v110, 4LL) & 1) == 0 )
      goto LABEL_216;
    v95 = (unsigned __int8 *)(*(__int64 (__fastcall **)(__int64, __int64, char *))(*(_QWORD *)a1 + 16LL))(a1, v97, v96);
    if ( !v95 )
    {
      if ( v97 )
        goto LABEL_216;
    }
    if ( (TPersistedTypeBase::GetPersistedMember(
            (TPersistedTypeBase *)a2,
            (TPersistedTypeBase::TPersistedMember *)v110,
            (TPersistedTypeBase::TPersistedMember *)&v108) & 1) != 0 )
    {
      if ( v108 < 0 )
        v107 = (__int64 *)(v102 + (HIDWORD(v108) & 0xFFFFFFF));
      else
        v107 = (__int64 *)(*(_QWORD *)a3 + (HIDWORD(v108) & 0xFFFFFFF));
      v64 = 0LL;
      if ( (((unsigned int)v108 >> 4) & 3) == 2 || (~((unsigned int)v108 >> 4) & 3) == 0 )
        v64 = v107 + 1;
      TPersistedTypeBase::TLoadCode::TLoadCode((TPersistedTypeBase::TLoadCode *)&v93);
      if ( *(_QWORD *)(a2 + 40) )
      {
        v92 = (*(__int64 (__fastcall **)(__int64, __int64, __int64, __int64, __int64 **, unsigned __int8 **, __int64 *, char *, __int64 *))(a2 + 40))(
                a1,
                2LL,
                a2,
                a3,
                &v107,
                &v95,
                &v97,
                v96,
                &v102);
        v93 = v92;
      }
      if ( (TPersistedTypeBase::TLoadCode::IsError((TPersistedTypeBase::TLoadCode *)&v93) & 1) != 0 )
      {
LABEL_214:
        if ( (v96[0] & 1) != 0 )
          sub_90D1D0(v95);
        goto LABEL_216;
      }
      if ( (TPersistedTypeBase::TLoadCode::IsNoDefault((TPersistedTypeBase::TLoadCode *)&v93) & 1) == 0 )
      {
        if ( (v110[0] & 0xF) <= 0xA && ((unsigned __int8)v108 & 0xFu) <= 0xA )
        {
          if ( (v109 & 0x20000000) != 0 )
          {
            if ( (unsigned __int64)(v109 & 0x3FFFFFF) + 4 > v97 )
              goto LABEL_214;
            v8 = Readu32LittleEndian(&v95[v109 & 0x3FFFFFF]);
            v109 = v109 & 0xFC000000 | v8 & 0x3FFFFFF;
            v108 = v108 & 0xFFFFFFFFFFFFFFF0LL | 1;
            v110[0] = v110[0] & 0xFFFFFFFFFFFFFFF0LL | 1;
          }
          v63 = TPersistedTypeBase::TypeToSize[v110[0] & 0xF];
          v62 = TPersistedTypeBase::TypeToSize[v108 & 0xF];
          v9 = Idiv(v97, v63);
          v61 = v9;
          if ( !v10 )
          {
            if ( (((unsigned int)v108 >> 4) & 3) != 0 && (((unsigned int)v108 >> 4) & 3) != 1 )
            {
              v12 = (__int64 *)sub_908B54(v9 * v62);
              *v107 = (__int64)v12;
              v107 = v12;
              if ( !v12 )
                goto LABEL_214;
              *v64 = v61;
            }
            else
            {
              if ( (((unsigned int)v108 >> 4) & 3) == 1 )
              {
                v11 = (__int64 *)sub_908B54((v109 & 0x3FFFFFF) * v62);
                *v107 = (__int64)v11;
                v107 = v11;
                if ( !v11 )
                  goto LABEL_216;
              }
              if ( v61 != (v109 & 0x3FFFFFF) )
                memset(v107, 0, (v109 & 0x3FFFFFF) * v62);
              v91 = v61;
              v90 = v109 & 0x3FFFFFF;
              v61 = *(_QWORD *)min<long>(&v91, &v90);
            }
            v60 = v95;
            v59 = (char *)v107;
            v58 = v61;
            while ( (--v58 & 0x8000000000000000LL) == 0 )
            {
              dest = 0uLL;
              memcpy(&dest, v60, v63);
              v60 += v63;
              if ( (TPersistedTypeBase::TPersistedMember::IsSignedInteger((TPersistedTypeBase::TPersistedMember *)v110) & 1) != 0
                && (((unsigned __int64)dest >> (8 * (unsigned __int8)v63 - 1)) & 1) != 0 )
              {
                *(_QWORD *)&dest = dest | (-1LL << (8 * (unsigned __int8)v63));
              }
              if ( v109 < 0 && v63 != 4 )
              {
                v62 = 4LL;
                if ( (dest & 0x8000) != 0 )
                  v13 = 255;
                else
                  v13 = 0;
                *(_QWORD *)&dest = (unsigned int)RGB8ToTColor(
                                                   v13,
                                                   (8 * (((unsigned __int64)(unsigned __int16)dest >> 10) & 0x1F)) | ((__int64)(((unsigned __int64)(unsigned __int16)dest >> 10) & 0x1F) >> 2),
                                                   (8 * (((unsigned __int64)(unsigned __int16)dest >> 5) & 0x1F)) | ((__int64)(((unsigned __int64)(unsigned __int16)dest >> 5) & 0x1F) >> 2),
                                                   (8 * (dest & 0x1F)) | ((__int64)(dest & 0x1F) >> 2));
              }
              if ( (TPersistedTypeBase::TPersistedMember::IsInteger((TPersistedTypeBase::TPersistedMember *)v110) & 1) != 0
                && (v108 & 0xF) == 10 )
              {
                if ( (TPersistedTypeBase::TPersistedMember::IsSignedInteger((TPersistedTypeBase::TPersistedMember *)v110) & 1) != 0 )
                  fIntToHP(dest, &dest);
                else
                  fIntToHP(dest, &dest);
              }
              else if ( (TPersistedTypeBase::TPersistedMember::IsInteger((TPersistedTypeBase::TPersistedMember *)v110) & 1) != 0
                     && (v108 & 0xF) == 8 )
              {
                if ( (TPersistedTypeBase::TPersistedMember::IsSignedInteger((TPersistedTypeBase::TPersistedMember *)v110) & 1) != 0 )
                  fIntToHP(dest, &v88);
                else
                  fIntToHP(dest, &v88);
                *(_QWORD *)&dest = fPack(&v88);
              }
              else if ( (TPersistedTypeBase::TPersistedMember::IsInteger((TPersistedTypeBase::TPersistedMember *)v110) & 1) != 0
                     && (v108 & 0xF) == 9 )
              {
                TPersistedTypeBase::TPersistedMember::IsSignedInteger((TPersistedTypeBase::TPersistedMember *)v110);
                *(double *)&dest = (double)(unsigned __int64)dest;
              }
              else if ( (v110[0] & 0xF) == 10 || (v110[0] & 0xF) == 8 )
              {
                if ( (v110[0] & 0xF) != 8 || (v108 & 0xF) == 8 )
                {
                  if ( (v110[0] & 0xF) != 8 && (v108 & 0xF) == 8 )
                    *(_QWORD *)&dest = fPack(&dest);
                }
                else
                {
                  fUnpack();
                }
                if ( (TPersistedTypeBase::TPersistedMember::IsInteger((TPersistedTypeBase::TPersistedMember *)&v108) & 1) != 0 )
                {
                  if ( (TPersistedTypeBase::TPersistedMember::IsSignedInteger((TPersistedTypeBase::TPersistedMember *)&v108) & 1) != 0 )
                    *(_QWORD *)&dest = fHPToi64(&dest);
                  else
                    *(_QWORD *)&dest = fHPTou64(&dest);
                }
                else if ( (v108 & 0xF) == 9 )
                {
                  *(_QWORD *)&dest = HPRealToDouble(&dest);
                }
              }
              else if ( (v110[0] & 0xF) == 9
                     && (TPersistedTypeBase::TPersistedMember::IsInteger((TPersistedTypeBase::TPersistedMember *)&v108) & 1) != 0 )
              {
                if ( (TPersistedTypeBase::TPersistedMember::IsSignedInteger((TPersistedTypeBase::TPersistedMember *)&v108) & 1) != 0 )
                  *(_QWORD *)&dest = (__int64)*(double *)&dest;
                else
                  *(_QWORD *)&dest = (unsigned __int64)*(double *)&dest;
              }
              else if ( (v110[0] & 0xF) == 9 && (v108 & 0xF) == 8 )
              {
                fDoubleToHP(&v87, *(double *)&dest);
                *(_QWORD *)&dest = fPack(&v87);
              }
              else if ( (v110[0] & 0xF) == 9 && (v108 & 0xF) == 10 )
              {
                fDoubleToHP(&dest, *(double *)&dest);
              }
              memcpy(v59, &dest, v62);
              v59 += v62;
            }
          }
          goto LABEL_210;
        }
        if ( (v110[0] & 0xF) == (v108 & 0xF) )
        {
          switch ( v110[0] & 0xF )
          {
            case 11:
              if ( (v97 & 1) == 0 )
              {
                v86 = (unsigned __int64)v97 >> 1;
                if ( (((unsigned int)v108 >> 4) & 3) == 1 && (v109 & 0x3FFFFFF) == 0
                  || (((unsigned int)v108 >> 4) & 3) == 2 )
                {
                  v57 = (wchar_t *)sub_908B54(4 * v97);
                  if ( !v57 )
                    goto LABEL_214;
                  hpwchartowchar_t(v57, v95, v86);
                  if ( v64 )
                    *v64 = v86;
                  *v107 = (__int64)v57;
                }
                else if ( (((unsigned int)v108 >> 4) & 3) != 0 && (((unsigned int)v108 >> 4) & 3) != 1 )
                {
                  v55 = (unsigned __int16 *)v95;
                  v54 = 0LL;
                  v53 = v86;
                  while ( v53 >= 1 )
                  {
                    ++v54;
                    v52 = wcsend2(v55, v53) + 2;
                    v53 -= (v52 - (__int64)v55) >> 1;
                    v55 = (unsigned __int16 *)v52;
                  }
                  v16 = sub_908B54(8 * v54);
                  *v107 = v16;
                  v51 = (_QWORD *)v16;
                  if ( !v16 )
                    goto LABEL_214;
                  *v64 = v54;
                  for ( i = (unsigned __int16 *)v95; (--v54 & 0x8000000000000000LL) == 0; i = (unsigned __int16 *)v50 )
                  {
                    v50 = wcsend2(i, v86 - 1) + 2;
                    *v51++ = wcharTowchar_t(i, (v50 - (__int64)i) >> 1);
                    v86 -= (v50 - (__int64)i) >> 1;
                  }
                }
                else
                {
                  if ( (((unsigned int)v108 >> 4) & 3) == 1 )
                  {
                    v14 = (__int64 *)sub_908B54(4LL * (v109 & 0x3FFFFFF));
                    *v107 = (__int64)v14;
                    v107 = v14;
                    if ( !v14 )
                      goto LABEL_214;
                  }
                  memset(v107, 0, 4 * (v109 & 0x3FFFFFF));
                  v85 = v109 & 0x3FFFFFF;
                  v35 = v95;
                  v34 = (wchar_t *)v107;
                  v15 = (__int64 *)min<long>(&v86, &v85);
                  hpwchartowchar_t(v34, v35, *v15);
                }
              }
              break;
            case 12:
              CHPList<THPObj *>::CHPList(&v83);
              v82 = (__int64)v95;
              while ( v97 >= 1 )
              {
                v81 = (THPObj *)THPObj::NewFromMem2((THPObj *)&v97, &v82, v17);
                if ( v81 )
                {
                  if ( (*((_BYTE *)v81 + 2) & 0xF) != 5 )
                  {
                    THPBList<THPObj *,&(void DoNothingWithParam_HPList<THPObj *>(THPObj * &)),&(void DoNothingWithParam_HPList<CHPROList<THPObj *>>(THPObj * &)),(THPListCopy)0>::add(
                      &v83,
                      &v81);
                    if ( (v108 & 0x4000000000000000LL) != 0 )
                      THPObj::embed(v81);
                  }
                }
                else
                {
                  THPBList<THPObj *,&(void DoNothingWithParam_HPList<THPObj *>(THPObj * &)),&(void DoNothingWithParam_HPList<CHPROList<THPObj *>>(THPObj * &)),(THPListCopy)0>::add(
                    &v83,
                    &v81);
                }
              }
              if ( (((unsigned int)v108 >> 4) & 3) == 2 )
              {
                *v107 = (__int64)v83;
                *v64 = v84;
                v83 = 0LL;
                v84 = 0LL;
              }
              else
              {
                memset(v107, 0, 8 * (v109 & 0x3FFFFFF));
                v80 = v109 & 0x3FFFFFF;
                v18 = (__int64 *)min<long>(&v84, &v80);
                v49 = *v18;
                memcpy(v107, v83, 8 * *v18);
                while ( v49 < v84 )
                {
                  v19 = v49++;
                  v20 = (THPObj **)THPBList<THPObj *,&(void DoNothingWithParam_HPList<THPObj *>(THPObj * &)),&(void DoNothingWithParam_HPList<CHPROList<THPObj *>>(THPObj * &)),(THPListCopy)0>::operator[](
                                     &v83,
                                     v19);
                  THPObj::debedDel(*v20);
                }
              }
              CHPList<THPObj *>::~CHPList(&v83);
              break;
            case 13:
              giac::dbgprint_vector<giac::gen>::dbgprint_vector(v106, v6, v7);
              CHPList<THPObj *>::CHPList(v79);
              v78 = v95;
              while ( v97 >= 1 )
              {
                v48 = v78;
                v77 = v97;
                LoadGenFromFile(&v77, &v78);
                std::imvector<giac::gen>::push_back(v106, v76);
                giac::gen::~gen((giac::gen *)v76);
                v97 += v48 - (_BYTE *)v78;
              }
              if ( v64 )
              {
                v21 = std::imvector<giac::gen>::size(v106);
                *v64 = v21;
                v109 = v109 & 0xFC000000 | v21 & 0x3FFFFFF;
              }
              if ( (~((unsigned int)v108 >> 4) & 3) != 0 )
              {
                v75 = std::imvector<giac::gen>::size(v106);
                v74 = v109 & 0x3FFFFFF;
                v45 = *(_QWORD *)min<long>(&v75, &v74);
                if ( (((unsigned int)v108 >> 4) & 3) != 2 && (((unsigned int)v108 >> 4) & 3) != 1
                  || (v24 = (__int64 *)calloc2(v45, 8LL), *v107 = (__int64)v24, (v107 = v24) != 0LL) )
                {
                  v44 = v107;
                  for ( j = 0LL; j < v45; ++j )
                  {
                    v31 = std::imvector<giac::gen>::operator[](v106, j);
                    giac::gen::operator=(v44, v31);
                  }
                  goto LABEL_169;
                }
                v41 = 4;
              }
              else
              {
                v22 = std::imvector<giac::gen>::size(v106);
                v23 = (__int64 *)calloc2(v22, 8LL);
                *v107 = (__int64)v23;
                v107 = v23;
                if ( !v23 )
                {
                  v41 = 4;
                  goto LABEL_170;
                }
                v47 = (giac::gen **)v107;
                for ( k = 0LL; k < std::imvector<giac::gen>::size(v106); ++k )
                {
                  v33 = (giac::gen *)operator new(8uLL);
                  v32 = (giac::gen *)std::imvector<giac::gen>::operator[](v106, k);
                  giac::gen::gen(v33, v32);
                  *v47++ = v33;
                }
LABEL_169:
                v41 = 0;
              }
LABEL_170:
              CHPList<THPObj *>::~CHPList(v79);
              giac::dbgprint_vector<giac::gen>::~dbgprint_vector(v106);
              if ( v41 )
                goto LABEL_214;
              break;
            case 14:
              SubFromId = TPersistedTypeBase::GetSubFromId((TPersistedTypeBase *)a2, ((unsigned int)v108 >> 7) & 0x7FFF);
              if ( !SubFromId )
                goto LABEL_214;
              v73 = (char *)v95;
              if ( (~((unsigned int)v108 >> 4) & 3) == 0 )
              {
                v39 = v107;
                while ( v97 >= 4 )
                {
                  v38 = Readu32LittleEndianInc<unsigned char>(&v73);
                  v97 -= 4LL;
                  v97 -= v38;
                  if ( (v97 & 0x8000000000000000LL) != 0 )
                    break;
                  CPersistenceMemoryStream::CPersistenceMemoryStream((CPersistenceMemoryStream *)v105, v73, v38);
                  v73 += v38;
                  v72[1] = 0LL;
                  v72[0] = 0LL;
                  ++qword_177CEE8;
                  v71 = sub_90B0AC(v105, SubFromId, v72, v102);
                  --qword_177CEE8;
                  if ( (TPersistedTypeBase::TLoadCode::IsError((TPersistedTypeBase::TLoadCode *)&v71) & 1) != 0 )
                    sub_90D1D0(v72[0]);
                  else
                    THPBList<void *,&(void DoNothingWithParam_HPList<void *>(void * &)),&(void DoNothingWithParam_HPList<CHPROList<void *>>(void * &)),(THPListCopy)0>::add(
                      v39,
                      v72);
                  CPersistenceMemoryStream::~CPersistenceMemoryStream((CPersistenceMemoryStream *)v105);
                }
                break;
              }
              if ( v64 )
                *v64 = 0LL;
              v37 = 0LL;
LABEL_186:
              if ( (((unsigned int)v108 >> 4) & 3) == 2
                || (v25 = v109 & 0x3FFFFFF, v109 = v109 & 0xFC000000 | ((v109 & 0x3FFFFFF) - 1) & 0x3FFFFFF,
                                            v30 = 0,
                                            v25) )
              {
                v30 = v97 > 3;
              }
              if ( !v30 )
                break;
              v36 = Readu32LittleEndianInc<unsigned char>(&v73);
              v97 -= 4LL;
              v97 -= v36;
              if ( (v97 & 0x8000000000000000LL) != 0 )
                break;
              CPersistenceMemoryStream::CPersistenceMemoryStream((CPersistenceMemoryStream *)v104, v73, v36);
              v73 += v36;
              if ( (((unsigned int)v108 >> 4) & 3) != 0 )
                v29 = 0LL;
              else
                v29 = (char *)v107 + v37;
              v69 = v29;
              if ( (((unsigned int)v108 >> 4) & 3) != 0 )
                v28 = 0;
              else
                v28 = SubFromId[1];
              v70 = v28;
              ++qword_177CEE8;
              v68 = sub_90B0AC(v104, SubFromId, &v69, v102);
              --qword_177CEE8;
              if ( (TPersistedTypeBase::TLoadCode::IsError((TPersistedTypeBase::TLoadCode *)&v68) & 1) != 0 )
              {
                sub_90D1D0(v69);
                v42 = 26;
LABEL_206:
                CPersistenceMemoryStream::~CPersistenceMemoryStream((CPersistenceMemoryStream *)v104);
                if ( v42 && v42 == 4 )
                  goto LABEL_214;
                goto LABEL_186;
              }
              if ( (((unsigned int)v108 >> 4) & 3) == 2 || (((unsigned int)v108 >> 4) & 3) == 1 )
              {
                v26 = sub_90D138(*v107, v37 + v70);
                *v107 = v26;
                if ( !v26 )
                {
                  v42 = 4;
                  goto LABEL_206;
                }
                memcpy((void *)(*v107 + v37), v69, v70);
                sub_90D1D0(v69);
                if ( v64 )
                  ++*v64;
              }
              v37 += v70;
              v42 = 0;
              goto LABEL_206;
          }
        }
      }
LABEL_210:
      if ( (v96[0] & 1) != 0 )
        sub_90D1D0(v95);
      if ( (v109 & 0x40000000) != 0 )
        break;
    }
    else
    {
      if ( *(_QWORD *)(a2 + 40) )
      {
        v94 = (*(__int64 (__fastcall **)(__int64, __int64, __int64, __int64, __int64 **, unsigned __int8 **, __int64 *, char *, __int64 *))(a2 + 40))(
                a1,
                3LL,
                a2,
                a3,
                &v107,
                &v95,
                &v97,
                v96,
                &v102);
        v101 = v94;
      }
      if ( (v96[0] & 1) != 0 )
        sub_90D1D0(v95);
      if ( (TPersistedTypeBase::TLoadCode::IsError((TPersistedTypeBase::TLoadCode *)&v101) & 1) != 0 )
      {
        v103 = v101;
        goto LABEL_220;
      }
    }
  }
  if ( *(_QWORD *)(a2 + 40) )
    v101 = (*(__int64 (__fastcall **)(__int64, __int64, __int64, __int64, _QWORD, _QWORD, _QWORD, _QWORD, __int64 *))(a2 + 40))(
             a1,
             4LL,
             a2,
             a3,
             0LL,
             0LL,
             0LL,
             0LL,
             &v102);
  v103 = v101;
LABEL_220:
  _ReadStatusReg(ARM64_SYSREG(3, 3, 13, 0, 2));
  return v103;
}