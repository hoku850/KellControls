using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using KellCommons;

namespace KellControls
{
    [DefaultProperty("AdvText")]
    [DefaultEvent("LinkClicked")]
    public partial class KellAdvertisingBar : UserControl
    {
        public KellAdvertisingBar()
        {
            InitializeComponent();

            linkLabel1.Links[0].LinkData = KellLinkData.Default;

            tt = new ToolTip();
            tt.SetToolTip(linkLabel1, this.AdvDescription);
        }

        ToolTip tt;
        bool showUri;
        int index = -1;
        bool isRandom;
        string beginString = "<li>" + Environment.NewLine + "  <a href=\"";
        string endString = "\" target=\"_blank\"";
        string host;
        string listPageUrl;
        List<WebPageInfo> webs;
        int maxWebCount = 10;
        int reduceLastWords;
        bool distinct;

        public delegate void OnEndDirectoryHandler(object sender, int count);
        /// <summary>
        /// 当IsRandom为默认值（false）时才生效！
        /// </summary>
        public event OnEndDirectoryHandler EndThisDiretory;

        public delegate void OnCollectDirectoryFinishedHandler(object sender);
        public event OnCollectDirectoryFinishedHandler CollectDirectoryFinished;

        [Description("去掉重复Uri")]
        [DefaultValue(false)]
        public bool Distinct
        {
            get { return distinct; }
            set { distinct = value; }
        }

        [Description("去掉显示文本的最后几个字符")]
        [DefaultValue(0)]
        public int ReduceLastWords
        {
            get { return reduceLastWords; }
            set { reduceLastWords = value; }
        }

        [Description("默认的最大获取页面数")]
        [DefaultValue(10)]
        public int MaxWebCount
        {
            get { return maxWebCount; }
            set { maxWebCount = value; }
        }

        [Browsable(false)]
        public string ListPageUrl
        {
            get { return listPageUrl; }
            set
            {
                if (CollectionProcessor.VerifyUrl(value))
                {
                    listPageUrl = value;
                    host = CollectionProcessor.GetHost(listPageUrl);
                    Thread thr = new Thread(new ParameterizedThreadStart(GetHtmlList));
                    thr.Start(listPageUrl);
                }
            }
        }

        private void GetHtmlList(object arg)
        {
            try
            {
                string content = CollectionProcessor.GetWebContent(arg.ToString());
                webs = CollectionProcessor.GetWebListByListPageContent(content, host, beginString, endString, false, maxWebCount, distinct);
            }
            catch (Exception e)
            {
                throw new Exception("GetHtmlList:" + e.Message);
            }
            finally
            {
                OnCollectDirectoryFinished();
            }
        }

        private void OnCollectDirectoryFinished()
        {
            if (CollectDirectoryFinished != null)
            {
                CollectDirectoryFinished(this);
            }
        }

        public void StopCollecting()
        {
            CollectionProcessor.StopCollecting();
        }

        [Description("获取页面的格式前缀")]
        public string BeginString
        {
            get { return beginString; }
            set { beginString = value; }
        }

        [Description("获取页面的格式后缀")]
        public string EndString
        {
            get { return endString; }
            set { endString = value; }
        }

        [Description("切换页面是否随机")]
        [DefaultValue(false)]
        public bool IsRandom
        {
            get { return isRandom; }
            set { isRandom = value; }
        }

        [Browsable(false)]
        public List<WebPageInfo> Webs
        {
            get { return webs; }
        }

        public void ChangeLink()
        {
            if (webs != null && webs.Count > 0)
            {
                string url = "";
                if (isRandom)
                {
                    index = new Random().Next(webs.Count);
                    url = webs[index].Url;
                }
                else
                {
                    index++;
                    if (index > webs.Count - 1)
                        this.OnEndThisDiretory(webs.Count);
                    index = index > webs.Count - 1 ? 0 : index;
                    url = webs[index].Url;
                }
                this.AdvText = webs[index].Title;
                this.AdvDescription = webs[index].Description;
                this.AdvUri = url;
            }
        }

        private void OnEndThisDiretory(int count)
        {
            if (EndThisDiretory != null)
            {
                EndThisDiretory(this, count);
            }
        }

        [Description("弹出的ToolTip中是否显示Uri")]
        [DefaultValue(false)]
        public bool ShowUri
        {
            get { return showUri; }
            set 
            {
                showUri = value;
                if (showUri)
                    tt.SetToolTip(linkLabel1, this.AdvDescription + Environment.NewLine + this.AdvUri);
                else
                    tt.SetToolTip(linkLabel1, this.AdvDescription);
            }
        }

        public event LinkLabelLinkClickedEventHandler LinkClicked;

        private void OnLinkClicked(LinkLabelLinkClickedEventArgs e)
        {
            if (LinkClicked != null)
            {
                LinkClicked(this, e);
            }
        }

        [Description("广告栏中显示的文本")]
        [DefaultValue("未来科技")]
        public string AdvText
        {
            get
            {
                return linkLabel1.Text;
            }
            set
            {
                try
                {
                    if (reduceLastWords > 0 && value.Length > reduceLastWords)
                        linkLabel1.Text = value.Substring(0, value.Length - reduceLastWords);
                    else
                        linkLabel1.Text = value;
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("广告文本设置失败：" + e.Message);
                }
            }
        }

