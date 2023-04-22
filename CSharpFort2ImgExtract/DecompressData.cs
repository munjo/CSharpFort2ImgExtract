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
        public int readSize0;
        public int outMemWriteOffset;
        public byte[] outMem;
        public int readSize1;

        public ushort uShort0;
        public ushort uShort1;
        public short short2;

        public ushort uShort3;
        public short short4;

        public ushort[] test0 = new ushort[256];
        public ushort[] test1 = new ushort[2038]; // 불확실
        public ushort[] test2 = new ushort[4096];
        public ushort[] test3 = new ushort[2042]; // 불확실
        public byte[] test4 = new byte[510];
        public byte[] test5 = new byte[19];

        public byte[] inMem = new byte[8192];

        public uint DAT_0044f82c = 0;
    }
}
