using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections;
using System.Configuration;

namespace KellControls
{
    [DefaultEvent("DocumentCompleted")]
    public class WebFormRestorer : UserControl
    {
        private MenuStrip menuStrip1;
        private ToolStripMenuItem 保存表单ToolStripMenuItem;
        private ToolStripMenuItem 载入表单ToolStripMenuItem;
        private ToolStripMenuItem 高级设置ToolStripMenuItem;
        private OpenFileDialog openFileDialog1;
        private Timer timer1;
        private System.ComponentModel.IContainer components;
        private TextBox textBox1;
        private Button button1;
        private Button button2;
        private static WebBrowser webBrowser1;
        private Button button3;
        private static bool ready;

        public static bool Ready
        {
            get { return ready; }
        }

        [Browsable(false)]
        public WebBrowser Browser
        {
            get { return webBrowser1; }
        }

        public void Navigate(string url)
        {
            webBrowser1.Navigate(url);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.保存表单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.载入表单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.高级设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.保存表单ToolStripMenuItem,
            this.载入表单ToolStripMenuItem,
            this.高级设置ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(675, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 保存表单ToolStripMenuItem
            // 
            this.保存表单ToolStripMenuItem.Enabled = false;
            this.保存表单ToolStripMenuItem.Name = "保存表单ToolStripMenuItem";
            this.保存表单ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.保存表单ToolStripMenuItem.Text = "保存表单";
            this.保存表单ToolStripMenuItem.Click += new System.EventHandler(this.保存表单ToolStripMenuItem_Click);
            // 
            // 载入表单ToolStripMenuItem
            // 
            this.载入表单ToolStripMenuItem.Name = "载入表单ToolStripMenuItem";
            this.载入表单ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.载入表单ToolStripMenuItem.Text = "载入表单";
            this.载入表单ToolStripMenuItem.Click += new System.EventHandler(this.载入表单ToolStripMenuItem_Click);
            // 
            // 高级设置ToolStripMenuItem
            // 
            this.高级设置ToolStripMenuItem.Name = "高级设置ToolStripMenuItem";
            this.高级设置ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.高级设置ToolStripMenuItem.Text = "高级设置";
            this.高级设置ToolStripMenuItem.Click += new System.EventHandler(this.高级设置ToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(215, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(304, 21);
            this.textBox1.TabIndex = 2;
            this.textBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseClick);
            this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            this.textBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseMove);
            this.textBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseUp);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(522, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "转到>>";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(574, 1);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "<后退";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(626, 1);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(50, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "前进>";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // webBrowser1
            // 
            webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            webBrowser1.Location = new System.Drawing.Point(0, 25);
            webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            webBrowser1.Name = "webBrowser1";
            webBrowser1.Size = new System.Drawing.Size(675, 387);
            webBrowser1.TabIndex = 6;
            webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            webBrowser1.FileDownload += new System.EventHandler(this.webBrowser1_FileDownload);
            webBrowser1.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser1_Navigated);
            webBrowser1.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser1_Navigating);
            webBrowser1.NewWindow += new System.ComponentModel.CancelEventHandler(this.webBrowser1_NewWindow);
            webBrowser1.ProgressChanged += new System.Windows.Forms.WebBrowserProgressChangedEventHandler(this.webBrowser1_ProgressChanged);
            // 
            // WebFormRestorer
            // 
            this.Controls.Add(webBrowser1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "WebFormRestorer";
            this.Size = new System.Drawing.Size(675, 412);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public static Template temp;
        public static bool auto;
        bool set;
        public static int interval = 10;
        public static string SavePath = Application.StartupPath + "\\Saved\\";
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

        public WebFormRestorer()
        {
            InitializeComponent();

            temp = new Template();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (!set)//导航的
            {
                PageUtility p = new PageUtility();
                HtmlElementCollection hs = webBrowser1.Document.GetElementsByTagName("input");
                HtmlElementCollection hs1 = webBrowser1.Document.GetElementsByTagName("textarea");
                HtmlElementCollection hs2 = webBrowser1.Document.GetElementsByTagName("select");
                List<HtmlElement> list = hs.MergeCollection(hs1);
                list.MergeCollection(hs2);

                List<ItemUtility> items = new List<ItemUtility>();
                foreach (HtmlElement he in list)
                {
                    ItemUtility item = new ItemUtility();
                    string name = he.Name;
                    if (string.IsNullOrEmpty(name))
                        name = he.Id;
                    name = name ?? "";
                    item.NameValue = new KeyValuePair<string, string>(name, GetValue(he));
                    items.Add(item);
                }
                p.Form = new FormUtility(items);
                p.URL = webBrowser1.Document.Url.AbsoluteUri;
                temp.Page = p;
                temp.Name = webBrowser1.Document.Title;
            }
            else//载入的
            {
                SetFormItems();
            }
            set = false;
            ready = true;
            OnDocumentCompleted(e);
            if (auto)
                timer1.Start();
            保存表单ToolStripMenuItem.Enabled = true;
        }

        private void SetFormItems()
        {
            HtmlElementCollection hs = webBrowser1.Document.GetElementsByTagName("input");
            HtmlElementCollection hs1 = webBrowser1.Document.GetElementsByTagName("textarea");
            HtmlElementCollection hs2 = webBrowser1.Document.GetElementsByTagName("select");
            List<HtmlElement> list = hs.MergeCollection(hs1);
            list.MergeCollection(hs2);

            FormUtility form = temp.Page.Form;
            for (int i = 0; i < list.Count; i++)
            {
                HtmlElement he = list[i];
                string name = he.Name;
                if (string.IsNullOrEmpty(name))
                    name = he.Id;
                name = name ?? "";
                if (name.ToLower() == form.Items[i].NameValue.Key.ToLower())
                {
                    SetValue(he, form.Items[i].NameValue.Value);
                }
                else
                {
                    for (int j = 0; j < form.Items.Count; j++)
                    {
                        if (i == j)
                            continue;
                        if (name.ToLower() == form.Items[j].NameValue.Key.ToLower())
                        {
                            SetValue(he, form.Items[j].NameValue.Value);
                            break;
                        }
                    }
                }
            }
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            ready = false;
            timer1.Stop();
            timer1.Interval = 1000 * interval;
            保存表单ToolStripMenuItem.Enabled = false;
            button2.Enabled = webBrowser1.CanGoBack;
            button3.Enabled = webBrowser1.CanGoForward;
            OnNavigating(e);
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            textBox1.Text = webBrowser1.Url.AbsoluteUri;
            OnNavigated(e);
        }

        private void webBrowser1_FileDownload(object sender, EventArgs e)
        {
            OnDownload();
        }

        private void webBrowser1_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            OnProgressChanged(e);
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            try
            {
                string allNewWindow = ConfigurationManager.AppSettings["NewWindow"];
                if (!string.IsNullOrEmpty(allNewWindow) && allNewWindow == "0")
                    e.Cancel = true;
            }
            catch
            { }
        }

        private void 保存表单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
            string filename = SavePath + temp.Name + DateTime.Now.ToString("yyMMddHHmmss") + ".tmpl";
            SaveFormItems(filename);
            if (!auto)
                MessageBox.Show("[" + temp.Name + "] 表单模板保存完毕！\n您还可以选择自动保存，请在高级设置菜单中设定为定时自动保存。", "温馨提示");
            else
                MessageBox.Show("[" + temp.Name + "] 表单模板保存完毕！", "温馨提示");
        }

