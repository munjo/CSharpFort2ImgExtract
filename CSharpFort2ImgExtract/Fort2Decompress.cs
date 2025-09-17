using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 압축방식은 deflate 압축방식으로
 LSZZ 압축으로 한번 압축후
 허프만 압축으로 구간마다 한번더 압축한다.
 */

namespace CSharpFort2ImgExtract
{
    public class Fort2Decompress
    {
        /// <summary>
        /// DEFLATE 압축 해제 프로세스 시작 메서드
        /// </summary>
        /// <param name="outputBuffer">출력 데이터 저장 버퍼</param>
        /// <param name="compressedDataBuffer">압축된 입력 데이터 버퍼</param>
        /// <param name="compressedDataLength">압축 데이터 길이</param>
        /// <param name="decompressedDataLength">출력 데이터 길이</param>
        public static void StartDecompression(byte[] outputBuffer, byte[] compressedDataBuffer, int compressedDataLength, int decompressedDataLength)
        {
            int decompressionChunkSize;
            DecompressionContext context = new DecompressionContext();

            // 컨텍스트 초기화
            context.compressedData = compressedDataBuffer;
            context.remainingDecompressedSize = decompressedDataLength;
            context.currentInputPosition = 0;
            context.totalCompressedSize = compressedDataLength;
            context.outMemWriteOffset = 0;
            context.decompressedOutput = outputBuffer;
            context.remainReadData = compressedDataLength;
            InitializeDecompressionContext(context);

            // 청크 단위 압축 해제 처리
            while (context.remainingDecompressedSize != 0)
            {
                decompressionChunkSize = context.remainingDecompressedSize;
                decompressionChunkSize = Math.Min(decompressionChunkSize, 8192);

                ProcessDecompressionChunk(context, decompressionChunkSize, context.slidingWindowBuffer);
                OutMemoryCopy(context, context.slidingWindowBuffer, decompressionChunkSize);
                context.remainingDecompressedSize -= decompressionChunkSize;
            }
            return;
        }

        // 00402EA0
        /// <summary>
        /// 콘텍스트 초기화
        /// </summary>
        /// <param name="context"></param>
        static void InitializeDecompressionContext(DecompressionContext context)
        {
            context.currentBitBuffer = 0;
            context.nextBitValue = 0;
            context.availableBits = 0;
            FillBitBuffer(context, 16);
            context.remainingTreeFlags = 0;
            context.windowOffset = 0;

            return;
        }

        // 00402EC0
        /// <summary>
        /// 8KB 청크 단위 압축 해제 처리 메인 루틴
        /// </summary>
        /// <param name="context"></param>
        /// <param name="remainingOutputSize">남은 출력 데이터 크기</param>
        /// <param name="outputBuffer">출력 데이터 버퍼</param>
        static void ProcessDecompressionChunk(DecompressionContext context, int remainingOutputSize, byte[] outputBuffer)
        {
            uint decodedSymbol;
            uint backReferenceDistance;
            uint currentOutputPosition;

            currentOutputPosition = 0;
            context.windowOffset--;

            while (-1 < context.windowOffset)
            {
                outputBuffer[currentOutputPosition] = outputBuffer[context.windowHeadPosition];
                context.windowHeadPosition = ++context.windowHeadPosition % 8192;
                currentOutputPosition++;
                if (currentOutputPosition == remainingOutputSize)
                {
                    return;
                }
                context.windowOffset--;
            }
            do
            {
                while (true)
                {
                    decodedSymbol = DecodeHuffmanSymbol(context);
                    if (255 < decodedSymbol)
                        break;
                    outputBuffer[currentOutputPosition] = (byte)decodedSymbol;
                    currentOutputPosition++;
                    if (currentOutputPosition == remainingOutputSize)
                    {
                        return;
                    }
                }
                // 사전에 있는 값이 기본 정의된 값(255)보다 클때
                context.windowOffset = (short)(decodedSymbol - 253);
                backReferenceDistance = DecodeDistanceValue(context);
                context.windowHeadPosition = (uint)(currentOutputPosition - 1 - backReferenceDistance) % 8192;
                context.windowOffset--;
                while (-1 < context.windowOffset)
                {
                    outputBuffer[currentOutputPosition] = outputBuffer[context.windowHeadPosition];
                    context.windowHeadPosition = ++context.windowHeadPosition % 8192;
                    currentOutputPosition++;
                    if (currentOutputPosition == remainingOutputSize)
                    {
                        return;
                    }
                    context.windowOffset--;
                }
            } while (true);
        }

