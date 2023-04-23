
namespace CSharpFort2ImgExtract
{
    partial class SaveForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveForm));
            this.CB_palNameAdd = new System.Windows.Forms.CheckBox();
            this.B_save = new System.Windows.Forms.Button();
            this.B_close = new System.Windows.Forms.Button();
            this.B_savePath = new System.Windows.Forms.Button();
            this.RB_skip = new System.Windows.Forms.RadioButton();
            this.RB_overwrite = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.CB_selectAll = new System.Windows.Forms.CheckBox();
            this.CLB_imageList = new System.Windows.Forms.CheckedListBox();
            this.L_savePath = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CB_palNameAdd
            // 
            this.CB_palNameAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_palNameAdd.AutoSize = true;
            this.CB_palNameAdd.Checked = true;
            this.CB_palNameAdd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_palNameAdd.Location = new System.Drawing.Point(156, 203);
            this.CB_palNameAdd.Name = "CB_palNameAdd";
            this.CB_palNameAdd.Size = new System.Drawing.Size(149, 16);
            this.CB_palNameAdd.TabIndex = 0;
            this.CB_palNameAdd.Text = "폴더명에 .pal이름 추가";
            this.CB_palNameAdd.UseVisualStyleBackColor = true;
            // 
            // B_save
            // 
            this.B_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_save.Location = new System.Drawing.Point(156, 302);
            this.B_save.Name = "B_save";
            this.B_save.Size = new System.Drawing.Size(112, 23);
            this.B_save.TabIndex = 1;
            this.B_save.Text = "선택된 파일 저장";
            this.B_save.UseVisualStyleBackColor = true;
            this.B_save.Click += new System.EventHandler(this.B_save_Click);
            // 
            // B_close
            // 
            this.B_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_close.Location = new System.Drawing.Point(270, 302);
            this.B_close.Name = "B_close";
            this.B_close.Size = new System.Drawing.Size(75, 23);
            this.B_close.TabIndex = 2;
            this.B_close.Text = "취소";
            this.B_close.UseVisualStyleBackColor = true;
            this.B_close.Click += new System.EventHandler(this.B_close_Click);
            // 
            // B_savePath
            // 
            this.B_savePath.Location = new System.Drawing.Point(215, 31);
            this.B_savePath.Name = "B_savePath";
            this.B_savePath.Size = new System.Drawing.Size(75, 23);
            this.B_savePath.TabIndex = 5;
            this.B_savePath.Text = "찾아보기";
            this.B_savePath.UseVisualStyleBackColor = true;
            this.B_savePath.Click += new System.EventHandler(this.B_savePath_Click);
            // 
            // RB_skip
            // 
            this.RB_skip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RB_skip.AutoSize = true;
            this.RB_skip.Checked = true;
            this.RB_skip.Location = new System.Drawing.Point(158, 280);
            this.RB_skip.Name = "RB_skip";
            this.RB_skip.Size = new System.Drawing.Size(71, 16);
            this.RB_skip.TabIndex = 6;
            this.RB_skip.TabStop = true;
            this.RB_skip.Text = "건너뛰기";
            this.RB_skip.UseVisualStyleBackColor = true;
            // 
            // RB_overwrite
            // 
            this.RB_overwrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RB_overwrite.AutoSize = true;
            this.RB_overwrite.Location = new System.Drawing.Point(235, 280);
            this.RB_overwrite.Name = "RB_overwrite";
            this.RB_overwrite.Size = new System.Drawing.Size(71, 16);
            this.RB_overwrite.TabIndex = 7;
            this.RB_overwrite.Text = "덮어쓰기";
            this.RB_overwrite.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(156, 265);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "중복된 이미지 파일명";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(156, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "저장위치";
            // 
            // CB_selectAll
            // 
            this.CB_selectAll.AutoSize = true;
            this.CB_selectAll.Location = new System.Drawing.Point(12, 12);
            this.CB_selectAll.Name = "CB_selectAll";
            this.CB_selectAll.Size = new System.Drawing.Size(76, 16);
            this.CB_selectAll.TabIndex = 11;
            this.CB_selectAll.Text = "전체 선택";
            this.CB_selectAll.UseVisualStyleBackColor = true;
            this.CB_selectAll.CheckedChanged += new System.EventHandler(this.CB_selectAll_CheckedChanged);
            // 
            // CLB_imageList
            // 
            this.CLB_imageList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.CLB_imageList.FormattingEnabled = true;
            this.CLB_imageList.Location = new System.Drawing.Point(12, 31);
            this.CLB_imageList.Name = "CLB_imageList";
            this.CLB_imageList.Size = new System.Drawing.Size(138, 292);
            this.CLB_imageList.TabIndex = 12;
            this.CLB_imageList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CLB_imageList_ItemCheck);
            // 
            // L_savePath
            // 
            this.L_savePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.L_savePath.BackColor = System.Drawing.SystemColors.Window;
            this.L_savePath.Location = new System.Drawing.Point(156, 57);
            this.L_savePath.Name = "L_savePath";
            this.L_savePath.Size = new System.Drawing.Size(189, 117);
            this.L_savePath.TabIndex = 13;
            this.L_savePath.Text = "selectedPath";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(154, 222);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "(하이컬러 파일은 적용안됨)";
            // 
            // SaveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 337);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.L_savePath);
            this.Controls.Add(this.CLB_imageList);
            this.Controls.Add(this.CB_selectAll);
            this.Controls.Add(this.B_save);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RB_overwrite);
            this.Controls.Add(this.RB_skip);
            this.Controls.Add(this.B_savePath);
            this.Controls.Add(this.B_close);
            this.Controls.Add(this.CB_palNameAdd);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(372, 376);
            this.Name = "SaveForm";
            this.ShowInTaskbar = false;
            this.Text = "저장";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SaveForm_FormClosing);
            this.Load += new System.EventHandler(this.SaveForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox CB_palNameAdd;
        private System.Windows.Forms.Button B_save;
        private System.Windows.Forms.Button B_close;
        private System.Windows.Forms.Button B_savePath;
        private System.Windows.Forms.RadioButton RB_skip;
        private System.Windows.Forms.RadioButton RB_overwrite;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox CB_selectAll;
        private System.Windows.Forms.CheckedListBox CLB_imageList;
        private System.Windows.Forms.Label L_savePath;
        private System.Windows.Forms.Label label4;
    }
}