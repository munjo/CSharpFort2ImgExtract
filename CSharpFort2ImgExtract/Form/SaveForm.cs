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
    public partial class SaveForm : Form
    {
        private string SelectedPath
        {
            get => option.SelectedPath;
            set
            {
                option.SelectedPath = value;
                L_savePath.Text = value;
            } 
        }

        public SaveOption Option
        {
            get => option;
        }

        SaveOption option;

        public SaveForm()
        {
            InitializeComponent();

            option = new SaveOption();

            SelectedPath = Directory.GetCurrentDirectory();
        }

        public DialogResult ShowDialog(List<Fort2Img> fort2Imgs)
        {
            for (int i = 0; i < fort2Imgs.Count; i++)
            {
                CLB_imageList.Items.Add(fort2Imgs[i].Num, true);
            }

            DialogResult dialogResult = ShowDialog();

            return dialogResult;
        }

        private void SaveForm_Load(object sender, EventArgs e)
        {
            var items = CLB_imageList.CheckedItems;
            B_save.Enabled = (0 < items.Count);
            CB_selectAll.Checked = items.Count == CLB_imageList.Items.Count;
        }

        private void SaveForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<int> saveIndex = new List<int>();

            for (int a = 0; a < CLB_imageList.Items.Count; a++)
            {
                if (CLB_imageList.GetItemChecked(a))
                {
                    saveIndex.Add(a);
                }
            }
            option.SaveIndex = saveIndex;

            CLB_imageList.Items.Clear();

            if (RB_skip.Checked)
            {
                option.DuplicateFileMode = DuplicateFileOption.Skip;
            }
            else
            {
                option.DuplicateFileMode = DuplicateFileOption.Overwrite;
            }

            option.PalNameAdd = CB_palNameAdd.Checked;
        }

        private void B_savePath_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog();

            if(result == DialogResult.OK)
            {
                SelectedPath = folderBrowserDialog1.SelectedPath;
            }
        }

        private void B_close_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CLB_imageList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (CLB_imageList.CheckedItems.Count == 1 
                && e.NewValue == CheckState.Unchecked)
            {
                B_save.Enabled = false;
            }
            else
            {
                B_save.Enabled = true;
            }

            if (!CB_selectAll.Focused)
            {
                if (CLB_imageList.CheckedItems.Count == CLB_imageList.Items.Count - 1
                && e.NewValue == CheckState.Checked)
                {
                    CB_selectAll.Checked = true;
                }
                else
                {
                    CB_selectAll.Checked = false;
                }
            }
        }

        private void CB_selectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_selectAll.Focused)
            {
                if (CB_selectAll.Checked)
                {
                    for (int i = 0; i < CLB_imageList.Items.Count; i++)
                    {
                        CLB_imageList.SetItemChecked(i, true);
                    }
                }
                else
                {
                    for (int i = 0; i < CLB_imageList.Items.Count; i++)
                    {
                        CLB_imageList.SetItemChecked(i, false);
                    }
                }
            }
        }

        private void B_save_Click(object sender, EventArgs e)
        {

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
