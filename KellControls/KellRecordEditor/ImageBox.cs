using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace KellControls
{
    public class ImageBox: UserControl
    {
        private PictureBox pictureBox1;
        private OpenFileDialog openFileDialog1;
        string filepath;
        public delegate void ImageChangedHandler(object sender, System.Drawing.Image e);
        public event ImageChangedHandler ImageChanged;
        ToolTip tt;

        public ImageBox()
        {
            InitializeComponent();
            tt = new ToolTip();
            tt.Popup += new PopupEventHandler(tt_Popup);
            tt.OwnerDraw = true;
            tt.Draw += new DrawToolTipEventHandler(tt_Draw);
            tt.InitialDelay = 0;
            tt.ReshowDelay = 0;
            tt.AutomaticDelay = 0;
            tt.SetToolTip(this.pictureBox1, "Image");
        }

        private void OnImageChanged(System.Drawing.Image e)
        {
            if (ImageChanged != null)
                ImageChanged(this, e);
        }

        public string Filepath
        {
            get { return filepath; }
            set 
            {
                if (System.IO.File.Exists(value))
                {
                    filepath = value;
                    this.pictureBox1.Image = (System.Drawing.Image)System.Drawing.Image.FromFile(filepath).Clone();
                    OnImageChanged(this.pictureBox1.Image);
                }
            }
        }

        public void ImportImageByStream(System.IO.Stream stream)
        {
            this.filepath = null;
            this.pictureBox1.Image = (System.Drawing.Image)System.Drawing.Image.FromStream(stream).Clone();
            OnImageChanged(this.pictureBox1.Image);
        }

        public System.Drawing.Image Image
        {
            get
            {
                return this.pictureBox1.Image;
            }
        }
    
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "常见图片(bmp,jpg,jpeg,png,gif)|*.bmp;*.jpg;*.jpeg;*.png;*.gif|所有图片|*.*";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(21, 21);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // ImageBox
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pictureBox1);
            this.Name = "ImageBox";
            this.Size = new System.Drawing.Size(21, 21);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(filepath) && System.IO.File.Exists(filepath))
            {
                openFileDialog1.FileName = filepath;
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.Filepath = openFileDialog1.FileName;
            }
            openFileDialog1.Dispose();
        }

        void tt_Popup(object sender, PopupEventArgs e)
        {
            e.ToolTipSize = new Size(Image.Width, Image.Height);
        }

        void tt_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.Graphics.DrawImage(Image, 0, 0, e.Bounds.Width, e.Bounds.Height);
        }
    }
}
