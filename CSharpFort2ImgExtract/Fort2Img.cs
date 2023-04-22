using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFort2ImgExtract
{
    public class Fort2Img
    {
        public int Num { get; set; }
        public Fort2ImgType Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public short XOffset0 { get; set; }
        public short YOffset0 { get; set; }
        public byte[] OutMem0 { get; set; }

        public short XOffset1 { get; set; }
        public short YOffset1 { get; set; }
        public byte[] OutMem1 { get; set; }
    }

    public enum Fort2ImgType
    {
        TransparentValueImage = 0,
        BitmapImage,
    }
}
