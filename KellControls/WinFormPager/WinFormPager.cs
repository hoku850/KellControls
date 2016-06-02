using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data;

namespace KellControls
{
    [DefaultEvent("PageIndexChanged"), DefaultProperty("RecordCount"), Description("WinForm下的分页控件")]
public class WinFormPager : UserControl
{
    // Fields
    private string _BtnTextFirst = "首页";
    private string _BtnTextLast = "末页";
    private string _BtnTextNext = "下一页";
    private string _BtnTextPrevious = "上一页";
    private DisplayStyleEnum _DisplayStyle = DisplayStyleEnum.图片;
    private string _PageIndexText = "页码";
    private string _JumpText = "跳转";
    private int _PageCount;
    private int _PageIndex = 1;
    private int _PageSize = 10;
    private int _RecordCount;
    private TextImageRalitionEnum _TextImageRalition = TextImageRalitionEnum.图片显示在文字前方;
    private Button btnFirst;
    private Button btnLast;
    private Button btnNext;
    private Button btnPrevious;
    private Button btnToPageIndex;
    private IContainer components;
    private ImageList imglstPager;
    //private Label label1;
    //private Label label3;
    private Label lblPager;
    private string PagerText = "总共{0}条记录,当前第{1}页,共{2}页,每页{3}条记录";
    private ToolTip toolTipPager;
    private TextBox txtToPageIndex;
    private Panel panel1;
        private const int MinHeight = 23;

    // Events
    [Description("更改页面索引事件"), Category("自定义事件")]
    public event EventHandler PageIndexChanged;

    // Methods
    public WinFormPager()
    {
        this.InitializeComponent();
    }

    private void btnFirst_Click(object sender, EventArgs e)
    {
        this._PageIndex = 1;
        this.SetPagerText();
        this.SetBtnEnabled();
        this.CustomEvent(sender, e);
    }

    private void btnLast_Click(object sender, EventArgs e)
    {
        this._PageIndex = this._PageCount;
        this.SetPagerText();
        this.SetBtnEnabled();
        this.CustomEvent(sender, e);
    }

