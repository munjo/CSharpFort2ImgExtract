﻿using System;
using System.Collections.Generic;
using System.Drawing;
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
        public short[] XOffset { get; set; }
        public short[] YOffset { get; set; }
        public byte[][] OutMem { get; set; }

        public Bitmap[] Images { get; set; }
    }

    public enum Fort2ImgType
    {
        TransparentValueImage = 0,
        BitmapImage,
    }
}