        //00404860
        /// <summary>
        /// 허프만 트리를 사용하여 압축된 데이터에서 심볼 디코딩
        /// </summary>
        /// <param name="context"></param>
        /// <returns>디코딩된 심볼 값(0-255: 리터럴 바이트, 256+: 길이 코드)</returns>
        static uint DecodeHuffmanSymbol(DecompressionContext context)
        {
            ushort bitWindow;
            ushort currentNode;
            int bitMask;

            // 허프만 트리 초기화 체크
            if (context.remainingTreeFlags == 0)
            {
                context.remainingTreeFlags = (ushort)ExtractBitsFromBuffer(context, 16);
                BuildHuffmanTree(context, 19, 5, 3);
                InitializeMainHuffmanTree(context);
                BuildHuffmanTree(context, 14, 4, -1);
            }

            // 비트 버퍼에서 12비트 윈도우 추출
            bitWindow = context.currentBitBuffer;
            context.remainingTreeFlags--;

            // 허프만 트리 탐색 시작
            currentNode = context.literalRootNodes[bitWindow >> 4];
            bitMask = 8;

            // 리프 노드 도달 시까지 트리 탐색
            while (509 < currentNode)
            {
                // 현재 비트 확인 (0: 좌측, 1: 우측 자식 노드)
                if ((bitWindow & bitMask) == 0)
                {
                    currentNode = context.leftChildNodes[currentNode];
                }
                else
                {
                    currentNode = context.rightChildNodes[currentNode];
                }
                bitMask >>= 1; // 다음 비트로 이동
            }

            FillBitBuffer(context, context.literalLengthCodeSizes[currentNode]);

            return currentNode;
        }

        // 00404930
        /// <summary>
        /// 허프만 트리를 사용하여 역참조 거리 디코딩
        /// </summary>
        /// <param name="context"></param>
        /// <returns>역참조 거리 값</returns>
        static uint DecodeDistanceValue(DecompressionContext context)
        {
            ushort distanceValue;
            ushort currentNode;
            int bitMask;
            int extraBits;

            currentNode = context.distanceRootNodes[context.currentBitBuffer >> 8];

            // 허프만 트리 탐색 루프 (13: 최대 허용 트리 깊이)
            bitMask = 128;

            while (currentNode > 13)
            {
                // 현재 비트 검사 (0: 좌측, 1: 우측 자식 노드)
                if ((context.currentBitBuffer & bitMask) == 0)
                {
                    currentNode = context.leftChildNodes[currentNode];
                }
                else
                {
                    currentNode = context.rightChildNodes[currentNode];
                }
                bitMask >>= 1;
            }
            distanceValue = 0;
            FillBitBuffer(context, context.distanceCodeSizes[currentNode]);
            if (currentNode != 0)
            {
                extraBits = currentNode - 1;
                distanceValue = (ushort)ExtractBitsFromBuffer(context, (short)extraBits);
                distanceValue += (ushort)(1 << extraBits);
            }
            return distanceValue;
        }

