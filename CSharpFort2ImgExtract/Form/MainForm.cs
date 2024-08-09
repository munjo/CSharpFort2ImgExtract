using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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
        Color[] palette = new Color[256];
        bool isHighColorImage = false;
        SaveForm saveForm;
        string imgName;
        string palateName;
        // 저장위치(처음 위치는 해당 프로그램 위치)
        public bool IsHighColorImage { get => isHighColorImage; }
        public List<Fort2Img> Fort2Imgs { get => fort2Imgs; }
        public string ImgName {
            get => imgName;
            private set
            {
                imgName = value;

                if (value == null || value == string.Empty)
                {
                    L_openImg.Text = "불러온 이미지 없음";
                }
                else
                {
                    L_openImg.Text = value;
                }
            }
        }

        public string PalateName {
            get => palateName;
            private set
            {
                palateName = value;

                if (value == null || value == string.Empty)
                {
                    L_openPal.Text = "불러온 팔레트 없음(.img 파일 전용)";
                }
                else
                {
                    L_openPal.Text = value;
                }
            }
        }

        public MainForm()
        {
            InitializeComponent();
            palette = Fort2ImgConversion.GetPaletteColors(DefaultPalette.Palette);

            saveForm = new SaveForm();
            ImgName = string.Empty;
            PalateName = string.Empty;
        }

        private void B_openImg_Click(object sender, EventArgs e)
        {
            OFD_openImg.FileName = imgName;

            DialogResult result = OFD_openImg.ShowDialog();
            if (result == DialogResult.OK)
            {
                LB_imgList.SelectedIndex = -1;
                LB_imgList.DataSource = null;

                bool check = DecompressImage(OFD_openImg.FileName);

                if (check)
                {
                    GetImages();
                    LB_imgList.DataSource = fort2Imgs;
                    LB_imgList.DisplayMember = "Num";
                    LB_imgList.SelectedIndex = 0;
                    ImgName = Path.GetFileName(OFD_openImg.FileName);
                }
                else
                {
                    ImgName = string.Empty;
                }
            }
        }

        private void B_openPal_Click(object sender, EventArgs e)
        {
            DialogResult result = OFD_openPal.ShowDialog();
            if (result == DialogResult.OK)
            {
                int indexBackup = LB_imgList.SelectedIndex;
                LB_imgList.SelectedIndex = -1;

                bool check = GetPalette(OFD_openPal.FileName);

                if (check)
                {
                    // 256색 이미지일 경우 팔레트를 새로적용하여 이미지 불러오기
                    if (IsHighColorImage == false)
                    {
                        GetImages();
                    }

                    LB_imgList.SelectedIndex = indexBackup;
                    PalateName = Path.GetFileName(OFD_openPal.FileName);
                }
            }
        }

        /// <summary>
        /// 압축해제후 이미지값 가져오기
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>성공 여부</returns>
        private bool DecompressImage(string fileName)
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
                foreach (var value in fort2Imgs)
                {
                    for (int a = 0; a < (value?.Images?.Length ?? 0); a++)
                    {
                        value.Images[a]?.Dispose();
                    }
                }
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
                    if (total <= 0)
                    {
                        Console.WriteLine("잘못된 파일");
                        result = false;
                    }

                    for (int a = 0; a < total; a++)
                    {
                        Fort2Img fort2Img = new Fort2Img();

                        fort2Img.Num = a + 1;
                        fort2Img.XOffset = new short[2];
                        fort2Img.YOffset = new short[2];
                        fort2Img.Images = new Bitmap[2];
                        fort2Img.OutMem = new byte[2][];

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
                            throw new Exception("이미지의 타입값이 옳바르지 않음");
                        }
                        fort2Img.Width = sr.ReadInt32();
                        fort2Img.Height = sr.ReadInt32();
                        fort2Img.XOffset[0] = sr.ReadInt16();
                        fort2Img.YOffset[0] = sr.ReadInt16();
                        outSize0 = sr.ReadInt32();
                        if (IsHighColorImage)
                        {
                            outSize0 *= 2;
                        }
                        dataSize = sr.ReadInt32();
                        if (dataSize > fs.Length)
                        {
                            throw new Exception("이미지데이터 크기 값이 파일 크기보다 큽니다.");
                        }
                        readMem = sr.ReadBytes(dataSize);
                        fort2Img.OutMem[0] = new byte[outSize0];

                        Fort2Decompress.DecompressStart(fort2Img.OutMem[0], readMem, dataSize, outSize0);

                        outSize1 = sr.ReadInt32();
                        if (IsHighColorImage)
                        {
                            outSize1 *= 2;
                        }
                        if (outSize1 != 0)
                        {
                            fort2Img.XOffset[1] = sr.ReadInt16();
                            fort2Img.YOffset[1] = sr.ReadInt16();
                            dataSize = sr.ReadInt32();
                            if (dataSize > fs.Length)
                            {
                                throw new Exception("이미지데이터 크기 값이 파일 크기보다 큽니다.");
                            }
                            readMem = sr.ReadBytes(dataSize);
                            fort2Img.OutMem[1] = new byte[outSize1];

                            Fort2Decompress.DecompressStart(fort2Img.OutMem[1], readMem, dataSize, outSize1);
                        }

                        fort2Imgs.Add(fort2Img);
                    }
                }
                catch (Exception exc)
                {
                    if (0 < fort2Imgs.Count)
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

        /// <summary>
        /// 이미지 값을 이미지로 변환
        /// </summary>
        public void GetImages()
        {
            foreach (var Img in fort2Imgs)
            {
                for (int a = 0; a < Img.Images.Length; a++)
                {
                    try
                    {
                        if (IsHighColorImage)
                        {
                            Img.Images[a] = Fort2ImgConversion.Fort2ImgDraw(Img, palette, a);
                        }
                        else
                        {
                            Img.Images[a]?.Dispose();
                            Img.Images[a] = Fort2ImgConversion.Fort2Color256ImgDraw(Img, palette, a);
                        }
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("이미지를 그리는데 문제 발생");
                        Console.WriteLine(exc);

                        Img.Images[a]?.Dispose();
                        Img.Images[a] = null;
                    }
                }
            }
        }

        /// <summary>
        /// 팔레트 파일 불러오기
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>성공여부</returns>
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

                        /*
                         
                        int dataSize = sr.ReadInt32();
                        byte[] readMem = sr.ReadBytes(dataSize);
                        int outSize;
                        byte[] outMem;
                        
                        if (ch == 1)
                        {
                            int outSize = 65536;
                            byte[] outMem = new Byte[outSize];
                        }
                        else
                        {
                            int outSize = 196608;
                            byte[] outMem = new Byte[outSize];
                        }

                        Fort2Decompress.DecompressStart(fort2Img.OutMem, readMem, dataSize, outSize);

                        */
                    }
                    else
                    {
                        throw new Exception("데이터 체크값이 옳바르지 않음");
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

        /// <summary>
        /// 목록 선택시 이미지 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LB_imgList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var img = (Fort2Img)LB_imgList.SelectedItem;
            var layer = 0;

            if (img != null)
            {
                TLP_imgLayer.Enabled = img.OutMem[1] != null;

                if (RB_layer1.Checked && TLP_imgLayer.Enabled)
                {
                    layer = 1;
                }

                PB_img.Image = img.Images[layer];

                L_imgSize.Text = string.Format("{0}×{1} px", 
                    img.Images[layer]?.Width ?? 0,
                    img.Images[layer]?.Height ?? 0);

                textBox1.Text = img.XOffset[layer].ToString();
                textBox2.Text = img.YOffset[layer].ToString();
            }
            else
            {
                PB_img.Image = null;
                L_imgSize.Text = "0×0 px";

                textBox1.Text = "";
                textBox2.Text = "";
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

        private void B_save_Click(object sender, EventArgs e)
        {
            //saveForm = new SaveForm(this);
            var result = saveForm.ShowDialog(fort2Imgs);

            if (result == DialogResult.OK)
            {
                SaveOption saveOption = saveForm.Option;

                // 팔레트이름 추가 옵션이 꺼져있거나
                // 불러온 팔레트 파일이 없거나
                // 하이칼라 이미지라면
                // 폴더명에 팔레트이름 붙이지 않음
                if (!saveForm.Option.PalNameAdd
                    || PalateName == string.Empty
                    || IsHighColorImage)
                {
                    saveOption.PalNameAdd = false;
                }

                try
                {
                    ImageSave.Save(fort2Imgs ,saveOption, ImgName, PalateName);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc);
                    MessageBox.Show("이미지 파일을 저장하는데 실패했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
