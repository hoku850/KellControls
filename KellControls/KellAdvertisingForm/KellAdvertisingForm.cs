using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;

namespace KellControls
{
    public partial class KellAdvertisingForm : Form
    {
        public Bitmap BackgroundBitmap = null;

        private string titleText;
        private string contentText;
        private string anchorUri;
        private Color normalTitleColor = Color.FromArgb(0, 0, 0);
        private Font normalTitleFont = new Font("宋体", 12, FontStyle.Regular, GraphicsUnit.Pixel);
        private Color normalContentColor = Color.FromArgb(0, 0, 0);
        private Font normalContentFont = new Font("宋体", 12, FontStyle.Regular, GraphicsUnit.Pixel);

        public Rectangle TitleRectangle;
        public Rectangle TitlebarRectangle;
        public Rectangle ContentRectangle;
        public Rectangle CloseBtnRectangle;

        private Rectangle WorkAreaRectangle;

        private int SavedTop;
        private int currentTop = 1;
        private int intervalValue = 2;

        public const int WM_NCLBUTTONDOWN = 0x00A1; //消息:左键点击 winuser.h
        public const int HT_CAPTION = 0x0002; //标题栏

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam); //发送消息 //winuser.h 中有函数原型定义
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture(); //释放鼠标捕捉 winuser.h
        [DllImportAttribute("user32.dll")] //winuser.h
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);

        public int CurrentState = 0; //0=hide 1=uptoshow 2=showing 3=downtohide 4=hiding 

        public bool ShowWebPage
        {
            get
            {
                return this.webBrowser1.Visible;
            }
            set
            {
                this.webBrowser1.Visible = value;
            }
        }

        public string Url
        {
            get
            {
                if (this.webBrowser1.Url != null)
                {
                    string u = this.webBrowser1.Url.AbsoluteUri;
                    if (u.EndsWith("/"))
                        u = u.Substring(0, u.Length - 1);
                    return u;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                try
                {
                    this.webBrowser1.Url = new Uri(value);
                    this.ShowWebPage = true;
                }
                catch { }
            }
        }

        public bool ShowLogo
        {
            get
            {
                return this.pictureBox1.Visible;
            }
            set
            {
                this.pictureBox1.Visible = value;
                //if (value)
                //    pictureBox1.Dock = DockStyle.Right;
                //else
                //    pictureBox1.Dock = DockStyle.Fill;
            }
        }

        public Image Logo
        {
            get
            {
                return this.pictureBox1.Image;
            }
            set
            {
                this.pictureBox1.Image = value;
            }
        }

        public KellAdvertisingForm()
        {
            InitializeComponent();
        }

        public void SetBackgroundBitmap(Image image, Color transparencyColor)
        {
            BackgroundBitmap = new Bitmap(image);
            Width = BackgroundBitmap.Width;
            Height = BackgroundBitmap.Height;
            Region = BitmapToRegion(BackgroundBitmap, transparencyColor);
        }

        public Region BitmapToRegion(Bitmap bitmap, Color transparencyColor)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap", "图像不能为空！");
            Region region = RegionRectangleBmp.RegionRectangleUtil.GetRegionFromBmp((Bitmap)bitmap.Clone(), transparencyColor);
            return region;
        }

        protected void DrawText(Graphics grfx)
        {
            if (titleText != null && titleText.Length != 0)
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                sf.FormatFlags = StringFormatFlags.NoWrap;
                sf.Trimming = StringTrimming.EllipsisCharacter;
                grfx.DrawString(titleText, normalTitleFont, new SolidBrush(normalTitleColor), TitleRectangle, sf);
            }

            if (contentText != null && contentText.Length != 0)
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                sf.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
                sf.Trimming = StringTrimming.Word;
                grfx.DrawString(contentText, normalContentFont, new SolidBrush(normalContentColor), ContentRectangle, sf);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (this.ShowWebPage)
            {
                base.OnPaintBackground(e);
            }
            else
            {
                Graphics grfx = e.Graphics;
                grfx.PageUnit = GraphicsUnit.Pixel;

                Graphics offScreenGraphics;
                Bitmap offscreenBitmap;

                offscreenBitmap = new Bitmap(BackgroundBitmap.Width, BackgroundBitmap.Height);
                offScreenGraphics = Graphics.FromImage(offscreenBitmap);

                if (BackgroundBitmap != null)
                {
                    offScreenGraphics.DrawImage(BackgroundBitmap, 0, 0, BackgroundBitmap.Width, BackgroundBitmap.Height);
                }

                DrawText(offScreenGraphics);

                grfx.DrawImage(offscreenBitmap, 0, 0);
            }
        }
        
        public void ShowForm(string ftitletext, string fcontenttext, string fanchorUri, Rectangle fRegionofFormTitle, Rectangle fRegionofFormTitlebar, Rectangle fRegionofFormContent, Rectangle fRegionofCloseBtn)
        {
            titleText = ftitletext;
            contentText = fcontenttext;
            anchorUri = fanchorUri;
            this.ShowWebPage = false;
            WorkAreaRectangle = Screen.GetWorkingArea(WorkAreaRectangle);
            this.Top = WorkAreaRectangle.Height + this.Height;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Normal;
            this.SetBounds(WorkAreaRectangle.Width - this.Width, WorkAreaRectangle.Height - currentTop, this.Width, this.Height);
            CurrentState = 1;
            timer1.Enabled = true;
            TitleRectangle = fRegionofFormTitle;
            TitlebarRectangle = fRegionofFormTitlebar;
            ContentRectangle = fRegionofFormContent;
            CloseBtnRectangle = fRegionofCloseBtn;
            this.Show();
        }

        public void ShowForm(string ftitletext, string Uri, Rectangle fRegionofFormTitle, Rectangle fRegionofFormTitlebar, Rectangle fRegionofFormContent, Rectangle fRegionofCloseBtn)
        {
            titleText = ftitletext;
            this.Url = Uri;
            WorkAreaRectangle = Screen.GetWorkingArea(WorkAreaRectangle);
            this.Top = WorkAreaRectangle.Height + this.Height;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Normal;
            this.SetBounds(WorkAreaRectangle.Width - this.Width, WorkAreaRectangle.Height - currentTop, this.Width, this.Height);
            CurrentState = 1;
            timer1.Enabled = true;
            TitleRectangle = fRegionofFormTitle;
            TitlebarRectangle = fRegionofFormTitlebar;
            ContentRectangle = fRegionofFormContent;
            CloseBtnRectangle = fRegionofCloseBtn;
            this.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (CurrentState == 1) //上升
            {
                this.SetBounds(WorkAreaRectangle.Width - this.Width, WorkAreaRectangle.Height - currentTop, this.Width, this.Height);
                currentTop = currentTop + intervalValue * 4;
                if (this.Top <= WorkAreaRectangle.Height - this.Height)
                {
                    timer1.Enabled = false;
                    CurrentState = 2;
                    timer2.Enabled = true; //显示停留计时
                    currentTop = 1;
                }
            }
            else if (CurrentState == 3) //下降
            {
                if (this.Bounds.Contains(Cursor.Position))
                {
                    timer1.Enabled = false;
                    timer2.Start();
                    return;
                }
                this.SetBounds(WorkAreaRectangle.Width - this.Width, SavedTop + currentTop, this.Width, this.Height);
                currentTop = currentTop + intervalValue;
                if (this.Top >= WorkAreaRectangle.Height)
                {
                    timer1.Enabled = false;

                    this.Hide();
                    CurrentState = 0;
                    currentTop = 1;
                }
            }
            else if (CurrentState == 4)
            {
                if (this.Bounds.Contains(Cursor.Position))
                {
                    this.Opacity = 1;
                    timer1.Enabled = false;
                    timer2.Start();
                    return;
                }
                if (this.Opacity <= 0)
                {
                    timer1.Enabled = false;
                    this.Hide();
                    CurrentState = 0;
                    currentTop = 1;
                }
                this.Opacity -= 0.025;
            }
        }

        private void TaskbarForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.Bounds.Contains(this.PointToScreen(e.Location)))
                {
                    CurrentState = 2;
                    currentTop = 1;
                    SavedTop = this.Top;
                }
                if (TitlebarRectangle.Contains(e.Location)) //单击标题栏时拖动
                {
                    ReleaseCapture(); //释放鼠标捕捉
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0); //发送左键点击的消息至该窗体(标题栏)
                    CurrentState = 4;
                }
                if (CloseBtnRectangle.Contains(e.Location)) //单击Close按钮关闭
                {
                    this.Hide();
                    CurrentState = 0;
                    currentTop = 1;
                }
                if (ContentRectangle.Contains(e.Location)) //单击内容区域
                {
                    try
                    {
                        System.Diagnostics.Process.Start(anchorUri);
                    }
                    catch// (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            currentTop = 1;
            SavedTop = this.Top;
            if (this.Bounds.Contains(Cursor.Position))
                return;
            timer2.Enabled = false;
            if (CurrentState != 4)
                CurrentState = 3;
            timer1.Enabled = true;
        }

        private void TaskbarForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (ContentRectangle.Contains(e.Location) || CloseBtnRectangle.Contains(e.Location))
            {
                Cursor = Cursors.Hand;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