        //00404590
        /// <summary>
        /// 허프만 트리 구성 및 코드 길이 테이블 초기화
        /// </summary>
        /// <param name="context"></param>
        /// <param name="codeCount">처리할 코드 수</param>
        /// <param name="bitsPerCode">코드당 비트 수</param>
        /// <param name="initialBits">초기 비트 수</param>
        static void BuildHuffmanTree(DecompressionContext context, ushort codeCount, short bitsPerCode, short initialBits)
        {
            ushort bitWindow;
            short bitMask;
            short validCodeCount;
            short bitsToConsume;
            uint codeLengthInfo;
            int remaining;
            int remainingCodes;
            ushort codeLength;
            short currentIndex;

            // 코드 길이 정보 추출
            codeLengthInfo = ExtractBitsFromBuffer(context, bitsPerCode);
            validCodeCount = (short)codeLengthInfo;

            // 해당 메소드가 2번째로 호출될 때에 OutCurrentBitProgress 메소드의 반환값이 0일 경우가 있음
            // OutCurrentBitProgress 메소드의 반환값이 0이라면
            // 뒤로 이동하는 값이 한가지 밖에 없다는 뜻
            if (validCodeCount == 0) {
                // 뒤로 이동할 값을 받아온 후에
                codeLengthInfo = ExtractBitsFromBuffer(context, bitsPerCode);
                // 코드 길이 테이블 초기화
                for (int i = 0; i < codeCount; i++)
                {
                    context.distanceCodeSizes[i] = 0;
                }
                // backLocationHighTree 배열의 값을 모두 방금 받아온 뒤로 이동할 값으로 초기화
                for (int i = 0; i < 256; i++) 
                {
                    context.distanceRootNodes[i] = (ushort)codeLengthInfo;
                }
                return;
            }

            // 가져온 횟수 만큼 데이터 가져오기
            currentIndex = 0;
            while (currentIndex < validCodeCount)
            {
                bitWindow = context.currentBitBuffer;
                // 맨 앞 3개의 비트만 확인
                codeLength = (ushort)(bitWindow >> 13);

                // 입력된 비트 데이터가 00000111일때
                if (codeLength == 0b111)
                {
                    bitMask = 4096;

                    // 00010000 00000000 비트부터 0인 비트 찾기
                    while ((bitWindow & bitMask) != 0)
                    {
                        bitMask >>= 1;
                        if (bitMask == 0)
                        {
                            throw new InvalidOperationException("Invalid code length");
                        }

                        codeLength++;
                    }

                    bitsToConsume = (short)(codeLength - 3);
                }
                // 입력된 비트 데이터가 00000111가 아닐때
                else
                {
                    bitsToConsume = 3;
                }
                // 입력된 데이터에서 비트 수 만큼 데이터 추출
                FillBitBuffer(context, bitsToConsume);
                context.distanceCodeSizes[currentIndex] = (byte)codeLength;
                currentIndex++;

                if (currentIndex == initialBits)
                {
                    remainingCodes = (short)ExtractBitsFromBuffer(context, 2);

                    while (remainingCodes != 0)
                    {
                        context.distanceCodeSizes[currentIndex] = 0;
                        currentIndex++;
                        remainingCodes--;
                    }
                }
            }

            // 만약 sVar11이 param_2보다 작을 경우, 남은 공간을 0으로 채워줍니다.
            if (currentIndex < codeCount)
            {
                remaining = codeCount - currentIndex;
                for (int i = 0; i < remaining; i++)
                {
                    context.distanceCodeSizes[currentIndex + i] = 0;
                }
            }
            // 마지막으로 FUN_00404a00() 함수를 호출하여 decompressedDataBuffer
            // 배열을 translationTable 배열을 이용하여 원래의 데이터로 변환합니다.
            BuildDecodingTables(context, codeCount, context.distanceCodeSizes, 8, context.distanceRootNodes);
            return;
        }

