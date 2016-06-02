using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace KellControls
{
    public partial class KellMediaAdvertisingForm : Form
    {
        private Rectangle WorkAreaRectangle;

        public KellMediaAdvertisingForm(string advUri, string linkUrl)
        {
            InitializeComponent();
            axWindowsMediaPlayer1.uiMode = "none";
            axWindowsMediaPlayer1.enableContextMenu = false;
            AdvUri = advUri;
            LinkUrl = linkUrl;
        }

        public string AdvUri { get; set; }

        public string LinkUrl { get; set; }

        private void KellMediaAdvertisingForm_Load(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.URL = AdvUri;
        }

        private void KellMediaAdvertisingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                axWindowsMediaPlayer1.close();
            }
            catch
            {
                axWindowsMediaPlayer1.Dispose();
            }
        }

        private void axWindowsMediaPlayer1_MouseUpEvent(object sender, AxWMPLib._WMPOCXEvents_MouseUpEvent e)
        {
            if (e.nButton == 1)
            {
                Process.Start(LinkUrl);
            }
        }

        public void ShowForm()
        {
            WorkAreaRectangle = Screen.GetWorkingArea(WorkAreaRectangle);
            this.Top = WorkAreaRectangle.Height + this.Height;
            this.SetBounds(WorkAreaRectangle.Width - this.Width, WorkAreaRectangle.Height - this.Height, this.Width, this.Height);
            this.Show();
        }

        public void ShowForm(string advUri, string linkUrl)
        {
            AdvUri = advUri;
            LinkUrl = linkUrl;
            axWindowsMediaPlayer1.URL = AdvUri;
            WorkAreaRectangle = Screen.GetWorkingArea(WorkAreaRectangle);
            this.Top = WorkAreaRectangle.Height + this.Height;
            this.SetBounds(WorkAreaRectangle.Width - this.Width, WorkAreaRectangle.Height - this.Height, this.Width, this.Height);
            this.Show();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Close();
            }
        }
    }
}
