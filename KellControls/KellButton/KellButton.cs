using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;

namespace KellControls
{
    [DefaultEvent("Click")]
    public partial class KellButton : UserControl
    {
        #region 变量

        //三种不同状态下的图片
        Image _normalImage = null;
        Image _moveImage = null;
        Image _downImage = null;
        Image _focusImage = null;
        Image _changeImage1 = null;
        Image _changeImage2 = null;
        Image _changeImage3 = null;
        Image _changeImage4 = null;
        Image _changeImage5 = null;
        byte transTolerance = 0;
        Color transColor = Color.FromArgb(255, 0, 255);
        bool focus;
        string text;
        public delegate void TextChangedHandler(object sender, string LastText);
        [Browsable(true)]
        public new event TextChangedHandler TextChanged;

        #endregion

        #region 属性
        [Browsable(true)]
        public override string Text
        {
            get { return text; }
            set 
            {
                if (text != value)
                {
                    string lastText = text;
                    text = value;
                    ChangeText();
                    if (TextChanged != null)
                        TextChanged(this, lastText);
                }
            }
        }

        public Image ChangeImage1
        {
            get { return _changeImage1; }
            set { _changeImage1 = value; }
        }

        public Image ChangeImage2
        {
            get { return _changeImage2; }
            set { _changeImage2 = value; }
        }

        public Image ChangeImage3
        {
            get { return _changeImage3; }
            set { _changeImage3 = value; }
        }

        public Image ChangeImage4
        {
            get { return _changeImage4; }
            set { _changeImage4 = value; }
        }

        public Image ChangeImage5
        {
            get { return _changeImage5; }
            set { _changeImage5 = value; }
        }

        /// <summary>
        /// 透明色容差（默认为0，即无容差）
        /// </summary>
        [Description("透明色容差")]
        [DefaultValue(0)]
        public byte TransparentTolerance
        {
            get { return transTolerance; }
            set { transTolerance = value; }
        }

        /// <summary>
        /// 透明色
        /// </summary>
        [Description("透明色")]
        [DefaultValue(typeof(Color), "#FF00FF")]
        public Color TransparentColor
        {
            get { return transColor; }
            set { transColor = value; }
        }

        public Image NormalImage
        {
            get { return _normalImage; }
            set
            {
                _normalImage = value;
                MakeTransparent(_normalImage);
                this.label1.Image = _normalImage;
            }
        }

        public Image DownImage
        {
            get { return _downImage; }
            set
            {
                _downImage = value;
                MakeTransparent(_downImage);
            }
        }

        public Image MoveImage
        {
            get { return _moveImage; }
            set
            {
                _moveImage = value;
                MakeTransparent(_moveImage);
            }
        }

        public Image FocusImage
        {
            get { return _focusImage; }
            set
            {
                _focusImage = value;
                MakeTransparent(_focusImage);
            }
        }

        public Image Image
        {
            get { return this.label1.Image; }  //控件运行时会自动运行get方法得到值
        }

        #endregion

        #region 构造函数

        public KellButton()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

            CreateSkins();

            InitializeComponent();

            this.label1.Image = _normalImage;
            this.Text = this.Name;
            timer1.Start();
        }

        #endregion

        #region 辅助函数

        private void MakeTransparent(Image image)
        {
            Bitmap bitmap = image as Bitmap;
            bitmap.MakeTransparent(transColor);
            for (int i = 0 - transTolerance; i <= transTolerance; i++)
            {
                if (i != 0)
                {
                    byte r = (byte)(transColor.R + i);
                    if (r < 0) r = 0;
                    if (r > 255) r = 255;
                    byte g = (byte)(transColor.G + i);
                    if (g < 0) r = 0;
                    if (g > 255) r = 255;
                    byte b = (byte)(transColor.B + i);
                    if (b < 0) r = 0;
                    if (b > 255) r = 255;
                    Color c = Color.FromArgb(r, g, b);
                    if (c != Color.Transparent)
                        bitmap.MakeTransparent(c);
                }
            }
        }

        #endregion

        #region 事件

        private void CreateSkins()
        {
            Assembly ass = Assembly.LoadFrom("Resources.dll");
            _normalImage = Image.FromStream(ass.GetManifestResourceStream(@"KellControls.Resources.Resources.Button.btnnomal.bmp"));
            _moveImage = Image.FromStream(ass.GetManifestResourceStream(@"KellControls.Resources.Resources.Button.btnfore.bmp"));
            _downImage = Image.FromStream(ass.GetManifestResourceStream(@"KellControls.Resources.Resources.Button.btndown.bmp"));
            _focusImage = Image.FromStream(ass.GetManifestResourceStream(@"KellControls.Resources.Resources.Button.focus.bmp"));
            _changeImage1 = Image.FromStream(ass.GetManifestResourceStream(@"KellControls.Resources.Resources.Button.change1.bmp"));
            _changeImage2 = Image.FromStream(ass.GetManifestResourceStream(@"KellControls.Resources.Resources.Button.change2.bmp"));
            _changeImage3 = Image.FromStream(ass.GetManifestResourceStream(@"KellControls.Resources.Resources.Button.change3.bmp"));
            _changeImage4 = Image.FromStream(ass.GetManifestResourceStream(@"KellControls.Resources.Resources.Button.change4.bmp"));
            _changeImage5 = Image.FromStream(ass.GetManifestResourceStream(@"KellControls.Resources.Resources.Button.change5.bmp"));
            MakeTransparent(_normalImage);
            MakeTransparent(_moveImage);
            MakeTransparent(_downImage);
            MakeTransparent(_focusImage);
            MakeTransparent(_changeImage1);
            MakeTransparent(_changeImage2);
            MakeTransparent(_changeImage3);
            MakeTransparent(_changeImage4);
            MakeTransparent(_changeImage5);
        }
        