        [Description("广告栏中的链接地址")]
        [DefaultValue("http://www.xn--2qus7hcfs21e.net")]
        public string AdvUri
        {
            get
            {
                if (linkLabel1.Links[0].LinkData != null && linkLabel1.Links[0].LinkData is KellLinkData)
                {
                    return ((KellLinkData)linkLabel1.Links[0].LinkData).Uri;
                }
                else
                {
                    if (linkLabel1.Links[0].LinkData != null)
                    {
                        return linkLabel1.Links[0].LinkData.ToString();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultWebsite"]))
                            return ConfigurationManager.AppSettings["DefaultWebsite"];
                        else
                            return "http://www.xn--2qus7hcfs21e.net";
                    }
                }
            }
            set
            {
                try
                {
                    if (linkLabel1.Links[0].LinkData != null && linkLabel1.Links[0].LinkData is KellLinkData)
                        ((KellLinkData)linkLabel1.Links[0].LinkData).Uri = value;
                    else
                        linkLabel1.Links[0].LinkData = value;
                    linkLabel1.Links[0].Length = value.Length;
                    if (showUri)
                        tt.SetToolTip(linkLabel1, this.AdvDescription + Environment.NewLine + this.AdvUri);
                    else
                        tt.SetToolTip(linkLabel1, this.AdvDescription);
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("广告链接目标设置失败：" + e.Message);
                }
            }
        }

        [Description("弹出的ToolTip中显示的广告描述文字")]
        [DefaultValue("未来科技")]
        public string AdvDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(linkLabel1.Links[0].Description))
                    return linkLabel1.Links[0].Description;
                else
                    return this.AdvText;
            }
            set
            {
                linkLabel1.Links[0].Description = value;
            }
        }

        [Description("广告栏中显示文字的字体")]
        public Font AdvFont
        {
            get
            {
                return linkLabel1.Font;
            }
            set
            {
                try
                {
                    linkLabel1.Font = value;
                    this.Font = value;
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("字体设置失败：" + e.Message);
                }
            }
        }

        [Browsable(false)]
        public KellLinkData AdvLinkData
        {
            get
            {
                if (linkLabel1.Links[0].LinkData != null && linkLabel1.Links[0].LinkData is KellLinkData)
                    return (KellLinkData)linkLabel1.Links[0].LinkData;
                else
                    return KellLinkData.Default;
            }
            set
            {
                try
                {
                    linkLabel1.Links[0].LinkData = value;
                    linkLabel1.Links[0].Length = value.Uri.Length;
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("广告链接对象设置失败：" + e.Message);
                }
            }
        }
        /// <summary>
        /// 设置广告的文本及链接地址
        /// </summary>
        /// <param name="text">显示的文本</param>
        /// <param name="uri">链接的目标地址（可以是网址，也可以是目录/文件）</param>
        public void SetLinkAttribute(string text, string uri)
        {
            this.AdvText = text;
            this.AdvUri = uri;
        }
        /// <summary>
        /// 设置广告的文本、链接地址以及地址参数
        /// </summary>
        /// <param name="text">显示的文本</param>
        /// <param name="uri">链接的目标地址（可以是网址，也可以是目录/文件）</param>
        /// <param name="argStr">运行目标地址要调用的参数字符串</param>
        public void SetLinkAttribute(string text, string uri, string argStr)
        {
            this.AdvText = text;
            KellLinkData linkData = new KellLinkData(uri, argStr);
            this.AdvLinkData = linkData;
        }
        /// <summary>
        /// 设置广告的文本和链接对象
        /// </summary>
        /// <param name="text">显示的文本</param>
        /// <param name="linkData">链接对象</param>
        public void SetLinkAttribute(string text, KellLinkData linkData)
        {
            this.AdvText = text;
            this.AdvLinkData = linkData;
        }

        private void AdvertisingBar_FontChanged(object sender, EventArgs e)
        {
            linkLabel1.Font = this.Font;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (e.Link != null && e.Link.LinkData != null)
                {
                    try
                    {
                        if (e.Link.LinkData is KellLinkData)
                        {
                            KellLinkData linkData = (KellLinkData)e.Link.LinkData;
                            if (linkData.Args != null && linkData.Args.Length > 0)
                                Process.Start(linkData.Uri, linkData.Args[0].ToString());
                            else
                                Process.Start(linkData.Uri);
                        }
                        else
                        {
                            Process.Start(e.Link.LinkData.ToString());
                        }
                    }
                    catch// (Exception ex)
                    { 
                        //MessageBox.Show(ex.Message); 
                    }
                }
            }
            OnLinkClicked(e);
        }
    }

    public class KellLinkData
    {
        string uri;

        public string Uri
        {
            get { return uri; }
            set { uri = value; }
        }
        object[] args;

        public object[] Args
        {
            get { return args; }
            set { args = value; }
        }

        public KellLinkData(string uri, params object[] args)
        {
            this.uri = uri;
            this.args = args;
        }

        public static KellLinkData Default
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultWebsite"]))
                    return new KellLinkData(ConfigurationManager.AppSettings["DefaultWebsite"]);
                else
                    return new KellLinkData("http://www.xn--2qus7hcfs21e.net");
            }
        }

        public override string ToString()
        {
            return Uri;
        }
    }
}
