using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFort2ImgExtract
{
    class Fort2Compress
    {
        private const int WindowSize = 8192;
        private const int LookAheadBufferSize = 256;
        private const int MinimumMatchLength = 3;

        public static string LZSSCompress(byte[] input)
        {
            List<ushort> compressed = new List<ushort>();
            List<ushort> compressed = new List<ushort>();

            int index = 0;
            while (index < input.Length)
            {
                int matchIndex = -1;
                int matchLength = -1;

                for (int i = Math.Max(index - WindowSize, 0); i < index; i++)
                {
                    int length = 0;
                    while (length < LookAheadBufferSize && index + length < input.Length && input[i + length] == input[index + length])
                    {
                        length++;
                    }

                    if (length >= MinimumMatchLength && length > matchLength)
                    {
                        matchIndex = i;
                        matchLength = length;
                    }
                }

                if (matchIndex != -1 && matchLength != -1)
                {
                    compressed.Append($"({matchIndex},{matchLength})");
                    index += matchLength;
                }
                else
                {
                    compressed.Append(input[index]);
                    index++;
                }
            }

            return compressed.ToString();
        }
    }
}
