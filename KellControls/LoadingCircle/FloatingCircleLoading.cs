using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KellControls
{
    public partial class FloatingCircleLoading : Form
    {
        public FloatingCircleLoading()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            Application.DoEvents();
        }
        public FloatingCircleLoading(byte width)
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            this.Size = new Size(width, width);
            Application.DoEvents();
        }

        bool down;
        Point drop;

        private void LoadingCircle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                down = true;
                drop = e.Location;
            }
        }

        private void LoadingCircle_MouseMove(object sender, MouseEventArgs e)
        {
            if (down)
            {
                Point screen = LoadingCircle.PointToScreen(e.Location);
                this.SetDesktopLocation(screen.X - drop.X, screen.Y - drop.Y);
            }
        }

        private void LoadingCircle_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                down = false;
            }
        }

        private void 关闭载入提示ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
