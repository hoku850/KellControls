using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KellControls.Node;

namespace KellControls
{
    [DefaultEvent("SelectedIndexChanged")]
    public partial class KellLinkComboBox : UserControl
    {
        string upLevelString = "上一级";

        [Browsable(true)]
        public string UpLevelString
        {
            get { return upLevelString; }
            set { upLevelString = value; }
        }
        string newItemString = "新增项";

        [Browsable(true)]
        public string NewItemString
        {
            get { return newItemString; }
            set { newItemString = value; }
        }
        bool loadover = false;
        KellNode node;
        LinkDirection direction;
        List<ComboBoxTip> subComboBoxs;
        ToolTip tt;
        object selected;
        ComboBox currentCombo;
        //bool manual;
        
        ///// <summary>
        ///// 获取是否为用户鼠标点击
        ///// </summary>
        //[Browsable(false)]
        //public bool Manual
        //{
        //    get { return manual; }
        //}

        [Browsable(false)]
        public ComboBox CurrentCombo
        {
            get { return currentCombo; }
        }
        bool addNewType;

        [Browsable(true)]
        public bool AddNewType
        {
            get { return addNewType; }
            set { addNewType = value; }
        }

        [Browsable(false)]
        public object SelectedItem
        {
            get { return selected; }
        }

        public delegate void UpLevelHandler(object sender, ComboBox e);
        public event UpLevelHandler UpLeveled;
        public event EventHandler SelectedIndexChanged;
        public event MouseEventHandler MouseClicked;

        private void OnUpLevel(ComboBox e)
        {
            if (UpLeveled != null)
                UpLeveled(this, e);
        }

        private void OnSelectedIndexChanged(ComboBox combo, EventArgs e)
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(combo, e);
        }

        private void OnMouseClick(ComboBox combo, MouseEventArgs e)
        {
            if (MouseClicked != null)
                MouseClicked(combo, e);
        }

        public class ComboBoxTip
        {
            public ComboBox Combo;
            public ToolTip Tip;
        }

        public KellLinkComboBox()
        {
            InitializeComponent();
            tt = new ToolTip();
            tt.SetToolTip(comboBox1, " ");
            tt.Popup += new PopupEventHandler(tt_Popup);
            subComboBoxs = new List<ComboBoxTip>();
        }

        public KellLinkComboBox(KellNode node, bool addNewType = false)
        {
            InitializeComponent();
            AddNewType = addNewType;
            tt = new ToolTip();
            tt.SetToolTip(comboBox1, " ");
            tt.Popup += new PopupEventHandler(tt_Popup);
            subComboBoxs = new List<ComboBoxTip>();
            LoadRootNode(node);
        }

        void tt_Popup(object sender, PopupEventArgs e)
        {
            ToolTip tt = (ToolTip)sender;
            tt.ToolTipTitle = e.AssociatedControl.Text;
        }

        private void LoadRootNode(KellNode node)
        {
            this.node = node;
            if (node != null)
            {
                LoadType(node);
                List<KellNode> typeLink = GetFirstChildCycle(node);
                SelectComboBoxItem(comboBox1, typeLink[0]);
                for (int i = 1; i < typeLink.Count; i++)
                {
                    SelectComboBoxItem(subComboBoxs[i].Combo, typeLink[i]);
                }
                for (int i = typeLink.Count; i < subComboBoxs.Count; i++)
                {
                    subComboBoxs[i].Combo.Visible = false;
                }
            }
        }

        public List<KellNode> GetFirstChildCycle(KellNode node)
        {
            List<KellNode> typeLink = new List<KellNode>();
            if (node != null)
            {
                KellNode child = node.FirstNode;
                if (child != null)
                {
                    int i = child.Level + 1;
                    if (!child.IsLeaf)
                    {
                        typeLink.Add(child);

                        CreateNewComboBoxTip(i);

                        GetFirstChildCycle(child);
                    }
                    else
                    {
                        typeLink.Add(child);

                        CreateNewComboBoxTip(i);
                    }
                }
            }
            return typeLink;
        }

