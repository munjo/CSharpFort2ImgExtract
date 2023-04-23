using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFort2ImgExtract
{
    class ImageSave
    {
        static public void Save(List<Fort2Img> fort2Imgs, SaveOption saveOption, string imgNameExt, string PalateNameExt = "")
        {
            string folderName = Path.GetFileNameWithoutExtension(imgNameExt);

            if (saveOption.PalNameAdd)
            {
                folderName += string.Format("(pal-{0})", Path.GetFileNameWithoutExtension(PalateNameExt));
            }

            string path = Path.Combine(saveOption.SelectedPath, folderName);

            DirectoryInfo di = new DirectoryInfo(path);

            // 폴더가 없다면 폴더 생성
            if (di.Exists == false)
            {
                di.Create();
            }

            for (int a = 0; a < saveOption.SaveIndex.Count; a++)
            {
                int index = saveOption.SaveIndex[a];

                for (int b = 0; b < fort2Imgs[index].Images.Length; b++)
                {
                    if (fort2Imgs[index].Images[b] == null)
                    {
                        continue;
                    }

                    // 저장될 이미지 제목
                    string fileName = Path.GetFileNameWithoutExtension(imgNameExt) + string.Format("-{0:D4}", fort2Imgs[index].Num);
                    // 이미지 레이어가 2개라면 뒤에 숫자를 붙여준다.
                    if (fort2Imgs[index].Images[1] != null)
                    {
                        fileName += string.Format("-{0}", b);
                    }
                    fileName += ".png";

                    //중복된 파일이 있다면
                    if (File.Exists(Path.Combine(path, fileName)))
                    {
                        // 덮어쓰기 옵션
                        if (saveOption.DuplicateFileMode == DuplicateFileOption.Overwrite)
                        {
                            // 이미지 저장
                            fort2Imgs[index].Images[b].Save(Path.Combine(path, fileName), ImageFormat.Png);
                        }
                    }
                    else
                    {
                        // 이미지 저장
                        fort2Imgs[index].Images[b].Save(Path.Combine(path, fileName), ImageFormat.Png);
                    }
                }

            }
        }
    }
}
