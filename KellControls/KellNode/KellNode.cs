using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KellControls
{
    public abstract class KellNode
    {
        string id;

        public string ID
        {
            get { return id; }
            protected set { id = value; }
        }

        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        protected KellNode parent;

        public KellNode Parent
        {
            get { return parent; }
        }

        internal List<KellNode> children = new List<KellNode>();

        public void AddChild(KellNode node)
        {
            children.Add(node);
            node.parent = this;
        }

        public void AddChildren(List<KellNode> nodes)
        {
            foreach (KellNode node in nodes)
            {
                AddChild(node);
            }
        }

        public void RemoveChildByID(string id)
        {
            KellNode node = GetNodeByID(id, false);
            children.Remove(node);
        }

        public void ClearAllChild()
        {
            children.Clear();
        }

        public KellNode FirstNode
        {
            get
            {
                if (children != null && children.Count > 0)
                    return children[0];
                else
                    return null;
            }
        }

        public int CurrentLevelNodeCount
        {
            get
            {
                if (children != null)
                    return children.Count;
                else
                    return 0;
            }
        }

        bool check;

        public bool Checked
        {
            get { return check; }
            set { check = value; }
        }

        /// <summary>
        /// 是否为叶子节点。
        /// </summary>
        public virtual bool IsLeaf
        {
            get { return !HasChildren; }
            //set//利用该属性的set，可以将节点在叶子与非叶子的状态之间随意转换
            //{
            //    if (value == true && HasChildren)
            //    {
            //        throw new Exception("该节点下面有子节点，不能转化为叶子节点！请用ClearAllChild()方法实现该设置效果，同时会清空此节点下所有的子节点。");
            //    }
            //    else
            //    {
            //        isLeaf = value;
            //    }
            //}
        }
        char pathSpliteChar = '\\';

        public char PathSpliteChar
        {
            get { return pathSpliteChar; }
            set { pathSpliteChar = value; }
        }

        bool isRoot;

        /// <summary>
        /// 是否为根节点。利用该属性，可以任意在某一个树节点上“切割”指定的子树，使之成为一棵独立的树
        /// </summary>
        public bool IsRoot
        {
            get { return isRoot; }
            set { isRoot = value; }
        }

        /// <summary>
        /// 当前节点的层级（根节点为0，向下递增）
        /// </summary>
        public int Level
        {
            get
            {
                int level = 0;
                KellNode node = this;
                while (!node.IsRoot)
                {
                    node = node.Parent;
                    level++;
                }
                return level;
            }
        }

        /// <summary>
        /// 当前节点所在树的总层数（深度）
        /// </summary>
        public int LevelCount
        {
            get
            {
                int deepth = 1;
                return GetDeepth(this, ref deepth);
            }
        }

        /// <summary>
        /// 获取指定节点以下有几层（深度）
        /// </summary>
        /// <param name="node"></param>
        /// <param name="deepth"></param>
        /// <returns></returns>
        protected static int GetDeepth(KellNode node, ref int deepth)
        {
            foreach (KellNode nod in node.children)
            {
                while (!nod.IsLeaf)
                {
                    int newDeepth = GetDeepth(nod, ref deepth);
                    if (newDeepth > deepth)
                        deepth = newDeepth;
                }
            }
            return deepth;
        }

        /// <summary>
        /// 获取当前节点的完全路径
        /// </summary>
        public virtual string FullPath
        {
            get
            {
                string fullpath = this.Name;
                KellNode node = this;
                while (!node.IsRoot)
                {
                    node = node.Parent;
                    fullpath = node.Name + pathSpliteChar + fullpath;
                }
                return fullpath;
            }
        }

        /// <summary>
        /// 根据完全路径获取对应的节点(路径不分大小写)
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public virtual KellNode GetNodeByFullPath(string fullpath)
        {
            KellNode node = null;
            if (fullpath.EndsWith(pathSpliteChar.ToString()))
                fullpath = fullpath.Substring(0, fullpath.Length - 1);
            int level = fullpath.Split(new char[1] { pathSpliteChar }).Length;
            int thisLevel = this.FullPath.Split(new char[1] { pathSpliteChar }).Length;
            if (thisLevel == level)
            {//对比当前层
                if (this.FullPath.ToLower() == fullpath.ToLower())
                    node = this;
            }
            else if (thisLevel < level)
            {//往下找
                node = GetDown(this, fullpath);
            }
            else
            {//往上找
                node = GetUp(this, fullpath);
            }
            return node;
        }

        protected static KellNode GetDown(KellNode node, string path)
        {
            KellNode getit = null;
            if (node != null)
            {
                foreach (KellNode nod in node.children)
                {
                    string currpath = nod.FullPath.ToLower();
                    if (currpath == path.ToLower())
                        return nod;
                    if (path.ToLower().StartsWith(currpath) && !nod.IsLeaf)
                    {
                        getit = GetDown(nod, path);
                    }
                }
            }
            return getit;
        }

        protected static KellNode GetUp(KellNode node, string path)
        {
            KellNode getit = null;
            if (node != null)
            {
                KellNode parent = node.Parent;
                if (parent != null)
                {
                    string currpath = parent.FullPath.ToLower();
                    if (currpath == path.ToLower())
                        return parent;
                    if (currpath.StartsWith(path.ToLower()) && !parent.IsRoot)
                    {
                        getit = GetUp(parent, path);
                    }
                }
            }
            return getit;
        }

        public KellNode()
        {
            id = Guid.NewGuid().ToString("N");
        }

        public KellNode(string ID)
        {
            id = ID;
        }

        public void SetRoot()
        {
            isRoot = true;
        }

        public bool HasChildren
        {
            get
            {
                return children.Count > 0;
            }
        }

        object tag;

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public override string ToString()
        {
            return Name;
        }

        public abstract List<KellNode> GetSub();

        public virtual List<KellNode> GetAllCheckedNodes()
        {
            List<KellNode> checkedNodes = new List<KellNode>();
            Generic<KellNode>.GetCheckedTreeNode(this, checkedNodes);
            return checkedNodes;
        }

        public virtual KellNode GetNodeByID(string id, bool ignoreCase)
        {
            KellNode kellNode = null;
            foreach (KellNode node in this.children)
            {
                if (ignoreCase)
                {
                    if (node.ID.ToLower() == id.ToLower())
                    {
                        kellNode = node;
                        break;
                    }
                    else
                    {
                        GetNodeByID(id, true);
                    }
                }
                else
                {
                    if (node.ID == id)
                    {
                        kellNode = node;
                        break;
                    }
                    else
                    {
                        GetNodeByID(id, false);
                    }
                }
            }
            return kellNode;
        }

        public class Generic<T> where T : KellNode
        {
            public static void GetCheckedTreeNode(T node, List<T> checkedNodes)
            {
                foreach (T nod in node.children)
                {
                    if (nod.Checked)
                    {
                        checkedNodes.Add(nod);
                    }
                    GetCheckedTreeNode(nod, checkedNodes);
                }
            }

            public static T GetNodeByID(string id, T treenode, bool ignoreCase)
            {
                T kellNode = null;
                foreach (T node in treenode.children)
                {
                    if (ignoreCase)
                    {
                        if (node.ID.ToLower() == id.ToLower())
                        {
                            kellNode = node;
                            break;
                        }
                        else
                        {
                            GetNodeByID(id, node, true);
                        }
                    }
                    else
                    {
                        if (node.ID == id)
                        {
                            kellNode = node;
                            break;
                        }
                        else
                        {
                            GetNodeByID(id, node, false);
                        }
                    }
                }
                return kellNode;
            }
        }
    }

    public class DirectoryNode : KellNode
    {
        string searchPattern = "*.*";
        int dirImageIndex = 0;
        bool emptyDir;

        public override bool IsLeaf
        {
            get
            {
                return !HasChildren && !EmptyDir;
            }
        }

        public bool EmptyDir
        {
            get { return emptyDir; }
            set { emptyDir = value; }
        }

        public int DirImageIndex
        {
            get { return dirImageIndex; }
            set { dirImageIndex = value; }
        }
        int fileImageIndex = 1;

        public int FileImageIndex
        {
            get { return fileImageIndex; }
            set { fileImageIndex = value; }
        }

        public string SearchPattern
        {
            get { return searchPattern; }
            set { searchPattern = value; }
        }

        public DirectoryNode(string fullpath)
            : base()
        {
            this.Tag = fullpath;
            this.children = GetSub();
            CheckEmptyDir();
        }

        public DirectoryNode(string ID, string fullpath)
            : base(ID)
        {
            this.Tag = fullpath;
            this.children = GetSub();
            CheckEmptyDir();
        }

        public DirectoryNode(string ID, string searchPattern, string fullpath)
            : base(ID)
        {
            this.searchPattern = searchPattern;
            this.Tag = fullpath;
            this.children = GetSub();
            CheckEmptyDir();
        }

        private void CheckEmptyDir()
        {
            string fullName = this.Tag.ToString();
            if (Directory.Exists(fullName) && Directory.GetFiles(fullName).Length == 0)
            {
                this.emptyDir = true;
            }
        }

        public override List<KellNode> GetSub()
        {
            List<KellNode> dns = new List<KellNode>();
            if (this.Tag != null)
            {
                string fullName = this.Tag.ToString();
                if (Directory.Exists(fullName))
                {
                    DirectoryInfo di = new DirectoryInfo(fullName);
                    DirectoryInfo[] dis = di.GetDirectories(searchPattern);
                    foreach (DirectoryInfo d in dis)
                    {
                        dns.Add(new DirectoryNode(d.FullName) { Name = d.Name, parent = this });
                    }
                    FileInfo[] files = di.GetFiles(searchPattern);
                    foreach (FileInfo f in files)
                    {
                        dns.Add(new DirectoryNode(f.FullName) { Name = f.Name, parent = this });
                    }
                }
            }
            return dns;
        }

        public new List<DirectoryNode> GetAllCheckedNodes()
        {
            List<DirectoryNode> checkedNodes = new List<DirectoryNode>();
            Generic<DirectoryNode>.GetCheckedTreeNode(this, checkedNodes);
            return checkedNodes;
        }
    }
}

