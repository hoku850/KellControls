using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace KellCalendarEx
{
    [DefaultEvent("MonthChanged")]
    [DefaultProperty("TodayColor")]
    public partial class KellCalendarEx : UserControl
    {
        public KellCalendarEx()
        {
            InitializeComponent();
            tableLayoutPanel1.SetColumnSpan(currentMonLabel, 5);
            msgs = new Dictionary<string, List<LinkObject>>();
        }

        YearMonth currentMon = YearMonth.ToMonth;
        YearMonth lastMonth;
        int[,] mondata = new int[6, 7];
        bool created;
        Dictionary<string, List<LinkObject>> msgs;
        Color todayColor = Color.FromArgb(40, Color.Blue);
        public event EventHandler<YearMonth> MonthChanged;

        private void OnMonthChanged(YearMonth e)
        {
            if (e != lastMonth)
            {
                if (MonthChanged != null)
                {
                    MonthChanged(this, e);
                }
                lastMonth = e;
            }
        }

        [Description("“今日”的标识颜色")]
        public Color TodayColor
        {
            get
            {
                return todayColor;
            }
            set
            {
                todayColor = value;
            }
        }

        [Browsable(false)]
        public YearMonth CurrentMonth
        {
            get { return currentMon; }
            internal set
            {
                if (created && value.Year == currentMon.Year && value.Month == currentMon.Month)
                {
                }
                else
                {
                    if (value != null)
                    {
                        currentMon = value;
                        currentMonLabel.Text = currentMon.Year + "年" + currentMon.Month + "月";
                        LoadCurrentMonthWeeks();
                        created = true;
                    }
                }
            }
        }

        private void LoadCurrentMonthWeeks()
        {
            DateTime today = DateTime.Today;
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
            panel50.Controls.Clear();
            panel51.Controls.Clear();
            panel52.Controls.Clear();
            panel53.Controls.Clear();
            panel54.Controls.Clear();
            panel55.Controls.Clear();
            panel56.Controls.Clear();

            panel00.BackColor = Color.Transparent;
            panel01.BackColor = Color.Transparent;
            panel02.BackColor = Color.Transparent;
            panel03.BackColor = Color.Transparent;
            panel04.BackColor = Color.Transparent;
            panel05.BackColor = Color.Transparent;
            panel06.BackColor = Color.Transparent;
            panel10.BackColor = Color.Transparent;
            panel11.BackColor = Color.Transparent;
            panel12.BackColor = Color.Transparent;
            panel13.BackColor = Color.Transparent;
            panel14.BackColor = Color.Transparent;
            panel15.BackColor = Color.Transparent;
            panel16.BackColor = Color.Transparent;
            panel20.BackColor = Color.Transparent;
            panel21.BackColor = Color.Transparent;
            panel22.BackColor = Color.Transparent;
            panel23.BackColor = Color.Transparent;
            panel24.BackColor = Color.Transparent;
            panel25.BackColor = Color.Transparent;
            panel26.BackColor = Color.Transparent;
            panel30.BackColor = Color.Transparent;
            panel31.BackColor = Color.Transparent;
            panel32.BackColor = Color.Transparent;
            panel33.BackColor = Color.Transparent;
            panel34.BackColor = Color.Transparent;
            panel35.BackColor = Color.Transparent;
            panel36.BackColor = Color.Transparent;
            panel40.BackColor = Color.Transparent;
            panel41.BackColor = Color.Transparent;
            panel42.BackColor = Color.Transparent;
            panel43.BackColor = Color.Transparent;
            panel44.BackColor = Color.Transparent;
            panel45.BackColor = Color.Transparent;
            panel46.BackColor = Color.Transparent;
            panel50.BackColor = Color.Transparent;
            panel51.BackColor = Color.Transparent;
            panel52.BackColor = Color.Transparent;
            panel53.BackColor = Color.Transparent;
            panel54.BackColor = Color.Transparent;
            panel55.BackColor = Color.Transparent;
            panel56.BackColor = Color.Transparent;

            mondata.Init();

            int firstWeekID = (int)DateTime.Parse(currentMon.Year + "-" + currentMon.Month + "-1").DayOfWeek;
            if (firstWeekID != 0)
            {
                YearMonth prevMon = currentMon.PrevMonth;
                int daysInPrevMonth = DateTime.DaysInMonth(prevMon.Year, prevMon.Month);
                for (int i = firstWeekID; i > 0; i--)
                {
                    mondata[0, i - 1] = daysInPrevMonth--;
                }
            }
            int daysInMonth = DateTime.DaysInMonth(currentMon.Year, currentMon.Month);
            int r = 0;
            if (firstWeekID == 0)
                r = -1;
            for (int i = 0; i < daysInMonth; i++)
            {
                int c = (i + firstWeekID) % 7;
                if (c == 0) r++;
                if (DateTime.Parse(currentMon + "-" + Convert.ToString(i + 1)).Equals(today))
                {
                    mondata[r, c] = (i + 1) * 100;
                }
                else
                {
                    mondata[r, c] = i + 1;
                }
            }

            int finalWeekID = (int)DateTime.Parse(currentMon.Year + "-" + currentMon.Month + "-" + daysInMonth).DayOfWeek;

            int restDays = 6 * 7 - (firstWeekID + daysInMonth);
            int nextMonDay = 1;
            int begin = finalWeekID + 1;
            for (int i = begin; i < begin + restDays; i++)
            {
                int c = i % 7;
                if (c == 0) r++;
                mondata[r, c] = nextMonDay++;
            }

            int last = mondata[0, 0];
            for (int i = 0; i < mondata.GetLength(0); i++)
            {
                for (int j = 0; j < mondata.GetLength(1); j++)
                {
                    Label l = new Label();
                    l.AutoSize = true;
                    l.Location = new Point(0, 0);
                    l.Text = mondata[i, j].ToString();
                    l.Font = new Font("宋体", 8.0F, FontStyle.Bold);
                    if (j == 0)//星期日红色标识
                    {
                        l.ForeColor = Color.Red;
                    }
                    else
                    {
                        l.ForeColor = Color.Black;
                    }
                    l.BackColor = Color.Transparent;

                    if (i == 0 && j == 0)
                        panel00.Controls.Add(l);
                    if (i == 0 && j == 1)
                        panel01.Controls.Add(l);
                    if (i == 0 && j == 2)
                        panel02.Controls.Add(l);
                    if (i == 0 && j == 3)
                        panel03.Controls.Add(l);
                    if (i == 0 && j == 4)
                        panel04.Controls.Add(l);
                    if (i == 0 && j == 5)
                        panel05.Controls.Add(l);
                    if (i == 0 && j == 6)
                        panel06.Controls.Add(l);
                    if (i == 1 && j == 0)
                        panel10.Controls.Add(l);
                    if (i == 1 && j == 1)
                        panel11.Controls.Add(l);
                    if (i == 1 && j == 2)
                        panel12.Controls.Add(l);
                    if (i == 1 && j == 3)
                        panel13.Controls.Add(l);
                    if (i == 1 && j == 4)
                        panel14.Controls.Add(l);
                    if (i == 1 && j == 5)
                        panel15.Controls.Add(l);
                    if (i == 1 && j == 6)
                        panel16.Controls.Add(l);
                    if (i == 2 && j == 0)
                        panel20.Controls.Add(l);
                    if (i == 2 && j == 1)
                        panel21.Controls.Add(l);
                    if (i == 2 && j == 2)
                        panel22.Controls.Add(l);
                    if (i == 2 && j == 3)
                        panel23.Controls.Add(l);
                    if (i == 2 && j == 4)
                        panel24.Controls.Add(l);
                    if (i == 2 && j == 5)
                        panel25.Controls.Add(l);
                    if (i == 2 && j == 6)
                        panel26.Controls.Add(l);
                    if (i == 3 && j == 0)
                        panel30.Controls.Add(l);
                    if (i == 3 && j == 1)
                        panel31.Controls.Add(l);
                    if (i == 3 && j == 2)
                        panel32.Controls.Add(l);
                    if (i == 3 && j == 3)
                        panel33.Controls.Add(l);
                    if (i == 3 && j == 4)
                        panel34.Controls.Add(l);
                    if (i == 3 && j == 5)
                        panel35.Controls.Add(l);
                    if (i == 3 && j == 6)
                        panel36.Controls.Add(l);
                    //if (firstWeekID == 0 && daysInMonth == 28)
                    //{
                    //}
                    //else
                    //{
                    if (i == 4 && j == 0)
                        panel40.Controls.Add(l);
                    if (i == 4 && j == 1)
                        panel41.Controls.Add(l);
                    if (i == 4 && j == 2)
                        panel42.Controls.Add(l);
                    if (i == 4 && j == 3)
                        panel43.Controls.Add(l);
                    if (i == 4 && j == 4)
                        panel44.Controls.Add(l);
                    if (i == 4 && j == 5)
                        panel45.Controls.Add(l);
                    if (i == 4 && j == 6)
                        panel46.Controls.Add(l);
                    if (i == 5 && j == 0)
                        panel50.Controls.Add(l);
                    if (i == 5 && j == 1)
                        panel51.Controls.Add(l);
                    if (i == 5 && j == 2)
                        panel52.Controls.Add(l);
                    if (i == 5 && j == 3)
                        panel53.Controls.Add(l);
                    if (i == 5 && j == 4)
                        panel54.Controls.Add(l);
                    if (i == 5 && j == 5)
                        panel55.Controls.Add(l);
                    if (i == 5 && j == 6)
                        panel56.Controls.Add(l);
                    //}
                    if (mondata[i, j] >= 100)
                    {
                        l.Text = Convert.ToString((mondata[i, j] / 100));
                        l.Parent.BackColor = todayColor;
                    }
                    int day = mondata[i, j];
                    YearMonth cur = currentMon;
                    if (firstWeekID != 0 && i == 0)
                    {
                        cur = currentMon.PrevMonth;
                    }
                    if (day < last)
                    {
                        cur = currentMon.NextMonth;
                    }
                    last = day;
                    string msgKey = cur.Year.ToString() + cur.Month.ToString() + day.ToString();
                    if (msgs.ContainsKey(msgKey))
                    {
                        SetLinkInDay(cur.Year, cur.Month, day, msgs[msgKey]);
                    }
                }
            }
        }

        private void CalendarEx_Load(object sender, EventArgs e)
        {
            GotoToMonth();
        }

        void lastMon_Click(object sender, EventArgs e)
        {
            CurrentMonth = currentMon.PrevMonth;
            OnMonthChanged(CurrentMonth);
        }

        void nextMon_Click(object sender, EventArgs e)
        {
            CurrentMonth = currentMon.NextMonth;
            OnMonthChanged(CurrentMonth);
        }

        private void currentMonLabel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GotoToMonth();
        }

        public void GotoToMonth()
        {
            CurrentMonth = YearMonth.ToMonth;
            OnMonthChanged(CurrentMonth);
        }

        public void ClearLinkMsg()
        {
            msgs.Clear();
        }

        public void AddLinkMsg(int year, int month, int day, List<LinkObject> links)
        {
            string key = year.ToString() + month.ToString() + day.ToString();
            if (msgs.ContainsKey(key))
            {
                msgs.Remove(key);
            }
            msgs.Add(key, links);
            SetLinkInDay(year, month, day, links);
        }

        private bool SetLinkInDay(int year, int month, int day, List<LinkObject> links)
        {
            if (links != null && links.Count > 0)
            {
                foreach (LinkObject link in links)
                {
                    Label l = new Label();
                    l.AutoSize = false;
                    l.Text = link.Text;
                    l.TextAlign = ContentAlignment.MiddleCenter;
                    l.Dock = DockStyle.Bottom;
                    l.Cursor = Cursors.Hand;
                    l.ForeColor = link.Status;
                    l.BackColor = Color.Transparent;
                    l.Tag = link;
                    l.MouseClick += OpenDetail;
                    DateTime today = DateTime.Today;
                    bool isToday = year == today.Year && month == today.Month && day == today.Day;
                    Point position = GetCellAddressByDay(day, isToday);
                    if (position.X == 0 && position.Y == 0) panel00.Controls.Add(l);
                    if (position.X == 0 && position.Y == 1) panel01.Controls.Add(l);
                    if (position.X == 0 && position.Y == 2) panel02.Controls.Add(l);
                    if (position.X == 0 && position.Y == 3) panel03.Controls.Add(l);
                    if (position.X == 0 && position.Y == 4) panel04.Controls.Add(l);
                    if (position.X == 0 && position.Y == 5) panel05.Controls.Add(l);
                    if (position.X == 0 && position.Y == 6) panel06.Controls.Add(l);
                    if (position.X == 1 && position.Y == 0) panel10.Controls.Add(l);
                    if (position.X == 1 && position.Y == 1) panel11.Controls.Add(l);
                    if (position.X == 1 && position.Y == 2) panel12.Controls.Add(l);
                    if (position.X == 1 && position.Y == 3) panel13.Controls.Add(l);
                    if (position.X == 1 && position.Y == 4) panel14.Controls.Add(l);
                    if (position.X == 1 && position.Y == 5) panel15.Controls.Add(l);
                    if (position.X == 1 && position.Y == 6) panel16.Controls.Add(l);
                    if (position.X == 2 && position.Y == 0) panel20.Controls.Add(l);
                    if (position.X == 2 && position.Y == 1) panel21.Controls.Add(l);
                    if (position.X == 2 && position.Y == 2) panel22.Controls.Add(l);
                    if (position.X == 2 && position.Y == 3) panel23.Controls.Add(l);
                    if (position.X == 2 && position.Y == 4) panel24.Controls.Add(l);
                    if (position.X == 2 && position.Y == 5) panel25.Controls.Add(l);
                    if (position.X == 2 && position.Y == 6) panel26.Controls.Add(l);
                    if (position.X == 3 && position.Y == 0) panel30.Controls.Add(l);
                    if (position.X == 3 && position.Y == 1) panel31.Controls.Add(l);
                    if (position.X == 3 && position.Y == 2) panel32.Controls.Add(l);
                    if (position.X == 3 && position.Y == 3) panel33.Controls.Add(l);
                    if (position.X == 3 && position.Y == 4) panel34.Controls.Add(l);
                    if (position.X == 3 && position.Y == 5) panel35.Controls.Add(l);
                    if (position.X == 3 && position.Y == 6) panel36.Controls.Add(l);
                    if (position.X == 4 && position.Y == 0) panel40.Controls.Add(l);
                    if (position.X == 4 && position.Y == 1) panel41.Controls.Add(l);
                    if (position.X == 4 && position.Y == 2) panel42.Controls.Add(l);
                    if (position.X == 4 && position.Y == 3) panel43.Controls.Add(l);
                    if (position.X == 4 && position.Y == 4) panel44.Controls.Add(l);
                    if (position.X == 4 && position.Y == 5) panel45.Controls.Add(l);
                    if (position.X == 4 && position.Y == 6) panel46.Controls.Add(l);
                    if (position.X == 5 && position.Y == 0) panel50.Controls.Add(l);
                    if (position.X == 5 && position.Y == 1) panel51.Controls.Add(l);
                    if (position.X == 5 && position.Y == 2) panel52.Controls.Add(l);
                    if (position.X == 5 && position.Y == 3) panel53.Controls.Add(l);
                    if (position.X == 5 && position.Y == 4) panel54.Controls.Add(l);
                    if (position.X == 5 && position.Y == 5) panel55.Controls.Add(l);
                    if (position.X == 5 && position.Y == 6) panel56.Controls.Add(l);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OpenDetail(object sender, MouseEventArgs e)
        {
            Label l = sender as Label;
            LinkObject link = l.Tag as LinkObject;
            if (link != null)
            {
                if (link.IsInternal)
                {
                    try
                    {
                        if (link.Args != null)
                            link.Delegate.Method.Invoke(link.Target, link.Args);
                        else
                            link.Delegate.Method.Invoke(link.Target, null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("打开内部程序时出错：" + ex.Message);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(link.Address))
                    {
                        try
                        {
                            Process.Start(link.Address);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("打开外部程序时出错：" + ex.Message);
                        }
                    }
                }
            }
        }

        private Point GetCellAddressByDay(int day, bool isToday)
        {
            for (int i = 0; i < mondata.GetLength(0); i++)
            {
                for (int j = 0; j < mondata.GetLength(1); j++)
                {
                    if (isToday)
                    {
                        if (mondata[i, j] == day * 100)
                        {
                            return new Point(i, j);
                        }
                    }
                    else if (mondata[i, j] == day)
                    {
                        return new Point(i, j);
                    }
                }
            }
            return Point.Empty;
        }
    }

    public static class ArrayExtensions
    {
        public static void Init(this int[,] thisArray)
        {
            for (int i = 0; i < thisArray.GetLength(0); i++)
            {
                for (int j = 0; j < thisArray.GetLength(1); j++)
                {
                    thisArray[i, j] = 0;
                }
            }
        }
    }

    [Serializable]
    public class LinkObject
    {
        string text;

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }
        Color status;

        public Color Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }
        bool isInternal;

        public bool IsInternal
        {
            get { return isInternal; }
            set { isInternal = value; }
        }
        string address;

        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        Delegate delgt;

        public Delegate Delegate
        {
            get { return delgt; }
            set { delgt = value; }
        }
        object target;

        public object Target
        {
            get { return target; }
            set { target = value; }
        }
        object[] args;

        public object[] Args
        {
            get
            {
                return args;
            }

            set
            {
                args = value;
            }
        }

        public LinkObject(string text, Color status, bool isInternal, string address = null, Delegate delgt = null, object target = null, object[] args = null)
        {
            this.text = text;
            this.status = status;
            this.address = address;
            this.isInternal = isInternal;
            this.delgt = delgt;
            this.target = target;
            this.args = args;
        }

        public override string ToString()
        {
            return text;
        }
    }

    public class YearMonth : EventArgs
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public YearMonth(DateTime time)
        {
            this.Year = time.Year;
            this.Month = time.Month;
        }

        public YearMonth(int year, int month)
        {
            this.Year = year;
            this.Month = month;
        }

        public static bool operator==(YearMonth a, YearMonth b)
        {
            if (!object.Equals(a, null) && !object.Equals(b, null))
                return a.Year == b.Year && a.Month == b.Month;
            else if (object.Equals(a, null) && object.Equals(b, null))
                return true;
            else
                return false;
        }

        public static bool operator!=(YearMonth a, YearMonth b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is YearMonth)
            {
                YearMonth other = obj as YearMonth;
                if (other != null)
                    return this == other;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static YearMonth ToMonth
        {
            get
            {
                return new YearMonth(DateTime.Today);
            }
        }

        public YearMonth PrevMonth
        {
            get
            {
                int year = this.Year;
                int prevMonth = this.Month - 1;
                if (prevMonth == 0)
                {
                    prevMonth = 12;
                    year--;
                }
                return new YearMonth(year, prevMonth);
            }
        }

        public YearMonth NextMonth
        {
            get
            {
                int year = this.Year;
                int nextMonth = this.Month + 1;
                if (nextMonth == 13)
                {
                    nextMonth = 1;
                    year++;
                }
                return new YearMonth(year, nextMonth);
            }
        }

        public override string ToString()
        {
            return Year + "-" + Month;
        }
    }
}