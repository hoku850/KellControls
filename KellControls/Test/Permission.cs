using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KellControls;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Forms;

namespace Test
{
    [Serializable]
    class Permission : IPermission
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ID"></param>
        public Permission(int ID)
        {
            id = ID;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ID"></param>
        public Permission(int ID, string name)
        {
            id = ID;
            if (name != null && name.Trim() != "")
                this.name = name;
        }

        /// <summary>
        /// 由操作构造权限对象
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="action"></param>
        public Permission(int ID, IAction action)
        {
            id = ID;
            this.action = action;
        }

        /// <summary>
        /// 由名字和操作构造权限对象
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public Permission(int ID, string name, IAction action)
        {
            id = ID;
            if (name != null && name.Trim() != "")
                this.name = name;
            this.action = action;
        }

        int id;

        /// <summary>
        /// 权限ID
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        string name = "未命名权限";

        /// <summary>
        /// 权限的名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (value != null && value.Trim() != "")
                    name = value;
            }
        }

        string description;

        /// <summary>
        /// 权限描述
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        string permissionExtensionName = ".per";

        public string PermissionExtensionName
        {
            get { return permissionExtensionName; }
            set { permissionExtensionName = value; }
        }
        IAction action;

        /// <summary>
        /// 权限的操作
        /// </summary>
        public IAction Action
        {
            get { return action; }
            set { action = value; }
        }

        /// <summary>
        /// 权限名称与权限值的组合字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }

        public bool Save(params string[] filename)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "权限文件|*" + PermissionExtensionName;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(fs, this);
                    }
                    sfd.Dispose();
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    sfd.Dispose();
                    return false;
                }
            }
            sfd.Dispose();
            return false;
        }

        public bool Load(params string[] filename)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "权限文件|*." + PermissionExtensionName;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    IPermission p = null;
                    using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        p = (IPermission)bf.Deserialize(fs);
                    }
                    if (p != null)
                    {
                        this.id = p.ID;
                        this.name = p.Name;
                        this.description = p.Description;
                        this.action = p.Action;
                        ofd.Dispose();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    ofd.Dispose();
                    return false;
                }
            }
            ofd.Dispose();
            return false;
        }
    }
}
