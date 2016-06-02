using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

namespace KellControls
{
    [DefaultProperty("CodeType"), ToolboxBitmap(typeof(ValidateCode), "KellControls.Resources.Resources.ValidateCode"), Description("WinForm下的验证码控件")]
    public class ValidateCode : UserControl
    {
        // Fields
        private int _BlackPen = 50;
        private int _CodeLength = 4;
        private string _CodeText;
        private CodeTypeEnum _CodeType = CodeTypeEnum.数字;
        private string _TooltipText = "点击重新生成验证码";
        private IContainer components;
        private PictureBox picCode;
        private ToolTip toolTip1;

        // Methods
        public ValidateCode()
        {
            this.InitializeComponent();
        }

        private void CreateImage()
        {
            this.GetRanCode();
            Bitmap image = null;
            try
            {
                int maxAngleValue = 30;
                if (image != null)
                {
                    image = null;
                }
                int width = this.picCode.Width;//this._CodeLength * (((this._CodeType == CodeTypeEnum.数字和字母) || (this._CodeType == CodeTypeEnum.字母)) ? 0x10 : 14);
                int height = this.picCode.Height;//0x18;
                int maxLen = Math.Max(this.picCode.Width, this.picCode.Height);
                int minLen = Math.Min(this.picCode.Width, this.picCode.Height);
                int blockWidth= (minLen / 10) < 2 ? 2 : (minLen / 10);
                image = new Bitmap(width, height);
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    graphics.Clear(Color.AliceBlue);
                    graphics.DrawRectangle(new Pen(Color.Black, 0f), 0, 0, image.Width - 1, image.Height - 1);
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    Random random = new Random();
                    if (!string.IsNullOrEmpty(this._CodeText) && (this._CodeText.Length > 0))
                    {
                        char[] chArray = this._CodeText.ToCharArray();
                        StringFormat format = new StringFormat(StringFormatFlags.NoClip);// | StringFormatFlags.MeasureTrailingSpaces);
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Center;
                        Color[] colorArray = new Color[] { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
                        string[] strArray = new string[] { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
                        float maxSize = GetFontSize(this.picCode, this._CodeText) * 0.8F;
                        float[] numArray = new float[4];// { 10.0F, 12.0F, 14.0F, 16.0F };
                        for (int i = 0; i < numArray.Length; i++)
                        {
                            numArray[i] = maxSize;
                            maxSize -= 2.0F;
                        }
                        //int num7 = 9;
                        //if ((this._CodeType == CodeTypeEnum.数字和字母) || (this._CodeType == CodeTypeEnum.字母))
                        //{
                        //    num7 = 11;
                        //}
                        //Point point = new Point(num7, height / 2);
                        Point point = new Point((int)(maxSize * 0.85F), (int)(height * 0.55F));
                        for (int j = 0; j < chArray.Length; j++)
                        {
                            int index = random.Next(8);
                            int num10 = random.Next(5);
                            int num11 = random.Next(4);
                            Font font = new Font(strArray[num10], numArray[num11], FontStyle.Bold);
                            Brush brush = new SolidBrush(colorArray[index]);
                            graphics.TranslateTransform((float)point.X, (float)point.Y);
                            float angle = random.Next(-maxAngleValue, maxAngleValue);
                            graphics.RotateTransform(angle);
                            graphics.DrawString(chArray[j].ToString(), font, brush, 1f, 1f, format);
                            graphics.RotateTransform(-angle);
                            graphics.TranslateTransform(5f, (float)-point.Y);
                        }
                    }
                    graphics.ResetTransform();
                    for (int i = 0; i < this._BlackPen; i++)
                    {
                        Color rndColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                        float rndPenWidth = random.Next(0, 2);
                        Pen pen = new Pen(rndColor, rndPenWidth);
                        int x = random.Next(0, image.Width);
                        int y = random.Next(0, image.Height);
                        int rndWidth = random.Next(1, blockWidth);
                        int rndHeight = random.Next(1, blockWidth);
                        Brush brush = new SolidBrush(rndColor);
                        graphics.FillRectangle(brush, x, y, rndWidth, rndHeight);
                        int rndLenX = random.Next(10, maxLen);
                        int rndLenY = random.Next(10, maxLen);
                        int xx = random.Next(-50, image.Width);
                        int yy = random.Next(-50, image.Height);
                        graphics.DrawLine(pen, xx, yy, xx + rndLenX, yy + rndLenY);
                        int rndStartAngle = random.Next(10, 270);
                        int rndSweepAngle = random.Next(10, 270);
                        int xxx = random.Next(5, image.Width);
                        int yyy = random.Next(5, image.Height);
                        graphics.DrawArc(pen, xx, yy, xxx, yyy, rndStartAngle, rndSweepAngle);
                    }
                }
            }
            catch (Exception)
            {
            }
            this.picCode.Image = image;
        }

        private float GetFontSize(Control container, string str)
        {
            Font font = container.Font;
            string fontName = font.Name;
            using (Graphics g = container.CreateGraphics())
            {
                while (g.MeasureString(str, font, container.Width).Height > container.Height || g.MeasureString(str, font, container.Width).Width > container.Width)
                {
                    font = new Font(fontName, font.Size - 1);
                }
                while (g.MeasureString(str, font, container.Width).Height < container.Height - 1 && g.MeasureString(str, font, container.Width).Width < container.Width - 1)
                {
                    font = new Font(fontName, font.Size + 1);
                }
            }
            return font.Size;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void GetRanCode()
        {
            string str = "";
            string str2 = "123456789";
            try
            {
                if (this._CodeType == CodeTypeEnum.数字)
                {
                    str2 = "123456789";
                }
                else if (this._CodeType == CodeTypeEnum.字母)
                {
                    str2 = "abcdefghijkmnpqrstuvwxyz";
                }
                else if (this._CodeType == CodeTypeEnum.数字和字母)
                {
                    str2 = "123456789abcdefghijkmnpqrstuvwxyz";
                }
                Random random = new Random();
                for (int i = 0; i < this._CodeLength; i++)
                {
                    str = str + str2.Substring(random.Next(0, str2.Length), 1);
                }
            }
            catch (Exception)
            {
            }
            this._CodeText = str.ToUpper();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.picCode = new PictureBox();
            this.toolTip1 = new ToolTip(this.components);
            ((ISupportInitialize)this.picCode).BeginInit();
            this.SuspendLayout();
            this.picCode.Dock = DockStyle.Fill;
            this.picCode.Location = new Point(0, 0);
            this.picCode.Name = "picCode";
            this.picCode.Size = new Size(0x34, 0x16);
            this.picCode.TabIndex = 0;
            this.picCode.TabStop = false;
            this.toolTip1.SetToolTip(this.picCode, "点击重新生成验证码");
            this.picCode.Click += new EventHandler(this.picCode_Click);
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ToolTipIcon = ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "提示：";
            this.AutoScaleDimensions = new SizeF(6f, 12f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Transparent;
            this.Controls.Add(this.picCode);
            this.Cursor = Cursors.Hand;
            this.Name = "ValidateCode";
            this.Size = new Size(0x34, 0x16);
            this.Load += new EventHandler(this.ValidateCode_Load);
            ((ISupportInitialize)this.picCode).EndInit();
            this.ResumeLayout(false);
        }

        private void picCode_Click(object sender, EventArgs e)
        {
            this.CreateImage();
        }

        private void ValidateCode_Load(object sender, EventArgs e)
        {
            this._CodeText = "";
            this.toolTip1.SetToolTip(this.picCode, this.CodeToolTipText);
            this.CreateImage();
        }

        // Properties
        [Description("背景噪点线数"), Category("自定义属性"), DefaultValue(50)]
        public int CodeBlackPen
        {
            get
            {
                return this._BlackPen;
            }
            set
            {
                this._BlackPen = value;
                if (this._BlackPen < 0)
                {
                    this._BlackPen = 50;
                }
                this.CreateImage();
            }
        }

        [Category("自定义属性"), DefaultValue(4), Description("随机码的长度，取值范围：[2,8]")]
        public int CodeLength
        {
            get
            {
                return this._CodeLength;
            }
            set
            {
                this._CodeLength = value;
                if (this._CodeLength < 2)
                {
                    this._CodeLength = 2;
                }
                if (this._CodeLength > 8)
                {
                    this._CodeLength = 8;
                }
                this.CreateImage();
            }
        }

        [DefaultValue(""), Description("验证码内容"), Category("自定义属性")]
        public string CodeText
        {
            get
            {
                return this._CodeText;
            }
        }

        [Category("自定义属性"), DefaultValue("点击重新生成验证码"), Description("重新生成验证码提示内容")]
        public string CodeToolTipText
        {
            get
            {
                return this._TooltipText;
            }
            set
            {
                this._TooltipText = value;
            }
        }

        [Category("自定义属性"), Description("验证码类型：数字、字母、数字+字母"), DefaultValue(1)]
        public CodeTypeEnum CodeType
        {
            get
            {
                return this._CodeType;
            }
            set
            {
                this._CodeType = value;
                //if (this._CodeType == CodeTypeEnum.数字)
                //{
                //    this._CodeType = CodeTypeEnum.数字;
                //}
                //else if (this._CodeType == CodeTypeEnum.字母)
                //{
                //    this._CodeType = CodeTypeEnum.字母;
                //}
                //else if (this._CodeType == CodeTypeEnum.数字和字母)
                //{
                //    this._CodeType = CodeTypeEnum.数字和字母;
                //}
                this.CreateImage();
            }
        }

        // Nested Types
        public enum CodeTypeEnum
        {
            数字 = 1,
            数字和字母 = 3,
            字母 = 2
        }
    }
}