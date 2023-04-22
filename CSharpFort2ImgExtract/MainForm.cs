using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpFort2ImgExtract
{
    public partial class MainForm : Form
    {
        List<Fort2Img> fort2Imgs = new List<Fort2Img>();
        Bitmap bitmap;
        Color[] palette = new Color[256];
        bool isHighColorImage = false;
        SaveForm saveForm = new SaveForm();

        public MainForm()
        {
            InitializeComponent();
            palette = Fort2ImgConversion.GetPaletteColors(DefaultPalette.Palette);
        }

        private void B_openImg_Click(object sender, EventArgs e)
        {
            DialogResult result = OFD_openImg.ShowDialog();
            if(result == DialogResult.OK)
            {
                LB_imgList.DataSource = null;

                bool check = GetImgList(OFD_openImg.FileName);

                if (check)
                {
                    LB_imgList.SelectedIndexChanged -= LB_imgList_SelectedIndexChanged;
                    LB_imgList.DataSource = fort2Imgs;
                    LB_imgList.DisplayMember = "Num";
                    LB_imgList.SelectedIndexChanged += LB_imgList_SelectedIndexChanged;
                    L_openImg.Text = Path.GetFileName(OFD_openImg.FileName);
                }
                else
                {
                    L_openImg.Text = "불러온 이미지 없음";
                }
            }
        }

        private bool GetImgList(string fileName)
        {
            bool result = true;

            if (Path.GetExtension(fileName) == ".i16")
            {
                isHighColorImage = true;
            }
            else
            {
                isHighColorImage = false;
            }

            Console.WriteLine(fileName);

            using (FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fort2Imgs.Clear();

                int total;

                int outSize0;
                int outSize1;

                int dataSize;
                byte[] readMem;

                BinaryReader sr = new BinaryReader(fs);

                try 
                { 

                total = sr.ReadInt32();
                if(total <= 0)
                {
                    Console.WriteLine("잘못된 파일");
                    result = false;
                }

                    for (int a = 0; a < total; a++)
                    {
                        Fort2Img fort2Img = new Fort2Img();

                        fort2Img.Num = a + 1;
                        int type = sr.ReadInt32();
                        if (type == 0)
                        {
                            fort2Img.Type = Fort2ImgType.TransparentValueImage;
                        }
                        else if (type == 1)
                        {
                            fort2Img.Type = Fort2ImgType.BitmapImage;
                        }
                        else
                        {
                            new Exception("이미지의 타입값이 옳바르지 않음");
                        }
                        fort2Img.Width = sr.ReadInt32();
                        fort2Img.Height = sr.ReadInt32();
                        fort2Img.XOffset0 = sr.ReadInt16();
                        fort2Img.YOffset0 = sr.ReadInt16();
                        outSize0 = sr.ReadInt32();
                        if (isHighColorImage)
                        {
                            outSize0 *= 2;
                        }
                        dataSize = sr.ReadInt32();
                        readMem = sr.ReadBytes(dataSize);
                        fort2Img.OutMem0 = new byte[outSize0];

                        Fort2Decompress.DecompressStart(fort2Img.OutMem0, readMem, dataSize, outSize0);

                        outSize1 = sr.ReadInt32();
                        if (isHighColorImage)
                        {
                            outSize1 *= 2;
                        }
                        if (outSize1 != 0)
                        {
                            fort2Img.XOffset1 = sr.ReadInt16();
                            fort2Img.YOffset1 = sr.ReadInt16();
                            dataSize = sr.ReadInt32();
                            readMem = sr.ReadBytes(dataSize);
                            fort2Img.OutMem1 = new byte[outSize1];

                            Fort2Decompress.DecompressStart(fort2Img.OutMem1, readMem, dataSize, outSize1);

                        }

                        fort2Imgs.Add(fort2Img);
                    }
                }
                catch(Exception exc)
                {
                    if(0 < fort2Imgs.Count)
                    {
                        Console.WriteLine("일부 값을 불러오는데 문제가 있음");
                    }
                    else
                    {
                        Console.WriteLine("잘못된 파일");
                        result = false;
                    }
                    
                    Console.WriteLine(exc);
                }
            }

            return result;
        }

        private void B_openPal_Click(object sender, EventArgs e)
        {
            DialogResult result = OFD_openPal.ShowDialog();
            if (result == DialogResult.OK)
            {
                bool check = GetPalette(OFD_openPal.FileName);

                if (check && isHighColorImage == false)
                {
                    LB_imgList.SelectedIndex = -1;
                    L_openPal.Text = Path.GetFileName(OFD_openPal.FileName);
                }
            }
        }

        public bool GetPalette(string fileName)
        {
            bool result = true;

            Console.WriteLine(fileName);

            using (FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int check;
                byte[] colorData;

                BinaryReader sr = new BinaryReader(fs);

                try
                {
                    check = sr.ReadByte();
                    if (check == 1 || check == 2)
                    {
                        colorData = sr.ReadBytes(1024);
                        palette = Fort2ImgConversion.GetPaletteColors(colorData);
                    }
                    else
                    {
                        new Exception("데이터 체크값이 옳바르지 않음");
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine("잘못된 파일");
                    result = false;
                    Console.WriteLine(exc);
                }
            }

            return result;
        }

        private void LB_imgList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var img = (Fort2Img)LB_imgList.SelectedItem;
            var layer = 0;

            if (img != null)
            {
                TLP_imgLayer.Enabled = img.OutMem1 != null;

                if (RB_layer1.Checked && TLP_imgLayer.Enabled)
                {
                    layer = 1;
                }

                GetImage(img, layer);

                L_imgSize.Text = string.Format("{0}×{1} px", bitmap.Width, bitmap.Height);
            }
            else
            {
                PB_img.Image = null;
                L_imgSize.Text = "0×0 px";
            }
        }

        private void PB_img_MouseDown(object sender, MouseEventArgs e)
        {
            if (PB_img.BackgroundImage != null)
            {
                PB_img.BackgroundImage = null;
            }
            else
            {
                PB_img.BackgroundImage = Properties.Resources.imgBackground;
            }
        }

        public void GetImage(Fort2Img img, int layer)
        {
            bitmap?.Dispose();

            try
            {
                if (isHighColorImage)
                {
                    bitmap = Fort2ImgConversion.Fort2ImgDraw(img, palette, layer);
                }
                else
                {
                    bitmap = Fort2ImgConversion.Fort2Color256ImgDraw(img, palette, layer);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("이미지를 그리는데 문제 발생");

                Console.WriteLine(exc);
            }

            PB_img.Image = bitmap;
        }

        private void B_save_Click(object sender, EventArgs e)
        {
            saveForm.ShowDialog();
        }
    }
}
