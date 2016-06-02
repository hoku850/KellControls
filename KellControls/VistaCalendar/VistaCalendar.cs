using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace KellControls
{
    public partial class VistaCalendar : UserControl
    {
        Timer timer = new Timer();
        bool useToday = true;
        DateTime curDate;
        string strYearMonth, strDay, strDayOfWeek, strCnYearInfo;
        Font fontDefault = new Font("微软雅黑", 9);
        Font fontDay = new Font("微软雅黑", 46);
        Font fontCnYearInfo = new Font("微软雅黑", 7.5f);

        public enum VistaCalendarStyle
        {
            Default, Red, Purple, Green, Gray, Brown, Blue
        }

        private VistaCalendarStyle style = VistaCalendarStyle.Default;
        /// <summary>
        /// 样式
        /// </summary>
        [DefaultValue(VistaCalendarStyle.Default)]
        public VistaCalendarStyle Style
        {
            get { return style; }
            set { style = value; this.Invalidate(); }
        }

        public VistaCalendar()
        {
            InitializeComponent();

            curDate = DateTime.Today;
            UpdateCurrentDateInfo();

            timer.Interval = 6000;  //1分钟刷新一次
            timer.Enabled = true;
            timer.Tick += new EventHandler(timer_Tick);

            this.BackColor = Color.Transparent;
            this.DoubleBuffered = true;

            this.Paint += new PaintEventHandler(VistaCalendar_Paint);
            this.Resize += new EventHandler(VistaCalendar_Resize);
            this.MouseUp += new MouseEventHandler(VistaCalendar_MouseUp);
        }

        void VistaCalendar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(this, e.Location);
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (useToday && curDate != DateTime.Today)
            {
                curDate = DateTime.Today;
                UpdateCurrentDateInfo();

                this.Invalidate();
            }
        }

        /// <summary>
        /// 更新当前日期信息
        /// </summary>
        private void UpdateCurrentDateInfo()
        {
            strYearMonth = curDate.Year.ToString() + "年  " + curDate.Month.ToString() + "月";
            strDay = curDate.Day.ToString();
            strDayOfWeek = GetDayOfWeekString(curDate);

            ChineseCalendarInfo cn = new ChineseCalendarInfo(curDate);
            string cnInfo = cn.LunarYearSexagenary;
            cnInfo += cn.LunarYearAnimal + "年  ";
            cnInfo += cn.LunarMonthText + "月";
            cnInfo += cn.LunarDayText;
            strCnYearInfo = cnInfo;
        }

        /// <summary>
        /// 获取星期的中文字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private string GetDayOfWeekString(DateTime date)
        {
            string weekday;
            int dayOfWeek = (int)date.DayOfWeek;
            if (dayOfWeek == 1)
                weekday = "星期一 ";
            else if (dayOfWeek == 2)
                weekday = "星期二 ";
            else if (dayOfWeek == 3)
                weekday = "星期三 ";
            else if (dayOfWeek == 4)
                weekday = "星期四 ";
            else if (dayOfWeek == 5)
                weekday = "星期五 ";
            else if (dayOfWeek == 6)
                weekday = "星期六 ";
            else
                weekday = "星期日 ";
            return weekday;
        }

        private CustomRectangle positionRect = new CustomRectangle();
        /// <summary>
        /// 位置矩形
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CustomRectangle PositionRect
        {
            get { return positionRect; }
            set { positionRect = value; }
        }

        void VistaCalendar_Resize(object sender, EventArgs e)
        {
            if (positionRect == null) positionRect = new CustomRectangle();
            positionRect.Width = (int)ImageBg.Width;
            positionRect.Height = (int)ImageBg.Height;
            positionRect.Top = (int)(this.ClientSize.Height < ImageBg.Height ? 0 : (this.ClientSize.Height - ImageBg.Height) / 2f);
            positionRect.Left = (int)(this.ClientSize.Width < ImageBg.Width ? 0 : (this.ClientSize.Width - ImageBg.Width) / 2f);

            this.Invalidate();
        }

        internal SolidBrush FontBrush
        {
            get
            {
                if (style == VistaCalendarStyle.Green)
                    return new SolidBrush(Color.Black);
                else
                    return new SolidBrush(Color.White);
            }
        }

        void VistaCalendar_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            //背景
            e.Graphics.DrawImage(ImageBg, positionRect.ToRectangle());

            //绘制日期
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString(strYearMonth, fontDefault, FontBrush,
                new Rectangle((int)positionRect.X + 10, (int)positionRect.Y + 26, (int)positionRect.Width - 20, 20), format);
            e.Graphics.DrawString(strDayOfWeek, fontDefault, FontBrush,
                new Rectangle((int)positionRect.X + 10, (int)positionRect.Y + 106, (int)positionRect.Width - 20, 20), format);
            e.Graphics.DrawString(strCnYearInfo, fontCnYearInfo, FontBrush,
                new Rectangle((int)positionRect.X + 10, (int)positionRect.Y + 122, (int)positionRect.Width - 20, 20), format);

            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            e.Graphics.DrawString(strDay, fontDay, new SolidBrush(Color.Black),
                new Rectangle((int)positionRect.X + 12, (int)positionRect.Y + 40, (int)positionRect.Width - 20, 80), format);
            e.Graphics.DrawString(strDay, fontDay, new SolidBrush(Color.White),
                new Rectangle((int)positionRect.X + 10, (int)positionRect.Y + 38, (int)positionRect.Width - 20, 80), format);

        }

        /// <summary>
        /// 背景图片
        /// </summary>
        private Bitmap ImageBg
        {
            get
            {
                switch (style)
                {
                    case VistaCalendarStyle.Blue:
                        return KellControls.Properties.Resources.Calendar_bg_blue;
                    case VistaCalendarStyle.Brown:
                        return KellControls.Properties.Resources.Calendar_bg_brown;
                    case VistaCalendarStyle.Gray:
                        return KellControls.Properties.Resources.Calendar_bg_gray;
                    case VistaCalendarStyle.Green:
                        return KellControls.Properties.Resources.Calendar_bg_green;
                    case VistaCalendarStyle.Purple:
                        return KellControls.Properties.Resources.Calendar_bg_purple;
                    case VistaCalendarStyle.Red:
                        return KellControls.Properties.Resources.Calendar_bg_red;
                    default:
                        return KellControls.Properties.Resources.Calendar_bg;
                }
            }
        }

        private void mnu炫菲橘黄_Click(object sender, EventArgs e)
        {
            this.style = VistaCalendarStyle.Default;
            this.Invalidate();
        }

        private void mnu蔚然天蓝_Click(object sender, EventArgs e)
        {
            this.style = VistaCalendarStyle.Blue;
            this.Invalidate();
        }

        private void mnu烟熏黛紫_Click(object sender, EventArgs e)
        {
            this.style = VistaCalendarStyle.Purple;
            this.Invalidate();
        }

        private void mnu时尚晶红_Click(object sender, EventArgs e)
        {
            this.style = VistaCalendarStyle.Red;
            this.Invalidate();
        }

        private void mnu流光荧绿_Click(object sender, EventArgs e)
        {
            this.style = VistaCalendarStyle.Green;
            this.Invalidate();
        }

        private void mnu榛情咖啡_Click(object sender, EventArgs e)
        {
            this.style = VistaCalendarStyle.Brown;
            this.Invalidate();
        }

        private void mnu沉稳雅灰_Click(object sender, EventArgs e)
        {
            this.style = VistaCalendarStyle.Gray;
            this.Invalidate();
        }

        private void mnu转到上一月_Click(object sender, EventArgs e)
        {
            useToday = false;
            curDate = curDate.AddMonths(-1);
            UpdateCurrentDateInfo();
            this.Invalidate();
        }

        private void mnu转到上一天_Click(object sender, EventArgs e)
        {
            useToday = false;
            curDate = curDate.AddDays(-1);
            UpdateCurrentDateInfo();
            this.Invalidate();
        }

        private void mnu转到今天_Click(object sender, EventArgs e)
        {
            useToday = true;
            curDate = DateTime.Today;
            UpdateCurrentDateInfo();
            this.Invalidate();
        }

        private void mnu转到下一天_Click(object sender, EventArgs e)
        {
            useToday = false;
            curDate = curDate.AddDays(1);
            UpdateCurrentDateInfo();
            this.Invalidate();
        }

        private void mnu转到下一月_Click(object sender, EventArgs e)
        {
            useToday = false;
            curDate = curDate.AddMonths(1);
            UpdateCurrentDateInfo();
            this.Invalidate();
        }

    }
}