        public static void GetFormItems()
        {
            if (temp != null && temp.Page.Form != null)
            {
                HtmlElementCollection hs = webBrowser1.Document.GetElementsByTagName("input");
                HtmlElementCollection hs1 = webBrowser1.Document.GetElementsByTagName("textarea");
                HtmlElementCollection hs2 = webBrowser1.Document.GetElementsByTagName("select");
                List<HtmlElement> list = hs.MergeCollection(hs1);
                list.MergeCollection(hs2);

                FormUtility f = temp.Page.Form;
                for (int i = 0; i < f.Items.Count; i++)
                {
                    string nam = f.Items[i].NameValue.Key;
                    nam = nam ?? "";
                    string name = list[i].Name;
                    if (string.IsNullOrEmpty(name))
                        name = list[i].Id;
                    name = name ?? "";
                    if (nam.ToLower() == name.ToLower())
                    {
                        string val = GetValue(list[i]);
                        f.SetItem(i, val);
                    }
                    else
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            if (i == j)
                                continue;
                            name = list[j].Name;
                            if (string.IsNullOrEmpty(name))
                                name = list[j].Id;
                            name = name ?? "";
                            if (name.ToLower() == nam.ToLower())
                            {
                                string val = GetValue(list[j]);
                                f.SetItem(i, val);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void 载入表单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
            openFileDialog1.Filter = "表单模板|*.tmpl";
            openFileDialog1.InitialDirectory = SavePath;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SetTemplate(openFileDialog1.FileName);
            }
            openFileDialog1.Dispose();
        }

        public void SetTemplate(string filename)
        {
            temp.LoadTemplate(filename);
            set = true;
            webBrowser1.Navigate(temp.Page.URL);
        }

        private void 高级设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings st = new Settings();
            st.ShowDialog();
            if (auto && webBrowser1.ReadyState == WebBrowserReadyState.Complete)
                timer1.Start();
            else
                timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string filename = SavePath + webBrowser1.Document.Title + DateTime.Now.ToString("yyMMddHHmmss") + "_Auto.tmpl";
            SaveFormItems(filename);
        }

        public static void SaveFormItems(string filename)
        {
            GetFormItems();
            temp.SaveTemplate(filename);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Goto();
        }

        private void Goto()
        {
            string address = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(address)) return;
            if (address == "about:blank") return;
            if (!address.StartsWith("http://"))
                address = "http://" + address;
            if (IsValidURL(address))
                webBrowser1.Navigate(address);
            else
                MessageBox.Show("网址错误！");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Goto();
            }
        }