        private void SelectComboBoxItem(ComboBox combo, KellNode node)
        {
            for (int i = 0; i < combo.Items.Count; i++)
            {
                if (((KellNode)combo.Items[i]).ID == node.ID)
                {
                    combo.SelectedIndex = i;
                    break;
                }
            }
        }

        public string GetTypeId()
        {
            if (subComboBoxs != null && subComboBoxs.Count > 0)
            {
                return ((KellNode)subComboBoxs[subComboBoxs.Count - 1].Combo.SelectedItem).ID;
            }
            else
            {
                if (comboBox1.SelectedItem != null)
                    return ((KellNode)comboBox1.SelectedItem).ID;
            }
            return "0";
        }

        public enum LinkDirection
        {
            Horizontal,
            Vertical
        }

        public KellNode Node
        {
            get { return node; }
            set
            {
                LoadRootNode(node);
            }
        }

        public LinkDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public bool Loadover
        {
            get { return loadover; }
            set { loadover = value; }
        }

        [Browsable(false)]
        public Type Type
        {
            get
            {
                if (node != null)
                    return node.GetType();
                else
                    return typeof(string);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadover)
            {
                ComboBox combo = (ComboBox)sender;
                if (combo.SelectedItem != null && combo.SelectedItem.ToString() != NewItemString)
                {
                    currentCombo = combo;
                    selected = combo.SelectedItem;
                    //if (addNewType && manual)
                    //{
                    //    manual = false;
                    //}
                    LoadSubType(combo);
                    OnSelectedIndexChanged(combo, e);
                }
            }
        }

        public void UpLevel(ComboBox combo)
        {
            if (combo.Name != "comboBox1")
            {
                int level = subComboBoxs.IndexOfComboBox(combo);
                for (int i = level; i < subComboBoxs.Count; i++)
                {
                    subComboBoxs[i].Combo.Visible = false;
                }
                OnUpLevel(combo);
            }
        }

        public void GetAddParentId()
        {
            if (currentCombo.Name != "comboBox1")
            {
                int level = subComboBoxs.IndexOfComboBox(currentCombo);
                for (int i = level + 1; i < subComboBoxs.Count; i++)
                {
                    subComboBoxs[i].Combo.Visible = false;
                }
            }
            else
            {
                for (int i = 0; i < subComboBoxs.Count; i++)
                {
                    subComboBoxs[i].Combo.Visible = false;
                }
            }
        }

        private void LoadType(KellNode node)
        {
            if (node != null)
            {
                loadover = false;

                this.SuspendLayout();
                comboBox1.Items.Clear();
                subComboBoxs.Clear();
                this.Controls.RemoveByKey("SubCombo");
                List<KellNode> types = node.GetSub();
                foreach (KellNode type in types)
                {
                    comboBox1.Items.Add(type);
                }
                if (comboBox1.Items.Count > 0)
                    comboBox1.SelectedIndex = 0;

                if (addNewType)
                    comboBox1.Items.Add(NewItemString);
                //SetSubType(types);
                this.ResumeLayout(true);

                loadover = true;
            }
        }

        private void SetSubType(List<KellNode> types)
        {
            if (types.Count > 0)
            {
                foreach (KellNode n in types)
                {
                    List<KellNode> subs = n.GetSub();
                    if (subs.Count > 0)
                    {
                        subComboBoxs[n.Level - 1].Combo.Visible = true;
                        foreach (KellNode sub in subs)
                        {
                            subComboBoxs[n.Level - 1].Combo.Items.Add(sub);
                        }
                        if (subComboBoxs[n.Level - 1].Combo.Items.Count > 0)
                            subComboBoxs[n.Level - 1].Combo.SelectedIndex = 0;
                        if (addNewType)
                            subComboBoxs[n.Level - 1].Combo.Items.Add(NewItemString);
                        subComboBoxs[n.Level - 1].Combo.Items.Add(UpLevelString);
                        SetSubType(subs[0].GetSub());
                    }
                }
            }
        }