    private void btnNext_Click(object sender, EventArgs e)
    {
        int num = this._PageIndex;
        if (num == this._PageCount)
            return;
        try
        {
            num++;
            this._PageIndex = num;
            this.SetPagerText();
            this.SetBtnEnabled();
            this.CustomEvent(sender, e);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void btnPrevious_Click(object sender, EventArgs e)
    {
        int num = this._PageIndex;
        if (num == 1)
            return;
        try
        {
            num--;
            this._PageIndex = num;
            this.SetPagerText();
            this.SetBtnEnabled();
            this.CustomEvent(sender, e);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void btnToPageIndex_Click(object sender, EventArgs e)
    {
        string text = this.txtToPageIndex.Text;
        int num = this._PageIndex;
        if (string.IsNullOrEmpty(text))
        {
            num = 1;
            this.txtToPageIndex.Text = "1";
        }
        else
        {
                if (int.TryParse(text, out num))
                {
                    if (num > this._PageCount)
                    {
                        num = this._PageCount;
                        this.txtToPageIndex.Text = this._PageCount.ToString();
                    }
                    else
                    {
                        this._PageIndex = num;
                        this.SetPagerText();
                        this.SetBtnEnabled();
                        this.CustomEvent(sender, e);
                    }
                }
                else
                {
                    MessageBox.Show("请输入正整数！");
                }
        }
    }

    private void CustomEvent(object sender, EventArgs e)
    {
        if (PageIndexChanged != null)
            PageIndexChanged(sender, e);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (this.components != null))
        {
            this.components.Dispose();
        }
        base.Dispose(disposing);
    }

    protected int GetPageCount(int RecordCount, int PageSize)
    {
        int num = 0;
        string str = (Convert.ToDouble(RecordCount) / Convert.ToDouble(PageSize)).ToString();
        if (str.IndexOf(".") < 0)
        {
            return Convert.ToInt32(str);
        }
        string[] strArray = Regex.Split(str, @"\.", RegexOptions.IgnoreCase);
        if (!string.IsNullOrEmpty(strArray[1].ToString()))
        {
            num = Convert.ToInt32(strArray[0]) + 1;
        }
        return num;
    }

    private void InitializeComponent()
    {
        this.components = new Container();
        //ComponentResourceManager manager = new ComponentResourceManager(typeof(WinFormPager));
        this.lblPager = new Label();
        this.btnFirst = new Button();
        this.imglstPager = new ImageList(this.components);
        this.btnPrevious = new Button();
        this.btnNext = new Button();
        this.btnLast = new Button();
        this.btnToPageIndex = new Button();
        this.txtToPageIndex = new TextBox();
        this.toolTipPager = new ToolTip(this.components);
        //this.label3 = new Label();
        //this.label1 = new Label();
        this.panel1 = new Panel();
        base.SuspendLayout();
        //manager.ApplyResources(this.lblPager, "lblPager");
        this.lblPager.BackColor = Color.Transparent;
        this.lblPager.Name = "lblPager";
        this.lblPager.AutoSize = false;
        this.lblPager.TextAlign = ContentAlignment.MiddleLeft;
        this.lblPager.Dock = DockStyle.Fill;
            //manager.ApplyResources(this.btnFirst, "btnFirst");
            this.btnFirst.BackColor = Color.Transparent;
        this.btnFirst.Cursor = Cursors.Hand;
        this.btnFirst.FlatAppearance.BorderSize = 0;
        this.btnFirst.ForeColor = SystemColors.ControlText;
        this.btnFirst.ImageList = this.imglstPager;
        this.btnFirst.Name = "btnFirst";
        this.btnFirst.TabStop = false;
        this.btnFirst.Width = 50;
            this.btnFirst.Dock = DockStyle.Right;
            this.toolTipPager.SetToolTip(this.btnFirst, _BtnTextFirst);
        this.btnFirst.UseVisualStyleBackColor = false;
        this.btnFirst.Click += new EventHandler(this.btnFirst_Click);
        //this.imglstPager.ImageStream = (ImageListStreamer) manager.GetObject("imglstPager.ImageStream");

        this.imglstPager.Images.Add(KellControls.Properties.Resources.resultset_first);
        this.imglstPager.Images.Add(KellControls.Properties.Resources.resultset_previous);
        this.imglstPager.Images.Add(KellControls.Properties.Resources.resultset_next);
        this.imglstPager.Images.Add(KellControls.Properties.Resources.resultset_last);
        this.imglstPager.TransparentColor = Color.Transparent;
        this.imglstPager.Images.SetKeyName(0, "resultset_first.png");
        this.imglstPager.Images.SetKeyName(1, "resultset_previous.png");
        this.imglstPager.Images.SetKeyName(2, "resultset_next.png");
        this.imglstPager.Images.SetKeyName(3, "resultset_last.png");
        //manager.ApplyResources(this.btnPrevious, "btnPrevious");
        this.btnPrevious.BackColor = Color.Transparent;
        this.btnPrevious.Cursor = Cursors.Hand;
        this.btnPrevious.FlatAppearance.BorderSize = 0;
        this.btnPrevious.ForeColor = SystemColors.ControlText;
        this.btnPrevious.ImageList = this.imglstPager;
        this.btnPrevious.Name = "btnPrevious";
        this.btnPrevious.TabStop = false;
        this.btnPrevious.Width = 50;
            btnPrevious.Dock = DockStyle.Right;
            this.toolTipPager.SetToolTip(this.btnPrevious, _BtnTextPrevious);
        this.btnPrevious.UseVisualStyleBackColor = false;
        this.btnPrevious.Click += new EventHandler(this.btnPrevious_Click);
        //manager.ApplyResources(this.btnNext, "btnNext");
        this.btnNext.BackColor = Color.Transparent;
        this.btnNext.Cursor = Cursors.Hand;
        this.btnNext.FlatAppearance.BorderSize = 0;
        this.btnNext.ForeColor = SystemColors.ControlText;
        this.btnNext.ImageList = this.imglstPager;
        this.btnNext.Name = "btnNext";
        this.btnNext.TabStop = false;
        this.btnNext.Width = 50;
            this.btnNext.Dock = DockStyle.Right;
            this.toolTipPager.SetToolTip(this.btnNext, _BtnTextNext);
        this.btnNext.UseVisualStyleBackColor = false;
        this.btnNext.Click += new EventHandler(this.btnNext_Click);
        //manager.ApplyResources(this.btnLast, "btnLast");
        this.btnLast.BackColor = Color.Transparent;
        this.btnLast.Cursor = Cursors.Hand;
        this.btnLast.FlatAppearance.BorderSize = 0;
        this.btnLast.ForeColor = SystemColors.ControlText;
        this.btnLast.ImageList = this.imglstPager;
        this.btnLast.Name = "btnLast";
        this.btnLast.TabStop = false;
        this.btnLast.Width = 50;
            this.btnLast.Dock = DockStyle.Right;
            this.toolTipPager.SetToolTip(this.btnLast, _BtnTextLast);
        this.btnLast.UseVisualStyleBackColor = false;
        this.btnLast.Click += new EventHandler(this.btnLast_Click);
        //manager.ApplyResources(this.label3, "label3");
        //this.label3.BackColor = Color.Transparent;
        //this.label3.Name = "label3";
        //    this.label3.Text = "label3";
        //manager.ApplyResources(this.btnToPageIndex, "btnToPageIndex");
        this.btnToPageIndex.BackColor = Color.Transparent;
        this.btnToPageIndex.Cursor = Cursors.Hand;
        this.btnToPageIndex.FlatAppearance.BorderSize = 0;
        this.btnToPageIndex.FlatAppearance.MouseDownBackColor = Color.Transparent;
        this.btnToPageIndex.FlatAppearance.MouseOverBackColor = Color.Transparent;
        this.btnToPageIndex.Name = "btnToPageIndex";
        this.btnToPageIndex.TabStop = false;
        this.btnToPageIndex.Width = 50;
            this.btnToPageIndex.Dock = DockStyle.Right;
            this.toolTipPager.SetToolTip(this.btnToPageIndex, _JumpText);
        this.btnToPageIndex.UseVisualStyleBackColor = false;
        this.btnToPageIndex.Click += new EventHandler(this.btnToPageIndex_Click);
        //manager.ApplyResources(this.txtToPageIndex, "txtToPageIndex");
        this.txtToPageIndex.Name = "txtToPageIndex";
        this.txtToPageIndex.TabStop = false;
        this.txtToPageIndex.Width = 50;
            this.txtToPageIndex.Multiline = true;
            this.txtToPageIndex.Top = 1;
            this.txtToPageIndex.TextAlign = HorizontalAlignment.Center;
            this.txtToPageIndex.Dock = DockStyle.Right;
            this.toolTipPager.SetToolTip(this.txtToPageIndex, _PageIndexText);
        this.txtToPageIndex.KeyPress += new KeyPressEventHandler(this.txtToPageIndex_KeyPress);
        //manager.ApplyResources(this.label1, "label1");
        //this.label1.BackColor = Color.Transparent;
        //this.label1.Name = "label1";
        //    this.label1.Text = "label1";
            //manager.ApplyResources(this, "$this");
            this.AutoScaleMode = AutoScaleMode.Font;
        this.BackColor = Color.Transparent;
        this.BackgroundImage = KellControls.Properties.Resources.Pager_PageBg;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            //manager.ApplyResources(this.panel1, "panel1");
            //this.panel1.Size = new Size(200, MinHeight);
            this.panel1.Name = "panel1";
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = AutoSizeMode.GrowOnly;
            this.panel1.Dock = DockStyle.Right;
            this.panel1.BackColor = Color.Transparent;
            this.panel1.Controls.Add(this.btnFirst);
            this.panel1.Controls.Add(this.btnPrevious);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnLast);
            this.panel1.Controls.Add(this.txtToPageIndex);
            this.panel1.Controls.Add(this.btnToPageIndex);
            //this.panel1.Controls.Add(this.label1);
            //this.panel1.Controls.Add(this.label3);
            this.Controls.Add(panel1);
            this.Controls.Add(this.lblPager);
            this.Height = this.btnFirst.Height;
            this.Name = "WinFormPager";
            this.Load += new EventHandler(this.Pager_Load);
            this.Paint += new PaintEventHandler(this.Pager_Paint);
            this.MouseMove += new MouseEventHandler(this.WinFormPager_MouseMove);
            this.Resize += WinFormPager_Resize;
            this.ResumeLayout(false);
            this.PerformLayout();
    }

        private void WinFormPager_Resize(object sender, EventArgs e)
        {
            if (this.Height < MinHeight)
                this.Height = MinHeight;
        }

        private void Pager_Load(object sender, EventArgs e)
    {
        this.SetBtnEnabled();
        this.btnToPageIndex.Text = this._JumpText;
    }

    private void Pager_Paint(object sender, PaintEventArgs e)
    {
        this._PageCount = this.GetPageCount(this._RecordCount, this._PageSize);
        this.SetPagerText();
        this.SetDisplayStyle();
        //this.SetLabelLocation();
    }

    protected void SetBtnEnabled()
    {
        if (this._PageIndex == 1)
        {
            this.btnFirst.Enabled = false;
            this.btnPrevious.Enabled = false;
            this.btnNext.Enabled = true;
            this.btnLast.Enabled = true;
        }
        else if ((this._PageIndex > 1) && (this._PageIndex < this._PageCount))
        {
            this.btnFirst.Enabled = true;
            this.btnPrevious.Enabled = true;
            this.btnNext.Enabled = true;
            this.btnLast.Enabled = true;
        }
        else if (this._PageIndex == this._PageCount)
        {
            this.btnFirst.Enabled = true;
            this.btnPrevious.Enabled = true;
            this.btnNext.Enabled = false;
            this.btnLast.Enabled = false;
        }
    }

    private void SetDisplayStyle()
    {
        TextImageRelation imageBeforeText = TextImageRelation.ImageBeforeText;
        if (this.TextImageRalitions == TextImageRalitionEnum.图片显示在文字上方)
        {
            imageBeforeText = TextImageRelation.ImageAboveText;
        }
        else if (this.TextImageRalitions == TextImageRalitionEnum.图片显示在文字下方)
        {
            imageBeforeText = TextImageRelation.TextAboveImage;
        }
        else if (this.TextImageRalitions == TextImageRalitionEnum.图片显示在文字前方)
        {
            imageBeforeText = TextImageRelation.ImageBeforeText;
        }
        else if (this.TextImageRalitions == TextImageRalitionEnum.图片显示在文字后方)
        {
            imageBeforeText = TextImageRelation.TextBeforeImage;
        }
        if (this.DisplayStyle == DisplayStyleEnum.图片)
        {
            this.btnFirst.ImageList = this.btnPrevious.ImageList = this.btnNext.ImageList = this.btnLast.ImageList = this.imglstPager;
            this.btnFirst.ImageIndex = 0;
            this.btnPrevious.ImageIndex = 1;
            this.btnNext.ImageIndex = 2;
            this.btnLast.ImageIndex = 3;
            this.btnFirst.Text = this.btnPrevious.Text = this.btnNext.Text = this.btnLast.Text = "";
            this.btnFirst.TextImageRelation = this.btnPrevious.TextImageRelation = this.btnNext.TextImageRelation = this.btnLast.TextImageRelation = TextImageRelation.Overlay;
        }
        else if (this.DisplayStyle == DisplayStyleEnum.文字)
        {
            this.btnFirst.ImageList = this.btnPrevious.ImageList = this.btnNext.ImageList = (ImageList) (this.btnLast.ImageList = null);
            this.btnFirst.Text = string.IsNullOrEmpty(this.BtnTextFirst) ? "首页" : this.BtnTextFirst;
            this.btnPrevious.Text = string.IsNullOrEmpty(this.BtnTextPrevious) ? "上一页" : this.BtnTextPrevious;
            this.btnNext.Text = string.IsNullOrEmpty(this.BtnTextNext) ? "下一页" : this.BtnTextNext;
            this.btnLast.Text = string.IsNullOrEmpty(this.BtnTextLast) ? "末页" : this.BtnTextLast;
            this.btnFirst.TextImageRelation = this.btnPrevious.TextImageRelation = this.btnNext.TextImageRelation = this.btnLast.TextImageRelation = TextImageRelation.Overlay;
        }
        else if (this.DisplayStyle == DisplayStyleEnum.图片及文字)
        {
            this.btnFirst.ImageList = this.btnPrevious.ImageList = this.btnNext.ImageList = this.btnLast.ImageList = this.imglstPager;
            this.btnFirst.ImageIndex = 0;
            this.btnPrevious.ImageIndex = 1;
            this.btnNext.ImageIndex = 2;
            this.btnLast.ImageIndex = 3;
            this.btnFirst.Text = string.IsNullOrEmpty(this.BtnTextFirst) ? "首页" : this.BtnTextFirst;
            this.btnPrevious.Text = string.IsNullOrEmpty(this.BtnTextPrevious) ? "上一页" : this.BtnTextPrevious;
            this.btnNext.Text = string.IsNullOrEmpty(this.BtnTextNext) ? "下一页" : this.BtnTextNext;
            this.btnLast.Text = string.IsNullOrEmpty(this.BtnTextLast) ? "末页" : this.BtnTextLast;
            this.btnFirst.TextImageRelation = this.btnPrevious.TextImageRelation = this.btnNext.TextImageRelation = this.btnLast.TextImageRelation = imageBeforeText;
        }
    }

    protected void SetLabelLocation()
    {
            int left = 0;//this.lblPager.Left + this.lblPager.Width + 2
            this.btnFirst.Left = left;
        this.btnPrevious.Left = this.btnFirst.Left + this.btnFirst.Width;
        this.btnNext.Left = this.btnPrevious.Left + this.btnPrevious.Width;
        this.btnLast.Left = this.btnNext.Left + this.btnNext.Width;
        this.txtToPageIndex.Left = this.btnLast.Left + this.btnLast.Width;
            this.btnToPageIndex.Left = this.txtToPageIndex.Left + this.txtToPageIndex.Width;
        //this.label3.Left = (this.btnToPageIndex.Left + this.btnToPageIndex.Width);
        //this.label1.Left = this.label3.Left + this.label3.Width;
    }

    private void SetPagerText()
    {
        string[] strArray = new string[] { this.RecordCount.ToString(), this.PageIndex.ToString(), this.PageCount.ToString(), this.PageSize.ToString() };
        this.lblPager.Text = string.Format(this.PagerText, (object[]) strArray);
    }

    private void txtToPageIndex_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsNumber(e.KeyChar) && (e.KeyChar != '\b'))
        {
            if (e.KeyChar == '\r')
            {
                this.btnToPageIndex_Click(new object(), new EventArgs());
            }
            else
            {
                e.Handled = true;
            }
        }
    }

