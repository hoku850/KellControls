using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KellControls;

namespace Test
{
    [Serializable]
    class Action : IAction
    {
        int id;

        /// <summary>
        /// 操作ID
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="ID"></param>
        public Action(int ID)
        {
            id = ID;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="module"></param>
        /// <param name="permissions"></param>
        public Action(int ID, IModule module, Dictionary<IPermissionType, bool> permissions)
        {
            id = ID;
            this.module = module;
            this.permissions = permissions;
        }

        /// <summary>
        /// 模块与操作类型的组合字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string pers = "";
            foreach (KeyValuePair<IPermissionType, bool> pt in permissions)
            {
                if (pers == "")
                {
                    if (pt.Value)
                        pers = pt.Key.Name;
                }
                else
                {
                    if (pt.Value)
                        pers += "," + pt.Key.Name;
                }
            }
            return this.module.Name + "[" + pers + "]";
        }

        IModule module;
        Dictionary<IPermissionType, bool> permissions;

        public IModule Module
        {
            get
            {
                return module;
            }
        }

        public Dictionary<IPermissionType, bool> Permissions
        {
            get
            {
                return permissions;
            }
            set
            {
                permissions = value;
            }
        }


        public List<IPermissionType> ValidPermissions
        {
            get
            {
                List<IPermissionType> ps = new List<IPermissionType>();
                foreach (KeyValuePair<IPermissionType, bool> pt in permissions)
                {
                    if (pt.Value)
                        ps.Add(pt.Key);
                }
                return ps;
            }
        }
    }
}
