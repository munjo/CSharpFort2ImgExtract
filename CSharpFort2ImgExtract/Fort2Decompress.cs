using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFort2ImgExtract
{
    public class Fort2Decompress
    {
        public static void DecompressStart(byte[] outMem, byte[] readMem, int readSize, int outSize)
        {
            int inMemMax;
            DecompressData data = new DecompressData();

            data.readMem = readMem;
            data.outSize = outSize;
            data.index = 0;
            data.readSize0 = readSize;
            data.outMemWriteOffset = 0;
            data.outMem = outMem;
            data.readSize1 = readSize;
            FUN_00402ea0(data);
            while (data.outSize != 0)
            {
                inMemMax = data.outSize;
                if (8192 < inMemMax)
                {
                    inMemMax = 8192;
                }
                FUN_00402ec0(data, inMemMax, data.inMem);
                OutMemoryCopy(data, data.inMem, inMemMax);
                data.outSize -= inMemMax;
            }
            return;
        }

        // 00402EA0
        static void FUN_00402ea0(DecompressData data)
        {
            data.uShort0 = 0;
            data.uShort1 = 0;
            data.short2 = 0;
            FUN_00402c00(data, 16);
            data.uShort3 = 0;
            data.short4 = 0;
            return;
        }

        // 00402C00
        static void FUN_00402c00(DecompressData data, short param_2)
        {
            data.uShort0 <<= param_2;
            if (data.short2 < param_2)
            {
                do
                {
                    param_2 -= data.short2;
                    data.uShort0 |= (ushort)(data.uShort1 << param_2);
                    if (data.readSize1 == 0)
                    {
                        data.uShort1 = 0;
                    }
                    else
                    {
                        data.readSize1--;
                        data.uShort1 = data.readMem[data.index];
                        data.index++;
                    }
                    data.short2 = 8;
                } while (8 < param_2);
            }
            data.short2 -= param_2;
            data.uShort0 |= (ushort)(data.uShort1 >> data.short2);
            return;
        }

        // 00402EC0
        static void FUN_00402ec0(DecompressData data, int outSize, byte[] inMem)
        {
            int uVar2;
            uint uVar3;
            uint readCount;

            readCount = 0;
            data.short4--;

            while (-1 < data.short4)
            {
                inMem[readCount] = inMem[data.DAT_0044f82c & 0xffff];
                readCount++;
                data.DAT_0044f82c = data.DAT_0044f82c & 0xffff0000 | (ushort)(data.DAT_0044f82c + 1) & 0xffff1fff;
                if (readCount == outSize)
                {
                    return;
                }
                data.short4--;
            }
            do
            {
                while (true)
                {
                    uVar2 = FUN_00404860(data);
                    if (255 < uVar2)
                        break;
                    inMem[readCount] = (byte)uVar2;
                    readCount++;
                    if (readCount == outSize)
                    {
                        return;
                    }
                }
                data.short4 = (short)(uVar2 - 253);
                uVar3 = FUN_00404930(data);
                data.DAT_0044f82c = data.DAT_0044f82c & 0xffff0000 | (ushort)(readCount - 1 - uVar3) & 0xffff1fff;
                data.short4--;
                while (-1 < data.short4)
                {
                    inMem[readCount] = inMem[data.DAT_0044f82c & 0xffff];
                    readCount++;
                    data.DAT_0044f82c = data.DAT_0044f82c & 0xffff0000 | (ushort)(data.DAT_0044f82c + 1) & 0xffff1fff;
                    if (readCount == outSize)
                    {
                        return;
                    }
                    data.short4--;
                }
            } while (true);
        }

        //00404860
        static int FUN_00404860(DecompressData data)
        {
            ushort uVar1;
            ushort uVar4;
            int local_4;

            if (data.uShort3 == 0)
            {
                data.uShort3 = (ushort)FUN_00402cb0(data, 16);
                FUN_00404590(data, 19, 5, 3);
                FUN_004046e0(data);
                FUN_00404590(data, 14, 4, -1);
            }
            uVar1 = data.uShort0;
            data.uShort3--;
            uVar4 = data.test2[uVar1 >> 4];

            local_4 = 8;
            while (509 < uVar4)
            {
                if ((uVar1 & local_4) == 0)
                {
                    uVar4 = data.test1[uVar4];
                }
                else
                {
                    uVar4 = data.test3[uVar4];
                }
                local_4 >>= 1;
            }
            FUN_00402c00(data, data.test4[uVar4]);
            return uVar4;
        }

        // 00404930
        static uint FUN_00404930(DecompressData data)
        {
            ushort uVar1;
            ushort uVar2;
            uint uVar3;
            int local_4;

            uVar3 = data.test0[data.uShort0 >> 8];
            if (13 < data.test0[data.uShort0 >> 8])
            {
                local_4 = 128;
                do
                {
                    if ((data.uShort0 & local_4) == 0)
                    {
                        uVar1 = data.test1[uVar3];
                    }
                    else
                    {
                        uVar1 = data.test3[uVar3];
                    }
                    uVar3 = uVar1;
                    local_4 >>= 1;
                } while (13 < uVar1);
            }
            uVar2 = 0;
            FUN_00402c00(data, data.test5[uVar3]);
            if ((short)uVar3 != 0)
            {
                uVar2 = (ushort)FUN_00402cb0(data, (short)(uVar3 - 1));
                uVar2 += (ushort)(1 << ((int)uVar3 - 1));
                uVar3 = (uint)uVar2;
            }
            return (uint)(uVar2 & 0xffff0000 | uVar3 & 0xffff);
        }

        //00404590
        static void FUN_00404590(DecompressData data, ushort param_2, short param_3, short param_4)
        {
            ushort uVar1;
            short sVar2;
            short sVar3;
            short sVar4;
            uint uVar5;
            ushort uVar6;
            int uVar7;
            int iVar8;
            ushort uVar10;
            short sVar11;

            sVar2 = param_4;
            uVar5 = FUN_00402cb0(data, param_3);
            sVar3 = (short)uVar5;
            if (sVar3 == 0) {
                uVar5 = FUN_00402cb0(data, param_3);
                for (iVar8 = 0; iVar8 < param_2; iVar8++) {
                    data.test5[iVar8] = 0;
                }
                for (iVar8 = 0; iVar8 < 256; iVar8++) {
                    data.test0[iVar8] = (ushort)uVar5;
                }
                return;
            }

            sVar11 = 0;
            while (sVar11 < sVar3)
            {
                uVar1 = data.uShort0;
                uVar10 = (ushort)(uVar1 >> 13);
                if (uVar10 == 7)
                {
                    param_4 = 4096;
                    uVar6 = (ushort)(uVar1 & 4096);
                    while (uVar6 != 0)
                    {
                        param_4 >>= 1;
                        uVar10++;
                        uVar6 = (ushort)(param_4 & uVar1);
                    }
                }
                if (uVar10 < 7)
                {
                    sVar4 = 3;
                }
                else
                {
                    sVar4 = (short)(uVar10 - 3);
                }
                FUN_00402c00(data, sVar4);
                data.test5[sVar11] = (byte)uVar10;
                sVar11++;
                if (sVar11 == sVar2)
                {
                    uVar5 = FUN_00402cb0(data, 2);
                    sVar4 = (short)(uVar5 - 1);

                    iVar8 = sVar4 + 1;
                    while (iVar8 != 0)
                    {
                        data.test5[sVar11] = 0;
                        sVar11++;
                        iVar8--;
                    }
                }
            }

            if (sVar11 < param_2)
            {
                uVar7 = param_2 - sVar11;
                for (uVar5 = 0; uVar5 < uVar7; uVar5++)
                {
                    data.test5[sVar11 + uVar5] = 0;
                }
            }
            FUN_00404a00(data, param_2, data.test5, 8, data.test0);
            return;
        }

        // 00404a00
        static void FUN_00404a00(DecompressData data, int param_2, byte[] param_3, int param_4, ushort[] param_5)
        {
            int a, b;
            ushort usVar1;
            int iVar1, iVar2, iVar3, iVar4;
            int conVar1, conVar2;
            ushort[] sArray1 = new ushort[18];
            ushort[] sArray2 = new ushort[18];
            // ushort* ptr;
            ushort ptrValue, ptrPos, ptrMode;

            conVar1 = param_2;

            for (a = 1; a < 17; a++) {
                sArray1[a] = 0;
            }

            for (a = 0; a < conVar1; a++) {
                sArray1[param_3[a]]++;
            }

            sArray2[1] = 0;
            for (a = 1; a <= 16; a++)
            {
                usVar1 = sArray1[a];
                usVar1 <<= (16 - a);
                usVar1 += sArray2[a];
                sArray2[a + 1] = usVar1;
            }

            iVar1 = param_4;
            conVar2 = 16 - iVar1;
            for (a = 1; a <= iVar1; a++)
            {
                sArray2[a] >>= conVar2;
                sArray1[a] = (ushort)(1 << (iVar1 - a));
            }
            if (a < 17)
            {
                iVar1 = 16 - a;
                iVar2 = 17 - a;
                for (b = 0; b < iVar2; b++)
                {
                    sArray1[a + b] = (ushort)(1 << iVar1);
                    iVar1--;
                }
            }

            iVar1 = sArray2[param_4 + 1] >> conVar2;
            usVar1 = (ushort)(1 << param_4);
            if (iVar1 != 0 && iVar1 != usVar1)
            {
                usVar1 -= (ushort)iVar1;
                for (a = 0; a < usVar1; a++)
                {
                    param_5[iVar1 + a] = 0;
                }
            }

            for (a = 0; a < conVar1; a++)
            {
                iVar1 = param_3[a];
                if (iVar1 != 0)
                {
                    iVar2 = sArray2[iVar1];
                    iVar3 = sArray1[iVar1] + iVar2;
                    if (param_4 < iVar1)
                    {
                        // 이부분은 포인터를 사용하지만
                        // unsafe를 사용하기 싫어서 변수로 구현
                        // ptr = &param_5[uVar11 >> bVar5];
                        ptrPos = 0;
                        ptrMode = 0;
                        for (b = iVar1 - param_4; b != 0; b--)
                        {
                            switch (ptrMode)
                            {
                                case 1:
                                    ptrValue = data.test1[ptrPos];
                                    break;
                                case 2:
                                    ptrValue = data.test3[ptrPos];
                                    break;
                                default:
                                    ptrValue = param_5[iVar2 >> conVar2];
                                    break;
                            }

                            // if(*ptr == 0)
                            if (ptrValue == 0)
                            {
                                data.test1[param_2] = 0;
                                data.test3[param_2] = 0;
                                // *ptr = (ushort)param_2;
                                switch (ptrMode)
                                {
                                    case 1:
                                        data.test1[ptrPos] = (ushort)param_2;
                                        ptrValue = data.test1[ptrPos];
                                        break;
                                    case 2:
                                        data.test3[ptrPos] = (ushort)param_2;
                                        ptrValue = data.test3[ptrPos];
                                        break;
                                    default:
                                        param_5[iVar2 >> conVar2] = (ushort)param_2;
                                        ptrValue = param_5[iVar2 >> conVar2];
                                        break;
                                }
                                param_2++;
                            }
                            if ((1 << (15 - param_4) & iVar2) == 0)
                            {
                                // ptr = &data.test1[ptr];
                                ptrMode = 1;
                            }
                            else
                            {
                                // ptr = &data.test2[ptr];
                                ptrMode = 2;
                            }
                            ptrPos = ptrValue;
                            iVar2 *= 2;
                        }
                        // *ptr = (ushort)a;
                        switch (ptrMode)
                        {
                            case 1:
                                data.test1[ptrPos] = (ushort)a;
                                break;
                            case 2:
                                data.test3[ptrPos] = (ushort)a;
                                break;
                            default:
                                param_5[iVar2 >> conVar2] = (ushort)a;
                                break;
                        }
                    }
                    else if (iVar2 < iVar3)
                    {
                        iVar4 = iVar3 - iVar2;
                        for (b = 0; b < iVar4; b++)
                        {
                            param_5[iVar2 + b] = (ushort)a;
                        }
                    }
                    sArray2[iVar1] = (ushort)iVar3;
                }
            }
            return;
        }



        // 004046e0
        static void FUN_004046e0(DecompressData data)
        {
            short sVar1;
            ushort uVar2;
            int iVar3;
            int uVar5;
            ushort uVar6;
            short sVar7;
            short sVar8;
            int local_4;

            uVar2 = FUN_00402cb0(data, 9);
            sVar7 = (short)uVar2;
            if (sVar7 != 0)
            {
                sVar8 = 0;
                while (sVar8 < sVar7)
                {
                    uVar6 = data.test0[data.uShort0 >> 8];
                    local_4 = 128;
                    while (18 < uVar6)
                    {
                        if ((local_4 & data.uShort0) == 0)
                        {
                            uVar6 = data.test1[uVar6];
                        }
                        else
                        {
                            uVar6 = data.test3[uVar6];
                        }
                        local_4 >>= 1;
                    }
                    FUN_00402c00(data, data.test5[uVar6]);
                    if ((short)uVar6 < 3)
                    {
                        if (uVar6 == 0)
                        {
                            sVar1 = 1;
                        }
                        else if (uVar6 == 1)
                        {
                            uVar2 = FUN_00402cb0(data, 4);
                            sVar1 = (short)(uVar2 + 3);
                        }
                        else
                        {
                            uVar2 = FUN_00402cb0(data, 9);
                            sVar1 = (short)(uVar2 + 20);
                        }

                        iVar3 = sVar1;
                        while (0 < iVar3)
                        {
                            data.test4[sVar8] = 0;
                            iVar3--;
                            sVar8++;
                        }
                    }
                    else
                    {
                        data.test4[sVar8] = (byte)(uVar6 - 2);
                        sVar8++;
                    }
                }

                if (sVar8 < 510)
                {
                    uVar5 = 510 - sVar8;
                    for (uVar2 = 0; uVar2 < uVar5; uVar2++)
                    {
                        data.test4[sVar8 + uVar2] = 0;
                    }
                }
                FUN_00404a00(data, 510, data.test4, 12, data.test2);
                return;
            }
            uVar2 = FUN_00402cb0(data, 9);
            for (iVar3 = 0; iVar3 < 510; iVar3++)
            {
                data.test4[iVar3] = 0;
            }

            for (iVar3 = 0; iVar3 < 4096; iVar3++)
            {
                data.test2[iVar3] = (ushort)uVar2;
            }
            return;
        }

        // 00402CB0
        static ushort FUN_00402cb0(DecompressData data, short param_2)
        {
            ushort sVar1;

            sVar1 = (ushort)data.uShort0;
            sVar1 >>= (ushort)(16 - param_2);
            FUN_00402c00(data, param_2);
            return sVar1;
        }

        // 00402E20
        static void OutMemoryCopy(DecompressData data, byte[] inMem, int size)
        {
            Array.Copy(inMem, 0, data.outMem, data.outMemWriteOffset, size);

            data.outMemWriteOffset += size;
            return;
        }
    }
}