        // 00404a00
        /// <summary>
        /// 디코딩 테이블 업데이트
        /// </summary>
        /// <param name="context"></param>
        /// <param name="codeCount">처리할 코드 수</param>
        /// <param name="codeLengths">코드 길이 배열</param>
        /// <param name="maxBits">최대 비트 수</param>
        /// <param name="rootNodes">루트 노드 배열</param>
        static void BuildDecodingTables(DecompressionContext context, int codeCount, byte[] codeLengths, int maxBits, ushort[] rootNodes)
        {
            int cLength;
            ushort nextCode;
            int rootCheck, currentNodeIndex;
            ushort maxCode;
            int bitShift;
            ushort[] codeRange = new ushort[17];
            ushort[] codeValues = new ushort[18];
            // ushort* ptr;
            ushort[] currentTree;
            ushort currentPos;

            // 1. 코드 길이별 빈도수 계산
            for (int i = 1; i < 17; i++) {
                codeRange[i] = 0;
            }

            for (int symbol = 0; symbol < codeCount; symbol++) {
                codeRange[codeLengths[symbol]]++;
            }

            // 2. 코드 범위 계산
            codeValues[1] = 0;
            for (int i = 1; i <= 16; i++)
            {
                nextCode = codeRange[i];
                nextCode <<= (16 - i);
                nextCode += codeValues[i];
                codeValues[i + 1] = nextCode;
            }

            // 3. 비트 시프트 연산 준비
            bitShift = 16 - maxBits;
            for (cLength = 1; cLength <= maxBits; cLength++)
            {
                codeValues[cLength] >>= bitShift;
                codeRange[cLength] = (ushort)(1 << (maxBits - cLength));
            }

            // 4. 남는 코드 범위 채우기
            if (cLength < 17)
            {
                int remainingBits = 16 - cLength;
                int remainCount = 17 - cLength;
                for (int i = 0; i < remainCount; i++)
                {
                    codeRange[cLength + i] = (ushort)(1 << remainingBits);
                    remainingBits--;
                }
            }

            // 5. 초기 노드 검증 및 초기화
            rootCheck = codeValues[maxBits + 1] >> bitShift;
            maxCode = (ushort)(1 << maxBits);
            if (rootCheck != 0 && rootCheck != maxCode)
            {
                ushort fillCount = (ushort)(maxCode - rootCheck);
                for (int i = 0; i < fillCount; i++)
                {
                    rootNodes[rootCheck + i] = 0;
                }
            }

            // 6. 디코딩 테이블 생성 메인 루프
            currentNodeIndex = codeCount;
            for (int symbol = 0; symbol < codeCount; symbol++)
            {
                cLength = codeLengths[symbol];
                if (cLength != 0)
                {
                    int startCode = codeValues[cLength];
                    int endCode = codeRange[cLength] + startCode;

                    // 7. 코드 길이에 따른 분기 처리
                    if (maxBits < cLength)
                    {
                        // 이부분은 포인터를 사용하지만
                        // unsafe를 사용하기 싫어서 변수로 구현
                        // ptr = &param_5[iVar2 >> conVar2];
                        currentPos = (ushort)(startCode >> bitShift);
                        currentTree = rootNodes;

                        // 8. 트리 깊이 확장
                        for (int depth = cLength - maxBits; depth != 0; depth--)
                        {
                            // if(*ptr == 0)
                            if (currentTree[currentPos] == 0)
                            {
                                context.leftChildNodes[currentNodeIndex] = 0;
                                context.rightChildNodes[currentNodeIndex] = 0;
                                // *ptr = (ushort)iVar5;
                                currentTree[currentPos] = (ushort)currentNodeIndex;
                                currentNodeIndex++;
                            }

                            // 9. 비트 패턴에 따라 트리 분기
                            if ((1 << (15 - maxBits) & startCode) == 0)
                            {
                                // ptr = &data.lowTreeValue0[*ptr];
                                currentPos = currentTree[currentPos];
                                currentTree = context.leftChildNodes;
                            }
                            else
                            {
                                // ptr = &data.lowTreeValue1[*ptr];
                                currentPos = currentTree[currentPos];
                                currentTree = context.rightChildNodes;
                            }
                            startCode *= 2;
                        }
                        // *ptr = (ushort)a;
                        currentTree[currentPos] = (ushort)symbol;
                    }
                    else if (startCode < endCode)
                    {
                        // 10. 직접 코드 할당
                        int rangeSize = endCode - startCode;
                        for (int i = 0; i < rangeSize; i++)
                        {
                            rootNodes[startCode + i] = (ushort)symbol;
                        }
                    }
                    codeValues[cLength] = (ushort)endCode;
                }
            }
            return;
        }



