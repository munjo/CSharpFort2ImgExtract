
namespace CSharpFort2ImgExtract
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.B_openImg = new System.Windows.Forms.Button();
            this.OFD_openImg = new System.Windows.Forms.OpenFileDialog();
            this.PB_img = new System.Windows.Forms.PictureBox();
            this.B_save = new System.Windows.Forms.Button();
            this.B_openPal = new System.Windows.Forms.Button();
            this.OFD_openPal = new System.Windows.Forms.OpenFileDialog();
            this.L_openImg = new System.Windows.Forms.Label();
            this.L_openPal = new System.Windows.Forms.Label();
            this.LB_imgList = new System.Windows.Forms.ListBox();
            this.L_imgSize = new System.Windows.Forms.Label();
            this.RB_layer0 = new System.Windows.Forms.RadioButton();
            this.RB_layer1 = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.TLP_imgLayer = new System.Windows.Forms.TableLayoutPanel();
            this.P_img = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.PB_img)).BeginInit();
            this.TLP_imgLayer.SuspendLayout();
            this.P_img.SuspendLayout();
            this.SuspendLayout();
            // 
            // B_openImg
            // 
            this.B_openImg.Location = new System.Drawing.Point(12, 12);
            this.B_openImg.Name = "B_openImg";
            this.B_openImg.Size = new System.Drawing.Size(105, 23);
            this.B_openImg.TabIndex = 0;
            this.B_openImg.Text = "이미지 불러오기";
            this.B_openImg.UseVisualStyleBackColor = true;
            this.B_openImg.Click += new System.EventHandler(this.B_openImg_Click);
            // 
            // OFD_openImg
            // 
            this.OFD_openImg.Filter = "포트리스2 이미지 파일|*.img; *.i16|포트리스2 256색 이미지|*.img|포트리스2 하이 컬러 이미지|*.i16|모든 파일|*.*";
            // 
            // PB_img
            // 
            this.PB_img.BackColor = System.Drawing.Color.Magenta;
            this.PB_img.BackgroundImage = global::CSharpFort2ImgExtract.Properties.Resources.imgBackground;
            this.PB_img.Location = new System.Drawing.Point(0, 0);
            this.PB_img.Margin = new System.Windows.Forms.Padding(0);
            this.PB_img.Name = "PB_img";
            this.PB_img.Size = new System.Drawing.Size(0, 0);
            this.PB_img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PB_img.TabIndex = 1;
            this.PB_img.TabStop = false;
            this.PB_img.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PB_img_MouseDown);
            // 
            // B_save
            // 
            this.B_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.B_save.Location = new System.Drawing.Point(12, 406);
            this.B_save.Name = "B_save";
            this.B_save.Size = new System.Drawing.Size(105, 23);
            this.B_save.TabIndex = 4;
            this.B_save.Text = "저장";
            this.B_save.UseVisualStyleBackColor = true;
            this.B_save.Click += new System.EventHandler(this.B_save_Click);
            // 
            // B_openPal
            // 
            this.B_openPal.Location = new System.Drawing.Point(12, 41);
            this.B_openPal.Name = "B_openPal";
            this.B_openPal.Size = new System.Drawing.Size(105, 23);
            this.B_openPal.TabIndex = 5;
            this.B_openPal.Text = "팔레트 불러오기";
            this.B_openPal.UseVisualStyleBackColor = true;
            this.B_openPal.Click += new System.EventHandler(this.B_openPal_Click);
            // 
            // OFD_openPal
            // 
            this.OFD_openPal.Filter = "포트리스2 팔레트 파일|*.pal|모든 파일|*.*";
            // 
            // L_openImg
            // 
            this.L_openImg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.L_openImg.Location = new System.Drawing.Point(123, 12);
            this.L_openImg.Name = "L_openImg";
            this.L_openImg.Size = new System.Drawing.Size(489, 23);
            this.L_openImg.TabIndex = 6;
            this.L_openImg.Text = "불러온 이미지 없음";
            this.L_openImg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // L_openPal
            // 
            this.L_openPal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.L_openPal.Location = new System.Drawing.Point(123, 41);
            this.L_openPal.Name = "L_openPal";
            this.L_openPal.Size = new System.Drawing.Size(489, 23);
            this.L_openPal.TabIndex = 7;
            this.L_openPal.Text = "불러온 팔레트 없음(기본 팔레트 사용)";
            this.L_openPal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LB_imgList
            // 
            this.LB_imgList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LB_imgList.FormattingEnabled = true;
            this.LB_imgList.ItemHeight = 12;
            this.LB_imgList.Location = new System.Drawing.Point(12, 73);
            this.LB_imgList.Name = "LB_imgList";
            this.LB_imgList.Size = new System.Drawing.Size(105, 328);
            this.LB_imgList.TabIndex = 8;
            this.LB_imgList.SelectedIndexChanged += new System.EventHandler(this.LB_imgList_SelectedIndexChanged);
            // 
            // L_imgSize
            // 
            this.L_imgSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.L_imgSize.Location = new System.Drawing.Point(241, 407);
            this.L_imgSize.Name = "L_imgSize";
            this.L_imgSize.Size = new System.Drawing.Size(371, 22);
            this.L_imgSize.TabIndex = 9;
            this.L_imgSize.Text = "0×0 px";
            this.L_imgSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // RB_layer0
            // 
            this.RB_layer0.Checked = true;
            this.RB_layer0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RB_layer0.Location = new System.Drawing.Point(53, 3);
            this.RB_layer0.Name = "RB_layer0";
            this.RB_layer0.Size = new System.Drawing.Size(26, 17);
            this.RB_layer0.TabIndex = 10;
            this.RB_layer0.TabStop = true;
            this.RB_layer0.Text = "0";
            this.RB_layer0.UseVisualStyleBackColor = true;
            this.RB_layer0.CheckedChanged += new System.EventHandler(this.LB_imgList_SelectedIndexChanged);
            // 
            // RB_layer1
            // 
            this.RB_layer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RB_layer1.Location = new System.Drawing.Point(85, 3);
            this.RB_layer1.Name = "RB_layer1";
            this.RB_layer1.Size = new System.Drawing.Size(26, 17);
            this.RB_layer1.TabIndex = 11;
            this.RB_layer1.Text = "1";
            this.RB_layer1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 23);
            this.label4.TabIndex = 12;
            this.label4.Text = "레이어";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TLP_imgLayer
            // 
            this.TLP_imgLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TLP_imgLayer.ColumnCount = 3;
            this.TLP_imgLayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.TLP_imgLayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.TLP_imgLayer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.TLP_imgLayer.Controls.Add(this.label4, 0, 0);
            this.TLP_imgLayer.Controls.Add(this.RB_layer1, 2, 0);
            this.TLP_imgLayer.Controls.Add(this.RB_layer0, 1, 0);
            this.TLP_imgLayer.Enabled = false;
            this.TLP_imgLayer.Location = new System.Drawing.Point(123, 406);
            this.TLP_imgLayer.Name = "TLP_imgLayer";
            this.TLP_imgLayer.RowCount = 1;
            this.TLP_imgLayer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP_imgLayer.Size = new System.Drawing.Size(112, 23);
            this.TLP_imgLayer.TabIndex = 13;
            // 
            // P_img
            // 
            this.P_img.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.P_img.AutoScroll = true;
            this.P_img.Controls.Add(this.PB_img);
            this.P_img.Location = new System.Drawing.Point(123, 73);
            this.P_img.Name = "P_img";
            this.P_img.Size = new System.Drawing.Size(489, 327);
            this.P_img.TabIndex = 14;
            this.P_img.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PB_img_MouseDown);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.P_img);
            this.Controls.Add(this.TLP_imgLayer);
            this.Controls.Add(this.L_imgSize);
            this.Controls.Add(this.LB_imgList);
            this.Controls.Add(this.L_openPal);
            this.Controls.Add(this.L_openImg);
            this.Controls.Add(this.B_openPal);
            this.Controls.Add(this.B_save);
            this.Controls.Add(this.B_openImg);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(260, 260);
            this.Name = "MainForm";
            this.Text = "포트리스2 이미지 뷰어";
            ((System.ComponentModel.ISupportInitialize)(this.PB_img)).EndInit();
            this.TLP_imgLayer.ResumeLayout(false);
            this.P_img.ResumeLayout(false);
            this.P_img.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button B_openImg;
        private System.Windows.Forms.OpenFileDialog OFD_openImg;
        private System.Windows.Forms.PictureBox PB_img;
        private System.Windows.Forms.Button B_save;
        private System.Windows.Forms.Button B_openPal;
        private System.Windows.Forms.OpenFileDialog OFD_openPal;
        private System.Windows.Forms.Label L_openImg;
        private System.Windows.Forms.Label L_openPal;
        private System.Windows.Forms.ListBox LB_imgList;
        private System.Windows.Forms.Label L_imgSize;
        private System.Windows.Forms.RadioButton RB_layer0;
        private System.Windows.Forms.RadioButton RB_layer1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel TLP_imgLayer;
        private System.Windows.Forms.Panel P_img;
    }
}

