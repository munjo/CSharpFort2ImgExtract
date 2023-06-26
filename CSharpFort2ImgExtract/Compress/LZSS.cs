using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFort2ImgExtract.Compress
{
    /*
    byte[] test1 = new byte[] {
            31, 248,

            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248,

            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248, 31, 248, 31, 248,
            31, 248, 31, 248, 31, 248,
            };

    byte[] test2 = new byte[]
    {11,0,1,0,0,0,3,0,97,99,96,12,0,1,0,0,0,4,0,99,96,96,96,12,0,1,0,1,0,4,0,96,96,96,99,11,0,1,0,2,0,3,0,96,96,99,10,0,1,0,3,0,2,0,99,97};
*/

    class LZSS
    {
        private const int WindowSize = 8192;
        private const int LookAheadBufferSize = 256;

        public static void Compress(byte[] data, List<LZSSValue> compressDatas)
        {
            int pos;
            int backMatchPos = -1;
            bool match = false;
            int value = 0;
            LZSSValue lzss;

            for (pos = 0; pos < data.Length; pos++)
            {
                if (!match)
                {
                    backMatchPos = FindEqualValues(data, pos);
                    if (backMatchPos != -1)
                    {
                        match = true;
                    }
                }
                if (match)
                {
                    if (data[pos] == data[pos - backMatchPos] &&
                        value < LookAheadBufferSize)
                    {
                        value++;
                    }
                    else
                    {
                        lzss = new LZSSValue(value + 253, backMatchPos - 1);
                        compressDatas.Add(lzss);
                        match = false;
                        value = 0;
                        backMatchPos = -1;
                        pos--;
                    }
                }
                else
                {
                    lzss = new LZSSValue(data[pos], -1);
                    compressDatas.Add(lzss);
                }
            }
            if (match)
            {
                lzss = new LZSSValue(value + 253, backMatchPos - 1);
                compressDatas.Add(lzss);
            }
        }

        public static int FindEqualValues(byte[] data, int pos)
        {
            int windowHead = pos - WindowSize;
            int windowTail = pos - 1;
            int matching = 0;

            for (int i = windowTail;
                -1 < i &&
                windowHead < i &&
                pos < data.Length - 2;
                i--)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (data[i + j] == data[pos + j])
                    {
                        matching++;
                    }
                    else
                    {
                        matching = 0;
                        break;
                    }
                }

                if (matching == 3)
                {
                    return pos - i;
                }
            }

            return -1;
        }
    }

    public struct LZSSValue
    {
        int DataValue;
        int BackPos;

        public LZSSValue(int dataValue, int backPos)
        {
            this.DataValue = dataValue;
            this.BackPos = backPos;
        }
    }
}