    private void WinFormPager_MouseMove(object sender, MouseEventArgs e)
    {
        foreach (Control control in base.Controls)
        {
            if (control is Button)
            {
                Button button = (Button) control;
                button.BackColor = Color.Transparent;
                button.FlatAppearance.BorderColor = Color.White;
                button.FlatAppearance.MouseDownBackColor = Color.Transparent;
                button.FlatAppearance.MouseOverBackColor = Color.Transparent;
            }
        }
    }

    // Properties
    [Category("自定义属性"), DefaultValue("首页"), Description("首页按钮文字,当DisplayStyle=文字或DisplayStyle=图片及文字时生效")]
    public string BtnTextFirst
    {
        get
        {
            return this._BtnTextFirst;
        }
        set
        {
            this._BtnTextFirst = value;
            this.SetDisplayStyle();
        }
    }

    [Description("末页按钮文字,当DisplayStyle=文字或DisplayStyle=图片及文字时生效"), DefaultValue("末页"), Category("自定义属性")]
    public string BtnTextLast
    {
        get
        {
            return this._BtnTextLast;
        }
        set
        {
            this._BtnTextLast = value;
            this.SetDisplayStyle();
        }
    }

    [DefaultValue("下一页"), Category("自定义属性"), Description("下一页按钮文字,当DisplayStyle=文字或DisplayStyle=图片及文字时生效")]
    public string BtnTextNext
    {
        get
        {
            return this._BtnTextNext;
        }
        set
        {
            this._BtnTextNext = value;
            this.SetDisplayStyle();
        }
    }

