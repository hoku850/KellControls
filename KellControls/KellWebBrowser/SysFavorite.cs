using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using System.Configuration;
using KellCommons;

namespace KellControls
{
    public partial class SysFavorite : Form
    {
        public SysFavorite()
        {
            InitializeComponent();

            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith(KellWebBrowser.SearchEngineConfigName, true, null))
                {
                    string[] engs = ConfigurationManager.AppSettings[key].Split('|');
                    if (engs.Length > 0)
                    {
                        comboBox1.Items.Add(engs[0].Substring(1));
                    }
                }
            }
            if (comboBox1.Items.Count == 0)
                comboBox1.Items.Add(NoneSearchEngine);
        }

        int homeIndex = -1;
        UrlObject home;
        bool loadOver;
        const string NoneSearchEngine = "搜索引擎未配置";

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = button5.Enabled = button1.Enabled = button2.Enabled = listBox1.SelectedIndex > -1;
            if (listBox1.SelectedIndex > -1)
            {
                UrlObject uo = listBox1.Items[listBox1.SelectedIndex] as UrlObject;
                textBox1.Text = uo.Title;
                textBox2.Text = uo.Url;
                if (uo.Image != null)
                    pictureBox1.Image = uo.Image.ToBitmap();
                else
                    pictureBox1.Image = null;
                numericUpDown1.Value = uo.Access;
                if (uo.IsHome)
                    button5.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                KellWebBrowser.Favorites[listBox1.SelectedIndex].Title = textBox1.Text;
                using (FileStream fs = new FileStream(KellWebBrowser.FavFile, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, KellWebBrowser.Favorites);
                }
                listBox1.Items[listBox1.SelectedIndex] = KellWebBrowser.Favorites[listBox1.SelectedIndex];
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("请先选择要修改标题的页面！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                if (System.Windows.Forms.MessageBox.Show("确定要删除[" + KellWebBrowser.Favorites[listBox1.SelectedIndex].Title + "]页面？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    KellWebBrowser.Favorites.RemoveAt(listBox1.SelectedIndex);
                    using (FileStream fs = new FileStream(KellWebBrowser.FavFile, FileMode.Create, FileAccess.Write))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(fs, KellWebBrowser.Favorites);
                    }
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("请先选择要删除的页面！");
            }
        }

        private void SysFavorite_Load(object sender, EventArgs e)
        {
            pictureBox1.Width = pictureBox1.Height = KellWebBrowser.IconWidth;
            KellWebBrowser.Favorites.Sort(new UrlComparer());
            foreach (UrlObject uo in KellWebBrowser.Favorites)
            {
                listBox1.Items.Add(uo);
                if (uo.IsHome)
                {
                    homeIndex = listBox1.Items.Count - 1;
                    home = uo;
                }
            }
            button3.Enabled = KellWebBrowser.HistoryUrlList.Count > 0;
            checkBox1.Checked = KellCommon.ReadIni("AppSettings", "NewWindow", KellCommon.IniFile) == "0" ? true : false;
            string search = KellCommon.ReadIni("AppSettings", "Search", KellCommon.IniFile);
            if (!string.IsNullOrEmpty(search))
            {
                int index;
                if (int.TryParse(search[0].ToString(), out index))
                {
                    comboBox1.SelectedIndex = index;
                }
            }
            if (comboBox1.Items.Count == 1 && comboBox1.Items[0] != null && comboBox1.Items[0].ToString() == NoneSearchEngine)
                comboBox1.SelectedIndex = 0;
            loadOver = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("确定要删除全部的访问痕迹？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                KellWebBrowser.HistoryUrlList.Clear();
                using (FileStream fs = new FileStream(KellWebBrowser.AccessFile, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, KellWebBrowser.HistoryUrlList);
                }
                button3.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start(KellWebBrowser.LogFile);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string val = checkBox1.Checked ? "0" : "1";
            KellCommon.WriteIni("AppSettings", "NewWindow", val, KellCommon.IniFile);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                KellWebBrowser.Favorites[listBox1.SelectedIndex].Access = (int)numericUpDown1.Value;
                using (FileStream fs = new FileStream(KellWebBrowser.FavFile, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, KellWebBrowser.Favorites);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                KellWebBrowser.Home = DnsConvert.ToPunycode(KellWebBrowser.Favorites[listBox1.SelectedIndex].Url);
                foreach (UrlObject u in KellWebBrowser.Favorites)
                {
                    u.IsHome = false;
                }
                KellWebBrowser.Favorites[listBox1.SelectedIndex].IsHome = true;
                foreach (UrlObject u in KellWebBrowser.HistoryUrlList)
                {
                    u.IsHome = DnsConvert.ToPunycode(u.Url) == KellWebBrowser.Home;
                }
                listBox1.Items[listBox1.SelectedIndex] = KellWebBrowser.Favorites[listBox1.SelectedIndex];
                if (homeIndex > -1)
                {
                    listBox1.Items[homeIndex] = home;
                }
                homeIndex = listBox1.SelectedIndex;
                home = KellWebBrowser.Favorites[listBox1.SelectedIndex];
                //MessageBox.Show("主页设置完毕！");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadOver && comboBox1.SelectedIndex > -1 && comboBox1.Items[comboBox1.SelectedIndex] != null && comboBox1.Items[comboBox1.SelectedIndex].ToString() != NoneSearchEngine)
            {
                string s = KellCommon.ReadAppSetting(KellWebBrowser.SearchEngineConfigName + comboBox1.SelectedIndex);
                if (!string.IsNullOrEmpty(s))
                {
                    KellCommon.WriteIni("AppSettings", "Search", s.Replace('+', '&'), KellCommon.IniFile);
                }
            }
        }
    }
}
