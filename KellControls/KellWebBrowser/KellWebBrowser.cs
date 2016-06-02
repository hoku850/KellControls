using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using KellCommons;

namespace KellControls
{
    public partial class KellWebBrowser : UserControl
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct OLECMDTEXT
        {
            public uint cmdtextf;
            public uint cwActual;
            public uint cwBuf;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
            public char rgwz;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OLECMD
        {
            public uint cmdID;
            public uint cmdf;
        }

        // IOleCommandTarget的Interop定义
        [ComImport, Guid("b722bccb-4e68-101b-a2bc-00aa00404770"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleCommandTarget
        {
            //重要: 下面方法的顺序非常重要，因为本示例中我们使用的是早期绑定，详见MSDN中有关.NET/COM互操作的参考。
            void QueryStatus(ref Guid pguidCmdGroup, UInt32 cCmds,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] OLECMD[] prgCmds, ref OLECMDTEXT CmdText);
            void Exec(ref Guid pguidCmdGroup, uint nCmdId, uint nCmdExecOpt, ref object pvaIn, ref object pvaOut);
        }

        private Guid cmdGuid = new Guid("ED016940-BD5B-11CF-BA4E-00C04FD70816");
        private enum MiscCommandTarget { Find = 1, ViewSource, Options }

        private mshtml.HTMLDocument GetDocument()
        {
            try
            {
                mshtml.HTMLDocument htm = (mshtml.HTMLDocument)axWebBrowser1.Document;
                return htm;
            }
            catch
            {
                throw (new Exception("不能从WebBrowser控件中获取文件对象"));
            }
        }

        /// <summary>
        /// 显示“选项”对话框的方法
        /// </summary>
        public void InternetOptions()
        {
            IOleCommandTarget cmdt;
            Object o = new object();
            try
            {
                cmdt = (IOleCommandTarget)GetDocument();
                cmdt.Exec(ref cmdGuid, (uint)MiscCommandTarget.Options,
             (uint)SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, ref o, ref o);
            }
            catch
            {
                // 注意：因为该过程相应的CMDID是在Internet Explorer处理
                // ，所以此处的异常代码块将总被激活，即使该对话框及其操作成功。
                //当然，你可以通过浏览器选择设置来禁止这种错误的出现。
                //不过，即使出现这种提示，对你的主机也无任何损害。
            }
            finally
            {
                axWebBrowser1.Stop();
            }
        }


        public KellWebBrowser()
        {
            InitializeComponent();

            webBrowser1.StatusTextChanged += new EventHandler(webBrowser1_StatusTextChanged);

            iconWidth = kellComboBox1.ItemHeight;

            File.WriteAllText(LogFile, "", Encoding.UTF8);

            toolTip1.SetToolTip(button6, "右键可以设置当前页为主页");
            toolTip1.SetToolTip(button4, "右键可以查看和管理收藏夹");

            if (!File.Exists(AccessFile))
            {
                FileStream fs = File.Create(AccessFile);
                fs.Close();
            }
            if (!File.Exists(FavFile))
            {
                FileStream fs = File.Create(FavFile);
                fs.Close();
            }

            LoadHistoryList();

            LoadFavorites();

            SyncAccToFav();

            GoHome(); //MessageBox.Show("Default:" + Encoding.Default.WebName + "\nGB2312:" + Encoding.GetEncoding("GB2312").WebName + "\nUTF8:" + Encoding.UTF8.WebName);
        }

        void webBrowser1_StatusTextChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = webBrowser1.StatusText;
        }

        static string searchEngineConfigName = "SearchEngine";

        /// <summary>
        /// 若是配置文件中的搜索引擎配置名不是以"SearchEngine"开头，就要在调用KellWebBrowser控件的宿主程序中设置本属性
        /// </summary>
        public static string SearchEngineConfigName
        {
            get { return KellWebBrowser.searchEngineConfigName; }
            set { KellWebBrowser.searchEngineConfigName = value; }
        }

        static int iconWidth;

        public static int IconWidth
        {
            get { return KellWebBrowser.iconWidth; }
        }

        public static string LogFile
        {
            get
            {
                return Application.StartupPath + "\\Error.log";
            }
        }

        static List<UrlObject> favs;

        public static List<UrlObject> Favorites
        {
            get { return favs; }
        }

        public static string Home
        {
            get
            {
                return KellCommon.ReadIni("AppSettings", "DefaultWebsite", KellCommon.IniFile).Trim().ToLower();
            }
            set
            {
                KellCommon.WriteIni("AppSettings", "DefaultWebsite", value, KellCommon.IniFile);

                foreach (UrlObject u1 in accs)
                {
                    u1.IsHome = DnsConvert.ToPunycode(u1.Url) == value;
                }
                foreach (UrlObject u2 in favs)
                {
                    u2.IsHome = DnsConvert.ToPunycode(u2.Url) == value;
                }
                using (FileStream fs = new FileStream(AccessFile, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, accs);
                }
                using (FileStream fs = new FileStream(FavFile, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, favs);
                }
            }
        }

        public static string FavFile
        {
            get
            {
                return Application.StartupPath + "\\favList.dat";
            }
        }

        static List<UrlObject> accs;

        public static List<UrlObject> HistoryUrlList
        {
            get { return accs; }
        }

        public static string AccessFile
        {
            get
            {
                return Application.StartupPath + "\\accList.dat";
            }
        }

        static string title;

        public static string DocumentTitle
        {
            get
            {
                return title;
            }
        }

        static string url;

        public static string Url
        {
            get
            {
                return url ?? "";
            }
        }

        static int access;

        public static int Access
        {
            get
            {
                return access;
            }
        }

        static Icon icon;

        public static Icon DocumentIcon
        {
            get
            {
                return icon;
            }
        }

        static bool isHome;

        public static bool IsHome
        {
            get { return isHome; }
        }

        public static void SyncAccToFav()
        {
            foreach (UrlObject uo in favs)
            {
                uo.IsHome = DnsConvert.ToPunycode(uo.Url) == Home;
                UrlObject url;
                if (accs.ContainsUrl(uo.Url, out url))
                {
                    uo.Access = url.Access;
                    uo.IsHome = url.IsHome;
                }
            }
        }

        private void LoadFavorites()
        {
            contextMenuStrip1.Items.Clear();

            ToolStripMenuItem ts = null;
            if (DocumentIcon != null)
                ts = new ToolStripMenuItem("收藏当前页...", DocumentIcon.ToBitmap());
            else
                ts = new ToolStripMenuItem("收藏当前页...");
            //ts.Tag = Url;
            ts.Click += new EventHandler(ts_Click);
            ts.ToolTipText = Url == "" ? "[当前页为空]" : Url;
            contextMenuStrip1.Items.Add(ts);

            ToolStripMenuItem ts1 = new ToolStripMenuItem("管理收藏夹...", KellControls.Properties.Resources.SysFavorite.ToBitmap());
            //ts1.Tag = Url;
            ts1.Click += new EventHandler(ts1_Click);
            ts1.ToolTipText = "管理收藏夹以及本软件的其它设置";
            contextMenuStrip1.Items.Add(ts1);

            if (File.Exists(AccessFile))
            {
                using (FileStream fs = new FileStream(FavFile, FileMode.Open, FileAccess.Read))
                {
                    if (fs.Length > 0)
                    {
                        try
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            favs = (List<UrlObject>)bf.Deserialize(fs);
                        }
                        catch (Exception e)
                        {
                            favs = new List<UrlObject>();
                            System.Windows.Forms.MessageBox.Show(e.Message);
                        }
                    }
                    else
                    {
                        favs = new List<UrlObject>();
                    }
                }
            }
            else
            {
                favs = new List<UrlObject>();
            }
            if (favs != null)
            {
                favs.Sort(new UrlComparer());
                foreach (UrlObject url in favs)
                {
                    ToolStripMenuItem tsi = null;
                    if (url.Image != null)
                        tsi = new ToolStripMenuItem(url.Title + " [访问次数：" + url.Access + "]", url.Image.ToBitmap());
                    else
                        tsi = new ToolStripMenuItem(url.Title + " [访问次数：" + url.Access + "]");
                    tsi.Tag = url.Url;
                    tsi.Click += new EventHandler(tsi_Click);
                    tsi.ToolTipText = url.Url;
                    contextMenuStrip1.Items.Add(tsi);
                }
            }
        }

