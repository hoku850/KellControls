using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

namespace KellControls
{
    public partial class AddFavorite : Form
    {
        public AddFavorite()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool save = false;
            UrlObject uo;
            if (KellWebBrowser.Favorites.ContainsUrl(textBox2.Text, out uo))
            {
                if (MessageBox.Show("当前页面已经存在于收藏夹中，确定要覆盖原来的？", "覆盖提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    uo.Title = textBox1.Text;
                    uo.Image = KellWebBrowser.DocumentIcon;
                    UrlObject u;
                    if (KellWebBrowser.HistoryUrlList.ContainsUrl(textBox2.Text, out u))
                    {
                        uo.Access = u.Access;
                    }
                    if (DnsConvert.ToPunycode(uo.Url) == KellWebBrowser.Home)
                        uo.IsHome = true;
                    save = true;
                }
            }
            else
            {
                uo = new UrlObject(textBox1.Text, textBox2.Text, KellWebBrowser.DocumentIcon);
                UrlObject u;
                if (KellWebBrowser.HistoryUrlList.ContainsUrl(textBox2.Text, out u))
                {
                    uo.Access = u.Access;
                }
                if (DnsConvert.ToPunycode(uo.Url) == KellWebBrowser.Home)
                    uo.IsHome = true;
                KellWebBrowser.Favorites.Add(uo);
                save = true;
            }
            if (save)
            {
                using (FileStream fs = new FileStream(KellWebBrowser.FavFile, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, KellWebBrowser.Favorites);
                }
                MessageBox.Show("当前页面已经成功保存到收藏夹中！");
            }
        }

        private void AddFavorite_Load(object sender, EventArgs e)
        {
            pictureBox1.Width = pictureBox1.Height = KellWebBrowser.IconWidth;
            if (KellWebBrowser.DocumentIcon != null)
                pictureBox1.Image = KellWebBrowser.DocumentIcon.ToBitmap();
            textBox1.Text = KellWebBrowser.DocumentTitle;
            textBox2.Text = KellWebBrowser.Url;
            label4.Text = KellWebBrowser.Access.ToString();
        }
    }
}
