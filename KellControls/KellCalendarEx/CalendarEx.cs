using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KellCalendarEx
{
    public partial class CalendarEx : UserControl
    {
        public CalendarEx()
        {
            InitializeComponent();
        }

        DateTime currentMon;

        public DateTime CurrentMonth
        {
            get { return currentMon; }
            set
            {
                currentMon = value;
                currentMonLabel.Text = currentMon.Year + "年" + currentMon.Month + "月";
                LoadCurrentMonthWeeks();
            }
        }

        void LoadCurrentMonthWeeks()
        {
            panel00.Controls.Clear();
            panel01.Controls.Clear();
            panel02.Controls.Clear();
            panel03.Controls.Clear();
            panel04.Controls.Clear();
            panel05.Controls.Clear();
            panel06.Controls.Clear();
            panel10.Controls.Clear();
            panel11.Controls.Clear();
            panel12.Controls.Clear();
            panel13.Controls.Clear();
            panel14.Controls.Clear();
            panel15.Controls.Clear();
            panel16.Controls.Clear();
            panel20.Controls.Clear();
            panel21.Controls.Clear();
            panel22.Controls.Clear();
            panel23.Controls.Clear();
            panel24.Controls.Clear();
            panel25.Controls.Clear();
            panel26.Controls.Clear();
            panel30.Controls.Clear();
            panel31.Controls.Clear();
            panel32.Controls.Clear();
            panel33.Controls.Clear();
            panel34.Controls.Clear();
            panel35.Controls.Clear();
            panel36.Controls.Clear();
            panel40.Controls.Clear();
            panel41.Controls.Clear();
            panel42.Controls.Clear();
            panel43.Controls.Clear();
            panel44.Controls.Clear();
            panel45.Controls.Clear();
            panel46.Controls.Clear();
            int[,] mon = new int[5, 7];
            int firstWeekID = (int)DateTime.Parse(currentMon.Year + "-" + currentMon.Month + "-1").DayOfWeek;
            if (firstWeekID != 0)
            {
                for (int i = 0; i < firstWeekID; i++)
                {
                    mon[0, i] = i + 1;
                }
            }
            int daysInMonth = DateTime.DaysInMonth(currentMon.Year, currentMon.Month);
            int finalWeekID = (int)DateTime.Parse(currentMon.Year + "-" + currentMon.Month + "-" + daysInMonth).DayOfWeek;
            if (finalWeekID != 6)
            {
                int nextCount = 6 - finalWeekID;
                for (int i = 0; i < nextCount; i++)
                {
                    mon[4, i] = i + 1;
                }
            }
            int r = -1;
            for (int i = 0; i < daysInMonth; i++)
            {
                if (i % 7 == 0)
                    r++;
                mon[r, i % 7] = i + 1;
            }
            for (int i = 0; i < mon.GetLength(0); i++)
            {
                for (int j = 0; j < mon.GetLength(1); j++)
                {
                    Label l = new Label();
                    l.Location = new Point(0, 0);
                    l.Text = mon[i, j].ToString();
                    l.Font = new Font("宋体", 9.0F, FontStyle.Bold);
                    if (i == 0)//星期日红色标识
                    {
                        l.ForeColor = Color.Red;
                    }
                    else
                    {
                        l.ForeColor = Color.Black;
                    }
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                }
            }
        }

        private void CalendarEx_Load(object sender, EventArgs e)
        {
            CurrentMonth = DateTime.Today;
        }

        void lastMon_Click(object sender, EventArgs e)
        {
            int year = currentMon.Year;
            int lastMonth = currentMon.Month - 1;
            if (lastMonth == 0)
            {
                lastMonth = 12;
                year--;
            }
            CurrentMonth = new DateTime(year, lastMonth, 1);
        }

        void nextMon_Click(object sender, EventArgs e)
        {
            int year = currentMon.Year;
            int nextMonth = currentMon.Month + 1;
            if (nextMonth == 13)
            {
                nextMonth = 1;
                year++;
            }
            CurrentMonth = new DateTime(year, nextMonth, 1);
        }
    }
}
