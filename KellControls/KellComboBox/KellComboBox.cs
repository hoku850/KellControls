using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace KellControls
{
    public class KellComboBox : ComboBox
    {
        //APIs
        //TODO: INSTANT C# TODO TASK: Insert the following converted event handlers at the end of the 'InitializeComponent' method for forms, 'Page_Init' for web pages, or into a constructor for other classes:


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private extern static int SendMessage(System.IntPtr hWnd, int Msg, System.Int32 wParam, System.IntPtr lParam);


        //properties
        private string mDividerFormat = "";
        private Color mGroupColor = System.Drawing.SystemColors.WindowText; //ForeColor 'mediumblue
        private bool[] mItemsChecks;
        private bool mCheckBoxes;
        private Color mGridColor = Color.FromArgb(240, 248, 255);

        //vars - last selected item
        private bool mRedrawingUnselected;
        private Graphics mGraphics;
        private Rectangle mBounds;
        private Int32 mIndex;
        private char mItemSeparator = ',';
        //private char mItemSeparator2 = '&';
        private Graphics mGraphics_t;
        private Rectangle mBounds_t;
        private Int32 mIndex_t;

        private long mHoverIndex;

        private Timer mTimer1;

        //private bool manualText;

        //public bool ManualText
        //{
        //    get { return manualText; }
        //    set { manualText = value; }
        //}
        //events
        public delegate void ComboBoxItemHandler(Int32 eIndex);
        public event ComboBoxItemHandler ComboBoxItemFocus;

        //vars
        private Int32 mLastSelectedIndex = -1;


        //constructor
        public KellComboBox()
        {
            // set draw mode to owner draw
            this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed; //DrawMode.OwnerDrawFixed
            //this.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public override int SelectedIndex
        {
            get
            {
                return base.SelectedIndex;
            }
            set
            {
                base.SelectedIndex = value;
                mLastSelectedIndex = value;
                this.Refresh();
            }
        }

        public new int ItemHeight
        {
            get
            {
                return base.ItemHeight;
            }
            set
            {
                base.ItemHeight = value;
                for (int i = 0; i < this.Items.Count; i++)
                {
                    ComboBoxItem cbi = this.GetItem(i);
                    cbi.ImageSize = new Size(base.ItemHeight, base.ItemHeight);
                }
            }
        }
        
        public void AddItem(object value, string text)
        {
            lock (this)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Value = value;
                item.Text = text;
                this.Items.Add(item);
            }
        }
        public void AddItem(object value, string text, bool check)
        {
            lock (this)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Value = value;
                item.Text = text;
                item.Checked = check;
                this.Items.Add(item);
            }
        }
        public void AddItem(object value, string text, Image image)
        {
            lock (this)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Value = value;
                item.Text = text;
                item.Image = image;
                item.ImageSize = new Size(this.ItemHeight, this.ItemHeight);
                this.Items.Add(item);
            }
        }
        public void AddItem(object value, string text, bool check, Image image)
        {
            lock (this)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Value = value;
                item.Text = text;
                item.Checked = check;
                item.Image = image;
                item.ImageSize = new Size(this.ItemHeight, this.ItemHeight);
                this.Items.Add(item);
            }
        }
        public void AddItem(object value, string text, Image image, Size imageSize)
        {
            lock (this)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Value = value;
                item.Text = text;
                item.Image = image;
                item.ImageSize = imageSize;
                this.Items.Add(item);
            }
        }
        public void AddItem(object value, string text, Image image, int imageOffset, Size imageSize)
        {
            lock (this)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Value = value;
                item.Text = text;
                item.Image = image;
                item.ImageOffset = imageOffset;
                item.ImageSize = imageSize;
                this.Items.Add(item);
            }
        }
        public void AddItem(object value, string text, bool check, Image image, int imageOffset, Size imageSize)
        {
            lock (this)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Value = value;
                item.Text = text;
                item.Checked = check;
                item.Image = image;
                item.ImageOffset = imageOffset;
                item.ImageSize = imageSize;
                this.Items.Add(item);
            }
        }
        public void RemoveItemByValue(object value)
        {
            lock (this)
            {
                for (int i = this.Items.Count - 1; i > -1; i--)
                {
                    ComboBoxItem item = (ComboBoxItem)this.Items[i];
                    if (item.Value.Equals(value))
                        this.Items.Remove(item);
                }
            }
        }
        public void RemoveItemByIndex(int index)
        {
            lock (this)
            {
                this.Items.RemoveAt(index);
            }
        }
        public void RemoveItemByText(string text, bool ignoreCase)
        {
            lock (this)
            {
                for (int i = this.Items.Count - 1; i < -1; i--)
                {
                    ComboBoxItem item = (ComboBoxItem)this.Items[i];
                    if (ignoreCase)
                    {
                        if (item.Text.ToLower() == text.ToLower())
                            this.Items.Remove(item);
                    }
                    else
                    {
                        if (item.Text == text)
                            this.Items.Remove(item);
                    }
                }
            }
        }
        public ComboBoxItem GetItem(int index)
        {
            if (index >= 0)
            {
                return this.Items[index] as ComboBoxItem;
            }
            else
            {
                return null;
            }
        }
        public ComboBoxItem GetCurrentItem()
        {
            int index = base.SelectedIndex;
            if (index >= 0)
            {
                return this.Items[index] as ComboBoxItem;
            }
            else
            {
                return null;
            }
        }
        public List<ComboBoxItem> GetCheckedItems()
        {
            List<ComboBoxItem> items = new List<ComboBoxItem>();
            foreach (object item in this.Items)
            {
                ComboBoxItem ci = (ComboBoxItem)item;
                if (ci.Checked)
                {
                    items.Add(ci);
                }
            }
            return items;
        }
        public object GetItemValue(int index)
        {
            if (index >= 0)
            {
                ComboBoxItem item = this.Items[index] as ComboBoxItem;
                if (item != null)
                    return item.Value;
            }
            return null;
        }
        public string GetItemText(int index)
        {
            if (index >= 0)
            {
                ComboBoxItem item = this.Items[index] as ComboBoxItem;
                if (item != null)
                    return item.Text;
            }
            return "";
        }

        public object GetCurrentItemValue()
        {
            int index = base.SelectedIndex;
            if (index >= 0)
            {
                ComboBoxItem item = this.Items[index] as ComboBoxItem;
                if (item != null)
                    return item.Value;
            }
            return null;
        }
        public string GetCurrentItemText()
        {
            int index = base.SelectedIndex;
            if (index >= 0)
            {
                ComboBoxItem item = this.Items[index] as ComboBoxItem;
                if (item != null)
                    return item.Text;
            }
            return "";
        }

        //properties
        [System.ComponentModel.Description("Use this property to set divider flag.  Recommend you use three hyphens ---."), System.ComponentModel.Category("Power Properties")]
        public string DividerFormat
        {
            get
            {

                return mDividerFormat;

            }
            set
            {

                mDividerFormat = value;

            }

        }
        [System.ComponentModel.Description("Use this property to set the ForeColor of the grouping text."), System.ComponentModel.Category("Power Properties")]
        public Color GroupColor
        {

            get
            {

                return mGroupColor;

            }
            set
            {

                mGroupColor = value;

            }

        }
        [System.ComponentModel.Description("Use this property to set the BackColor of the grid."), System.ComponentModel.Category("Power Properties")]
        public Color GridColor
        {

            get
            {

                return mGridColor;

            }
            set
            {

                mGridColor = value;

            }

        }
        //ORIGINAL LINE: Public Property ItemsChecks(ByVal xIndex As Int32) As Boolean
        //INSTANT C# NOTE: C# does not support parameterized properties - the following property has been divided into two methods:
        [System.ComponentModel.Description("Use this property to get/set corresponding checkboc values."), System.ComponentModel.Category("Power Properties")]
        public bool GetItemsChecks(Int32 xIndex)
        {

            return mItemsChecks[xIndex];


        }
        public void SetItemsChecks(Int32 xIndex, bool value)
        {
            mItemsChecks[xIndex] = value;
        }
        [System.ComponentModel.Description("Use this property to enable checkboxes."), System.ComponentModel.Category("Power Properties")]
        public bool ShowCheckBoxes
        {
            get
            {
                return mCheckBoxes;
            }
            set
            {
                mCheckBoxes = value;
            }
        }
        [System.ComponentModel.Description("Use this property to set item separator1 character."), System.ComponentModel.Category("Power Properties")]
        public char ItemSeparator
        {
            get
            {
                return mItemSeparator;
            }
            set
            {
                mItemSeparator = value;
            }
        }
        [System.ComponentModel.Description("Use this property to set item separator2 character."), System.ComponentModel.Category("Power Properties")]
        //public char ItemSeparator2
        //{
        //    get
        //    {
        //    return mItemSeparator2;
        //    }
        //    set
        //    {
        //    mItemSeparator2 = value;
        //    }
        //}


        //overrides
        protected override void OnTextChanged(System.EventArgs e)
        {


            base.OnTextChanged(e);

        }
        protected override void OnSelectedIndexChanged(System.EventArgs e)
        {

            //cancels SIC when user try to select a group
            if (this.DividerFormat.Length > 0 && this.Text.Contains(this.mDividerFormat))
            {
                this.SelectedIndex = mLastSelectedIndex; //NOTE restorre last valid selection
                return;
            }

            mLastSelectedIndex = this.SelectedIndex;
            base.OnSelectedIndexChanged(e);

        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {

            Int32 zX1 = 0;
            Pen zPen = null;
            float zWidth = 0F;
            string zText = null;
            Font zFont = null;
            Color zFore = new Color();
            System.Windows.Forms.VisualStyles.CheckBoxState zState = 0;

            if (e.Index < 0)
            {
                base.OnDrawItem(e);
                return; //fix for designer error - hope doesnt effect runtime drawing
            }
            Rectangle r = e.Bounds;
            ComboBoxItem item = (ComboBoxItem)this.Items[e.Index];

            //NOTE 1st part of if statement draw group item, 2nd draws actual item
            if (this.Items[e.Index].ToString().Contains(this.mDividerFormat) & mDividerFormat.Length > 0)
            {
                //background
                if (e.Index == SelectedIndex)
                {
                    e.DrawBackground();
                }
                zText = item.Text;// this.Items[e.Index].ToString();
                zText = " " + zText.Replace(this.mDividerFormat, "") + " ";
                zFont = new Font(Font, FontStyle.Bold); //group item
                if (e.BackColor == System.Drawing.SystemColors.Highlight)
                {
                    zFore = Color.Gainsboro; //Color.LightGray 'Color.Silver 'gray colr lets user know that shouldn't be selecting a group item
                }
                else
                {
                    zFore = this.GroupColor; //e.ForeColor
                }
                zPen = new Pen(zFore);
                //draw group
                zWidth = e.Graphics.MeasureString(zText, zFont).Width;
                zX1 = Convert.ToInt32(e.Bounds.Width - zWidth) / 2;
                e.Graphics.DrawRectangle(zPen, new Rectangle(e.Bounds.X, e.Bounds.Y + e.Bounds.Height / 2, zX1, 1));
                e.Graphics.DrawRectangle(zPen, new Rectangle(e.Bounds.Width - zX1, e.Bounds.Y + e.Bounds.Height / 2, e.Bounds.Width, 1));
                if (item.Image != null)
                    e.Graphics.DrawImage(item.Image, new Rectangle(new Point(item.ImageOffset, r.Height * e.Index), item.ImageSize));
                e.Graphics.DrawString(zText, zFont, new SolidBrush(zFore), zX1 + item.ImageOffset + item.ImageSize.Width + 4, r.Height * e.Index);
            }
            else
            {

                if (mRedrawingUnselected)
                {
                    if (mIndex % 2 == 0)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.White), mBounds); //e.Bounds)'FIX for wrong bound being passed
                    }
                    else
                    {
                        e.Graphics.FillRectangle(new SolidBrush(mGridColor), mBounds); //e.Bounds) 'FIX for wrong bound being passed
                    }
                }
                else
                {
                    if (e.Index == SelectedIndex)
                    {
                        e.DrawBackground();
                    }
                    else
                    {
                        if (e.Index % 2 == 0)
                        {
                            e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                        }
                        else
                        {
                            e.Graphics.FillRectangle(new SolidBrush(mGridColor), e.Bounds);
                        }
                    }
                }


                //checkbox work
                if (mCheckBoxes)
                {
                    if (e.BackColor == System.Drawing.SystemColors.Highlight)
                    {
                        if (this.GetItemsChecks(e.Index))
                        {
                            zState = System.Windows.Forms.VisualStyles.CheckBoxState.CheckedHot;
                        }
                        else
                        {
                            zState = System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedHot;
                        }
                    }
                    else
                    {
                        if (this.GetItemsChecks(e.Index))
                        {
                            zState = System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal;
                        }
                        else
                        {
                            zState = System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;
                        }
                    }
                    zX1 = this.FontHeight; /// 2 'variable re-use
                    zPen = new Pen(Color.Black, 1F);
                    zPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    //e.Graphics.DrawLine(zPen, e.Bounds.Left, e.Bounds.Top + zX1, e.Bounds.Left + e.Bounds.Width, e.Bounds.Top + zX1)
                    System.Windows.Forms.CheckBoxRenderer.DrawCheckBox(e.Graphics, new System.Drawing.Point(0, e.Bounds.Y + 1), e.Bounds, "", this.Font, false, zState);
                }
                int checkBoxWidth = 0;
                if (mCheckBoxes)
                {
                    checkBoxWidth = 20;
                }
                int imageWid = this.ItemHeight;
                if (item.Image != null)
                {
                    e.Graphics.DrawImage(item.Image, new Rectangle(new Point(item.ImageOffset + checkBoxWidth, r.Height * e.Index), item.ImageSize));
                imageWid = item.ImageSize.Width;
            }
                //text work
                e.Graphics.DrawString(item.Text, Font, new SolidBrush(e.ForeColor), new PointF(item.ImageOffset + checkBoxWidth + imageWid + 4, r.Height * e.Index));//this.Items[e.Index].ToString()
            }

            //exits here when redrawing unselected
            if (mRedrawingUnselected)
            {
                mRedrawingUnselected = false;
            }
            else
            {

                //NOTE just seems to work 100% when timer is used in conjunction with .Bound override of old graphics property
                if (SelectedIndex == e.Index)
                {
                    mGraphics_t = e.Graphics;
                    mBounds_t = e.Bounds;
                    mIndex_t = e.Index;
                    mTimer1 = new Timer();
                    mTimer1.Interval = 5;
                    mTimer1.Enabled = true;
                    mTimer1.Tick += new EventHandler(mTimer1_Tick);
                }

            }

            base.OnDrawItem(e);

        }
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {

            const Int32 WM_SETCURSOR = 32;
            const Int32 WM_WINDOWPOSCHANGING = 70;
            const Int32 WM_COMMAND = 273;
            const int WM_CTLCOLORLISTBOX = 308;
            const Int32 CB_ADDSTRING = 323;
            const Int32 CB_GETCURSEL = 327;
            const Int32 WM_LBUTTONUP = 514;
            const Int32 OCM_COMMAND = 8465;

            Int32 yPos = 0;

            //INSTANT C# NOTE: The following VB 'Select Case' included range-type or non-constant 'Case' expressions and was converted to C# 'if-else' logic:
            //			Select Case true
            //ORIGINAL LINE: Case m.Msg = WM_WINDOWPOSCHANGING //.DropDownClose (more effective catching of )
            if (true == (m.Msg == WM_WINDOWPOSCHANGING)) //.DropDownClose (more effective catching of )
            {



            }
            //ORIGINAL LINE: Case m.Msg = WM_CTLCOLORLISTBOX Or m.Msg = WM_SETCURSOR //.ItemHover (more effective catching of )
            else if (true == (m.Msg == WM_CTLCOLORLISTBOX | m.Msg == WM_SETCURSOR)) //.ItemHover (more effective catching of )
            {
                yPos = this.PointToClient(System.Windows.Forms.Cursor.Position).Y;
                if (this.DropDownStyle == ComboBoxStyle.Simple)
                {
                    yPos -= (this.ItemHeight + 10);
                }
                else
                {
                    yPos -= (this.Size.Height + 1);
                }
                mHoverIndex = yPos / this.ItemHeight;

                if (mHoverIndex >= 0) //FIX for not raising event on mouse over text
                {
                    mHoverIndex = Convert.ToInt32(mHoverIndex);
                    if (mHoverIndex > -1 & mHoverIndex < this.Items.Count & this.DroppedDown)
                    {
                        if (ComboBoxItemFocus != null)
                            ComboBoxItemFocus(Convert.ToInt32(mHoverIndex));
                    }
                }
                else
                {
                    //NOTE should really raisevent with -1
                }

            }
            //ORIGINAL LINE: Case m.Msg = CB_ADDSTRING And m.WParam = new IntPtr(0) //.AddItem(s) for stretching ItemChecks array
            else if (true == (m.Msg == CB_ADDSTRING & m.WParam == new IntPtr(0))) //.AddItem(s) for stretching ItemChecks array
            {
                StretchCheckList();



            }
            //ORIGINAL LINE: Case m.Msg = WM_COMMAND And m.WParam = new System.IntPtr(66536) //intercepts WM_COMMAND (which prevent DropDownBox from closing after WM_LBUTTONDOWN)
            else if (true == (m.Msg == WM_COMMAND & m.WParam == new System.IntPtr(66536))) //intercepts WM_COMMAND (which prevent DropDownBox from closing after WM_LBUTTONDOWN)
            {
                //.(.ToString)

                //0. normal behaviour when no checkboxes
                if (!this.mCheckBoxes)
                {
                    base.WndProc(ref m);
                    return;
                }

                //cancels SIC when user try to select a group
                if (this.DividerFormat.Length > 0 && Convert.ToString(this.Items[Convert.ToInt32(mHoverIndex)]).Contains(this.mDividerFormat))
                {
                    //Me.SelectedIndex = mLastSelectedIndex 'NOTE restorre last valid selection
                    //Exit Sub
                    base.WndProc(ref m);
                    return;
                }

                //1. reconstructs (cancelled) child events from WM_COMMAND MSG
                SendMessage(this.Handle, OCM_COMMAND, 591814, new IntPtr(1705748)); //1 event reconstruuction
                SendMessage(this.Handle, OCM_COMMAND, 67526, new IntPtr(1705748)); //2 flagging redraw
                SendMessage(this.Handle, CB_GETCURSEL, 0, new IntPtr(0)); //3 event reconstruuction
                SendMessage(this.Handle, WM_LBUTTONUP, 0, new IntPtr(721012)); //4 flagging redraw

                //2. toggle checkbox
                if (SelectedIndex > -1)
                {
                    this.SetItemsChecks(SelectedIndex, !(this.GetItemsChecks(SelectedIndex)));
                }

                // Me.ItemsChecks(mHoverIndex) = Not Me.ItemsChecks(mHoverIndex)

                //3. throw 'hover' \ Msg308 Or Msg32  so item is repainted
                if (this.SelectedIndex == 0)
                {
                    yPos = this.Font.Height;
                }
                else
                {
                    yPos = this.Font.Height * -1;
                }
                System.Windows.Forms.Cursor.Position = new Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y + yPos);
                Application.DoEvents();
                System.Windows.Forms.Cursor.Position = new Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y - yPos);

                this.Text = GetCommaText(); //NEW CSV test for checkboxes

                //4. cancels event
                return;
            }

            base.WndProc(ref m);

        }


        //private
        private void StretchCheckList()
        {

            Array.Resize(ref mItemsChecks, this.Items.Count);

        }
        private void mTimer1_Tick(object sender, System.EventArgs e)
        {

            mTimer1.Enabled = false;
            mRedrawingUnselected = true;
            base.OnDrawItem(new System.Windows.Forms.DrawItemEventArgs(mGraphics, this.Font, mBounds, mIndex, DrawItemState.Default));
            mGraphics = mGraphics_t;
            mBounds = mBounds_t;
            mIndex = mIndex_t;

        }
        private string GetCommaText()
        {

            Int32 i = 0;
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");
            string s = null;
            //string zFirst = null;
            //string zLast = null;
            //Int32 zLastComa = 0;
            //string zconverterror=null;

            if (!this.ShowCheckBoxes)
            {
                return this.Text;
            }

            for (i = 0; i < this.Items.Count; i++)
            {
                if (this.GetItemsChecks(i) == true)
                {
                    sb.Append(this.Items[i]);
                    sb.Append(mItemSeparator);
                }
            }

            s = sb.ToString();
            if (s.Length == 0)
            {
                return this.Text;
            }
            s = s.Substring(0, s.Length - 1);
            //zLastComa = s.LastIndexOf(mItemSeparator1);
            //    if (zLastComa == -1)
            //    {
            //        return this.Text;
            //    }
            //zLast = s.Substring(zLastComa);
            //zFirst = s.Substring(0, zLastComa);
            //zconverterror = Convert.ToString(mItemSeparator2);
            //zconverterror += Convert.ToString(" ");
            //s = zFirst + Convert.ToString(" ");
            //s += zLast.Replace(Convert.ToString(mItemSeparator1), zconverterror);
            return s;

        }
    }


    public class ComboBoxItem
    {
        private string _text = null;
        private object _value = null;
        private Image _image = null;
        private int _offset;
        private Size _size = new Size(15, 15);
        private bool _checked = true;

        public string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                this._text = value;
            }
        }

        public object Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        public Image Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public int ImageOffset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public Size ImageSize
        {
            get { return _size; }
            set { _size = value; }
        }

        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; }
        }

        public override string ToString()
        {
            return this._text;
        }

        public static ComboBoxItem CreateInstance()
        {
            return new ComboBoxItem() { Text = "È«²¿", Value = "{0}" };
        }
    }

} //end of root namespace