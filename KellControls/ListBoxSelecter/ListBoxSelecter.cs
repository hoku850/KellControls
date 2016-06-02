using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace KellControls
{
    public partial class ListBoxSelecter : UserControl
    {
        public ListBoxSelecter()
        {
            InitializeComponent();
            
            tt = new ToolTip();
            tt.SetToolTip(comboBox1, " ");
            tt.Popup += new PopupEventHandler(tt_Popup);
            allItems = new List<object>();
        }

        void tt_Popup(object sender, PopupEventArgs e)
        {
            tt.ToolTipTitle = e.AssociatedControl.Text;
        }

        List<object> allItems;
        KellNode node;
        ToolTip tt;
        const int MinWidth = 186;
        const int MinHeight = 210;
        const int MinSplitWidth = 62;

        public bool IsLevelNode
        {
            get { return node != null; }
        }

        [Browsable(true)]
        [Category("数据")]
        [Description("供选定项的集合")]
        public List<object> AllItems
        {
            get
            {
                return allItems;
            }
            set
            {
                allItems = value;
                listBox1.Items.Clear();
                listBox1.Items.AddRange(allItems.ToArray());
                node = null;
                comboBox1.Visible = false;
                button6.Visible = false;
            }
        }

        [Browsable(false)]
        public KellNode Node
        {
            get
            {
                return node;
            }
            set
            {
                node = value;
                if (node != null)
                {
                    comboBox1.Text = node.FullPath;
                    listBox1.Items.Clear();
                    listBox1.Items.AddRange(node.GetSub().ToArray());
                    allItems.Clear();
                    comboBox1.Visible = true;
                    button6.Visible = true;

                    if (node.IsRoot)
                        button6.Enabled = false;
                    else
                        button6.Enabled = true;
                }
            }
        }

        private void Goto()
        {
            if (this.node != null)
            {
                KellNode node = this.node.GetNodeByFullPath(comboBox1.Text);
                if (node != null)
                {
                    listBox1.Items.Clear();
                    listBox1.Items.AddRange(node.GetSub().ToArray());
                    comboBox1.Text = node.FullPath;
                    this.node = node;
                }
                if (node.IsRoot)
                    button6.Enabled = false;
                else
                    button6.Enabled = true;
            }
        }

        private void UpLevel()
        {
            if (node != null && !node.IsRoot)
            {
                comboBox1.Text = node.Parent.FullPath;
                Goto();
            }
        }

        private void DownLevel(KellNode node)
        {
            if (node != null && !node.IsLeaf)
            {
                comboBox1.Text = node.FullPath;
                Goto();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            listBox2.Items.AddRange(listBox1.Items);
            listBox1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            object[] items = new object[listBox2.Items.Count];
            listBox2.Items.CopyTo(items, 0);
            for (int i = 0; i < items.Length; i++)
            {
                object item = items[i];
                CancelSelect(item);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems != null && listBox1.SelectedItems.Count > 0)
            {
                object[] items = new object[listBox1.SelectedItems.Count];
                listBox1.SelectedItems.CopyTo(items, 0);
                for (int i = 0; i < items.Length; i++)
                {
                    object item = items[i];
                    if (!listBox2.Items.Contains(item))
                    {
                        listBox2.Items.Add(item);
                        listBox1.Items.Remove(item);
                    }
                }
            }
                        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItems != null && listBox2.SelectedItems.Count > 0)
            {
                object[] items = new object[listBox2.SelectedItems.Count];
                listBox2.SelectedItems.CopyTo(items, 0);
                for (int i = 0; i < items.Length; i++)
                {
                    object item = items[i];
                    CancelSelect(item);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (button5.Text == "排序")
            {
                listBox1.Sorted = true;
                listBox2.Sorted = true;
                button5.Text = "默认";
            }
            else
            {
                listBox1.Sorted = false;
                listBox2.Sorted = false;
                Reload();
                button5.Text = "排序";
            }
        }

        private void Reload()
        {
            
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SelectItem(listBox1.SelectedItem);
            }
        }

        private void SelectItem(object item)
        {
            if (IsLevelNode)
            {
                KellNode node = (KellNode)item;
                if (node != null)
                {
                    if (node.IsLeaf)
                    {
                        if (!listBox2.Items.Contains(node))
                        {
                            listBox2.Items.Add(node);
                            listBox1.Items.Remove(node);
                        }
                    }
                    else
                    {
                        DownLevel(node);
                    }
                }
            }
            else
            {
                if (!listBox2.Items.Contains(item))
                {
                    listBox2.Items.Add(item);
                    listBox1.Items.Remove(item);
                }
            }
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                CancelSelect(listBox2.SelectedItem);
            }
        }

        private void CancelSelect(object item)
        {
            if (IsLevelNode)
            {
                KellNode node = (KellNode)item;
                if (node != null)
                {
                    if (node.Level == this.node.Level + 1)
                    {
                        if (!listBox1.Items.Contains(node))
                        {
                            listBox1.Items.Add(node);
                            listBox2.Items.Remove(node);
                        }
                    }
                    else
                    {
                        listBox2.Items.Remove(node);
                    }
                }
            }
            else
            {
                if (!listBox1.Items.Contains(item))
                {
                    listBox1.Items.Add(item);
                    listBox2.Items.Remove(item);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            UpLevel();
        }

        private void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Goto();
            }
        }

        private void ListBoxSelecter_Resize(object sender, EventArgs e)
        {
            if (this.Height < MinHeight)
                this.Height = MinHeight;
            if (this.Width < MinWidth)
                this.Width = MinWidth;
            if (this.Width > MinWidth)
            {
                int avgSplit = (this.Width - MinSplitWidth) / 2;
                listBox2.Width = listBox1.Width = avgSplit;
                button1.Left = listBox1.Width + 5;
                button2.Left = listBox1.Width + 5;
                button3.Left = listBox1.Width + 5;
                button4.Left = listBox1.Width + 5;
                button5.Left = listBox1.Width + 5;
                button6.Left = listBox1.Width + 5;
            }
        }
    }
}