        // 004046e0
        /// <summary>
        /// 메인 허프만 트리 초기화 및 코드 길이 테이블 구성
        /// </summary>
        /// <param name="context"></param>
        static void InitializeMainHuffmanTree(DecompressionContext context)
        {
            short repeatCount;
            ushort codeLengthInfo;
            ushort currentNode;
            short validCodeCount;
            short currentIndex;
            int bitMask;

            // 9비트로 유효 코드 수 추출
            codeLengthInfo = ExtractBitsFromBuffer(context, 9);
            validCodeCount = (short)codeLengthInfo;

            if (validCodeCount != 0)
            {
                currentIndex = 0;
                while (currentIndex < validCodeCount)
                {
                    // 허프만 트리 탐색 시작
                    currentNode = context.distanceRootNodes[context.currentBitBuffer >> 8];
                    bitMask = 128;

                    // 트리 리프 노드 탐색 루프
                    while (18 < currentNode)
                    {
                        if ((bitMask & context.currentBitBuffer) == 0)
                        {
                            currentNode = context.leftChildNodes[currentNode];
                        }
                        else
                        {
                            currentNode = context.rightChildNodes[currentNode];
                        }
                        bitMask >>= 1;
                    }

                    // 코드 길이 정보 갱신
                    FillBitBuffer(context, context.distanceCodeSizes[currentNode]);

                    // 특수 코드 처리 (0-2)
                    if ((short)currentNode < 3)
                    {
                        if (currentNode == 0)
                        {
                            repeatCount = 1;
                        }
                        else if (currentNode == 1)
                        {
                            codeLengthInfo = ExtractBitsFromBuffer(context, 4);
                            repeatCount = (short)(codeLengthInfo + 3);
                        }
                        else
                        {
                            codeLengthInfo = ExtractBitsFromBuffer(context, 9);
                            repeatCount = (short)(codeLengthInfo + 20);
                        }

                        // 반복 패턴 처리
                        while (0 < repeatCount)
                        {
                            context.literalLengthCodeSizes[currentIndex] = 0;
                            repeatCount--;
                            currentIndex++;
                        }
                    }
                    else
                    {
                        // 일반 코드 길이 저장
                        context.literalLengthCodeSizes[currentIndex] = (byte)(currentNode - 2);
                        currentIndex++;
                    }
                }

                // 남는 공간 제로 패딩
                if (currentIndex < 510)
                {
                    int remaining = 510 - currentIndex;
                    for (int i = 0; i < remaining; i++)
                    {
                        context.literalLengthCodeSizes[currentIndex + i] = 0;
                    }
                }

                // 디코딩 테이블 생성
                BuildDecodingTables(context, 510, context.literalLengthCodeSizes, 12, context.literalRootNodes);
                return;
            }

            // 단일 코드 길이 특수 처리
            codeLengthInfo = ExtractBitsFromBuffer(context, 9);
            for (int i = 0; i < 510; i++)
            {
                context.literalLengthCodeSizes[i] = 0;
            }

            // 전체 트리 노드 동일 값 설정
            for (int i = 0; i < 4096; i++)
            {
                context.literalRootNodes[i] = (ushort)codeLengthInfo;
            }
            return;
        }

        // 00402CB0
        /// <summary>
        /// 압축 데이터에서 다음 N 비트를 읽어와 반환합니다.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="numberExtractBits">읽어올 비트 수</param>
        /// <returns>읽어온 비트 값을 ushort 타입으로 반환</returns>
        static ushort ExtractBitsFromBuffer(DecompressionContext context, short numberExtractBits)
        {
            ushort value;

            // uShort0은 현재 읽어온 비트 값이 들어있는 변수입니다.
            // FUN_00402c00 함수를 호출해 uShort0에서 param_2 비트를 제외한 나머지 비트를 제거합니다.
            value = (ushort)context.currentBitBuffer;
            // sVar1에 현재 읽어온 비트 값 중 다음 N 비트를 할당합니다.
            value >>= (ushort)(16 - numberExtractBits);
            // 다음 비트를 읽기 위해 FUN_00402c00 함수를 호출합니다.
            FillBitBuffer(context, numberExtractBits);
            return value;
        }

        // 00402C00
        /// <summary>
        /// 압축 데이터의 비트를 읽어와서 DecompressData 클래스에 저장하는 역할을 합니다.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="numberExtractBits">읽어올 비트 수</param>
        static void FillBitBuffer(DecompressionContext context, short numberExtractBits)
        {
            context.currentBitBuffer <<= numberExtractBits;
            if (context.availableBits < numberExtractBits)
            {
                do
                {
                    numberExtractBits -= context.availableBits;
                    context.currentBitBuffer |= (ushort)(context.nextBitValue << numberExtractBits);
                    if (context.remainReadData == 0)
                    {
                        context.nextBitValue = 0;
                    }
                    else
                    {
                        context.remainReadData--;
                        context.nextBitValue = context.compressedData[context.currentInputPosition];
                        context.currentInputPosition++;
                    }
                    context.availableBits = 8;
                } while (8 < numberExtractBits);
            }
            context.availableBits -= numberExtractBits;
            context.currentBitBuffer |= (ushort)(context.nextBitValue >> context.availableBits);
            return;
        }

        // 00402E20
        static void OutMemoryCopy(DecompressionContext context, byte[] inMem, int size)
        {
            Array.Copy(inMem, 0, context.decompressedOutput, context.outMemWriteOffset, size);

            context.outMemWriteOffset += size;
            return;
        }
    }
}
