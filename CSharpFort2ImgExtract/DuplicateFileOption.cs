using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFort2ImgExtract
{
    public class SaveOption
    {
        public string SelectedPath { get; set; }
        public bool PalNameAdd { get; set; }
        public DuplicateFileOption DuplicateFileMode { get; set; }
        public List<int> SaveIndex { get; set; }
    }



    public enum DuplicateFileOption
    {
        Skip =0,
        Overwrite
    }
}