        private void LoadHistoryList()
        {
            kellComboBox1.Items.Clear();
            if (File.Exists(AccessFile))
            {
                using (FileStream fs = new FileStream(AccessFile, FileMode.Open, FileAccess.Read))
                {
                    if (fs.Length > 0)
                    {
                        try
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            accs = (List<UrlObject>)bf.Deserialize(fs);
                        }
                        catch (Exception e)
                        {
                            accs = new List<UrlObject>();
                            System.Windows.Forms.MessageBox.Show(e.Message);
                        }
                    }
                    else
                    {
                        accs = new List<UrlObject>();
                    }
                }
            }
            else
            {
                accs = new List<UrlObject>();
            }
            foreach (UrlObject u in accs)
            {
                u.IsHome = DnsConvert.ToPunycode(u.Url) == Home;
            }
            accs.Sort(new UrlComparer());
            foreach (UrlObject uo in accs)
            {
                if (uo.Image != null)
                    kellComboBox1.AddItem(uo.Url, uo.Title + Environment.NewLine + uo.Url, uo.Image.ToBitmap());
                else
                    kellComboBox1.AddItem(uo.Url, uo.Title + Environment.NewLine + uo.Url);
            }
        }

        void ts_Click(object sender, EventArgs e)
        {
            AddFavorite add = new AddFavorite();
            add.ShowDialog();
        }

        void ts1_Click(object sender, EventArgs e)
        {
            SysFavorite sys = new SysFavorite();
            sys.ShowDialog();
            if (HistoryUrlList.Count == 0)
            {
                kellComboBox1.Items.Clear();
            }
        }

        void tsi_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsi = (ToolStripMenuItem)sender;
            kellComboBox1.Text = tsi.Tag.ToString();
            Goto();
        }

        public event WebBrowserDocumentCompletedEventHandler DocumentCompleted;
        public event WebBrowserNavigatingEventHandler Navigating;
        public event WebBrowserNavigatedEventHandler Navigated;
        public event EventHandler Download;
        public event WebBrowserProgressChangedEventHandler ProgressChanged;

        private void OnDocumentCompleted(WebBrowserDocumentCompletedEventArgs e)
        {
            if (DocumentCompleted != null)
                DocumentCompleted(this, e);
        }

        private void OnNavigating(WebBrowserNavigatingEventArgs e)
        {
            if (Navigating != null)
                Navigating(this, e);
        }

        private void OnNavigated(WebBrowserNavigatedEventArgs e)
        {
            if (Navigated != null)
                Navigated(this, e);
        }

        private void OnDownload()
        {
            if (Download != null)
                Download(this, EventArgs.Empty);
        }

        private void OnProgressChanged(WebBrowserProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
                ProgressChanged(this, e);
        }

        private void GoHome()
        {
            string home = DnsConvert.ToPunycode(Home);
            if (!string.IsNullOrEmpty(home))
                webBrowser1.Navigate(home);
            else
                webBrowser1.Navigate("www.xn--2qus7hcfs21e.net");
        }

        private void Goto()
        {
            string address = kellComboBox1.Text.Trim();
            this.Navigate(address);
        }

        public void Navigate(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                GoHome();
                return;
            }
            if (address == "about:blank") return;
            if (!address.StartsWith("http://", true, null))
                address = "http://" + address;

            address = DnsConvert.ToPunycode(address);

            if (IsValidURL(address))
            {
                HttpWebResponse res = null;
                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(address);
                    res = (HttpWebResponse)req.GetResponse();
                    webBrowser1.Navigate(address);
                }
                catch (Exception ex)
                {
                    Log("Goto", ex);
                    string engine = KellCommon.ReadIni("AppSettings", "Search", KellCommon.IniFile);
                    if (!string.IsNullOrEmpty(engine))
                    {
                        string[] engEnc = engine.Split('|');
                        if (engEnc.Length > 2)
                            webBrowser1.Navigate(engEnc[1] + "=" + System.Web.HttpUtility.UrlEncode(kellComboBox1.Text.Trim(), Encoding.GetEncoding(engEnc[2].ToLower())));
                        else
                            webBrowser1.Navigate(engEnc[1] + "=" + kellComboBox1.Text.Trim());
                    }
                }
                finally
                {
                    if (res != null)
                        res.Close();
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("网址错误！");
            }
        }

        public void Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                GoHome();
                return;
            }

            if (keyword == "about:blank") return;

            string engine = KellCommon.ReadIni("AppSettings", "Search", KellCommon.IniFile);
            if (!string.IsNullOrEmpty(engine))
            {
                string[] engEnc = engine.Split('|');
                if (engEnc.Length > 2)
                    webBrowser1.Navigate(engEnc[1] + "=" + System.Web.HttpUtility.UrlEncode(keyword, Encoding.GetEncoding(engEnc[2].ToLower())));
                else
                    webBrowser1.Navigate(engEnc[1] + "=" + keyword);
            }
        }

        /// <summary>
        /// 是否有效URL地址
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidURL(string strIn)
        {
            // Return true if strIn is in valid e-mail format. 
            return Regex.IsMatch(strIn, @"[a-zA-z]+://[^\s]*");
        }

        bool finished;
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (!finished)
            {
                title = webBrowser1.DocumentTitle;
                icon = GetIcon();
                url = webBrowser1.Url.AbsoluteUri;
                isHome = DnsConvert.ToPunycode(url) == Home;
                toolStripMenuItem1.Enabled = !isHome;

                UrlObject uo;
                if (!accs.ContainsUrl(url, out uo))
                {
                    UrlObject u = new UrlObject(title, url, icon);
                    u.IsHome = isHome;
                    accs.Add(u);
                }

                SaveAccessRecordSyncFav();
                LoadHistoryList();
                access = accs.GetUrlAccessCount(Url);
                finished = true;
            }
            button8.Enabled = true;
            toolStripProgressBar1.Visible = false;
            OnDocumentCompleted(e);
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            button2.Enabled = webBrowser1.CanGoBack;
            button3.Enabled = webBrowser1.CanGoForward;
            button8.Enabled = false;
            finished = false;
            OnNavigating(e);
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            kellComboBox1.Text = webBrowser1.Url.AbsoluteUri;
            axWebBrowser1.Navigate(webBrowser1.Url.AbsoluteUri);
            OnNavigated(e);
        }

        private Icon GetIcon()
        {
            Icon icon = null;

            string icoUri = webBrowser1.Url.Scheme + "://" + DnsConvert.ToPunycode(webBrowser1.Url.Authority) + "/favicon.ico";

            if (IsValidURL(icoUri))
            {
                HttpWebResponse res = null;
                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(icoUri);
                    res = (HttpWebResponse)req.GetResponse();
                    Stream s = res.GetResponseStream();

                    if (s != null)
                    {
                        if (s.CanRead)
                        {
                            byte[] data = new byte[100];
                            List<byte> buffer = new List<byte>();
                            int len = 0;
                            while ((len = s.Read(data, 0, data.Length)) > 0)
                            {
                                buffer.AddRange(data.Take<byte>(len));
                            }
                            MemoryStream ms = new MemoryStream(buffer.ToArray());
                            icon = new Icon(ms, iconWidth, iconWidth);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log("Download Icon", ex);
                }
                finally
                {
                    if (res != null)
                        res.Close();
                }
            }
            return icon;
        }

        public void Log(string logType, Exception e)
        {
            File.AppendAllText(LogFile, "--- " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " ---\r\n" + logType + ": " + e.Message + "\r\n", Encoding.UTF8);
        }

        private void SaveAccessRecordSyncFav()
        {
            foreach (UrlObject u1 in accs)
            {
                u1.IsHome = DnsConvert.ToPunycode(u1.Url) == Home;
            }
            foreach (UrlObject u2 in favs)
            {
                u2.IsHome = DnsConvert.ToPunycode(u2.Url) == Home;
            }
            if (accs.UpdateUrl(url, title, icon))
            {
                using (FileStream fs = new FileStream(AccessFile, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, accs);
                }
            }
            UrlObject u;
            if (accs.ContainsUrl(url, out u))
            {
                UrlObject uo;
                if (favs.ContainsUrl(url, out uo))
                {
                    uo.Access = u.Access;
                    uo.Title = u.Title;
                    uo.Image = u.Image;
                    using (FileStream fs = new FileStream(FavFile, FileMode.Create, FileAccess.Write))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(fs, favs);
                    }
                }
            }
        }

        private void webBrowser1_FileDownload(object sender, EventArgs e)
        {
            OnDownload();
        }

        private void webBrowser1_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Maximum = (int)e.MaximumProgress;
            int val = (int)e.CurrentProgress;
            val = val < toolStripProgressBar1.Minimum ? toolStripProgressBar1.Minimum : val;
            val = val > toolStripProgressBar1.Maximum ? toolStripProgressBar1.Maximum : val;
            toolStripProgressBar1.Value = val;
            OnProgressChanged(e);
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            try
            {
                string allNewWindow = KellCommon.ReadIni("AppSettings", "NewWindow", KellCommon.IniFile);
                if (!string.IsNullOrEmpty(allNewWindow) && allNewWindow == "0")
                    e.Cancel = true;
            }
            catch
            { }
        }
        #region 选定网址
        int lastSelectLength;
        bool move;
        private void kellComboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!move)
            {
                if (lastSelectLength < kellComboBox1.Text.Length)
                {
                    kellComboBox1.SelectAll();
                    lastSelectLength = kellComboBox1.Text.Length;
                }
                else
                {
                    kellComboBox1.SelectionLength = 0;
                    lastSelectLength = 0;
                }
            }
        }

        private void kellComboBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (move)
            {
                lastSelectLength = kellComboBox1.SelectionLength;
                move = false;
            }
        }

        private void kellComboBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                move = true;
            }
        }
        #endregion
        private void kellComboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Goto();
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            LoadFavorites();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Goto();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh(WebBrowserRefreshOption.Completely);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            GoHome();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddFavorite add = new AddFavorite();
            add.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Home = DnsConvert.ToPunycode(Url);
            toolStripMenuItem1.Enabled = false;
        }

        private void kellComboBox1_DropDownClosed(object sender, EventArgs e)
        {
            if (kellComboBox1.SelectedIndex > -1)
            {
                kellComboBox1.Text = kellComboBox1.GetItemValue(kellComboBox1.SelectedIndex).ToString();
                Goto();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            webBrowser1.Stop();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Snap();
        }

        private void Snap()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG文件|*.png|JPG文件|*.jpg|GIF文件|*.gif|BMP文件|*.bmp|ICO文件|*.ico";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                ImageFormat IF = ImageFormat.Bmp;
                switch (sfd.FilterIndex)
                {
                    case 1:
                        IF = ImageFormat.Png;
                        break;
                    case 2:
                        IF = ImageFormat.Jpeg;
                        break;
                    case 3:
                        IF = ImageFormat.Gif;
                        break;
                    case 4:
                        IF = ImageFormat.Bmp;
                        break;
                    case 5:
                        IF = ImageFormat.Icon;
                        break;
                }
                SaveCurrentImage(sfd.FileName, IF);
                //using (Bitmap bmp = new Bitmap(webBrowser1.Width, webBrowser1.Height))
                //{
                //    webBrowser1.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                //    bmp.Save(sfd.FileName, IF);
                //}
            }
            sfd.Dispose();
        }

        private void SaveWholeImage(string path, ImageFormat format)
        {
            int browserWidth = webBrowser1.Width;
            int browserHeight = webBrowser1.Height;
            Rectangle rectBody = webBrowser1.Document.Body.ScrollRectangle;
            int width = rectBody.Width + 20;
            int height = rectBody.Height;
            webBrowser1.Width = width;
            webBrowser1.Height = height;
            try
            {
                SnapLibrary.Snapshot snap = new SnapLibrary.Snapshot();
                using (Bitmap bmp = snap.TakeSnapshot(webBrowser1.ActiveXInstance, new Rectangle(0, 0, width, height)))
                {
                    //this.webBrowser.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
                    bmp.Save(path, format);
                    //using (Image img = SnapLibrary.ImageHelper.GetThumbnailImage(bmp, bmp.Width, bmp.Height))
                    //{
                    //    SnapLibrary.ImageHelper.SaveImage(img, path, format);
                    //}
                }
            }
            catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message); }
            webBrowser1.Width = browserWidth;
            webBrowser1.Height = browserHeight;
        }

        private void SaveCurrentImage(string path, ImageFormat format)
        {
            Rectangle rectBody = webBrowser1.Document.Body.ScrollRectangle;
            int left = rectBody.Left;// webBrowser1.Document.Body.ScrollLeft;
            int top = rectBody.Top;// webBrowser1.Document.Body.ScrollTop;
            int width = webBrowser1.Width - 20;
            int height = webBrowser1.Height;
            try
            {
                SnapLibrary.Snapshot snap = new SnapLibrary.Snapshot();
                using (Bitmap bmp = snap.TakeSnapshot(webBrowser1.ActiveXInstance, new Rectangle(left, top, width, height)))
                {
                    //this.webBrowser.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
                    bmp.Save(path, format);
                    //using (Image img = SnapLibrary.ImageHelper.GetThumbnailImage(bmp, bmp.Width, bmp.Height))
                    //{
                    //    SnapLibrary.ImageHelper.SaveImage(img, path, format);
                    //}
                }
            }
            catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message); }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            InternetOptions();
        }

        DockStyle orginalDock;
        FormBorderStyle orginalFormBorder;
        FormWindowState orginalFormWindow;
        bool first = true;
        private void KellWebBrowser_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                FullScreen();
            }
        }

        public void FullScreen()
        {
            Form from = this.ParentForm;
            if (first)
            {
                orginalDock = this.Dock;
                orginalFormBorder = from.FormBorderStyle;
                orginalFormWindow = from.WindowState;
                first = false;
            }
            if (from.FormBorderStyle != FormBorderStyle.None)
            {
                this.Dock = DockStyle.Fill;
                from.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                from.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.Dock = orginalDock;
                from.FormBorderStyle = orginalFormBorder;
                from.WindowState = orginalFormWindow;
            }
        }
    }

    [Serializable]
    public class UrlObject : IComparable<UrlObject>
    {
        string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        string url;

        public string Url
        {
            get { if (url != null) return url.ToLower(); else return ""; }
            set { url = value; }
        }
        Icon image;

        public Icon Image
        {
            get { return image; }
            set { image = value; }
        }
        int access;

        public int Access
        {
            get { return access; }
            set { access = value; }
        }

        bool isHome;

        public bool IsHome
        {
            get { return isHome; }
            set { isHome = value; }
        }

        public UrlObject(string title, string url, Icon icon)
        {
            this.title = title;
            this.url = url;
            this.image = icon;
        }

        public override string ToString()
        {
            if (!isHome)
                return title;
            else
                return title + " [主页]";
        }

        /// <summary>
        /// 倒序(无效！请用.Sort(new UrlComparer())方法！)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(UrlObject other)
        {
            return other.Access.CompareTo(this.Access);
        }
    }

    public class UrlComparer : IComparer<UrlObject>
    {
        /// <summary>
        /// 倒序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        int IComparer<UrlObject>.Compare(UrlObject x, UrlObject y)
        {
            System.Collections.Comparer c = new System.Collections.Comparer(System.Globalization.CultureInfo.CurrentCulture);
            return c.Compare(y.Access, x.Access);
        }
    }

    public class DnsConvert
    {
        public static bool IsGBK(string input)
        {
            Regex r = new Regex(@"[u4e00-u9fa5]+");
            Match mc = r.Match(input);
            return mc.Length > 0;
        }
        public static bool IsAscii(string input)
        {
            foreach (char c in input)
            {
                if (c > 256)
                    return false;
            }
            return true;
        }
        public static string ToPunycode(string gbk)
        {
            if (!IsGBK(gbk))
                return gbk;
            try
            {
                IdnMapping dd = new IdnMapping();
                return dd.GetAscii(gbk);
            }
            catch// (Exception e)
            {
                //MessageBox.Show(e.Message);
                return gbk;
            }
        }
        public static string ToGBKcode(string puny)
        {
            if (!IsAscii(puny))
                return puny;
            try
            {
                IdnMapping dd = new IdnMapping();
                return dd.GetUnicode(puny);
            }
            catch// (Exception e)
            {
                //MessageBox.Show(e.Message);
                return puny;
            }
        }
    }

    public static class UrlObjectExtensions
    {
        public static bool ContainsUrl(this List<UrlObject> urls, string urlString, out UrlObject uo)
        {
            uo = null;
            foreach (UrlObject url in urls)
            {
                if (DnsConvert.ToPunycode(url.Url) == DnsConvert.ToPunycode(urlString.ToLower()))
                {
                    uo = url;
                    return true;
                }
            }
            return false;
        }

        public static bool UpdateUrl(this List<UrlObject> urls, string urlString, string title, Icon icon)
        {
            foreach (UrlObject url in urls)
            {
                if (DnsConvert.ToPunycode(url.Url) == DnsConvert.ToPunycode(urlString.ToLower()))
                {
                    url.Access++;
                    url.Title = title;
                    url.Image = icon;
                    return true;
                }
            }
            return false;
        }

        public static int GetUrlAccessCount(this List<UrlObject> urls, string urlString)
        {
            foreach (UrlObject url in urls)
            {
                if (DnsConvert.ToPunycode(url.Url) == DnsConvert.ToPunycode(urlString.ToLower()))
                {
                    return url.Access;
                }
            }
            return 0;
        }

        public static string GetUrlTitle(this List<UrlObject> urls, string urlString)
        {
            foreach (UrlObject url in urls)
            {
                if (DnsConvert.ToPunycode(url.Url) == DnsConvert.ToPunycode(urlString.ToLower()))
                {
                    return url.Title;
                }
            }
            return "";
        }

        public static Icon GetUrlIcon(this List<UrlObject> urls, string urlString)
        {
            foreach (UrlObject url in urls)
            {
                if (DnsConvert.ToPunycode(url.Url) == DnsConvert.ToPunycode(urlString.ToLower()))
                {
                    return url.Image;
                }
            }
            return null;
        }
    }
}