        private void LoadSubType(ComboBox combo)
        {
            if (combo.SelectedItem == null)
                return;

            if (!(combo.SelectedItem.GetType().Equals(Type)))
            {
                combo.Visible = false;
                int level = subComboBoxs.IndexOfComboBox(combo);
                for (int i = level; i < subComboBoxs.Count; i++)
                {
                    subComboBoxs[i].Combo.Visible = false;
                }
            }
            else
            {
                KellNode parent = (KellNode)combo.SelectedItem;
                int level = parent.Level;
                for (int i = subComboBoxs.Count; i < parent.LevelCount - 2; i++)
                {
                    CreateNewComboBoxTip(i);
                }
                for (int i = level - 1; i < subComboBoxs.Count; i++)
                {
                    if (parent != null)
                    {
                        List<KellNode> subs = parent.GetSub();
                        if (subs.Count > 0)
                        {
                            subComboBoxs[i].Combo.Visible = true;
                            subComboBoxs[i].Combo.Items.Clear();
                            foreach (KellNode sub in subs)
                            {
                                subComboBoxs[i].Combo.Items.Add(sub);
                            }
                            subComboBoxs[i].Combo.Items.Add(UpLevelString);
                            if (addNewType)
                                subComboBoxs[i].Combo.Items.Add(NewItemString);
                            subComboBoxs[i].Combo.Items.Add(UpLevelString);
                            subComboBoxs[i].Combo.SelectedIndex = 0;
                        }
                        else
                        {
                            subComboBoxs[i].Combo.Visible = false;
                        }
                        List<KellNode> subs1 = parent.GetSub();
                        if (subs1.Count > 0)
                            parent = subs1[0];
                        else
                            parent = null;
                    }
                }
            }
        }

        public void AddSubType()
        {
            if (addNewType)
            {
                ComboBox combo = this.CurrentCombo as ComboBox;
                if (combo != null)
                {
                    if (combo.Name != "comboBox1")
                    {
                        int level = subComboBoxs.IndexOfComboBox(combo);
                        if (subComboBoxs.Count < level + 2)
                        {
                            CreateNewComboBoxTip(subComboBoxs.Count - 1);
                        }
                        subComboBoxs[level + 1].Combo.Visible = true;
                        subComboBoxs[level + 1].Combo.Items.Clear();
                        subComboBoxs[level + 1].Combo.Items.Add(UpLevelString);
                        if (addNewType)
                            subComboBoxs[level + 1].Combo.Items.Add(NewItemString);
                        subComboBoxs[level + 1].Combo.SelectedIndex = 0;
                    }
                    else
                    {
                        if (subComboBoxs.Count < 1)
                        {
                            CreateNewComboBoxTip(subComboBoxs.Count - 1);
                        }
                        subComboBoxs[0].Combo.Visible = true;
                        subComboBoxs[0].Combo.Items.Clear();
                        subComboBoxs[0].Combo.Items.Add(UpLevelString);
                        if (addNewType)
                            subComboBoxs[0].Combo.Items.Add(NewItemString);
                        subComboBoxs[0].Combo.SelectedIndex = 0;
                    }
                }
            }
        }

        private void CreateNewComboBoxTip(int i)
        {
            this.SuspendLayout();
            ComboBox cb = new ComboBox();
            cb.Name = "SubCombo";
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
            //cb.MouseClick += new MouseEventHandler(comboBox1_MouseClick);
            if (direction == LinkDirection.Vertical)
            {
                cb.Location = new Point(0, 22 * i);
            }
            else
            {
                cb.Location = new Point(122 * i, 0);
            }
            ToolTip tt = new ToolTip();
            tt.SetToolTip(cb, " ");
            tt.Popup += new PopupEventHandler(tt_Popup);
            ComboBoxTip cbt = new ComboBoxTip();
            cbt.Combo = cb;
            cbt.Tip = tt;
            subComboBoxs.Add(cbt);
            this.Controls.Add(cb);
            this.ResumeLayout(true);
        }

        //private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        //{
        //    ComboBox combo = (ComboBox)sender;
        //    manual = true;
        //    OnMouseClick(combo, e);
        //}
    }

    public static class ComboBoxTipListExtensions
    {
        public static int IndexOfComboBox(this List<KellLinkComboBox.ComboBoxTip> cbt, ComboBox combo)
        {
            for (int index = 0; index < cbt.Count; index++)
            {
                if (cbt[index].Combo == combo)
                    return index;
            }
            return -1;
        }
    }
}
