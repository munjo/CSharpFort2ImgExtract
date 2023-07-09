using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFort2ImgExtract.Compress;

namespace CSharpFort2ImgExtract
{
    class Fort2Compress
    {
        public static void CompressStart(byte[] data)
        {
            List<LZSSValue> compressDatas = LZSS.Compress(data);
        }
    }
}
