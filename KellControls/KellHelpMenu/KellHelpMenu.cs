using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using KellCommons;

namespace KellControls
{
    public class KellHelpMenu : ToolStripMenuItem
    {
        string productName = "";

        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }
        private string productDescription;

        public string ProductDescription
        {
            get { return productDescription; }
            set { productDescription = value; }
        }

        ToolStripMenuItem about;

        public ToolStripMenuItem About
        {
            get { return about; }
            set { about = value; }
        }
        ToolStripMenuItem regReq;

        public ToolStripMenuItem RegReq
        {
            get { return regReq; }
            set { regReq = value; }
        }
        ToolStripMenuItem regRes;

        public ToolStripMenuItem RegRes
        {
            get { return regRes; }
            set { regRes = value; }
        }

        public KellHelpMenu(string productName, string productDescription, params ToolStripMenuItem[] others)
        {
            this.Name = "帮助ToolStripMenuItem";
            this.Size = new System.Drawing.Size(44, 21);
            this.Text = "帮助";

            this.productName = productName;
            this.productDescription = productDescription;

            this.about = new System.Windows.Forms.ToolStripMenuItem();
            this.regReq = new System.Windows.Forms.ToolStripMenuItem();
            this.regRes = new System.Windows.Forms.ToolStripMenuItem();

            // 
            // 帮助ToolStripMenuItem
            // 
            this.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.about,
            this.regReq,
            this.regRes});

            if (others != null)
                this.DropDownItems.AddRange(others);
            // 
            // 关于本软件ToolStripMenuItem
            // 
            this.about.Name = "关于本软件ToolStripMenuItem";
            this.about.Size = new System.Drawing.Size(152, 22);
            this.about.Text = "关于本软件";
            this.about.Click += new System.EventHandler(this.关于本软件ToolStripMenuItem_Click);
            // 
            // 在线注册ToolStripMenuItem
            // 
            this.regReq.Name = "在线注册ToolStripMenuItem";
            this.regReq.Size = new System.Drawing.Size(152, 22);
            this.regReq.Text = "在线申请注册";
            this.regReq.Click += new System.EventHandler(this.在线注册ToolStripMenuItem_Click);
            // 
            // 输入注册码ToolStripMenuItem
            // 
            this.regRes.Name = "输入注册码ToolStripMenuItem";
            this.regRes.Size = new System.Drawing.Size(152, 22);
            this.regRes.Text = "输入注册码";
            this.regRes.Click += new System.EventHandler(this.输入注册码ToolStripMenuItem_Click);
        }

        private void 关于本软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutSoftware();
        }

        private void 在线注册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Register.RegRequest reg = new Register.RegRequest();
            reg.SoftWareName = productName;
            reg.ShowDialog();
        }

        private void 输入注册码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegResponse.Reg re = new RegResponse.Reg();
            re.ShowDialog();
            //string regCode = re.RegCode;
        }

        public static void MainLoadAlert()
        {
            ComputerInfo.Computer com = ComputerInfo.Computer.Instance();

            string HardDiskSN = com.DiskPhysicalSN;

            string pwd = KellCommon.GetRegCodeByHardDiskSN(HardDiskSN);

            if (KellCommon.ReadIni("AppSettings", "RegCode", KellCommon.IniFile) != pwd)
            {
                System.Windows.Forms.MessageBox.Show("该软件尚未注册！\n\n请在“帮助”菜单下点击“在线申请注册”，等待软件商回复您的邮件\n您根据邮件回复的注册申请码，在“帮助”菜单下点击“输入注册码”进行注册\n非常感谢您使用本软件！\n\n如有不清楚的请直接联系软件商 QQ：421979530", "注册提示");
            }
        }

        void AboutSoftware()
        {
            string s = "                                  ";
            s += "欢迎使用" + productName + "！" + Environment.NewLine + Environment.NewLine + Environment.NewLine;
            s += productDescription + Environment.NewLine + Environment.NewLine + Environment.NewLine;
            s += "---------------------------------------------------------------------------------" + Environment.NewLine;
            s += "                                   未来科技 版权所有" + Environment.NewLine;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["version"]))
                System.Windows.Forms.MessageBox.Show(s, productName + " V" + ConfigurationManager.AppSettings["version"]);
            else
                System.Windows.Forms.MessageBox.Show(s, productName + " V1.0");
        }
    }
}
