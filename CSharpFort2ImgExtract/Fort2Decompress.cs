using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFort2ImgExtract
{
    public class Fort2Decompress
    {

        //=====================================테스트
        private static int[] test0;
        private static int[] test1;
        private static int testValue;
        private static int testCount;
        //===========================================
        public static void DecompressStart(byte[] outMem, byte[] readMem, int readSize, int outSize)
        {
            int inMemMax;
            DecompressData data = new DecompressData();

            data.readMem = readMem;
            data.outSize = outSize;
            data.index = 0;
            data.readSize = readSize;
            data.outMemWriteOffset = 0;
            data.outMem = outMem;
            data.remainReadData = readSize;
            InitDictionary(data);
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
        /// <summary>
        /// 초기 사전(Dictionary) 생성
        /// </summary>
        /// <param name="data"></param>
        static void InitDictionary(DecompressData data)
        {
            data.currentBitValue = 0;
            data.nextBitValue = 0;
            data.remainingBits = 0;
            CurrentBitProgress(data, 16);
            data.flags = 0;
            data.windowOffset = 0;

            //=====================================테스트
            test0 = new int[510];
            test1 = Enumerable.Repeat(-1, 510).ToArray();
            testCount = 0;
            //===========================================

            return;
        }

        // 00402EC0
        static void FUN_00402ec0(DecompressData data, int outSize, byte[] inMem)
        {
            uint uVar2;
            uint uVar3;
            uint readCount;

            readCount = 0;
            data.windowOffset--;

            while (-1 < data.windowOffset)
            {
                inMem[readCount] = inMem[data.DAT_0044f82c];
                data.DAT_0044f82c = ++data.DAT_0044f82c % 8192;
                readCount++;
                if (readCount == outSize)
                {
                    return;
                }
                data.windowOffset--;
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
                // 사전에 있는 값이 기본 정의된 값(255)보다 클때
                data.windowOffset = (short)(uVar2 - 253);
                uVar3 = FUN_00404930(data);
                data.DAT_0044f82c = (readCount - 1 - uVar3) % 8192;
                data.windowOffset--;
                while (-1 < data.windowOffset)
                {
                    inMem[readCount] = inMem[data.DAT_0044f82c];
                    data.DAT_0044f82c = ++data.DAT_0044f82c % 8192;
                    readCount++;
                    if (readCount == outSize)
                    {
                        return;
                    }
                    data.windowOffset--;
                }
            } while (true);
        }

        //00404860
        /// <summary>
        /// 디코딩 테이블 생성
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static uint FUN_00404860(DecompressData data)
        {
            ushort uVar1;
            ushort uVar4;
            int local_4;

            if (data.flags == 0)
            {
                data.flags = (ushort)OutCurrentBitProgress(data, 16);
                FUN_00404590(data, 19, 5, 3);
                FUN_004046e0(data);
                FUN_00404590(data, 14, 4, -1);
            }
            uVar1 = data.currentBitValue;
            data.flags--;
            uVar4 = data.dataHighTree[uVar1 >> 4];

            local_4 = 8;
            while (509 < uVar4)
            {
                if ((uVar1 & local_4) == 0)
                {
                    uVar4 = data.lowTreeValue0[uVar4];
                }
                else
                {
                    uVar4 = data.lowTreeValue1[uVar4];
                }
                local_4 >>= 1;
            }

            test0[uVar4]++;
            testValue = uVar1 >> (16 - data.dataTreeSizeTable[uVar4]);
            if (test1[uVar4] != testValue)
            {
                if(test1[uVar4] != -1)
                {
                    Console.WriteLine("test1[{0}] changed", uVar4);
                }
                
                test1[uVar4] = testValue;
            }

            CurrentBitProgress(data, data.dataTreeSizeTable[uVar4]);
            //=====================================테스트
            if (data.flags == 0)
            {
                Console.WriteLine("---------------- flags == 0 ----------------");
                //Console.WriteLine("------------------- test0-{0} -----------------", testCount);
                //for (int i = 0; i < 510; i++)
                //{
                //    Console.WriteLine("{0}", test0[i]);
                //}
                //Console.WriteLine("------------------- test1 ------------------");
                //for (int i = 0; i < 510; i++)
                //{
                //    if (test1[i] == -1)
                //    {
                //        Console.WriteLine("X");
                //    }
                //    else
                //    {
                //        Console.WriteLine("{0}", Convert.ToString(test1[i], 2));
                //    }
                //}
                Console.WriteLine("--------------------------------------------");

                test0 = new int[510];
                test1 = Enumerable.Repeat(-1, 510).ToArray();

                testCount++;
            }
            //===========================================

            return uVar4;
        }

        // 00404930
        /// <summary>
        /// 반복되는 문자열의 위치로 되돌아가기 위해 테이블에서 값을 가져옴
        /// </summary>
        /// <param name="data"></param>
        /// <returns>뒤로 되돌아갈 개수</returns>
        static uint FUN_00404930(DecompressData data)
        {
            ushort uVar1;
            ushort uVar2;
            uint uVar3;
            int local_4;

            uVar3 = data.backLocationHighTree[data.currentBitValue >> 8];
            if (13 < uVar3)
            {
                local_4 = 128;
                do
                {
                    if ((data.currentBitValue & local_4) == 0)
                    {
                        uVar1 = data.lowTreeValue0[uVar3];
                    }
                    else
                    {
                        uVar1 = data.lowTreeValue1[uVar3];
                    }
                    uVar3 = uVar1;
                    local_4 >>= 1;
                } while (13 < uVar1);
            }
            uVar2 = 0;
            CurrentBitProgress(data, data.backLocationTreeSizeTable[uVar3]);
            if ((short)uVar3 != 0)
            {
                uVar2 = (ushort)OutCurrentBitProgress(data, (short)(uVar3 - 1));
                uVar2 += (ushort)(1 << ((int)uVar3 - 1));
            }
            return uVar2;
        }

        //00404590
        /// <summary>
        /// 허프만 알고리즘을 사용하여 한번 압축해제한다.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="param_2">test5 배열의 길이</param>
        /// <param name="param_3">데이터 비트 수</param>
        /// <param name="param_4">초기 비트 수</param>
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
            uVar5 = OutCurrentBitProgress(data, param_3);
            sVar3 = (short)uVar5;

            // 해당 메소드가 2번째로 호출될 때에 OutCurrentBitProgress 메소드의 반환값이 0일 경우가 있음
            // OutCurrentBitProgress 메소드의 반환값이 0이라면
            // 뒤로 이동하는 값이 한가지 밖에 없다는 뜻
            if (sVar3 == 0) {
                // 뒤로 이동할 값을 받아온 후에
                uVar5 = OutCurrentBitProgress(data, param_3);
                // backLocationTreeSizeTable 배열은 사용할 필요가 없으니 모두 0으로 초기화
                for (iVar8 = 0; iVar8 < param_2; iVar8++)
                {
                    data.backLocationTreeSizeTable[iVar8] = 0;
                }
                // backLocationHighTree 배열의 값을 모두 방금 받아온 뒤로 이동할 값으로 초기화
                for (iVar8 = 0; iVar8 < 256; iVar8++) 
                {
                    data.backLocationHighTree[iVar8] = (ushort)uVar5;
                }
                return;
            }

            // 가져온 횟수 만큼 데이터 가져오기
            sVar11 = 0;
            while (sVar11 < sVar3)
            {
                uVar1 = data.currentBitValue;
                // 맨 앞 3개의 비트만 확인
                uVar10 = (ushort)(uVar1 >> 13);

                // 입력된 비트 데이터가 00000111일때
                if (uVar10 == 7)
                {
                    param_4 = 4096;

                    // 00010000 00000000 비트부터 0인 비트 찾기
                    uVar6 = (ushort)(uVar1 & 4096);
                    while (uVar6 != 0)
                    {
                        param_4 >>= 1;
                        uVar10++;
                        uVar6 = (ushort)(param_4 & uVar1);
                    }

                    sVar4 = (short)(uVar10 - 3);
                }
                // 입력된 비트 데이터가 00000111가 아닐때
                else
                {
                    sVar4 = 3;
                }
                // 입력된 데이터에서 비트 수 만큼 데이터 추출
                CurrentBitProgress(data, sVar4);
                data.backLocationTreeSizeTable[sVar11] = (byte)uVar10;
                sVar11++;

                if (sVar11 == sVar2)
                {
                    sVar4 = (short)OutCurrentBitProgress(data, 2);

                    iVar8 = sVar4;
                    while (iVar8 != 0)
                    {
                        data.backLocationTreeSizeTable[sVar11] = 0;
                        sVar11++;
                        iVar8--;
                    }
                }
            }

            // 만약 sVar11이 param_2보다 작을 경우, 남은 공간을 0으로 채워줍니다.
            if (sVar11 < param_2)
            {
                uVar7 = param_2 - sVar11;
                for (uVar5 = 0; uVar5 < uVar7; uVar5++)
                {
                    data.backLocationTreeSizeTable[sVar11 + uVar5] = 0;
                }
            }
            // 마지막으로 FUN_00404a00() 함수를 호출하여 decompressedDataBuffer
            // 배열을 translationTable 배열을 이용하여 원래의 데이터로 변환합니다.
            FUN_00404a00(data, param_2, data.backLocationTreeSizeTable, 8, data.backLocationHighTree);
            return;
        }

        // 00404a00
        /// <summary>
        /// 디코딩 테이블 업데이트
        /// </summary>
        /// <param name="data"></param>
        /// <param name="param_2"></param>
        /// <param name="param_3"></param>
        /// <param name="param_4"></param>
        /// <param name="param_5"></param>
        static void FUN_00404a00(DecompressData data, int param_2, byte[] param_3, int param_4, ushort[] param_5)
        {
            int a, b;
            ushort usVar1;
            int iVar1, iVar2, iVar3, iVar4;
            int conVar1, conVar2;
            ushort[] sArray1 = new ushort[17];
            ushort[] sArray2 = new ushort[18];
            // ushort* ptr;
            ushort[] ptrArray;
            ushort ptrPos;

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
                        // ptr = &param_5[iVar2 >> conVar2];
                        ptrPos = (ushort)(iVar2 >> conVar2);
                        ptrArray = param_5;
                        for (b = iVar1 - param_4; b != 0; b--)
                        {
                            // if(*ptr == 0)
                            if (ptrArray[ptrPos] == 0)
                            {
                                data.lowTreeValue0[param_2] = 0;
                                data.lowTreeValue1[param_2] = 0;
                                // *ptr = (ushort)param_2;
                                ptrArray[ptrPos] = (ushort)param_2;
                                param_2++;
                            }
                            if ((1 << (15 - param_4) & iVar2) == 0)
                            {
                                // ptr = &data.lowTreeValue0[*ptr];
                                ptrPos = ptrArray[ptrPos];
                                ptrArray = data.lowTreeValue0;
                            }
                            else
                            {
                                // ptr = &data.lowTreeValue1[*ptr];
                                ptrPos = ptrArray[ptrPos];
                                ptrArray = data.lowTreeValue1;
                            }
                            iVar2 *= 2;
                        }
                        // *ptr = (ushort)a;
                        ptrArray[ptrPos] = (ushort)a;
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

            uVar2 = OutCurrentBitProgress(data, 9);
            sVar7 = (short)uVar2;
            if (sVar7 != 0)
            {
                sVar8 = 0;
                while (sVar8 < sVar7)
                {
                    uVar6 = data.backLocationHighTree[data.currentBitValue >> 8];
                    local_4 = 128;
                    while (18 < uVar6)
                    {
                        if ((local_4 & data.currentBitValue) == 0)
                        {
                            uVar6 = data.lowTreeValue0[uVar6];
                        }
                        else
                        {
                            uVar6 = data.lowTreeValue1[uVar6];
                        }
                        local_4 >>= 1;
                    }
                    CurrentBitProgress(data, data.backLocationTreeSizeTable[uVar6]);
                    if ((short)uVar6 < 3)
                    {
                        if (uVar6 == 0)
                        {
                            sVar1 = 1;
                        }
                        else if (uVar6 == 1)
                        {
                            uVar2 = OutCurrentBitProgress(data, 4);
                            sVar1 = (short)(uVar2 + 3);
                        }
                        else
                        {
                            uVar2 = OutCurrentBitProgress(data, 9);
                            sVar1 = (short)(uVar2 + 20);
                        }

                        iVar3 = sVar1;
                        while (0 < iVar3)
                        {
                            data.dataTreeSizeTable[sVar8] = 0;
                            iVar3--;
                            sVar8++;
                        }
                    }
                    else
                    {
                        data.dataTreeSizeTable[sVar8] = (byte)(uVar6 - 2);
                        sVar8++;
                    }
                }

                if (sVar8 < 510)
                {
                    uVar5 = 510 - sVar8;
                    for (uVar2 = 0; uVar2 < uVar5; uVar2++)
                    {
                        data.dataTreeSizeTable[sVar8 + uVar2] = 0;
                    }
                }
                FUN_00404a00(data, 510, data.dataTreeSizeTable, 12, data.dataHighTree);
                return;
            }
            uVar2 = OutCurrentBitProgress(data, 9);
            for (iVar3 = 0; iVar3 < 510; iVar3++)
            {
                data.dataTreeSizeTable[iVar3] = 0;
            }

            for (iVar3 = 0; iVar3 < 4096; iVar3++)
            {
                data.dataHighTree[iVar3] = (ushort)uVar2;
            }
            return;
        }

        // 00402CB0
        /// <summary>
        /// 압축 데이터에서 다음 N 비트를 읽어와 반환합니다.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="numberReadBits">읽어올 비트 수</param>
        /// <returns>읽어온 비트 값을 ushort 타입으로 반환</returns>
        static ushort OutCurrentBitProgress(DecompressData data, short numberReadBits)
        {
            ushort sVar1;

            // uShort0은 현재 읽어온 비트 값이 들어있는 변수입니다.
            // FUN_00402c00 함수를 호출해 uShort0에서 param_2 비트를 제외한 나머지 비트를 제거합니다.
            sVar1 = (ushort)data.currentBitValue;
            // sVar1에 현재 읽어온 비트 값 중 다음 N 비트를 할당합니다.
            sVar1 >>= (ushort)(16 - numberReadBits);
            // 다음 비트를 읽기 위해 FUN_00402c00 함수를 호출합니다.
            CurrentBitProgress(data, numberReadBits);
            return sVar1;
        }

        // 00402C00
        /// <summary>
        /// 압축 데이터의 비트를 읽어와서 DecompressData 클래스에 저장하는 역할을 합니다.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="numberReadBits">읽어올 비트 수</param>
        static void CurrentBitProgress(DecompressData data, short numberReadBits)
        {
            data.currentBitValue <<= numberReadBits;
            if (data.remainingBits < numberReadBits)
            {
                do
                {
                    numberReadBits -= data.remainingBits;
                    data.currentBitValue |= (ushort)(data.nextBitValue << numberReadBits);
                    if (data.remainReadData == 0)
                    {
                        data.nextBitValue = 0;
                    }
                    else
                    {
                        data.remainReadData--;
                        data.nextBitValue = data.readMem[data.index];
                        data.index++;
                    }
                    data.remainingBits = 8;
                } while (8 < numberReadBits);
            }
            data.remainingBits -= numberReadBits;
            data.currentBitValue |= (ushort)(data.nextBitValue >> data.remainingBits);
            return;
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
