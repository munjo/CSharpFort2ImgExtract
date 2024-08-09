using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFort2ImgExtract
{
    class Fort2ImgConversion
    {

        static public Bitmap Fort2Color256ImgDraw(Fort2Img fort2Img, Color[] palette, int layer)
        {
            Bitmap bitmap = null;

            if (fort2Img.OutMem[layer] != null)
            {
                bitmap = new Bitmap(fort2Img.Width, fort2Img.Height);

                if (fort2Img.Type == Fort2ImgType.BitmapImage)
                {
                    DrawColor256Bitmap(fort2Img.OutMem[layer], bitmap, palette);
                }
                else if (fort2Img.Type == Fort2ImgType.TransparentValueImage)
                {
                    DrawColor256BitmapWithTransparency(fort2Img.OutMem[layer], bitmap, palette);
                }
            }
            return bitmap;
        }

        static public Bitmap Fort2ImgDraw(Fort2Img fort2Img, Color[] palette, int layer)
        {
            Bitmap bitmap = null;

            if (fort2Img.OutMem[layer] != null)
            {
                bitmap = new Bitmap(fort2Img.Width, fort2Img.Height);

                if (fort2Img.Type == Fort2ImgType.BitmapImage)
                {
                    DrawBitmap(fort2Img.OutMem[layer], bitmap);
                }
                else if (fort2Img.Type == Fort2ImgType.TransparentValueImage)
                {
                    DrawBitmapWithTransparency(fort2Img.OutMem[layer], bitmap);
                }
            }
            return bitmap;
        }

        static private void DrawColor256Bitmap(byte[] data, Bitmap bitmap, Color[] palette)
        {
            Point drawPos = new Point();
            for (int a = 0; a < data.Length; a++)
            {
                if (drawPos.Y >= bitmap.Height)
                {
                    break;
                }

                int paletteNum = data[a];
                bitmap.SetPixel(drawPos.X, drawPos.Y, palette[paletteNum]);
                drawPos.X++;

                if (drawPos.X >= bitmap.Width)
                {
                    drawPos.Y++;
                    drawPos.X = 0;
                }
            }
        }

        static private void DrawColor256BitmapWithTransparency(byte[] data, Bitmap bitmap, Color[] palette)
        {
            int Startoffset = 0;
            Point drawPos = new Point();

            while (Startoffset < data.Length)
            {
                if (drawPos.Y >= bitmap.Height)
                {
                    break;
                }

                int offset = Startoffset;
                ushort total = BitConverter.ToUInt16(data, offset);
                ushort lineData = BitConverter.ToUInt16(data, offset + 2);
                offset += 4;
                for (int a = 0; a < lineData; a++)
                {
                    ushort xOffset = BitConverter.ToUInt16(data, offset);
                    drawPos.X = xOffset;
                    ushort dataSize = BitConverter.ToUInt16(data, offset + 2);
                    offset += 4;
                    for (int b = 0; b < dataSize; b++)
                    {
                        if (drawPos.X >= bitmap.Width)
                        {
                            return;
                        }
                        int paletteNum = data[offset + b];
                        bitmap.SetPixel(drawPos.X, drawPos.Y, palette[paletteNum]);
                        drawPos.X++;
                    }
                    offset += dataSize;
                }
                if (total != 0)
                {
                    Startoffset += total;
                }
                else
                {
                    Startoffset = data.Length;
                }

                drawPos.Y++;
                drawPos.X = 0;
            }
        }

        static private void DrawBitmap(byte[] data, Bitmap bitmap)
        {
            Point drawPos = new Point();
            for (int a = 0; a < data.Length; a+=2)
            {
                if (drawPos.Y >= bitmap.Height)
                {
                    break;
                }

                ushort value = BitConverter.ToUInt16(data, a);
                bitmap.SetPixel(drawPos.X, drawPos.Y, HighColorReturn(value));
                drawPos.X++;

                if (drawPos.X >= bitmap.Width)
                {
                    drawPos.Y++;
                    drawPos.X = 0;
                }
            }
        }

        static private void DrawBitmapWithTransparency(byte[] data, Bitmap bitmap)
        {
            int Startoffset = 0;
            Point drawPos = new Point();

            while (Startoffset < data.Length)
            {
                if (drawPos.Y >= bitmap.Height)
                {
                    break;
                }

                int offset = Startoffset;
                ushort total = BitConverter.ToUInt16(data, offset);
                ushort lineData = BitConverter.ToUInt16(data, offset + 2);
                offset += 4;
                for (int a = 0; a < lineData; a++)
                {
                    ushort xOffset = BitConverter.ToUInt16(data, offset);
                    drawPos.X = xOffset;
                    ushort dataSize = BitConverter.ToUInt16(data, offset + 2);
                    dataSize *= 2;
                    offset += 4;
                    for (int b = 0; b < dataSize; b += 2)
                    {
                        if (drawPos.X >= bitmap.Width)
                        {
                            return;
                        }
                        ushort value = BitConverter.ToUInt16(data, offset + b);
                        bitmap.SetPixel(drawPos.X, drawPos.Y, HighColorReturn(value));
                        drawPos.X++;
                    }
                    offset += dataSize;
                }
                if (total != 0)
                {
                    Startoffset += total;
                }
                else
                {
                    Startoffset = data.Length;
                }

                drawPos.Y++;
                drawPos.X = 0;
            }
        }

        static public Color HighColorReturn(ushort value)
        {
            int red = value >> 11;
            int green = (value >> 5) & 63;
            int blue = value & 31;

            red = red * 8;
            green = green * 4;
            blue = blue * 8;

            Color result = Color.FromArgb(red, green, blue);

            return result;
        }

        static public Color[] GetPaletteColors(ReadOnlyCollection<byte> data)
        {
            Color[] palette = GetPaletteColors(data.ToArray());

            return palette;
        }

        static public Color[] GetPaletteColors(byte[] data)
        {
            Color[] palette = new Color[256];
            int range = data.Length / 4;
            int a;

            for (a = 0; a < range && a < 256; a++)
            {
                palette[a] = Color.FromArgb(data[a * 4], data[a * 4 + 1], data[a * 4 + 2]);
            }
            for (; a < 256; a++)
            {
                palette[a] = Color.FromArgb(0, 0, 0);
            }

            return palette;
        }
    }
}
