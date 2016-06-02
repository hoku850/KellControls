using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace KellControls
{
    public partial class KellPermissionEditor : UserControl
    {
        public class PermissionArgs
        {
            int id;

            public int PermissionID
            {
                get { return id; }
                internal set { id = value; }
            }
            KeyValuePair<IPermissionType, bool> permission;

            public KeyValuePair<IPermissionType, bool> PermissionItem
            {
                get { return permission; }
                internal set { permission = value; }
            }
        }

        public delegate void PermissionChangedHandler(object sender, PermissionArgs e);

        public event PermissionChangedHandler PermissionChanged;
        private void OnPermissionChanged(PermissionArgs e)
        {
            changed = true;
            if (PermissionChanged != null)
                PermissionChanged(this, e);
        }
        public KellPermissionEditor()
        {
            InitializeComponent();
        }

        int itemWidth = 100;
        [DefaultValue(100)]
        [Browsable(true)]
        public int ItemWidth
        {
            get
            {
                return itemWidth;
            }
            set
            {
                itemWidth = value;
            }
        }

        int nameWidth = 140;
        [DefaultValue(140)]
        [Browsable(true)]
        public int NameWidth
        {
            get
            {
                return nameWidth;
            }
            set
            {
                nameWidth = value;
            }
        }

        IPermission permission;
        private bool changed;

        /// <summary>
        /// 获取当前权限是否被改动
        /// </summary>
        public bool Changed
        {
            get { return changed; }
        }
        /// <summary>
        /// 获取或设置当前的权限
        /// </summary>
        public IPermission Permission
        {
            get { return permission; }
            set
            {
                permission = value;
                RefreshPermission();
            }
        }

        /// <summary>
        /// 获取当前权限所在的模块
        /// </summary>
        public IModule Module
        {
            get
            {
                if (permission != null)
                {
                    if (permission.Action != null)
                        return permission.Action.Module;
                }
                return null;
            }
        }

        /// <summary>
        /// 获取当前权限
        /// </summary>
        public Dictionary<IPermissionType, bool> Permissions
        {
            get
            {
                if (permission != null)
                    return permission.Action.Permissions;
                else
                    return new Dictionary<IPermissionType, bool>();
            }
        }

        private void RefreshPermission()
        {
            if (permission == null)
                return;

            this.Controls.Clear();
            this.SuspendLayout();
            CheckBox selectAll = new CheckBox();
            selectAll.Name = "SelectAll";
            selectAll.Text = "全选";
            selectAll.Width = 60;
            selectAll.Tag = permission;
            selectAll.CheckedChanged += new EventHandler(selectAll_CheckedChanged);
            this.Controls.Add(selectAll);
            Label lbl = new Label();
            lbl.Name = "LabelP";
            if (!string.IsNullOrEmpty(permission.Name))
                lbl.Text = permission.Name + "." + permission.Action.Module.Name + "：";
            else
                lbl.Text = permission.Action.Module.Name + "：";
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            lbl.Width = NameWidth;
            lbl.Left = 60;
            this.Controls.Add(lbl);
            int x = lbl.Right;
            foreach (KeyValuePair<IPermissionType, bool> de in permission.Action.Permissions)
            {
                CheckBox cb = new CheckBox();
                cb.Name = "CheckBoxP";
                cb.Text = de.Key.Name;
                cb.Checked = de.Value;
                cb.Location = new Point(x, 0);
                cb.Width = ItemWidth;
                cb.TextAlign = ContentAlignment.MiddleLeft;
                cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
                cb.Tag = de.Key;
                this.Controls.Add(cb);
                x += cb.Width;
            }
            this.ResumeLayout(true);
        }

        void selectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox all = (CheckBox)sender;
            Control[] cs = this.Controls.Find("CheckBoxP", false);
            if (cs.Length > 0)
            {
                foreach (Control c in cs)
                {
                    CheckBox cb = (CheckBox)c;
                    cb.Checked = all.Checked;
                }
            }
        }

        void cb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            Control[] cs = this.Controls.Find("SelectAll", false);
            if (cs.Length > 0)
            {
                IPermission per = (IPermission)cs[0].Tag;
                if (per != null)
                {
                    IPermissionType pt =(IPermissionType)cb.Tag;
                    permission.Action.Permissions[pt] = cb.Checked;
                    foreach (KeyValuePair<IPermissionType, bool> kv in permission.Action.Permissions)
                    {
                        if (kv.Key == pt)
                        {
                            OnPermissionChanged(new PermissionArgs() { PermissionID = per.ID, PermissionItem = kv });
                            break;
                        }
                    }
                }
            }
        }

        public bool SavePermission()
        {
            bool flag = false;
            flag = permission.Save();
            if (flag)
                changed = false;
            return flag;
        }

        public bool LoadPermission()
        {
            bool flag = permission.Load();
            if (flag)
            {
                RefreshPermission();
                changed = false;
            }
            return flag;
        }
    }
}