namespace KellControls.Node
{
    public static class Common
    {
        public static TriStateTreeNode KellNode2TriStateTreeNode(KellNode kellNode)
        {
            TriStateTreeNode tri = new TriStateTreeNode() { Name = kellNode.ID, Text = kellNode.Name, Tag = kellNode.Tag };
            return tri;
        }
        public static TriStateTreeNode KellNode2TriStateTreeNode(KellNode kellNode, bool checkboxVisible)
        {
            TriStateTreeNode tri = new TriStateTreeNode() { Name = kellNode.ID, Text = kellNode.Name, Tag = kellNode.Tag, CheckboxVisible = checkboxVisible };
            return tri;
        }
        public static TriStateTreeNode KellNode2TriStateTreeNode(KellNode kellNode, bool checkboxVisible, int selectedImageIndex)
        {
            TriStateTreeNode tri = new TriStateTreeNode() { Name = kellNode.ID, Text = kellNode.Name, Tag = kellNode.Tag, CheckboxVisible = checkboxVisible, SelectedImageIndex = selectedImageIndex };
            return tri;
        }
        public static void SetImageList(TriStateTreeView treeview, System.Windows.Forms.ImageList imageList, int selectedImageIndex = -1, int dirImageIndex = 0, int fileImageIndex = 1, int expandImageIndex = 2, int indexUnchecked = 3, int indexIndeterminate = 4, int indexChecked = 5)
        {
            treeview.ImageList = imageList;
            try
            {
                treeview.SelectedImageIndex = selectedImageIndex;
                treeview.DirImageIndex = dirImageIndex;
                treeview.FileImageIndex = fileImageIndex;
                treeview.ExpandImageIndex = expandImageIndex;
                treeview.UncheckedImageIndex = indexUnchecked;
                treeview.IndeterminateImageIndex = indexIndeterminate;
                treeview.CheckedImageIndex = indexChecked;
                treeview.Refresh();
            }
            catch
            { }
        }
        private static TriStateTreeNode GetNodeByName(string name, TriStateTreeNode treenode, bool ignoreCase)
        {
            TriStateTreeNode triNode = null;
            foreach (TriStateTreeNode node in treenode.Nodes)
            {
                if (ignoreCase)
                {
                    if (node.Name.ToLower() == name.ToLower())
                    {
                        triNode = node;
                        break;
                    }
                    else
                    {
                        GetNodeByName(name, node, true);
                    }
                }
                else
                {
                    if (node.Name == name)
                    {
                        triNode = node;
                        break;
                    }
                    else
                    {
                        GetNodeByName(name, node, false);
                    }
                }
            }
            return triNode;
        }
        public static TriStateTreeNode GetNodeByName(string name, TriStateTreeView treeview)
        {
            TriStateTreeNode findNode = null;
            foreach (TriStateTreeNode node in treeview.Nodes)
            {
                findNode = GetNodeByName(name, node, false);
                if (findNode != null)
                    break;
            }
            return findNode;
        }
        public static TriStateTreeNode GetNodeByName(string name, TriStateTreeView treeview, bool ignoreCase)
        {
            TriStateTreeNode findNode = null;
            foreach (TriStateTreeNode node in treeview.Nodes)
            {
                findNode = GetNodeByName(name, node, ignoreCase);
                if (findNode != null)
                    break;
            }
            return findNode;
        }
        public static KellNode GetNodeByID(string id, KellNode treenode, bool ignoreCase)
        {
            KellNode kellNode = null;
            foreach (KellNode node in treenode.children)
            {
                if (ignoreCase)
                {
                    if (node.ID.ToLower() == id.ToLower())
                    {
                        kellNode = node;
                        break;
                    }
                    else
                    {
                        GetNodeByID(id, node, true);
                    }
                }
                else
                {
                    if (node.ID == id)
                    {
                        kellNode = node;
                        break;
                    }
                    else
                    {
                        GetNodeByID(id, node, false);
                    }
                }
            }
            return kellNode;
        }
        public static void LoadDirectory(DirectoryNode dir, TriStateTreeNode treenode, bool cycle, bool checkboxVisible = true)
        {
            if (cycle)
            {
                List<KellNode> sub = LoadDirectoryCycle(dir);
                LoadTreeNodes(dir, treenode, sub, cycle);
            }
            else
            {
                List<KellNode> nodes = dir.GetSub();
                LoadTreeNodes(dir, treenode, nodes, cycle);
            }
        }
        public static void LoadTreeNodes(DirectoryNode dir, TriStateTreeNode treenode, List<KellNode> nodes, bool cycle, bool checkboxVisible = true)
        {
            foreach (DirectoryNode node in nodes)
            {
                if (cycle)
                    treenode.Nodes.Add(new TriStateTreeNode(node.Name, Common.GetSubTriStateNodes(node, checkboxVisible, cycle).ToArray()) { Name = node.ID, Tag = node.Tag, ImageIndex = node.IsLeaf ? dir.FileImageIndex : dir.DirImageIndex, CheckboxVisible = checkboxVisible });
                else
                    treenode.Nodes.Add(new TriStateTreeNode(node.Name) { Name = node.ID, Tag = node.Tag, ImageIndex = node.IsLeaf ? dir.FileImageIndex : dir.DirImageIndex, CheckboxVisible = checkboxVisible });
            }
        }
        public static List<TriStateTreeNode> GetSubTriStateNodes(DirectoryNode dir, bool checkboxVisible, bool cycle)
        {
            List<TriStateTreeNode> tris = new List<TriStateTreeNode>();
            List<KellNode> nodes = dir.GetSub();
            foreach (DirectoryNode node in nodes)
            {
                if (cycle)
                {
                    TriStateTreeNode tri = new TriStateTreeNode(node.Name, GetSubTriStateNodes(node, checkboxVisible, cycle).ToArray()) { Name = node.ID, Tag = node.Tag, ImageIndex = node.IsLeaf ? dir.FileImageIndex : dir.DirImageIndex, CheckboxVisible = checkboxVisible };
                    tris.Add(tri);
                }
                else
                {
                    TriStateTreeNode tri = new TriStateTreeNode(node.Name) { Name = node.ID, Tag = node.Tag, ImageIndex = node.IsLeaf ? dir.FileImageIndex : dir.DirImageIndex, CheckboxVisible = checkboxVisible };
                    tris.Add(tri);
                }
            }
            return tris;
        }
        public static void LoadDirectory(DirectoryNode dir, System.Windows.Forms.TreeView tree, bool cycle)
        {
            tree.Nodes.Clear();
            System.Windows.Forms.TreeNode treenode = new System.Windows.Forms.TreeNode(dir.Name);
            if (cycle)
            {
                List<KellNode> sub = LoadDirectoryCycle(dir);
                LoadTreeNodes(dir, treenode, sub, cycle);
            }
            else
            {
                List<KellNode> nodes = dir.GetSub();
                LoadTreeNodes(dir, treenode, nodes, cycle);
            }
            tree.Nodes.Add(treenode);
        }
        public static void LoadTreeNodes(DirectoryNode dir, System.Windows.Forms.TreeNode treenode, List<KellNode> nodes, bool cycle, bool checkboxVisible = true)
        {
            foreach (DirectoryNode node in nodes)
            {
                if (cycle)
                    treenode.Nodes.Add(new System.Windows.Forms.TreeNode(node.Name, Common.GetSubTriStateNodes(node, checkboxVisible, cycle).ToArray()) { Name = node.ID, Tag = node.Tag });
                else
                    treenode.Nodes.Add(new System.Windows.Forms.TreeNode(node.Name) { Name = node.ID, Tag = node.Tag });
            }
        }
        public static List<System.Windows.Forms.TreeNode> GetSubTreeNodes(DirectoryNode dir, bool cycle)
        {
            List<System.Windows.Forms.TreeNode> tris = new List<System.Windows.Forms.TreeNode>();
            List<KellNode> nodes = dir.GetSub();
            foreach (DirectoryNode node in nodes)
            {
                if (cycle)
                {
                    System.Windows.Forms.TreeNode tri = new System.Windows.Forms.TreeNode(node.Name, GetSubTreeNodes(node, cycle).ToArray()) { Name = node.ID, Tag = node.Tag };
                    tris.Add(tri);
                }
                else
                {
                    System.Windows.Forms.TreeNode tri = new System.Windows.Forms.TreeNode(node.Name) { Name = node.ID, Tag = node.Tag };
                    tris.Add(tri);
                }
            }
            return tris;
        }
        public static void LoadDirectory(DirectoryNode dir, KellTreeControl tree, bool cycle)
        {
            tree.RootNodes.Clear();
            KellTreeNode treenode = tree.RootNodes.Add();
            treenode.Text = dir.Name;
            if (cycle)
            {
                List<KellNode> sub = LoadDirectoryCycle(dir);
                LoadTreeNodes(dir, treenode, sub, cycle);
            }
            else
            {
                List<KellNode> nodes = dir.GetSub();
                LoadTreeNodes(dir, treenode, nodes, cycle);
            }
        }
        public static void LoadTreeNodes(DirectoryNode dir, KellTreeNode treenode, List<KellNode> nodes, bool cycle)
        {
            foreach (DirectoryNode node in nodes)
            {
                KellTreeNode kellNode = treenode.ChildNodes.Add();
                kellNode.Name = node.ID;
                kellNode.Text = node.Name;
                kellNode.Tag = node.Tag;
                if (cycle)
                    GetSubKellTreeNodes(node, kellNode, cycle);
            }
        }
        public static void GetSubKellTreeNodes(DirectoryNode dir, KellTreeNode kellNode, bool cycle)
        {
            List<KellNode> nodes = dir.GetSub();
            foreach (DirectoryNode node in nodes)
            {
                KellTreeNode ktn = kellNode.ChildNodes.Add();
                ktn.Name = node.ID;
                ktn.Text = node.Name;
                ktn.Tag = node.Tag;
                if (cycle)
                    GetSubKellTreeNodes(node, ktn, cycle);
            }
        }
        public static List<KellNode> LoadDirectoryCycle(DirectoryNode dir)
        {
            List<KellNode> dns = new List<KellNode>();
            List<KellNode> nodes = dir.GetSub();
            foreach (DirectoryNode node in nodes)
            {
                List<KellNode> nods = LoadDirectoryCycle(node);
                foreach (DirectoryNode nod in nods)
                {
                    node.AddChild(nod);
                }
                dns.Add(node);
            }
            return dns;
        }
    }

    public class Generic<T> where T : TriStateTreeNode
    {
        public static List<T> GetAllCheckedNodes(TriStateTreeView tree)
        {
            List<T> checkedNodes = new List<T>();
            foreach (T node in tree.Nodes)
            {
                GetCheckedTreeNode(node, checkedNodes);
            }
            return checkedNodes;
        }
        private static void GetCheckedTreeNode(T node, List<T> checkedNodes)
        {
            foreach (T nod in node.Nodes)
            {
                if (nod.Checked)
                {
                    checkedNodes.Add(nod);
                }
                GetCheckedTreeNode(nod, checkedNodes);
            }
        }
    }
}