        /// <summary>
        /// 是否有效URL地址
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        private bool IsValidURL(string strIn)
        {
            // Return true if strIn is in valid e-mail format. 
            return Regex.IsMatch(strIn, @"[a-zA-z]+://[^\s]*");
        }

        int lastSelectLength;
        bool move;
        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!move)
            {
                if (lastSelectLength < textBox1.TextLength)
                {
                    textBox1.SelectAll();
                    lastSelectLength = textBox1.TextLength;
                }
                else
                {
                    textBox1.SelectionLength = 0;
                    lastSelectLength = 0;
                }
            }
        }

        private void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (move)
            {
                lastSelectLength = textBox1.SelectionLength;
                move = false;
            }
        }

        private void textBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                move = true;
            }
        }

        public static string GetValue(HtmlElement element)
        {
            if (IfTextAreaTypeItem(element))
            {
                return element.InnerText;
            }
            else if (IfSelectTypeItem(element))
            {
                string select = GetTheSelectOptionValue(element);
                if (select == "")
                    select = GetTheSelectOptionText(element);
                return select;
            }
            else if (IfCheckTypeItem(element))
            {
                return GetTheCheckRadioValue(element, webBrowser1);
            }
            return element.GetAttribute("value");
        }

        public static void SetValue(HtmlElement element, string value)
        {
            if (IfTextAreaTypeItem(element))
            {
                element.InnerText = value;
            }
            else if (IfSelectTypeItem(element))
            {
                CheckTheSelectWhenOptionValueIs(element, value);
            }
            else if (IfCheckTypeItem(element))
            {
                CheckTheCheckRadioWhenValueIs(element.Name, value, webBrowser1);
            }
            element.SetAttribute("value", value);
        }

        /// <summary>
        /// 当Option的显示文本为指定值时，选定该Option
        /// </summary>
        public static void CheckTheSelectWhenOptionTextIs(HtmlElement select, string option)
        {
            if (select.TagName.ToUpper() != "SELECT")
            {
                MessageBox.Show("指定的条目不是SELECT！");
            }
            if (select.GetAttribute("type").Substring(7).ToLower() == "one")
            {
                foreach (HtmlElement item in select.Children)
                {
                    if (item.InnerText == option)
                    {
                        item.SetAttribute("selected", "selected");
                        break;
                    }
                }
            }
            else //"multiple"
            {
                string[] options = option.Split(",".ToCharArray());
                List<string> unFind = new List<string>(options);
                foreach (HtmlElement item in select.Children)
                {
                    if (unFind.Contains(item.InnerText))
                    {
                        item.SetAttribute("selected", "selected");
                    }
                    else
                    {
                        item.SetAttribute("selected", "");
                    }
                }
            }
        }

        /// <summary>
        /// 当Option的值为指定值时，选定该Option
        /// </summary>
        public static void CheckTheSelectWhenOptionValueIs(HtmlElement select, string option)
        {
            if (select.TagName.ToUpper() != "SELECT")
            {
                MessageBox.Show("指定的条目不是SELECT！");
            }
            if (select.GetAttribute("type").Substring(7).ToLower() == "one")
            {
                foreach (HtmlElement item in select.Children)
                {
                    if (item.GetAttribute("value") == option)
                    {
                        item.SetAttribute("selected", "selected");
                        break;
                    }
                }
            }
            else //"multiple"
            {
                string[] options = option.Split(",".ToCharArray());
                List<string> unFind = new List<string>(options);
                foreach (HtmlElement item in select.Children)
                {
                    if (unFind.Contains(item.GetAttribute("value")))
                    {
                        item.SetAttribute("selected", "selected");
                    }
                    else
                    {
                        item.SetAttribute("selected", "");
                    }
                }
            }
        }

        /// <summary>
        /// 当Radio的值为指定值时，选定该Radio或CheckBox
        /// </summary>
        public static void CheckTheCheckRadioWhenValueIs(string elementName, string value, WebBrowser webBrowser1)
        {
            List<string> vals = new List<string>(value.Split(",".ToCharArray()));
            foreach (HtmlElement item in webBrowser1.Document.All.GetElementsByName(elementName))
            {
                if (IfCheckTypeItem(item))
                {
                    if (vals.Contains(item.GetAttribute("value")))
                    {
                        item.SetAttribute("checked", "checked");
                    }
                    else
                    {
                        item.SetAttribute("checked", "");
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定表单元素的Option的已选值
        /// </summary>
        public static string GetTheSelectOptionValue(HtmlElement item)
        {
            if (item.GetAttribute("type").Substring(7).ToLower() == "one")
            {
                foreach (HtmlElement sub in item.Children)
                {
                    string select = sub.GetAttribute("selected");
                    if (select != null)
                    {
                        return sub.GetAttribute("value");
                    }
                }
            }
            else //"multiple"
            {
                List<string> options = new List<string>();
                foreach (HtmlElement sub in item.Children)
                {
                    string select = sub.GetAttribute("selected");
                    if (select != null)
                    {
                        options.Add(sub.GetAttribute("value"));
                    }
                }
                return string.Join(",", options.ToArray());
            }
            return "";
        }

        /// <summary>
        /// 获取指定表单元素的Option的已选显示文本
        /// </summary>
        public static string GetTheSelectOptionText(HtmlElement item)
        {
            if (item.GetAttribute("type").Substring(7).ToLower() == "one")
            {
                foreach (HtmlElement sub in item.Children)
                {
                    string select = sub.GetAttribute("selected");
                    if (select != null)
                    {
                        return sub.InnerText;
                    }
                }
            }
            else //"multiple"
            {
                List<string> options = new List<string>();
                foreach (HtmlElement sub in item.Children)
                {
                    string select = sub.GetAttribute("selected");
                    if (select != null)
                    {
                        options.Add(sub.InnerText);
                    }
                }
                return string.Join(",", options.ToArray());
            }
            return "";
        }

        /// <summary>
        /// 获取指定名称的表单元素的Radio或CheckBox的可选值
        /// </summary>
        public static List<string> GetTheAllRadioOrCheckboxValue(HtmlElement item, WebBrowser webBrowser1)
        {
            List<string> values = new List<string>();
            foreach (HtmlElement sub in webBrowser1.Document.All.GetElementsByName(item.Name))
            {
                if (IfCheckTypeItem(sub))
                {
                    values.Add(sub.GetAttribute("value"));
                }
            }
            return values;
        }

        /// <summary>
        /// 获取指定表单元素的Radio或CheckBox的已选值
        /// </summary>
        public static string GetTheCheckRadioValue(HtmlElement item, WebBrowser webBrowser1)
        {
            List<string> vals = new List<string>();
            foreach (HtmlElement sub in webBrowser1.Document.All.GetElementsByName(item.Name))
            {
                if (IfCheckTypeItem(sub))
                {
                    string check = sub.GetAttribute("checked");
                    if (check != null && check.Trim().ToLower() != "false")
                    {
                        vals.Add(sub.GetAttribute("value"));
                    }
                }
            }
            return string.Join(",", vals.ToArray());
        }

        public static bool IfTextAreaTypeItem(HtmlElement item)
        {
            return (item.TagName.Length > 7 && item.TagName.Substring(0, 8).ToUpper() == "TEXTAREA");
        }

        /// <summary>
        /// 判断指定的表单元素是否为Check类型(如：Radio或CheckBox)
        /// </summary>
        public static bool IfCheckTypeItem(HtmlElement item)
        {
            return (item.TagName.ToUpper() == "INPUT" && (item.GetAttribute("type").ToLower() == "radio" || item.GetAttribute("type").ToLower() == "checkbox"));
        }

        /// <summary>
        /// 判断指定的表单元素是否为Select类型
        /// </summary>
        public static bool IfSelectTypeItem(HtmlElement item)
        {
            return (item.TagName.Length > 5 && item.TagName.Substring(0, 6).ToUpper() == "SELECT");
        }
    }

    public static class HtmlElementCollectionExtensions
    {
        public static List<HtmlElement> MergeCollection(this HtmlElementCollection hec, HtmlElementCollection other)
        {
            List<HtmlElement> list = new List<HtmlElement>();
            IEnumerator<HtmlElement> ie = hec.OfType<HtmlElement>().GetEnumerator();
            while (ie.MoveNext())
            {
                list.Add(ie.Current);
            }
            IEnumerator<HtmlElement> ie1 = other.OfType<HtmlElement>().GetEnumerator();
            while (ie1.MoveNext())
            {
                list.Add(ie1.Current);
            }
            return list;
        }

        public static void MergeCollection(this List<HtmlElement> list, HtmlElementCollection other)
        {
            IEnumerator<HtmlElement> ie = other.OfType<HtmlElement>().GetEnumerator();
            while (ie.MoveNext())
            {
                list.Add(ie.Current);
            }
        }
    }
}
