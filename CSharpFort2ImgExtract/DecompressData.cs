using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFort2ImgExtract
{
    public class DecompressData
    {
        public byte[] readMem;
        public int outSize;
        public int index;
        public int readSize;
        public int outMemWriteOffset;
        public byte[] outMem;
        public int remainReadData;

        public ushort currentBitValue;
        public ushort nextBitValue;
        public short remainingBits;
        public ushort flags;
        public short windowOffset;

        /// <summary>
        /// ushort[256]
        /// 뒤로 이동할 값
        /// </summary>
        public ushort[] backLocationHighTree = new ushort[256];
        /// <summary>
        /// ushort[1019]
        /// </summary>
        public ushort[] lowTreeValue0 = new ushort[1019];
        /// <summary>
        /// ushort[4096]
        /// InMem에 들어가는 데이터가 있는 트리 배열
        /// </summary>
        public ushort[] dataHighTree = new ushort[4096];
        /// <summary>
        /// ushort[1021]
        /// </summary>
        public ushort[] lowTreeValue1 = new ushort[1021];
        /// <summary>
        /// byte[510]
        ///  데이터가 있는 트리 사이즈
        /// </summary>
        public byte[] dataTreeSizeTable = new byte[510];
        /// <summary>
        /// byte[19]
        /// 뒤로 이동할 값의 트리 사이즈
        /// </summary>
        public byte[] backLocationTreeSizeTable = new byte[19];
        /// <summary>
        /// byte[8192]
        /// </summary>
        public byte[] dataBuffer = new byte[8192];

        /// <summary>
        /// DAT_0044f82c
        /// </summary>
        public uint headPosition = 0;
    }
}
