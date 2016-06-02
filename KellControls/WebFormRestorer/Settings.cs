using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace KellControls
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            WebFormRestorer.auto = numericUpDown1.Enabled = checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "表单模板|*.tmpl";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                WebFormRestorer.SaveFormItems(saveFileDialog1.FileName);
            }
            saveFileDialog1.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(WebFormRestorer.SavePath))
            {
                Directory.CreateDirectory(WebFormRestorer.SavePath);
            }
            Process.Start(WebFormRestorer.SavePath);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            WebFormRestorer.interval = (int)numericUpDown1.Value;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = WebFormRestorer.auto;
            numericUpDown1.Value = WebFormRestorer.interval;
            button1.Enabled = WebFormRestorer.Ready;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button1.Enabled = WebFormRestorer.Ready;
        }
    }
}
