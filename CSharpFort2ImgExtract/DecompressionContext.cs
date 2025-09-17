using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFort2ImgExtract
{
    public class DecompressionContext
    {
        public byte[] compressedData;
        public int remainingDecompressedSize;
        public int currentInputPosition;
        public int totalCompressedSize;
        public int outMemWriteOffset;
        public byte[] decompressedOutput;
        public int remainReadData;

        public ushort currentBitBuffer;
        public ushort nextBitValue;
        public short availableBits;
        public ushort remainingTreeFlags;
        public short windowOffset;

        // 허프만 트리 구조
        /// <summary>
        /// ushort[256]
        /// 뒤로 이동할 값
        /// </summary>
        public ushort[] distanceRootNodes = new ushort[256];
        /// <summary>
        /// ushort[1019]
        /// </summary>
        public ushort[] leftChildNodes = new ushort[1019];
        /// <summary>
        /// ushort[4096]
        /// InMem에 들어가는 데이터가 있는 트리 배열
        /// </summary>
        public ushort[] literalRootNodes = new ushort[4096];
        /// <summary>
        /// ushort[1021]
        /// </summary>
        public ushort[] rightChildNodes = new ushort[1021];
        // 코드 길이 테이블
        /// <summary>
        /// byte[510]
        ///  데이터가 있는 트리 사이즈
        /// </summary>
        public byte[] literalLengthCodeSizes = new byte[510];
        /// <summary>
        /// byte[19]
        /// 뒤로 이동할 값의 트리 사이즈
        /// </summary>
        public byte[] distanceCodeSizes = new byte[19];
        /// <summary>
        /// byte[8192]
        /// </summary>
        public byte[] slidingWindowBuffer = new byte[8192];

        /// <summary>
        /// DAT_0044f82c
        /// </summary>
        public uint windowHeadPosition = 0;
    }
}