    [DefaultValue("上一页"), Description("上一页按钮文字,当DisplayStyle=文字或DisplayStyle=图片及文字时生效"), Category("自定义属性")]
    public string BtnTextPrevious
    {
        get
        {
            return this._BtnTextPrevious;
        }
        set
        {
            this._BtnTextPrevious = value;
            this.SetDisplayStyle();
        }
    }

    [Category("自定义属性"), DefaultValue(1), Description("显示类型：图片、文字、图片及文字")]
    public DisplayStyleEnum DisplayStyle
    {
        get
        {
            return this._DisplayStyle;
        }
        set
        {
            this._DisplayStyle = value;
            this.SetDisplayStyle();
        }
    }

    [Description("跳转按钮文字"), Category("自定义属性"), DefaultValue("跳转")]
    public string JumpText
    {
        get
        {
            return this._JumpText;
        }
        set
        {
            this._JumpText = value;
            this.btnToPageIndex.Text = this._JumpText;
        }
    }

    private int PageCount
    {
        get
        {
            return this._PageCount;
        }
    }

    [DefaultValue(1), Category("自定义属性"), Description("当前显示的页数")]
    public int PageIndex
    {
        get
        {
            return this._PageIndex;
        }
        set
        {
            this._PageIndex = value;
        }
    }

    [DefaultValue(10), Description("每页显示的记录数"), Category("自定义属性")]
    public int PageSize
    {
        get
        {
            return this._PageSize;
        }
        set
        {
            if (value < 1)
            {
                value = 1;
            }
            this._PageSize = value;
            this.Refresh();
        }
    }

    [Description("要分页的总记录数"), Category("自定义属性")]
    public int RecordCount
    {
        get
        {
            return this._RecordCount;
        }
        set
        {
            this._RecordCount = value;
            this.Refresh();
        }
    }

    [DefaultValue(3), Description("图片和文字显示相对位置,当DisplayStyle=文字或DisplayStyle=图片及文字时生效"), Category("自定义属性")]
    public TextImageRalitionEnum TextImageRalitions
    {
        get
        {
            return this._TextImageRalition;
        }
        set
        {
            this._TextImageRalition = value;
            this.SetDisplayStyle();
        }
    }

    // Nested Types
    public enum DisplayStyleEnum
    {
        图片 = 1,
        图片及文字 = 3,
        文字 = 2
    }

    public enum TextImageRalitionEnum
    {
        图片显示在文字后方 = 4,
        图片显示在文字前方 = 3,
        图片显示在文字上方 = 1,
        图片显示在文字下方 = 2
    }
}
}
