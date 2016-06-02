using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KellControls;
using KellCommons.DEncrypt;
using System.Configuration;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //DateTime d = DateTime.Parse("2016-1-30");
            //DateTime l = d.AddMonths(1);
            //MessageBox.Show(l.ToString());
        }
        
        KellAdvertisingForm tf;

        private void button1_Click(object sender, EventArgs e)
        {
            Module module = new Module();
            module.ID = 1;
            module.Name = "模块1";
            module.Description = DateTime.Now.ToString();
            Dictionary<IPermissionType, bool> pers = new Dictionary<IPermissionType, bool>();
            pers.Add(new PermissionType() { ID = 1, Name = "查看" }, true);
            pers.Add(new PermissionType() { ID = 2, Name = "添加" }, true);
            pers.Add(new PermissionType() { ID = 3, Name = "搜索" }, false);
            pers.Add(new PermissionType() { ID = 4, Name = "统计" }, false);
            pers.Add(new PermissionType() { ID = 5, Name = "修改" }, false);
            pers.Add(new PermissionType() { ID = 6, Name = "删除" }, false);
            Action action = new Action(1, module, pers);
            Permission per = new Permission(1, "测试权限1", action);
            kellPermissionEditor1.Permission = per;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(kellPermissionEditor1.Permission.Action.ToString(), kellPermissionEditor1.Permission.Name);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!kellPermissionEditor1.Changed)
            {
                if (MessageBox.Show("当前权限毫无更改，确认保存么？", "保存提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
            }
            bool flag = kellPermissionEditor1.SavePermission();
            if (flag)
            {
                MessageBox.Show("保存成功！");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            kellPermissionEditor1.LoadPermission();
        }

        private void kellPermissionEditor1_PermissionChanged(object sender, KellPermissionEditor.PermissionArgs e)
        {
            //MessageBox.Show(e.PermissionItem.Key.Name + "[" + e.PermissionItem.Value.ToString() + "]");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            kellAdvertisingBar1.ListPageUrl = ConfigurationManager.AppSettings["ListPageUrl"];
            kellAdvertisingBar1.MaxWebCount = int.Parse(ConfigurationManager.AppSettings["MaxWebCount"]);

            tf = new KellAdvertisingForm();

            numericUpDown2.Value = kellPermissionEditor1.NameWidth;
            numericUpDown1.Value = kellPermissionEditor1.ItemWidth;

            TypeClass type = new TypeClass(1, "类型0", DateTime.Now.ToString(), null);
            List<TypeClass> types = new List<TypeClass>();
            TypeClass type1 = new TypeClass(2, "类型1", DateTime.Now.ToString(), type);
            TypeClass type2 = new TypeClass(3, "类型2", DateTime.Now.ToString(), type);
            TypeClass type3 = new TypeClass(4, "类型3", DateTime.Now.ToString(), type);
            TypeClass type4 = new TypeClass(5, "类型4", DateTime.Now.ToString(), type2);
            TypeClass type5 = new TypeClass(6, "类型5", DateTime.Now.ToString(), type2);
            types.Add(type1);
            types.Add(type2);
            types.Add(type3);
            types.Add(type4);
            types.Add(type5);
            TypeNode node = new TypeNode(type, types);
            kellLinkComboBox1.Node = node;
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day - 1;
            List<KellCalendarEx.LinkObject> links = new List<KellCalendarEx.LinkObject>();
            KellCalendarEx.LinkObject link = new KellCalendarEx.LinkObject("故障信息", Color.Red, true, null, new Action<string>(ShowErr), this, new object[]{ "过车时出现故障，故障代码[1002]" });
            KellCalendarEx.LinkObject link2 = new KellCalendarEx.LinkObject("提醒信息", Color.Blue, true, null, new Action<string>(ShowErr), this, new object[] { "过车时缓慢，速度为10km/h" });
            links.Add(link);
            links.Add(link2);
            kellCalendarEx1.AddLinkMsg(year, month, day, links);
            kellCalendarEx1.MonthChanged += KellCalendarEx1_MonthChanged;
        }

        private void KellCalendarEx1_MonthChanged(object sender, KellCalendarEx.YearMonth e)
        {
            MessageBox.Show(e.ToString());
        }

        private void ShowErr(string msg)
        {
            MessageBox.Show(msg);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            kellPermissionEditor1.ItemWidth = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            kellPermissionEditor1.NameWidth = (int)numericUpDown2.Value;
        }

        private void kellLinkComboBox1_UpLeveled(object sender, ComboBox e)
        {
            MessageBox.Show(e.Name);
        }

        //private void webFormRestorer1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        //{
        //    this.Text = webFormRestorer1.Browser.Document.Title;
        //}

        //private void kellWebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        //{
        //    this.Text = KellWebBrowser.DocumentTitle;
        //    this.Icon = KellWebBrowser.DocumentIcon;
        //}

        private void button5_Click(object sender, EventArgs e)
        {
            DesUtility des = new DesUtility();
            textBox2.Text = des.Encrypt(textBox1.Text.Trim());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DesUtility des = new DesUtility();
            textBox1.Text = des.Decrypt(textBox2.Text.Trim());
        }

        private void kellAdvertisingBar1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //if (e.Link.LinkData != null)
            //    MessageBox.Show("LinkClicked:[" + e.Link.LinkData.ToString() + "]");
            //else
            //    MessageBox.Show("LinkClicked:链接对象为空！");
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            kellAdvertisingBar1.ChangeLink();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.F11)
            //{
            //    kellWebBrowser1.FullScreen();
            //}
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (tf.CurrentState == 0)
            {
                tf.Hide();
                tf.ShowLogo = true;
                tf.SetBackgroundBitmap(new Bitmap("popup2.bmp"), Color.FromArgb(255, 0, 255));
                Rectangle TitleRectangle = new Rectangle(5, 1, 70, 25);
                Rectangle TitlebarRectangle = new Rectangle(1, 1, tf.Width, 25);
                Rectangle ContentRectangle = new Rectangle(75, 40, 155, 60);
                Rectangle ClosebtnRectangle = new Rectangle(tf.Width - 25, 1, 25, 25);
                tf.Opacity = 1;
                tf.ShowForm("提示", "未来科技", ConfigurationManager.AppSettings["website"], TitleRectangle, TitlebarRectangle, ContentRectangle, ClosebtnRectangle);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (tf.CurrentState == 0)
            {
                tf.Hide();
                tf.ShowLogo = false;
                tf.SetBackgroundBitmap(new Bitmap("popup2.bmp"), Color.FromArgb(255, 0, 255));
                Rectangle TitleRectangle = new Rectangle(5, 1, 70, 25);
                Rectangle TitlebarRectangle = new Rectangle(1, 1, tf.Width, 25);
                Rectangle ContentRectangle = Rectangle.Empty;
                Rectangle ClosebtnRectangle = new Rectangle(tf.Width - 25, 1, 25, 25);
                tf.Opacity = 1;
                tf.ShowForm("提示", ConfigurationManager.AppSettings["webpage"], TitleRectangle, TitlebarRectangle, ContentRectangle, ClosebtnRectangle);
                //MessageBox.Show(tf.Url);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (tf.CurrentState == 0)
            {
                tf.Hide();
                tf.ShowLogo = false;
                tf.SetBackgroundBitmap(new Bitmap("popup1.bmp"), Color.FromArgb(255, 0, 255));
                Rectangle TitleRectangle = new Rectangle(5, 1, 70, 25);
                Rectangle TitlebarRectangle = new Rectangle(1, 1, 125, tf.Height);
                Rectangle ContentRectangle = new Rectangle(125, 100, 170, 50);
                Rectangle ClosebtnRectangle = new Rectangle(tf.Width - 25, 100, 25, 25);
                tf.Opacity = 1;
                tf.ShowForm("提示", "未来科技论坛", ConfigurationManager.AppSettings["bbs"], TitleRectangle, TitlebarRectangle, ContentRectangle, ClosebtnRectangle);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            KellMediaAdvertisingForm madv = new KellMediaAdvertisingForm(ConfigurationManager.AppSettings["AdvUri"], ConfigurationManager.AppSettings["LinkUrl"]);
            madv.ShowForm();
        }
    }
    public class TypeNode : KellControls.KellNode
    {
        TypeClass type;
        List<TypeClass> types;

        public TypeNode(TypeClass type, List<TypeClass> types)
            : base()
        {
            this.type = type;
            this.types = types;
        }

        public override List<KellControls.KellNode> GetSub()
        {
            List<KellControls.KellNode> children = new List<KellControls.KellNode>();
            foreach (TypeClass t in types)
            {
                if (t.Parent == type)
                {
                    children.Add(new TypeNode(t, types));
                }
            }
            return children;
        }
    }
    public class TypeClass
    {
        public TypeClass()
        {

        }

        public TypeClass(int ID, string TypeName, string Description, TypeClass Parent)
        {
            this.id = ID;
            this.typeName = TypeName;
            this.description = Description;
            this.parent = Parent;
        }

        int id;

        public int Id
        {
            get { return id; }
        }
        string typeName = "";

        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }
        string description = "";

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        TypeClass parent;

        public TypeClass Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public override string ToString()
        {
            return TypeName;
        }
    }
}