        private void ChangeText()
        {
            SizeF size = this.CreateGraphics().MeasureString(text, this.Font);
            Point center = new Point(this.Width / 2, this.Height / 2);
            int offsetX = 4;
            int offsetY = 0;

            CreateSkins();

            Graphics g = Graphics.FromImage(_normalImage);
            g.DrawString(text, this.Font, new SolidBrush(this.ForeColor), new RectangleF(center.X - size.Width / 2 - offsetX, center.Y - size.Height / 2 - offsetY, size.Width, size.Height));
            g = Graphics.FromImage(_moveImage);
            g.DrawString(text, this.Font, new SolidBrush(this.ForeColor), new RectangleF(center.X - size.Width / 2 - offsetX, center.Y - size.Height / 2 - offsetY, size.Width, size.Height));
            g = Graphics.FromImage(_downImage);
            g.DrawString(text, this.Font, new SolidBrush(this.ForeColor), new RectangleF(center.X - size.Width / 2 - offsetX, center.Y - size.Height / 2 - offsetY, size.Width, size.Height));
            g = Graphics.FromImage(_changeImage1);
            g.DrawString(text, this.Font, new SolidBrush(this.ForeColor), new RectangleF(center.X - size.Width / 2 - offsetX, center.Y - size.Height / 2 - offsetY, size.Width, size.Height));
            g = Graphics.FromImage(_changeImage2);
            g.DrawString(text, this.Font, new SolidBrush(this.ForeColor), new RectangleF(center.X - size.Width / 2 - offsetX, center.Y - size.Height / 2 - offsetY, size.Width, size.Height));
            g = Graphics.FromImage(_changeImage3);
            g.DrawString(text, this.Font, new SolidBrush(this.ForeColor), new RectangleF(center.X - size.Width / 2 - offsetX, center.Y - size.Height / 2 - offsetY, size.Width, size.Height));
            g = Graphics.FromImage(_changeImage4);
            g.DrawString(text, this.Font, new SolidBrush(this.ForeColor), new RectangleF(center.X - size.Width / 2 - offsetX, center.Y - size.Height / 2 - offsetY, size.Width, size.Height));
            g = Graphics.FromImage(_changeImage5);
            g.DrawString(text, this.Font, new SolidBrush(this.ForeColor), new RectangleF(center.X - size.Width / 2 - offsetX, center.Y - size.Height / 2 - offsetY, size.Width, size.Height));
            g = Graphics.FromImage(_focusImage);
            g.DrawString(text, this.Font, new SolidBrush(this.ForeColor), new RectangleF(center.X - size.Width / 2 - offsetX, center.Y - size.Height / 2 - offsetY, size.Width, size.Height));

            g.Dispose();
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            this.label1.Image = _moveImage;
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            this.label1.Image = _downImage;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            if (focus)
            {
                this.label1.Image = _focusImage;
            }
            else
            {
                Thread thr = new Thread(new ThreadStart(ChangeStyle));
                thr.Start();
            }
        }

        private void ChangeStyle()
        {
            this.label1.Image = (Image)_changeImage1.Clone();
            Thread.Sleep(100);
            this.label1.Image = (Image)_changeImage2.Clone();
            Thread.Sleep(100);
            this.label1.Image = (Image)_changeImage3.Clone();
            Thread.Sleep(100);
            this.label1.Image = (Image)_changeImage4.Clone();
            Thread.Sleep(100);
            this.label1.Image = (Image)_changeImage5.Clone();
            Thread.Sleep(100);
            this.label1.Image = _normalImage;
        }

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
            if (focus)
                this.label1.Image = _focusImage;
            else
                this.label1.Image = _normalImage;
        }


        private void label1_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
            this.Focus();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.label1.Image = _focusImage;
            focus = true;
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.label1.Image = _normalImage;
            focus = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                Point p = this.PointToClient(Cursor.Position);
                if (label1.Bounds.Contains(p))
                {
                    this.label1.Image = _moveImage;
                }
            }
        }

        private void label1_Resize(object sender, EventArgs e)
        {
            ChangeText();
        }

        #endregion

    }
}
