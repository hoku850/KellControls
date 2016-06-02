using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Xml;
using System.Windows.Forms.VisualStyles;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing.Imaging;

namespace KellControls
{
    using KellControls.WinFormsGloss.Controls;
    using System.IO;
    public sealed class ManifestResources
    {

        private string _baseNamespace;
        private System.Reflection.Assembly _assembly;

        public ManifestResources(string baseNamespace)
            : this(baseNamespace, System.Reflection.Assembly.GetCallingAssembly())
        {
        }

        public string[] ResourceNames
        {
            get
            {
                return _assembly.GetManifestResourceNames();
            }
        }

        public ManifestResources(string baseNamespace, System.Reflection.Assembly assembly)
        {
            if (baseNamespace == null)
            {
                throw new ArgumentNullException("baseNamespace");
            }
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            _baseNamespace = baseNamespace;
            _assembly = assembly;
        }

        public System.Xml.XmlDocument GetXmlDocument(string path)
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

            using (System.IO.Stream stream = GetStream(path))
            {
                if (stream == null)
                {
                    throw new ArgumentException(string.Format("Resource '{0}' not found.", path), "path");
                }
                xmlDoc.Load(stream);
            }

            return xmlDoc;
        }

        public string GetString(string path)
        {
            using (System.IO.Stream stream = GetStream(path))
            using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }

        public System.IO.Stream GetStream(string path)
        {
            return _assembly.GetManifestResourceStream(_baseNamespace + "." + path);
        }

        public System.Drawing.Icon GetIcon(string path)
        {
            using (System.IO.Stream stream = GetStream(path))
            {
                return new System.Drawing.Icon(stream);
            }
        }

        public System.Drawing.Image GetImage(string path)
        {
            using (System.IO.Stream stream = GetStream(path))
            {
                return System.Drawing.Image.FromStream(stream);
            }
        }
    }
    public class KellTreeControl : TreeControl
    {
        private Drawing.ColorTable _colorTable;
        private ContextMenuStrip _oldContextMenuStrip;
        internal bool _contextMenuOpen;
        int _dirImageIndex = 0;

        public int DirImageIndex
        {
            get
            {
                if (base.Icons == null)
                    return -1;
                if (this._dirImageIndex >= this.Icons.Count)
                    return Math.Max(0, this.Icons.Count - 1);
                return _dirImageIndex;
            }
            set
            {
                if (value == -1)
                    value = 0;
                if (value < 0)
                    throw new ArgumentException(string.Format("Index out of bounds! ({0}) index must be equal to or greater then {1}.", value.ToString(), "0"));
                if (this._dirImageIndex != value)
                {
                    _dirImageIndex = value;
                    if (base.IsHandleCreated)
                        base.RecreateHandle();
                }
            }
        }
        int _fileImageIndex = 1;

        public int FileImageIndex
        {
            get
            {
                if (base.Icons == null)
                    return -1;
                if (this._fileImageIndex >= this.Icons.Count)
                    return Math.Max(0, this.Icons.Count - 1);
                return _fileImageIndex;
            }
            set
            {
                if (value == -1)
                    value = 0;
                if (value < 0)
                    throw new ArgumentException(string.Format("Index out of bounds! ({0}) index must be equal to or greater then {1}.", value.ToString(), "0"));
                if (this._fileImageIndex != value)
                {
                    _fileImageIndex = value;
                    if (base.IsHandleCreated)
                        base.RecreateHandle();
                }
            }
        }
        int _expandImageIndex = 2;

        public int ExpandImageIndex
        {
            get
            {
                if (base.Icons == null)
                    return -1;
                if (this._expandImageIndex >= this.Icons.Count)
                    return Math.Max(0, this.Icons.Count - 1);
                return _expandImageIndex;
            }
            set
            {
                if (value == -1)
                    value = 0;
                if (value < 0)
                    throw new ArgumentException(string.Format("Index out of bounds! ({0}) index must be equal to or greater then {1}.", value.ToString(), "0"));
                if (this._expandImageIndex != value)
                {
                    _expandImageIndex = value;
                    if (base.IsHandleCreated)
                        base.RecreateHandle();
                }
            }
        }
        int _collapseImageIndex = 0;

        public int CollapseImageIndex
        {
            get
            {
                if (base.Icons == null)
                    return -1;
                if (this._collapseImageIndex >= this.Icons.Count)
                    return Math.Max(0, this.Icons.Count - 1);
                return _collapseImageIndex;
            }
            set
            {
                if (value == -1)
                    value = 0;
                if (value < 0)
                    throw new ArgumentException(string.Format("Index out of bounds! ({0}) index must be equal to or greater then {1}.", value.ToString(), "0"));
                if (this._collapseImageIndex != value)
                {
                    _collapseImageIndex = value;
                    if (base.IsHandleCreated)
                        base.RecreateHandle();
                }
            }
        }

        public KellTreeControl()
        {
            ColorTable = new Drawing.WindowsThemeColorTable();
            this.Icons.Clear();
            this.Icons.Add(KellControls.Properties.Resources.CollapsedFolderTreeItem16);
            this.Icons.Add(KellControls.Properties.Resources.FeedTreeItem16);
            this.Icons.Add(KellControls.Properties.Resources.ExpandedFolderTreeItem16);
        }

        public Drawing.ColorTable ColorTable
        {
            get
            {
                return _colorTable;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _colorTable = value;

                Renderer = new KellControls.Renderers.GlowRenderer(this);
            }
        }

        protected override void OnAfterSelect(TreeNodeEventArgs e)
        {
            base.OnAfterSelect(e);

            if (e.Node.Tag != null && File.Exists(e.Node.Tag.ToString()))
            {
                e.Node.SelectedImageIndex = FileImageIndex;
            }
            else
            {
                if (e.Node.IsExpanded)
                    e.Node.SelectedImageIndex = ExpandImageIndex;
                else
                    e.Node.SelectedImageIndex = CollapseImageIndex;
            }
        }

        protected override void OnAfterExpand(TreeNodeEventArgs e)
        {
            base.OnAfterExpand(e);

            //e.Node.ImageIndex = ExpandImageIndex;
        }

        protected override void OnAfterCollapse(TreeNodeEventArgs e)
        {
            base.OnAfterCollapse(e);

            //e.Node.ImageIndex = CollapseImageIndex;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            SuperToolTipManager.CloseToolTip();

            base.OnMouseDown(e);
        }

        protected override void OnContextMenuStripChanged(EventArgs e)
        {
            base.OnContextMenuStripChanged(e);

            if (_oldContextMenuStrip != null)
            {
                _oldContextMenuStrip.Opened -= new EventHandler(ContextMenuStrip_Opened);
                _oldContextMenuStrip.Closed -= new ToolStripDropDownClosedEventHandler(ContextMenuStrip_Closed);
            }

            if (ContextMenuStrip != null)
            {
                ContextMenuStrip.Opened += new EventHandler(ContextMenuStrip_Opened);
                ContextMenuStrip.Closed += new ToolStripDropDownClosedEventHandler(ContextMenuStrip_Closed);
            }

            _oldContextMenuStrip = ContextMenuStrip;
        }

        private void ContextMenuStrip_Opened(object sender, EventArgs e)
        {
            WinFormsUtility.Events.MenuLoop.NotifyEnterMenuLoop();

            _contextMenuOpen = true;
        }

        private void ContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            WinFormsUtility.Events.MenuLoop.NotifyExitMenuLoop();

            _contextMenuOpen = false;
        }
    }

    #region TreeNodeEvent

    public class TreeNodeEventArgs : EventArgs
    {
        public TreeNodeEventArgs(KellTreeNode treeNode)
        {
            if (treeNode == null)
            {
                throw new ArgumentNullException("treeNode");
            }

            _treeNode = treeNode;
        }

        public KellTreeNode Node
        {
            get
            {
                return _treeNode;
            }
        }

        private KellTreeNode _treeNode;
    }

    public delegate void TreeNodeEventHandler(object sender, TreeNodeEventArgs e);

    #endregion

    #region TreeNodeMouseEvent

    public class TreeNodeMouseEventArgs : TreeNodeEventArgs
    {
        public TreeNodeMouseEventArgs(KellTreeNode treeNode, MouseButtons buttons)
            : base(treeNode)
        {
            _buttons = buttons;
        }

        public MouseButtons Buttons
        {
            get
            {
                return _buttons;
            }
        }

        private MouseButtons _buttons;
    }

    public delegate void TreeNodeMouseEventHandler(object sender, TreeNodeMouseEventArgs e);

    #endregion

    #region TreeNodeRectangleEvent

    public class TreeNodeRectangleEventArgs : TreeNodeEventArgs
    {
        public TreeNodeRectangleEventArgs(KellTreeNode treeNode, Rectangle rect)
            : base(treeNode)
        {
            _rect = rect;
        }

        public Rectangle Rectangle
        {
            get
            {
                return _rect;
            }
        }

        private Rectangle _rect;
    }

    public delegate void TreeNodeRectangleEventHandler(object sender, TreeNodeRectangleEventArgs e);

    #endregion

    public interface IRenderer
    {
        void Setup();
        void Setdown();

        void PreRender(ITreeInfo treeInfo, ITreeEvents treeEvents);
        int MeasureIndent(Graphics g, ITreeInfo treeControl, KellTreeNode node);
        Size MeasureTreeNode(Graphics g, ITreeInfo treeInfo, KellTreeNode treeNode, bool needsWidth, bool needsHeight);

        void RenderBackground(Graphics g, Rectangle clip);
        void RenderTreeNode(Graphics g, ITreeInfo treeInfo, KellTreeNode treeNode, Rectangle nodeRectangle, Rectangle clip);
        void RenderTreeNodeRow(Graphics g, KellTreeNode treeNode, Rectangle nodeRectangle, Rectangle rowRectangle);

        void ProcessClick(Graphics g, KellTreeNode treeNode, Rectangle nodeRectangle, Point p, ITreeInfo treeInfo, ITreeEvents treeEvents);
        void ProcessDoubleClick(Graphics g, KellTreeNode treeNode, Rectangle nodeRectangle, Point p, ITreeInfo treeInfo, ITreeEvents treeEvents);
    }

    public sealed class KellTreeNode
    {
        private KellTreeNodeCollection _parentCollection;
        private ITreeInfo _treeInfo;
        private ITreeEvents _treeEvents;
        private string _text = string.Empty;
        private KellTreeNodeCollection _childNodes;
        private object _tag;
        private int? _index;
        private int? _depth;
        private int _expandedImageIndex = -1, _collapsedImageIndex = -1;
        private Font _font = SystemFonts.DialogFont;
        private string _name;
        private int selectedImageIndex = -1;
        private int imageIndex = 0;

        public int ImageIndex
        {
            [DebuggerStepThrough]
            get
            {
                return imageIndex;
            }
            set
            {
                imageIndex = value;

                _treeEvents.NodeUpdated(this);
            }
        }

        public int SelectedImageIndex
        {
            [DebuggerStepThrough]
            get
            {
                return selectedImageIndex;
            }
            set
            {
                selectedImageIndex = value;

                _treeEvents.NodeUpdated(this);
            }
        }

        public string Name
        {
            get { return _name; }
            internal set { _name = value; }
        }

        internal KellTreeNode(ITreeEvents treeEvents, ITreeInfo treeInfo)
        {
            _name = Guid.NewGuid().ToString("N");
            _treeEvents = treeEvents;
            _treeInfo = treeInfo;
            _childNodes = new KellTreeNodeCollection(this, treeEvents, treeInfo);
        }

        public string Text
        {
            [DebuggerStepThrough]
            get
            {
                return _text;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (_text == value)
                {
                    return;
                }

                _text = value;

                _treeEvents.NodeUpdated(this);
            }
        }

        public object Tag
        {
            [DebuggerStepThrough]
            get
            {
                return _tag;
            }
            [DebuggerStepThrough]
            set
            {
                _tag = value;

                _treeEvents.NodeUpdated(this);
            }
        }

        public int ExpandedImageIndex
        {
            [DebuggerStepThrough]
            get
            {
                return _expandedImageIndex;
            }
            set
            {
                _expandedImageIndex = value;

                _treeEvents.NodeUpdated(this);
            }
        }

        public int CollapsedImageIndex
        {
            [DebuggerStepThrough]
            get
            {
                return _collapsedImageIndex;
            }
            set
            {
                _collapsedImageIndex = value;

                _treeEvents.NodeUpdated(this);
            }
        }

        public KellTreeNodeCollection ChildNodes
        {
            [DebuggerStepThrough]
            get
            {
                return _childNodes;
            }
        }

        public KellTreeNodeCollection ParentCollection
        {
            [DebuggerStepThrough]
            get
            {
                return _parentCollection;
            }
            [DebuggerStepThrough]
            internal set
            {
                _parentCollection = value;
            }
        }

        public int Index
        {
            get
            {
                if (_index == null)
                {
                    _index = _parentCollection.IndexOf(this);
                }

                return _index.Value;
            }
        }

        public int Depth
        {
            get
            {
                if (_depth == null)
                {
                    int d = 0;

                    KellTreeNode tn = this.ParentCollection.ParentNode;

                    while (tn != null)
                    {
                        ++d;
                        tn = tn.ParentCollection.ParentNode;
                    }

                    _depth = d;
                }

                return _depth.Value;
            }
        }

        public bool IsExpanded
        {
            get
            {
                return _treeInfo.IsExpanded(this);
            }
        }

        public Icon Icon
        {
            get
            {
                if (IsExpanded)
                {
                    if (_expandedImageIndex == -1)
                    {
                        return null;
                    }
                    else
                    {
                        return _treeInfo.Icons[_expandedImageIndex];
                    }
                }
                else
                {
                    if (_collapsedImageIndex == -1)
                    {
                        return null;
                    }
                    else
                    {
                        return _treeInfo.Icons[_collapsedImageIndex];
                    }
                }
            }
        }

        public Font Font
        {
            get
            {
                return _font;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (_font.Equals(value))
                {
                    return;
                }

                _font = value;
                _treeEvents.NodeUpdated(this);
            }
        }

        internal void DirtyIndex()
        {
            _index = null;
            _depth = null;
        }

    }

    public interface ITreeEvents
    {
        void NodeUpdated(KellTreeNode treeNode);
        void NodeInserted(KellTreeNode treeNode);
        void NodeDeleted(KellTreeNode treeNode);

        void ToggleNodeExpansion(KellTreeNode treeNode);
        void SelectNode(KellTreeNode treeNode);
        void Expand(KellTreeNode treeNode);
        void Collapse(KellTreeNode treeNode);

        void UpdateAnimations();
    }

    public interface ITreeInfo
    {
        IDisposable SuspendUpdates();
        Graphics CreateGraphics();
        bool IsTreeFocused();
        bool IsUpdatesSuspended();
        bool IsMouseOverTree();
        List<Icon> Icons
        {
            get;
        }
        Size ViewportSize
        {
            get;
        }

        void BeginAnimating();
        void EndAnimating();
        void BeginAnimating(KellTreeNode treeNode, Rectangle subRect);
        void EndAnimating(KellTreeNode treeNode);

        bool IsSelected(KellTreeNode treeNode);
        bool IsExpanded(KellTreeNode treeNode);
        double ExpansionAnimationPosition(KellTreeNode treeNode);

        bool IsAnimating();

        void GetMouseOver(out KellTreeNode treeNode, out Point nodeRelative);
        KellTreeNode[] GetVisibleNodes();
        Size GetNodeSize(KellTreeNode treeNode);
    }

    public sealed class KellTreeNodeCollection : IEnumerable<KellTreeNode>
    {
        private KellTreeNode _parentNode;
        private ITreeEvents _treeEvents;
        private ITreeInfo _treeInfo;
        private List<KellTreeNode> _nodes = new List<KellTreeNode>();

        internal KellTreeNodeCollection(KellTreeNode parentNode, ITreeEvents treeEvents, ITreeInfo treeInfo)
        {
            _parentNode = parentNode;
            _treeEvents = treeEvents;
            _treeInfo = treeInfo;
        }

        public int Count
        {
            [DebuggerStepThrough]
            get
            {
                return _nodes.Count;
            }
        }

        public KellTreeNode this[int index]
        {
            [DebuggerStepThrough]
            get
            {
                return _nodes[index];
            }
        }

        public KellTreeNode ParentNode
        {
            [DebuggerStepThrough]
            get
            {
                return _parentNode;
            }
        }

        public KellTreeNode Add()
        {
            KellTreeNode treeNode = new KellTreeNode(_treeEvents, _treeInfo);

            treeNode.ParentCollection = this;
            _nodes.Add(treeNode);

            foreach (KellTreeNode node in _nodes)
            {
                node.DirtyIndex();
            }

            _treeEvents.NodeInserted(treeNode);

            return treeNode;
        }

        public KellTreeNode Insert(int index)
        {
            if (index > _nodes.Count)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            KellTreeNode treeNode = new KellTreeNode(_treeEvents, _treeInfo);

            treeNode.ParentCollection = this;
            _nodes.Insert(index, treeNode);

            foreach (KellTreeNode node in _nodes)
            {
                node.DirtyIndex();
            }

            _treeEvents.NodeInserted(treeNode);

            return treeNode;
        }

        public void Move(KellTreeNode node, int newIndex)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (newIndex > _nodes.Count)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            using (_treeInfo.SuspendUpdates())
            {
                _nodes.Remove(node);

                _treeEvents.NodeDeleted(node);

                if (newIndex > _nodes.Count)
                {
                    _nodes.Add(node);
                }
                else
                {
                    _nodes.Insert(newIndex, node);
                }

                foreach (KellTreeNode child in _nodes)
                {
                    child.DirtyIndex();
                }

                _treeEvents.NodeInserted(node);
            }
        }

        public void Remove(KellTreeNode treeNode)
        {
            if (treeNode == null)
            {
                throw new ArgumentNullException("treeNode");
            }
            if (!_nodes.Contains(treeNode))
            {
                throw new ArgumentException("Node is not a member of this collection.", "treeNode");
            }

            foreach (KellTreeNode node in _nodes)
            {
                node.DirtyIndex();
            }

            _nodes.Remove(treeNode);
            _treeEvents.NodeDeleted(treeNode);
            treeNode.ParentCollection = null;
        }

        public void Clear()
        {
            using (_treeInfo.SuspendUpdates())
            {
                List<KellTreeNode> nodes = new List<KellTreeNode>(_nodes);

                foreach (KellTreeNode child in nodes)
                {
                    Remove(child);
                }
            }
        }

        internal int IndexOf(KellTreeNode treeNode)
        {
            return _nodes.IndexOf(treeNode);
        }

        #region IEnumerable<KellTreeNode> Members

        IEnumerator<KellTreeNode> IEnumerable<KellTreeNode>.GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        #endregion
    }
}

namespace KellControls.Renderers
{
    public class StandardRenderer : IRenderer
    {
        #region IRenderer Members

        public void Setup()
        {
        }

        public void Setdown()
        {
        }

        public void PreRender(ITreeInfo treeInfo, ITreeEvents treeEvents)
        {
        }

        public void RenderBackground(Graphics g, Rectangle clip)
        {
            g.Clear(SystemColors.Window);
        }

        public int MeasureIndent(Graphics g, ITreeInfo treeInfo, KellTreeNode node)
        {
            return node.Depth * _indent;
        }

        public Size MeasureTreeNode(Graphics g, ITreeInfo treeInfo, KellTreeNode treeNode, bool needsWidth, bool needsHeight)
        {
            Size textSize = WinFormsUtility.Drawing.GdiPlusEx.MeasureString(g, treeNode.Text, treeNode.Font, int.MaxValue);
            Size ecSize = GetGlyphSize(g, treeNode.IsExpanded);

            int width = (int)textSize.Width + _leftSep + _ecSep + ecSize.Width + _rightSep;
            int height = (int)textSize.Height;

            if (treeNode.Icon != null)
            {
                width += _imageSep + treeNode.Icon.Size.Width;
                height = Math.Max(height, treeNode.Icon.Size.Height);
            }

            height += _verticalSep;

            return new Size(width, height);
        }

        public void RenderTreeNode(Graphics g, ITreeInfo treeInfo, KellTreeNode treeNode, Rectangle nodeRectangle, Rectangle clip)
        {
            bool isLast = (treeNode.Index == treeNode.ParentCollection.Count - 1);
            Size ecSize = GetGlyphSize(g, treeNode.IsExpanded);
            Point ecCenter = new Point(nodeRectangle.X + _leftSep + ecSize.Width / 2, nodeRectangle.Y + (nodeRectangle.Height - ecSize.Height) / 2 + ecSize.Height / 2);

            using (Brush brush = new HatchBrush(HatchStyle.Percent50, SystemColors.Window, SystemColors.GrayText))
            using (Pen pen = new Pen(brush))
            {
                g.DrawLine(pen, ecCenter, new Point(ecCenter.X + 12, ecCenter.Y));
                g.DrawLine(pen, ecCenter, new Point(ecCenter.X, nodeRectangle.Y));

                if (!isLast)
                {
                    g.DrawLine(pen, ecCenter, new Point(ecCenter.X, nodeRectangle.Bottom));
                }
            }

            int textX = nodeRectangle.X + ecSize.Width + _leftSep + _ecSep;

            if (treeNode.Icon != null)
            {
                Icon image = treeNode.Icon;

                g.DrawIconUnstretched(image, new Rectangle(textX, nodeRectangle.Y + (nodeRectangle.Height - image.Height) / 2, image.Width, image.Height));

                textX += image.Width + _imageSep;
            }

            if (treeInfo.IsSelected(treeNode))
            {
                Brush brush = treeInfo.IsTreeFocused() ? SystemBrushes.Highlight : SystemBrushes.Control;

                g.FillRectangle(brush, textX, nodeRectangle.Y, nodeRectangle.Right - textX + 2, nodeRectangle.Height - 1);

                if (treeInfo.IsTreeFocused())
                {
                    using (Brush hatchBrush = new HatchBrush(HatchStyle.Percent50, SystemColors.Highlight))
                    using (Pen pen = new Pen(hatchBrush))
                    {
                        g.DrawRectangle(pen, textX, nodeRectangle.Y, nodeRectangle.Right - textX + 2, nodeRectangle.Height - 1);
                    }
                }
            }

            WinFormsUtility.Drawing.GdiPlusEx.DrawString
                (g, treeNode.Text, treeNode.Font, SystemColors.ControlText
                , new Rectangle(textX + 2, nodeRectangle.Y + _verticalSep, int.MaxValue, int.MaxValue)
                , WinFormsUtility.Drawing.GdiPlusEx.TextSplitting.SingleLineEllipsis, WinFormsUtility.Drawing.GdiPlusEx.Ampersands.Display);

            if (treeNode.ChildNodes.Count > 0)
            {
                DrawGlyph(g, new Point(nodeRectangle.X + _leftSep, nodeRectangle.Y + (nodeRectangle.Height - ecSize.Height) / 2), treeNode.IsExpanded);
            }
        }

        public void RenderTreeNodeRow(Graphics g, KellTreeNode treeNode, Rectangle nodeRectangle, Rectangle rowRectangle)
        {
            Size ecSize = GetGlyphSize(g, treeNode.IsExpanded);
            KellTreeNode parent = treeNode.ParentCollection.ParentNode;

            using (Brush brush = new HatchBrush(HatchStyle.Percent50, SystemColors.Window, SystemColors.GrayText))
            using (Pen pen = new Pen(brush))
            {
                while (parent != null)
                {
                    bool isLast = (parent.Index == parent.ParentCollection.Count - 1);

                    if (!isLast)
                    {
                        g.DrawLine
                            (pen
                            , new Point(parent.Depth * _indent + _leftSep + ecSize.Width / 2, rowRectangle.Y)
                            , new Point(parent.Depth * _indent + _leftSep + ecSize.Width / 2, rowRectangle.Bottom));
                    }

                    parent = parent.ParentCollection.ParentNode;
                }
            }
        }

        public void ProcessClick(Graphics g, KellTreeNode treeNode, Rectangle nodeRectangle, Point p, ITreeInfo treeInfo, ITreeEvents treeEvents)
        {
            Size ecSize = GetGlyphSize(g, treeNode.IsExpanded);
            Rectangle ecBounds = new Rectangle(nodeRectangle.X + _leftSep, nodeRectangle.Y + (nodeRectangle.Height - ecSize.Height) / 2, ecSize.Width, ecSize.Height);

            if (ecBounds.Contains(p) && treeNode.ChildNodes.Count > 0)
            {
                if (!treeInfo.IsAnimating())
                {
                    treeEvents.ToggleNodeExpansion(treeNode);
                }
            }
            else
            {
                treeEvents.SelectNode(treeNode);
            }
        }

        public void ProcessDoubleClick(Graphics g, KellTreeNode treeNode, Rectangle nodeRectangle, Point p, ITreeInfo treeInfo, ITreeEvents treeEvents)
        {
            Size ecSize = GetGlyphSize(g, treeNode.IsExpanded);
            Rectangle ecBounds = new Rectangle(nodeRectangle.X + _leftSep, nodeRectangle.Y + (nodeRectangle.Height - ecSize.Height) / 2, ecSize.Width, ecSize.Height);

            if (!(ecBounds.Contains(p) && treeNode.ChildNodes.Count > 0))
            {
                if (treeNode.ChildNodes.Count > 0)
                {
                    treeEvents.ToggleNodeExpansion(treeNode);
                }
                else
                {
                    treeEvents.SelectNode(treeNode);
                }
            }
        }

        #endregion

        private Size GetGlyphSize(Graphics g, bool expanded)
        {
            if (VisualStyleRenderer.IsSupported)
            {
                VisualStyleElement vse = expanded ? VisualStyleElement.TreeView.Glyph.Opened : VisualStyleElement.TreeView.Glyph.Closed;
                VisualStyleRenderer vsr = new VisualStyleRenderer(vse);
                Size ecSize = vsr.GetPartSize(g, ThemeSizeType.Draw);

                return ecSize;
            }
            else
            {
                return new Size(9, 9);
            }
        }

        private void DrawGlyph(Graphics g, Point p, bool expanded)
        {
            if (VisualStyleRenderer.IsSupported)
            {
                VisualStyleElement vse = expanded ? VisualStyleElement.TreeView.Glyph.Opened : VisualStyleElement.TreeView.Glyph.Closed;
                VisualStyleRenderer vsr = new VisualStyleRenderer(vse);
                Size ecSize = vsr.GetPartSize(g, ThemeSizeType.Draw);

                vsr.DrawBackground(g, new Rectangle(p.X, p.Y, ecSize.Width, ecSize.Height));
            }
            else
            {
                g.FillRectangle(SystemBrushes.Window, new Rectangle(p.X, p.Y, 8, 8));

                using (Pen pen = new Pen(Color.FromArgb(128, 128, 128)))
                {
                    g.DrawRectangle(pen, new Rectangle(p.X, p.Y, 8, 8));
                }

                using (Pen pen = new Pen(Color.Black))
                {
                    g.DrawLine(pen, new Point(p.X + 2, p.Y + 4), new Point(p.X + 6, p.Y + 4));

                    if (!expanded)
                    {
                        g.DrawLine(pen, new Point(p.X + 4, p.Y + 2), new Point(p.X + 4, p.Y + 6));
                    }
                }
            }
        }

        private const int _indent = 16;
        private const int _verticalSep = 1;
        private const int _leftSep = 4;
        private const int _rightSep = 4;
        private const int _ecSep = 11;
        private const int _imageSep = 2;
    }

    public class GlowRenderer : IRenderer
    {
        public GlowRenderer(KellTreeControl treeControl)
        {
            if (treeControl == null)
            {
                throw new ArgumentNullException("treeControl");
            }

            _treeControl = treeControl;
            _colorTable = _treeControl.ColorTable;
        }

        #region IRenderer Members

        public void Setup()
        {
            if (_updateTimer == null)
            {
                _updateTimer = new Timer();
                _updateTimer.Interval = 50;
                _updateTimer.Enabled = true;

                _updateTimer.Tick += new EventHandler(_updateTimer_Tick);
            }
        }

        public void Setdown()
        {
            if (_updateTimer != null)
            {
                _updateTimer.Tick -= new EventHandler(_updateTimer_Tick);
                _updateTimer.Enabled = false;
                _updateTimer.Dispose();
                _updateTimer = null;
            }
        }

        public void PreRender(ITreeInfo treeInfo, ITreeEvents treeEvents)
        {
            _treeInfo = treeInfo;
            _treeEvents = treeEvents;
        }

        public void RenderBackground(Graphics g, Rectangle clip)
        {
            g.Clear(SystemColors.Window);
        }

        public int MeasureIndent(Graphics g, ITreeInfo treeInfo, KellTreeNode node)
        {
            return node.Depth * _indent;
        }

        public Size MeasureTreeNode(Graphics g, ITreeInfo treeInfo, KellTreeNode treeNode, bool needsWidth, bool needsHeight)
        {
            string text = treeNode.Text.Replace("&", "&&");

            Size textSize = WinFormsUtility.Drawing.GdiPlusEx.MeasureString(g, text, treeNode.Font, int.MaxValue);
            Size ecSize;

            if (VisualStyleRenderer.IsSupported)
            {
                VisualStyleElement vse = VisualStyleElement.TreeView.Glyph.Opened;
                VisualStyleRenderer vsr = new VisualStyleRenderer(vse);

                ecSize = vsr.GetPartSize(g, ThemeSizeType.Draw);
            }
            else
            {
                ecSize = new Size(9, 9);
            }

            int width = (int)textSize.Width + _leftSep + _ecSep + ecSize.Width + _rightSep + 16;
            int height = (int)textSize.Height;

            if (treeNode.Icon != null)
            {
                width += _imageSep + treeNode.Icon.Size.Width;
                height = Math.Max(height, treeNode.Icon.Size.Height);
            }

            height += _verticalSep;

            return new Size(width, height);
        }

        public void RenderTreeNode(Graphics g, ITreeInfo treeInfo, KellTreeNode treeNode, Rectangle nodeRectangle, Rectangle clip)
        {
            UnsetAnimateGlow(treeNode);
            UnsetAnimateMark(treeNode);

            bool isLast = (treeNode.Index == treeNode.ParentCollection.Count - 1);
            Point ecCenter = new Point(nodeRectangle.X + _leftSep + _ecSize / 2, nodeRectangle.Y + (nodeRectangle.Height - _ecSize) / 2 + _ecSize / 2);

            int textX = nodeRectangle.X + _ecSize + _leftSep + _ecSep;

            bool isSelected = treeInfo.IsSelected(treeNode);

            Rectangle bgRect = new Rectangle(nodeRectangle.X + _ecSize + _ecSep, nodeRectangle.Y, nodeRectangle.Width - _ecSize - 8, nodeRectangle.Height - 1);
            KellTreeNode mouseOver;
            Point nodePosition;

            treeInfo.GetMouseOver(out mouseOver, out nodePosition);

            _nodeUpdates.DoneUpdate(treeNode);

            double nodeGlow = GetNodeFade(treeNode);

            if (isSelected || nodeGlow > 0)
            {
                RenderNode(g, treeInfo, treeNode, bgRect, isSelected, nodeGlow);
            }

            if (treeNode.Icon != null)
            {
                Image image = GetImage(treeNode.Icon);
                Rectangle imageRect = new Rectangle(textX, nodeRectangle.Y + 1, image.Width, image.Height);

                if (clip.IntersectsWith(imageRect))
                {
                    g.DrawImageUnscaled(image, imageRect);
                }

                textX += image.Width + _imageSep;
            }

            Color textColor;
            Rectangle textRect = new Rectangle(textX + 2, nodeRectangle.Y + _verticalSep, nodeRectangle.Right - textX - 5, nodeRectangle.Height);

            if (isSelected)
            {
                if (_treeControl.Focused)
                {
                    textColor = _colorTable.GlowTextColor;
                }
                else
                {
                    textColor = _colorTable.TextColor;
                }
            }
            else
            {
                textColor = SystemColors.ControlText;
            }

            if (clip.IntersectsWith(textRect))
            {
                WinFormsUtility.Drawing.GdiPlusEx.DrawString
                    (g, treeNode.Text, treeNode.Font, textColor
                    , textRect
                    , WinFormsUtility.Drawing.GdiPlusEx.TextSplitting.SingleLineEllipsis, WinFormsUtility.Drawing.GdiPlusEx.Ampersands.Display);
            }

            Rectangle ecRect = new Rectangle(nodeRectangle.X, nodeRectangle.Y, 10, 10);

            if (clip.IntersectsWith(ecRect))
            {
                if (treeNode.ChildNodes.Count > 0)
                {
                    RenderExpansionMark(g, treeInfo, treeNode, nodeRectangle);
                }
            }
        }

        public void RenderTreeNodeRow(Graphics g, KellTreeNode treeNode, Rectangle nodeRectangle, Rectangle rowRectangle)
        {
        }

        public void ProcessClick(Graphics g, KellTreeNode treeNode, Rectangle nodeRectangle, Point p, ITreeInfo treeInfo, ITreeEvents treeEvents)
        {
            if (IsOverExpandCollapseMark(treeNode, nodeRectangle, p))
            {
                if (!treeInfo.IsAnimating())
                {
                    treeEvents.ToggleNodeExpansion(treeNode);
                }
            }
            else
            {
                treeEvents.SelectNode(treeNode);
            }
        }

        public void ProcessDoubleClick(Graphics g, KellTreeNode treeNode, Rectangle nodeRectangle, Point p, ITreeInfo treeInfo, ITreeEvents treeEvents)
        {
            if (!IsOverExpandCollapseMark(treeNode, nodeRectangle, p))
            {
                if (treeNode.ChildNodes.Count > 0)
                {
                    treeEvents.ToggleNodeExpansion(treeNode);
                }
                else
                {
                    treeEvents.SelectNode(treeNode);
                }
            }
        }

        #endregion

        private Image GetImage(Icon icon)
        {
            Image image;

            if (!_mapIconToImage.TryGetValue(icon, out image))
            {
                image = icon.ToBitmap();
                _mapIconToImage.Add(icon, image);
            }

            return image;
        }

        private VectorGraphics.Primitives.Container CreateNodeItem(VectorGraphics.Renderers.Renderer renderer, Rectangle nodeRect, bool isSelected, double glow)
        {
            VectorGraphics.Paint.Color glowHighlightColor = VectorGraphics.Renderers.GdiPlusUtility.Convert.Color(_colorTable.GlowHighlightColor);
            VectorGraphics.Paint.Color glossyGlowLightenerColor = VectorGraphics.Renderers.GdiPlusUtility.Convert.Color(_colorTable.GlossyGlowLightenerColor);
            VectorGraphics.Paint.Color glowColor = VectorGraphics.Renderers.GdiPlusUtility.Convert.Color(_colorTable.GlowColor);
            VectorGraphics.Paint.Color white = VectorGraphics.Paint.Color.White;

            VectorGraphics.Factories.RoundedRectangle roundedRectangleFactory = new VectorGraphics.Factories.RoundedRectangle();
            VectorGraphics.Factories.GlossyBrush glossyBrushFactory = new VectorGraphics.Factories.GlossyBrush(glossyGlowLightenerColor);

            VectorGraphics.Primitives.Container container = new VectorGraphics.Primitives.Container();

            VectorGraphics.Types.Rectangle rect = VectorGraphics.Renderers.GdiPlusUtility.Convert.Rectangle(nodeRect);

            VectorGraphics.Primitives.Path roundedRect = roundedRectangleFactory.Create(rect, 3);

            container.AddBack(roundedRect);

            if (isSelected)
            {
                VectorGraphics.Paint.Color borderColor = glowColor;
                VectorGraphics.Paint.Color glowStartColor = glowColor;
                VectorGraphics.Paint.Color glowEndColor = glowHighlightColor;

                if (!_treeControl.Focused)
                {
                    borderColor = VectorGraphics.Renderers.GdiPlusUtility.Convert.Color(_colorTable.GrayForegroundColor);
                    glowStartColor = VectorGraphics.Renderers.GdiPlusUtility.Convert.Color(_colorTable.GrayForegroundColor);
                    glowEndColor = VectorGraphics.Renderers.GdiPlusUtility.Convert.Color(_colorTable.GrayBackgroundColor);
                }

                roundedRect.Pen = new VectorGraphics.Paint.Pens.SolidPen(borderColor, 1);
                roundedRect.Brush = glossyBrushFactory.Create(glowStartColor, glowEndColor, nodeRect.Top, nodeRect.Bottom);

                rect = VectorGraphics.Types.Rectangle.Shrink(rect, 1);

                container.AddBack(CreateRoundRectHighlight(rect, 2));
            }
            else if (glow > 0)
            {
                VectorGraphics.Paint.Color borderColor = glowHighlightColor;
                VectorGraphics.Paint.Color glowStartColor = glowColor;
                VectorGraphics.Paint.Color glowEndColor = glowHighlightColor;

                borderColor = VectorGraphics.Paint.Color.Combine(borderColor, white, 0.7);
                glowStartColor = VectorGraphics.Paint.Color.Combine(glowStartColor, white, 0.4);
                glowEndColor = VectorGraphics.Paint.Color.Combine(glowEndColor, white, 0.4);

                borderColor = new VectorGraphics.Paint.Color(borderColor, glow);
                glowStartColor = new VectorGraphics.Paint.Color(glowStartColor, glow);
                glowEndColor = new VectorGraphics.Paint.Color(glowEndColor, glow);

                roundedRect.Pen = new VectorGraphics.Paint.Pens.SolidPen(borderColor, 1);
                roundedRect.Brush = glossyBrushFactory.Create(glowStartColor, glowEndColor, nodeRect.Top, nodeRect.Bottom);
            }

            return container;
        }

        private VectorGraphics.Primitives.Path CreateRoundRectHighlight(VectorGraphics.Types.Rectangle rect, double radius)
        {
            VectorGraphics.Primitives.Path path = new VectorGraphics.Primitives.Path();
            VectorGraphics.Paint.Color glossyGlowLightenerColor = VectorGraphics.Renderers.GdiPlusUtility.Convert.Color(_colorTable.GlossyGlowLightenerColor);

            path.Add(new VectorGraphics.Primitives.Path.Move(new VectorGraphics.Types.Point(rect.X, rect.Y + rect.Height - radius)));
            path.Add(new VectorGraphics.Primitives.Path.Line(new VectorGraphics.Types.Point(rect.X, rect.Y + radius)));
            path.Add(new VectorGraphics.Primitives.Path.EllipticalArc(radius, radius, 0, false, true, new VectorGraphics.Types.Point(rect.X + radius, rect.Y)));
            path.Add(new VectorGraphics.Primitives.Path.Line(new VectorGraphics.Types.Point(rect.X + rect.Width - radius, rect.Y)));

            path.Pen = new VectorGraphics.Paint.Pens.SolidPen(new VectorGraphics.Paint.Color(glossyGlowLightenerColor, 0.6), 1);

            return path;
        }

        private VectorGraphics.Primitives.Container CreateExpandCollapseItem(VectorGraphics.Renderers.Renderer renderer, double borderGlow, double glow, bool over)
        {
            VectorGraphics.Primitives.Container container = new VectorGraphics.Primitives.Container();

            VectorGraphics.Primitives.Path arrow = new VectorGraphics.Primitives.Path(new VectorGraphics.Primitives.Path.Command[]
					{
						new VectorGraphics.Primitives.Path.Move( new VectorGraphics.Types.Point( -_ecSize / 4, -_ecSize / 2 ) ),
						new VectorGraphics.Primitives.Path.Line( new VectorGraphics.Types.Point( _ecSize / 3, 0 ) ),
						new VectorGraphics.Primitives.Path.Line( new VectorGraphics.Types.Point( -_ecSize / 4, _ecSize / 2 ) ),
						new VectorGraphics.Primitives.Path.Close()
					});

            VectorGraphics.Paint.Color greyColor = VectorGraphics.Renderers.GdiPlusUtility.Convert.Color(_colorTable.GrayTextColor);
            VectorGraphics.Paint.Color glowDeepColor = VectorGraphics.Renderers.GdiPlusUtility.Convert.Color(_colorTable.GlowDeepColor);
            VectorGraphics.Paint.Color glowColor = VectorGraphics.Renderers.GdiPlusUtility.Convert.Color(_colorTable.GlowColor);
            VectorGraphics.Paint.Color bgColor = VectorGraphics.Renderers.GdiPlusUtility.Convert.Color(SystemColors.Window);

            glowColor = VectorGraphics.Paint.Color.Combine(glowColor, bgColor, 0.7);

            VectorGraphics.Paint.Color borderColor = VectorGraphics.Paint.Color.Combine(glowDeepColor, greyColor, glow);

            arrow.Pen = new VectorGraphics.Paint.Pens.SolidPen(new VectorGraphics.Paint.Color(borderColor, borderGlow), 1);

            container.AddBack(arrow);

            if (glow > 0)
            {
                arrow.Brush = new VectorGraphics.Paint.Brushes.SolidBrush(new VectorGraphics.Paint.Color(glowColor, glow));

                VectorGraphics.Factories.SoftShadow shadow = new VectorGraphics.Factories.SoftShadow
                    (renderer, new KellControls.VectorGraphics.Types.Point(0, 0), 3
                    , new VectorGraphics.Paint.Color(glowColor, glow));

                shadow.Apply(container);
            }

            return container;
        }

        private VectorGraphics.Renderers.GdiPlusRenderer CreateRenderer(Graphics g)
        {
            return new KellControls.VectorGraphics.Renderers.GdiPlusRenderer(delegate
            {
                return g;
            }, VectorGraphics.Renderers.GdiPlusRenderer.MarkerHandling.Ignore, 1);
        }

        private void RenderRowBackground(Graphics g, KellTreeNode treeNode, Rectangle bounds)
        {
            VectorGraphics.Primitives.Container visualItem = CreateRowVisualItem(bounds, 1);

            using (VectorGraphics.Renderers.GdiPlusRenderer renderer = CreateRenderer(g))
            {
                renderer.Render(g, visualItem, VectorGraphics.Renderers.GdiPlusUtility.Convert.Rectangle(bounds));
            }
        }

        private VectorGraphics.Primitives.Container CreateRowVisualItem(Rectangle bounds, double glow)
        {
            VectorGraphics.Types.Rectangle rect = VectorGraphics.Renderers.GdiPlusUtility.Convert.Rectangle(bounds);
            VectorGraphics.Factories.RoundedRectangle roundedRectangleFactory = new VectorGraphics.Factories.RoundedRectangle();

            VectorGraphics.Primitives.Container container = new VectorGraphics.Primitives.Container();

            VectorGraphics.Primitives.Path roundedRect = roundedRectangleFactory.Create(rect, 3);

            container.AddBack(roundedRect);

            return container;
        }

        private bool IsOverExpandCollapseMark(KellTreeNode treeNode, Rectangle nodeRectangle, Point p)
        {
            if (treeNode.ChildNodes.Count == 0)
            {
                return false;
            }

            Rectangle ecBounds = new Rectangle(nodeRectangle.X + _leftSep, nodeRectangle.Y + (nodeRectangle.Height - _ecSize) / 2, _ecSize, _ecSize);

            return ecBounds.Contains(p) && treeNode.ChildNodes.Count > 0;
        }

        private void RenderNode(Graphics g, ITreeInfo treeInfo, KellTreeNode treeNode, Rectangle nodeBounds, bool isSelected, double glow)
        {
            using (WinFormsUtility.Drawing.GdiPlusEx.SaveState(g))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;

                using (VectorGraphics.Renderers.GdiPlusRenderer renderer = CreateRenderer(g))
                {
                    VectorGraphics.Primitives.Container visualItem = CreateNodeItem(renderer, nodeBounds, isSelected, glow);

                    renderer.Render(g, visualItem);
                }
            }
        }

        private void RenderExpansionMark(Graphics g, ITreeInfo treeInfo, KellTreeNode treeNode, Rectangle nodeBounds)
        {
            double p = treeInfo.ExpansionAnimationPosition(treeNode);
            Point ecCenter = new Point(nodeBounds.X + _leftSep + _ecSize / 2 + 1, nodeBounds.Y + (nodeBounds.Height - _ecSize) / 2 + _ecSize / 2 - 1);

            Debug.Assert(p >= 0 && p <= 1);

            using (WinFormsUtility.Drawing.GdiPlusEx.SaveState(g))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;

                g.TranslateTransform(ecCenter.X, ecCenter.Y);
                g.RotateTransform((float)(45 * p));

                KellTreeNode mouseOver;
                Point nodePosition;
                bool over = false;

                _treeInfo.GetMouseOver(out mouseOver, out nodePosition);

                _markUpdates.DoneUpdate(treeNode);

                double borderGlow = GetOverallFade();
                double glow = GetMarkFade(treeNode);

                if (borderGlow > 0 || glow > 0)
                {
                    using (VectorGraphics.Renderers.GdiPlusRenderer renderer = CreateRenderer(g))
                    {
                        VectorGraphics.Primitives.Container visualItem = CreateExpandCollapseItem(renderer, borderGlow, glow, over);

                        renderer.Render(g, visualItem);
                    }
                }
            }
        }

        private void _updateTimer_Tick(object sender, EventArgs e)
        {
            if (_treeInfo == null)
            {
                return;
            }

            //
            // Overall tree animation.

            if ((_treeInfo.IsMouseOverTree() && _treeHistory.MouseOver == null)
                || (!_treeInfo.IsMouseOverTree() && _treeHistory.MouseOver != null))
            {
                _treeUpdates.NeedsUpdate(_treeInfo);
            }

            _treeHistory.Update(_treeInfo.IsMouseOverTree() ? _treeInfo : null);

            if (_treeUpdates.Items.Length > 0)
            {
                if (!_animating)
                {
                    foreach (KellTreeNode folderNode in _folderNodes)
                    {
                        _treeInfo.EndAnimating(folderNode);
                    }

                    _folderNodes = GetVisibleFolderNodes();

                    foreach (KellTreeNode folderNode in _folderNodes)
                    {
                        _treeInfo.BeginAnimating(folderNode, new Rectangle(4, 0, 10, 19));
                    }
                    _animating = true;
                }
                _treeUpdates.DoneAll();
            }
            else
            {
                if (_animating)
                {
                    foreach (KellTreeNode folderNode in _folderNodes)
                    {
                        _treeInfo.EndAnimating(folderNode);
                    }
                    _folderNodes.Clear();
                    _animating = false;
                }
            }

            //
            // Expand/collapse marks and node animations.

            KellTreeNode mouseOver;
            KellTreeNode ecMouseOver = null, nodeMouseOver = null;
            Point nodePosition;

            _treeInfo.GetMouseOver(out mouseOver, out nodePosition);

            if (_treeControl._contextMenuOpen || _treeInfo.IsAnimating())
            {
                mouseOver = null;
            }

            if (mouseOver != null)
            {
                if (IsOverExpandCollapseMark(mouseOver, new Rectangle(Point.Empty, MeasureTreeNode(_treeInfo.CreateGraphics(), _treeInfo, mouseOver, true, true)), nodePosition))
                {
                    ecMouseOver = mouseOver;
                }

                nodeMouseOver = mouseOver;
            }

            if (ecMouseOver != _markHistory.MouseOver)
            {
                if (_markHistory.MouseOver != null)
                {
                    _markUpdates.NeedsUpdate(_markHistory.MouseOver);
                }

                _markHistory.Update(ecMouseOver);

                if (_markHistory.MouseOver != null)
                {
                    _markUpdates.NeedsUpdate(_markHistory.MouseOver);
                }
            }
            if (nodeMouseOver != _nodeHistory.MouseOver)
            {
                if (_nodeHistory.MouseOver != null)
                {
                    _nodeUpdates.NeedsUpdate(_nodeHistory.MouseOver);
                }

                _nodeHistory.Update(nodeMouseOver);

                if (_nodeHistory.MouseOver != null)
                {
                    _nodeUpdates.NeedsUpdate(_nodeHistory.MouseOver);
                }
            }

            foreach (KellTreeNode tn in _markUpdates.Items)
            {
                SetAnimateMark(tn);
            }
            foreach (KellTreeNode tn in _nodeUpdates.Items)
            {
                SetAnimateGlow(tn);
            }
        }

        private List<KellTreeNode> GetVisibleFolderNodes()
        {
            List<KellTreeNode> nodes = new List<KellTreeNode>();

            foreach (KellTreeNode node in _treeInfo.GetVisibleNodes())
            {
                if (node.ChildNodes.Count > 0)
                {
                    nodes.Add(node);
                }
            }

            return nodes;
        }

        private void SetAnimateGlow(KellTreeNode treeNode)
        {
            if (!_animatingGlows.Contains(treeNode))
            {
                _animatingGlows.Add(treeNode);
                _treeInfo.BeginAnimating(treeNode, new Rectangle(Point.Empty, _treeInfo.GetNodeSize(treeNode)));
            }
        }

        private void UnsetAnimateGlow(KellTreeNode treeNode)
        {
            if (_animatingGlows.Contains(treeNode))
            {
                _animatingGlows.Remove(treeNode);
                _treeInfo.EndAnimating(treeNode);
            }
        }

        private void SetAnimateMark(KellTreeNode treeNode)
        {
            if (!_animatingMarks.Contains(treeNode))
            {
                _animatingMarks.Add(treeNode);
                _treeInfo.BeginAnimating(treeNode, new Rectangle(_leftSep - 3, 0, 14, 19));
            }
        }

        private void UnsetAnimateMark(KellTreeNode treeNode)
        {
            if (_animatingMarks.Contains(treeNode))
            {
                _animatingMarks.Remove(treeNode);
                _treeInfo.EndAnimating(treeNode);
            }
        }

        private double GetOverallFade()
        {
            double fadeInGlow = 0, fadeOutGlow = 0;

            if (_treeHistory.MouseOver == _treeInfo)
            {
                double fadeInTime = _treeHistory.GetTimeOver(_treeInfo) ?? 0;

                if (fadeInTime < _overallFadeIn)
                {
                    _treeUpdates.NeedsUpdate(_treeInfo);
                    fadeInGlow = fadeInTime / _overallFadeIn;
                }
                else
                {
                    fadeInGlow = 1;
                }
            }

            double fadeOutTime = _treeHistory.GetLastOver(_treeInfo);

            if (fadeOutTime < _overallFadeOut)
            {
                _treeUpdates.NeedsUpdate(_treeInfo);
                fadeOutGlow = 1 - fadeOutTime / _overallFadeOut;
            }

            double f = Math.Max(fadeInGlow, fadeOutGlow);

            f = Math.Min(Math.Max(f, 0), 1);

            return f;
        }

        private double GetMarkFade(KellTreeNode treeNode)
        {
            double fadeInGlow = 0, fadeOutGlow = 0;

            if (_markHistory.MouseOver == treeNode)
            {
                double fadeInTime = _markHistory.GetTimeOver(treeNode) ?? 0;

                if (fadeInTime < _markFadeIn)
                {
                    _markUpdates.NeedsUpdate(treeNode);
                    fadeInGlow = fadeInTime / _markFadeIn;
                }
                else
                {
                    fadeInGlow = 1;
                }
            }

            double fadeOutTime = _markHistory.GetLastOver(treeNode);

            if (fadeOutTime < _markFadeOut)
            {
                _markUpdates.NeedsUpdate(treeNode);
                fadeOutGlow = 1 - fadeOutTime / _markFadeOut;
            }

            double f = Math.Max(fadeInGlow, fadeOutGlow);

            f = Math.Min(Math.Max(f, 0), 1);

            return f;
        }

        private double GetNodeFade(KellTreeNode treeNode)
        {
            double fadeInGlow = 0, fadeOutGlow = 0;

            if (_nodeHistory.MouseOver == treeNode)
            {
                double fadeInTime = _nodeHistory.GetTimeOver(treeNode) ?? 0;

                if (fadeInTime < _nodeFadeIn)
                {
                    _nodeUpdates.NeedsUpdate(treeNode);
                    fadeInGlow = fadeInTime / _nodeFadeIn;
                }
                else
                {
                    fadeInGlow = 1;
                }
            }

            double fadeOutTime = _nodeHistory.GetLastOver(treeNode);

            if (fadeOutTime < _nodeFadeOut)
            {
                _nodeUpdates.NeedsUpdate(treeNode);
                fadeOutGlow = 1 - fadeOutTime / _nodeFadeOut;
            }

            double f = Math.Max(fadeInGlow, fadeOutGlow);

            f = Math.Min(Math.Max(f, 0), 1);

            return f;
        }

        private const int _indent = 16;
        private const int _verticalSep = 3;
        private const int _leftSep = 3;
        private const int _ecSep = 4;
        private const int _imageSep = 2;
        private const int _ecSize = 10;
        private const int _rightSep = 4;

        private const double _overallFadeIn = 0.2;
        private const double _overallFadeOut = 0.7;
        private const double _markFadeIn = 0.2;
        private const double _markFadeOut = 0.6;
        private const double _nodeFadeIn = 0.0;
        private const double _nodeFadeOut = 0.2;

        private KellTreeControl _treeControl;
        private Timer _updateTimer;
        private ITreeInfo _treeInfo;
        private ITreeEvents _treeEvents;
        private Drawing.GlowHistory<ITreeInfo> _treeHistory = new Drawing.GlowHistory<ITreeInfo>(null);
        private Drawing.GlowUpdates<ITreeInfo> _treeUpdates = new Drawing.GlowUpdates<ITreeInfo>();
        private Drawing.GlowHistory<KellTreeNode> _nodeHistory = new Drawing.GlowHistory<KellTreeNode>(null);
        private Drawing.GlowUpdates<KellTreeNode> _nodeUpdates = new Drawing.GlowUpdates<KellTreeNode>();
        private Drawing.GlowHistory<KellTreeNode> _markHistory = new Drawing.GlowHistory<KellTreeNode>(null);
        private Drawing.GlowUpdates<KellTreeNode> _markUpdates = new Drawing.GlowUpdates<KellTreeNode>();
        private bool _animating;
        private Drawing.ColorTable _colorTable;
        private Utility.Collections.Set<KellTreeNode> _animatingGlows = new Utility.Collections.Set<KellTreeNode>();
        private Utility.Collections.Set<KellTreeNode> _animatingMarks = new Utility.Collections.Set<KellTreeNode>();
        private List<KellTreeNode> _folderNodes = new List<KellTreeNode>();
        private Dictionary<Icon, Image> _mapIconToImage = new Dictionary<Icon, Image>();
    }
}

namespace KellControls.Drawing
{
    #region ColorTable

    public abstract class ColorTable
    {
        protected ColorTable()
        {
        }

        public Color[] Colors
        {
            get
            {
                return new Color[]
				{
					PrimaryColor,
					PrimaryBackgroundColor,
					PrimaryBrandingColor,
					GlowColor,
					GlowDeepColor,
					GrayForegroundColor,
					GrayBackgroundColor,
					TextColor,
					GrayTextColor,
					GlowTextColor,
					GrayPrimaryBackgroundColor,
					GlossyLightenerColor,
					GlossyGlowLightenerColor,
					PrimaryBorderColor
				};
            }
        }

        public abstract string Name
        {
            get;
        }

        public abstract string Description
        {
            get;
        }

        /// <summary>
        /// The primary foreground color.
        /// </summary>
        public abstract Color PrimaryColor
        {
            get;
        }

        /// <summary>
        /// The primary background color.
        /// </summary>
        public virtual Color PrimaryBackgroundColor
        {
            get
            {
                return WinFormsUtility.Drawing.ColorUtil.Combine(PrimaryColor, Color.White, 0.3);
            }
        }

        /// <summary>
        /// The color used for glowing items.
        /// </summary>
        public abstract Color GlowColor
        {
            get;
        }

        /// <summary>
        /// The color of the highlight around a glowing item.
        /// </summary>
        public abstract Color GlowHighlightColor
        {
            get;
        }

        /// <summary>
        /// The color of a pressed item.
        /// </summary>
        public abstract Color GlowDeepColor
        {
            get;
        }

        /// <summary>
        /// The inactive foreground color.
        /// </summary>
        public abstract Color GrayForegroundColor
        {
            get;
        }

        /// <summary>
        /// The inactive background color.
        /// </summary>
        public abstract Color GrayBackgroundColor
        {
            get;
        }

        /// <summary>
        /// Normal text color.
        /// </summary>
        public abstract Color TextColor
        {
            get;
        }

        /// <summary>
        /// Inactive text color.
        /// </summary>
        public abstract Color GrayTextColor
        {
            get;
        }

        /// <summary>
        /// A color used for main branding.
        /// </summary>
        public abstract Color PrimaryBrandingColor
        {
            get;
        }

        /// <summary>
        /// Glowing text color.
        /// </summary>
        public virtual Color GlowTextColor
        {
            get
            {
                return TextColor;
            }
        }

        /// <summary>
        /// An inactive background color which has been merged with the primary color slightly.
        /// </summary>
        public virtual Color GrayPrimaryBackgroundColor
        {
            get
            {
                return WinFormsUtility.Drawing.ColorUtil.Combine(PrimaryColor, GrayBackgroundColor, 0.2);
            }
        }

        /// <summary>
        /// The color used to make glossy brushes.
        /// </summary>
        public virtual Color GlossyLightenerColor
        {
            get
            {
                return Color.White;
            }
        }

        /// <summary>
        /// The color used to make glossy brushes, when glowing.
        /// </summary>
        public virtual Color GlossyGlowLightenerColor
        {
            get
            {
                return Color.White;
            }
        }

        /// <summary>
        /// The color used to make borders.
        /// </summary>
        public virtual Color PrimaryBorderColor
        {
            get
            {
                return GlossyLightenerColor;
            }
        }
    }

    #endregion

    #region WindowsThemeColorTable

    public class WindowsThemeColorTable : ColorTable
    {
        public override string Name
        {
            get
            {
                return "Windows Theme";
            }
        }

        public override string Description
        {
            get
            {
                return "Matching your Windows color scheme: blue, silver or olive.";
            }
        }

        public override Color PrimaryColor
        {
            get
            {
                if (_primaryColor == null)
                {
                    Color color;

                    if (VisualStyleRenderer.IsSupported)
                    {
                        VisualStyleRenderer vsr = new VisualStyleRenderer(VisualStyleElement.Window.Caption.Active);

                        color = vsr.GetColor(ColorProperty.FillColorHint);
                    }
                    else
                    {
                        color = SystemColors.Control;
                    }

                    double h = color.GetHue() / 360, s = color.GetSaturation();

                    _primaryColor = WinFormsUtility.Drawing.ColorUtil.FromHSL(h, s, 0.5);
                }
                return _primaryColor.Value;
            }
        }

        public override Color GlowColor
        {
            get
            {
                if (_glowColor == null)
                {
                    Color color;

                    if (VisualStyleRenderer.IsSupported)
                    {
                        color = VisualStyleInformation.ControlHighlightHot;
                    }
                    else
                    {
                        color = SystemColors.Highlight;
                    }

                    double h = color.GetHue() / 360, s = color.GetSaturation();

                    _glowColor = WinFormsUtility.Drawing.ColorUtil.FromHSL(h, s, 0.66);
                }
                return _glowColor.Value;
            }
        }

        public override Color GlowHighlightColor
        {
            get
            {
                if (_glowHighlightColor == null)
                {
                    Color color;

                    if (VisualStyleRenderer.IsSupported)
                    {
                        color = VisualStyleInformation.ControlHighlightHot;
                        color = WinFormsUtility.Drawing.ColorUtil.ModifyHue(color, 0.02);

                        double h = color.GetHue() / 360, s = color.GetSaturation();

                        _glowHighlightColor = WinFormsUtility.Drawing.ColorUtil.FromHSL(h, s, 0.6);
                    }
                    else
                    {
                        color = SystemColors.Window;

                        double h = color.GetHue() / 360, s = color.GetSaturation();

                        _glowHighlightColor = WinFormsUtility.Drawing.ColorUtil.FromHSL(h, s, 0.85);
                    }
                }

                return _glowHighlightColor.Value;
            }
        }

        public override Color GlowDeepColor
        {
            get
            {
                return WinFormsUtility.Drawing.ColorUtil.ModifyHue(VisualStyleInformation.ControlHighlightHot, -0.02);
            }
        }

        public override Color GrayForegroundColor
        {
            get
            {
                return SystemColors.GrayText;
            }
        }

        public override Color GrayBackgroundColor
        {
            get
            {
                return SystemColors.ControlLight;
            }
        }

        public override Color TextColor
        {
            get
            {
                return SystemColors.ControlText;
            }
        }

        public override Color GrayTextColor
        {
            get
            {
                return SystemColors.GrayText;
            }
        }

        public override Color PrimaryBrandingColor
        {
            get
            {
                if (_primaryBrandingColor == null)
                {
                    Color color;

                    if (VisualStyleRenderer.IsSupported)
                    {
                        VisualStyleRenderer vsr = new VisualStyleRenderer(VisualStyleElement.Window.Caption.Active);

                        color = vsr.GetColor(ColorProperty.FillColorHint);
                    }
                    else
                    {
                        color = SystemColors.Highlight;
                    }

                    double h = color.GetHue() / 360, s = color.GetSaturation();

                    _primaryBrandingColor = WinFormsUtility.Drawing.ColorUtil.FromHSL(h, s, 0.6);
                }
                return _primaryBrandingColor.Value;
            }
        }

        private Color? _primaryColor;
        private Color? _glowColor;
        private Color? _glowHighlightColor;
        private Color? _primaryBrandingColor;
    }

    #endregion

    #region WindowBackgroundColorTable

    public class WindowBackgroundColorTable : ColorTable
    {
        public WindowBackgroundColorTable(ColorTable basedOn)
        {
            _basedOn = basedOn;
        }

        public override string Name
        {
            get
            {
                return string.Format("Windows Background (based on {0})", _basedOn.Name);
            }
        }

        public override string Description
        {
            get
            {
                return string.Format("Windows Background (based on {0})", _basedOn.Description);
            }
        }

        public override Color PrimaryColor
        {
            get
            {
                return SystemColors.ControlDark;
            }
        }

        public override Color PrimaryBackgroundColor
        {
            get
            {
                return SystemColors.Control;
            }
        }

        public override Color TextColor
        {
            get
            {
                return SystemColors.WindowText;
            }
        }

        public override Color GrayTextColor
        {
            get
            {
                return SystemColors.GrayText;
            }
        }

        public override Color GlowTextColor
        {
            get
            {
                return _basedOn.GlowTextColor;
            }
        }

        public override Color GrayBackgroundColor
        {
            get
            {
                return SystemColors.Control;
            }
        }

        public override Color GlowHighlightColor
        {
            get
            {
                return _basedOn.GlowHighlightColor;
            }
        }

        public override Color GlowColor
        {
            get
            {
                return _basedOn.GlowColor;
            }
        }

        public override Color GlowDeepColor
        {
            get
            {
                return _basedOn.GlowDeepColor;
            }
        }

        public override Color PrimaryBrandingColor
        {
            get
            {
                return _basedOn.PrimaryBrandingColor;
            }
        }

        public override Color GrayForegroundColor
        {
            get
            {
                return SystemColors.Control;
            }
        }

        private ColorTable _basedOn;
    }

    #endregion

    #region XmlColorTable

    public class XmlColorTable : ColorTable
    {
        public XmlColorTable(Utility.Assemblies.ManifestResources res, string path)
        {
            XmlDocument xmlDoc = res.GetXmlDocument(path);

            XmlNode ctNode = xmlDoc.SelectSingleNode("ColorTable");

            _name = ctNode.Attributes["Name"].Value;
            _description = ctNode.Attributes["Description"].Value;

            foreach (XmlNode cNode in ctNode.ChildNodes)
            {
                string name = cNode.Name;
                string value = cNode.InnerText;

                if (value.Length != 7 || value[0] != '#')
                {
                    throw new InvalidOperationException(string.Format("Invalid color '{0}' in '{1}'.", value, name));
                }

                int v = int.Parse(value.Substring(1), System.Globalization.NumberStyles.AllowHexSpecifier);

                unchecked
                {
                    Color color = Color.FromArgb((int)0xff000000 | v);

                    _colors.Add(name, color);
                }
            }
        }

        public override string Name
        {
            get
            {
                return _name;
            }
        }

        public override string Description
        {
            get
            {
                return _description;
            }
        }

        public override Color PrimaryColor
        {
            get
            {
                return _colors["PrimaryColor"];
            }
        }

        public override Color GlowColor
        {
            get
            {
                return _colors["GlowColor"];
            }
        }

        public override Color GlowHighlightColor
        {
            get
            {
                return _colors["GlowHighlightColor"];
            }
        }

        public override Color GlowDeepColor
        {
            get
            {
                return _colors["GlowDeepColor"];
            }
        }

        public override Color GrayForegroundColor
        {
            get
            {
                return _colors["GrayForegroundColor"];
            }
        }

        public override Color GrayBackgroundColor
        {
            get
            {
                return _colors["GrayBackgroundColor"];
            }
        }

        public override Color TextColor
        {
            get
            {
                return _colors["TextColor"];
            }
        }

        public override Color GrayTextColor
        {
            get
            {
                return _colors["GrayTextColor"];
            }
        }

        public override Color PrimaryBrandingColor
        {
            get
            {
                return _colors["PrimaryBrandingColor"];
            }
        }

        public override Color GlowTextColor
        {
            get
            {
                Color c;

                if (_colors.TryGetValue("GlowTextColor", out c))
                {
                    return c;
                }
                else
                {
                    return base.GlowTextColor;
                }
            }
        }

        public override Color PrimaryBackgroundColor
        {
            get
            {
                Color c;

                if (_colors.TryGetValue("PrimaryBackgroundColor", out c))
                {
                    return c;
                }
                else
                {
                    return base.PrimaryBackgroundColor;
                }
            }
        }

        public override Color GlossyGlowLightenerColor
        {
            get
            {
                Color c;

                if (_colors.TryGetValue("GlossyGlowLightenerColor", out c))
                {
                    return c;
                }
                else
                {
                    return base.GlossyGlowLightenerColor;
                }
            }
        }

        public override Color GlossyLightenerColor
        {
            get
            {
                Color c;

                if (_colors.TryGetValue("GlossyLightenerColor", out c))
                {
                    return c;
                }
                else
                {
                    return base.GlossyLightenerColor;
                }
            }
        }

        public override Color GrayPrimaryBackgroundColor
        {
            get
            {
                Color c;

                if (_colors.TryGetValue("GrayPrimaryBackgroundColor", out c))
                {
                    return c;
                }
                else
                {
                    return base.GrayPrimaryBackgroundColor;
                }
            }
        }

        public override Color PrimaryBorderColor
        {
            get
            {
                Color c;

                if (_colors.TryGetValue("PrimaryBorderColor", out c))
                {
                    return c;
                }
                else
                {
                    return base.PrimaryBorderColor;
                }
            }
        }

        private string _name, _description;
        private Dictionary<string, Color> _colors = new Dictionary<string, Color>();
    }

    #endregion

    public sealed class GlowHistory<T>
    {
        private T _def;
        private Dictionary<T, DateTime> _lastOver = new Dictionary<T, DateTime>();
        private KeyValuePair<T, DateTime>? _firstOver;

        public GlowHistory(T def)
        {
            _def = def;
        }

        public T MouseOver
        {
            get
            {
                return _firstOver == null ? _def : _firstOver.Value.Key;
            }
        }

        public double? GetTimeOver(T t)
        {
            if (t == null)
            {
                throw new ArgumentNullException("t");
            }

            if (_firstOver == null)
            {
                return null;
            }
            else if (object.Equals(_firstOver.Value.Key, t))
            {
                return DateTime.Now.Subtract(_firstOver.Value.Value).TotalSeconds;
            }
            else
            {
                return null;
            }
        }

        public double GetLastOver(T t)
        {
            if (t == null)
            {
                throw new ArgumentNullException("t");
            }

            DateTime d;

            if (_lastOver.TryGetValue(t, out d))
            {
                return DateTime.Now.Subtract(d).TotalSeconds;
            }
            else
            {
                return double.MaxValue;
            }
        }

        public void Update(T over)
        {
            if (_firstOver == null || !object.Equals(_firstOver.Value.Key, over))
            {
                if (_firstOver != null)
                {
                    T old = _firstOver.Value.Key;

                    _lastOver[old] = DateTime.Now;
                }

                if (over != null)
                {
                    _firstOver = new KeyValuePair<T, DateTime>(over, DateTime.Now);
                }
            }
            if (over == null)
            {
                _firstOver = null;
            }
        }

    }

    public sealed class GlowUpdates<T>
    {
        private List<T> _items = new List<T>();

        public T[] Items
        {
            get
            {
                return _items.ToArray();
            }
        }

        public void NeedsUpdate(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (!_items.Contains(item))
            {
                _items.Add(item);
            }
        }

        public void DoneUpdate(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            _items.Remove(item);
        }

        public void DoneAll()
        {
            _items.Clear();
        }
    }
}

namespace KellControls.Utility.Collections
{
    public sealed class ComparativeTuple
    {
        public ComparativeTuple(object[] values)
        {
            _values = values;
        }

        public override int GetHashCode()
        {
            int hc = 0;

            foreach (object obj in _values)
            {
                if (obj != null)
                {
                    hc ^= obj.GetHashCode();
                }
            }

            return hc;
        }

        public override bool Equals(object obj)
        {
            ComparativeTuple ct = obj as ComparativeTuple;

            if (ct == null)
            {
                return false;
            }

            if (_values.Length != ct._values.Length)
            {
                return false;
            }

            for (int i = 0; i < _values.Length; ++i)
            {
                if (!object.Equals(_values[i], ct._values[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private object[] _values;
    }
    /// <summary>
    /// List style class that fires events when items are added / deleted.
    /// </summary>
    public class EventingList<T> : System.Collections.Generic.IList<T>
    {
        public enum EventType { Deleted, Added };
        public class EventInfo : EventArgs
        {
            public EventInfo(EventType eventType, T item)
            {
                this.EventType = eventType;
                this.Items = new T[] { item };
            }
            public EventInfo(EventType eventType, T[] items)
            {
                this.EventType = eventType;
                this.Items = items;
            }
            public readonly EventType EventType;
            public readonly T[] Items;
        }
        public T[] ToArray()
        {
            return _items.ToArray();
        }
        public event EventHandler<EventInfo> PreDataChanged;
        public event EventHandler<EventInfo> DataChanged;

        protected virtual void OnPreDataChanged(EventInfo eventInfo)
        {
            if (this.PreDataChanged != null)
            {
                this.PreDataChanged(this, eventInfo);
            }
        }
        protected virtual void OnDataChanged(EventInfo eventInfo)
        {
            if (this.DataChanged != null)
            {
                this.DataChanged(this, eventInfo);
            }
        }

        protected List<T> UnderlyingList
        {
            get
            {
                return _items;
            }
        }

        #region IList<T> Members

        public int IndexOf(T item)
        {
            return _items.IndexOf(item);
        }


        public void Insert(int index, T item)
        {
            OnPreDataChanged(new EventInfo(EventType.Added, item));
            _items.Insert(index, item);
            OnDataChanged(new EventInfo(EventType.Added, item));
        }

        public void RemoveAt(int index)
        {
            T item = _items[index];
            OnPreDataChanged(new EventInfo(EventType.Deleted, item));
            _items.RemoveAt(index);
            OnDataChanged(new EventInfo(EventType.Deleted, item));
        }

        public void MoveItem(int from, int to)
        {
            if (from < 0 || from >= _items.Count)
                throw new ArgumentOutOfRangeException("from");
            if (to < 0 || to >= _items.Count)
                throw new ArgumentOutOfRangeException("to");

            T temp = _items[from];
            if (to > from)
            {
                to--;
            }
            _items.RemoveAt(from);
            _items.Insert(to, temp);
        }

        public T this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                if (!_items[index].Equals(value))
                {
                    T itemToRemove = _items[index];

                    OnPreDataChanged(new EventInfo(EventType.Deleted, itemToRemove));
                    OnPreDataChanged(new EventInfo(EventType.Added, value));

                    _items[index] = value;

                    OnDataChanged(new EventInfo(EventType.Deleted, itemToRemove));
                    OnDataChanged(new EventInfo(EventType.Added, value));
                }
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            OnPreDataChanged(new EventInfo(EventType.Added, item));
            _items.Add(item);
            OnDataChanged(new EventInfo(EventType.Added, item));
        }

        public void AddRange(T[] items)
        {
            OnPreDataChanged(new EventInfo(EventType.Added, items));
            _items.AddRange(items);
            OnDataChanged(new EventInfo(EventType.Added, items));
        }



        public void Clear()
        {
            if (_items.Count > 0)
            {
                T[] items = _items.ToArray();
                OnPreDataChanged(new EventInfo(EventType.Deleted, items));
                _items.Clear();
                OnDataChanged(new EventInfo(EventType.Deleted, items));
            }
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void RemoveRange(int start, int count)
        {
            T[] items = _items.GetRange(start, count).ToArray();
            OnPreDataChanged(new EventInfo(EventType.Deleted, items));
            _items.RemoveRange(start, count);
            OnDataChanged(new EventInfo(EventType.Deleted, items));
        }

        public void RemoveRange(T[] items)
        {
            if (items.Length == 0)
                return;

            OnPreDataChanged(new EventInfo(EventType.Deleted, items));

            Dictionary<T, int> itemIndexes = new Dictionary<T, int>(_items.Count);
            int i = 0;
            foreach (T item in _items)
            {
                itemIndexes[item] = i++;
            }
            List<int> removeList = new List<int>();
            foreach (T item in items)
            {
                int index;
                if (itemIndexes.TryGetValue(item, out index))
                {
                    itemIndexes.Remove(item); // only allow to be deleted once
                    removeList.Add(index);
                }
            }
            if (removeList.Count == _items.Count)
            {
                _items.Clear();
            }
            else
            {
                removeList.Sort();
                int start = -1;
                int count = 0;
                for (i = removeList.Count - 1; i >= 0; i--)
                {
                    int removeAt = removeList[i];
                    if (removeAt + 1 == start)
                    {
                        start = removeAt;
                        count++;
                    }
                    else
                    {
                        if (start != -1)
                        {
                            _items.RemoveRange(start, count);
                        }
                        start = removeAt;
                        count = 1;
                    }
                }
                if (start != -1 && count > 0)
                {
                    _items.RemoveRange(start, count);
                }
            }
            OnDataChanged(new EventInfo(EventType.Deleted, items));
        }

        public bool Remove(T item)
        {
            int i = _items.IndexOf(item);
            if (i != -1)
            {
                this.RemoveAt(i);
                return true;
            }

            return false;
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion

        private List<T> _items = new List<T>();
    }

    [DebuggerDisplay("Count={_list.Count}")]
    public sealed class HashedList<T> : IEnumerable<T>, ICollection<T>
    {
        public HashedList()
        {
        }

        public HashedList(IEnumerable<T> items)
        {
            _list.AddRange(items);
            _positions = null;
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        public void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        public void Sort(Comparison<T> comparison)
        {
            _list.Sort(comparison);
            _positions = new Dictionary<T, int>();

            for (int i = 0; i < _list.Count; ++i)
            {
                _positions[_list[i]] = i;
            }
        }

        #region ICollection<T> Members

        public void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (Contains(item))
            {
                throw new ArgumentException("List already contains this item.", "item");
            }

            _list.Add(item);
            _positions.Add(item, _list.Count - 1);
        }

        public void Clear()
        {
            _list = new List<T>();
            _positions = new Dictionary<T, int>();
        }

        public bool Contains(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            EnsurePositions();

            return _positions.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (!Contains(item))
            {
                return false;
            }

            int position = _positions[item];

            _list.RemoveAt(position);

            _positions = null;

            return true;
        }

        #endregion

        public T this[int index]
        {
            get
            {
                return _list[index];
            }
        }

        public int BinarySearch(T item, IComparer<T> comparer)
        {
            return _list.BinarySearch(item, comparer);
        }

        public int IndexOf(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            EnsurePositions();

            int pos;

            if (!_positions.TryGetValue(item, out pos))
            {
                pos = -1;
            }

            return pos;
        }

        private void EnsurePositions()
        {
            if (_positions != null)
            {
                return;
            }

            _positions = new Dictionary<T, int>();

            for (int i = 0; i < _list.Count; ++i)
            {
                _positions.Add(_list[i], i);
            }
        }

        private List<T> _list = new List<T>();
        private Dictionary<T, int> _positions = new Dictionary<T, int>();
    }

    [DebuggerDisplay("Count={_values.Count}")]
    public sealed class Set<T> : IEnumerable<T>
    {
        public Set()
        {
            _values = new Dictionary<T, int>();
        }
        public Set(IEqualityComparer<T> comparer)
        {
            _values = new Dictionary<T, int>(comparer);
        }

        public Set(IEnumerable<T> ts)
            : this()
        {
            foreach (T t in ts)
            {
                if (!Contains(t))
                {
                    Add(t);
                }
            }
        }

        public int Count
        {
            get
            {
                return _values.Count;
            }
        }

        public Set<T> Copy()
        {
            return new Set<T>(this);
        }

        public void Clear()
        {
            _values.Clear();
        }

        public void Add(T t)
        {
            _values.Add(t, 0);
        }

        public void AddRange(IEnumerable<T> ts)
        {
            foreach (T t in ts)
            {
                Add(t);
            }
        }

        public void Remove(T t)
        {
            _values.Remove(t);
        }

        public void RemoveRange(IEnumerable<T> ts)
        {
            foreach (T t in ts)
            {
                this.Remove(t);
            }
        }

        public bool Contains(T t)
        {
            return _values.ContainsKey(t);
        }

        public Set<T> ShallowCopy()
        {
            Set<T> copy = new Set<T>();

            foreach (T t in this)
            {
                copy.Add(t);
            }

            return copy;
        }

        public T[] ToArray()
        {
            T[] ts = new T[Count];
            int pos = 0;

            foreach (T t in this)
            {
                ts[pos] = t;
                ++pos;
            }

            return ts;
        }

        public static Set<T> Intersect(params Set<T>[] sets)
        {
            if (sets == null)
            {
                throw new ArgumentNullException("sets");
            }
            if (sets.Length == 0)
            {
                return new Set<T>();
            }

            if (sets[0] == null)
            {
                throw new ArgumentNullException("sets[0]");
            }

            Set<T> counted = sets[0].ShallowCopy();

            for (int i = 1; i < sets.Length; ++i)
            {
                Set<T> set = sets[i];

                if (set == null)
                {
                    throw new ArgumentNullException(string.Format("sets[{0}]", i));
                }

                foreach (T t in set)
                {
                    int count;

                    if (counted._values.TryGetValue(t, out count))
                    {
                        counted._values[t] = count + 1;
                    }
                }
            }

            Set<T> intersection = new Set<T>();
            int c = sets.Length - 1;

            foreach (KeyValuePair<T, int> kvp in counted._values)
            {
                if (kvp.Value == c)
                {
                    intersection.Add(kvp.Key);
                }
            }

            return intersection;
        }

        public static void Differences(Set<T> first, Set<T> second, out T[] onlyInFirst, out T[] inBoth, out T[] onlyInSecond)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }

            List<T> listOnlyInFirst = new List<T>();
            List<T> listInBoth = new List<T>();
            List<T> listOnlyInSecond = new List<T>();

            foreach (T t in first)
            {
                if (second.Contains(t))
                {
                    listInBoth.Add(t);
                }
                else
                {
                    listOnlyInFirst.Add(t);
                }
            }
            foreach (T t in second)
            {
                if (!first.Contains(t))
                {
                    listOnlyInSecond.Add(t);
                }
            }

            onlyInFirst = listOnlyInFirst.ToArray();
            inBoth = listInBoth.ToArray();
            onlyInSecond = listOnlyInSecond.ToArray();
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            foreach (KeyValuePair<T, int> kvp in _values)
            {
                yield return kvp.Key;
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        private Dictionary<T, int> _values;
    }
}

namespace KellControls.VectorGraphics.Types
{
    [DebuggerDisplay("Rectangle({_x},{_y},{_width},{_height})")]
    public sealed class Rectangle
    {
        public Rectangle(double x, double y, double width, double height)
        {
            if (double.IsNaN(x))
            {
                throw new ArgumentException("Not a number.", "x");
            }
            if (double.IsNaN(y))
            {
                throw new ArgumentException("Not a number.", "y");
            }
            if (double.IsNaN(width))
            {
                throw new ArgumentException("Not a number.", "width");
            }
            if (double.IsNaN(height))
            {
                throw new ArgumentException("Not a number.", "height");
            }

            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public double X
        {
            [DebuggerStepThrough]
            get
            {
                return _x;
            }
        }

        public double Y
        {
            [DebuggerStepThrough]
            get
            {
                return _y;
            }
        }

        public double Width
        {
            [DebuggerStepThrough]
            get
            {
                return _width;
            }
        }

        public double Height
        {
            [DebuggerStepThrough]
            get
            {
                return _height;
            }
        }

        public double Left
        {
            [DebuggerStepThrough]
            get
            {
                return _x;
            }
        }

        public double Right
        {
            [DebuggerStepThrough]
            get
            {
                return _x + _width;
            }
        }

        public double Top
        {
            [DebuggerStepThrough]
            get
            {
                return _y;
            }
        }

        public double Bottom
        {
            [DebuggerStepThrough]
            get
            {
                return _y + _height;
            }
        }

        public Point TopLeft
        {
            [DebuggerStepThrough]
            get
            {
                return new Point(_x, _y);
            }
        }

        public Point TopRight
        {
            [DebuggerStepThrough]
            get
            {
                return new Point(_x + _width, _y);
            }
        }

        public Point BottomLeft
        {
            [DebuggerStepThrough]
            get
            {
                return new Point(_x, _y + _height);
            }
        }

        public Point BottomRight
        {
            [DebuggerStepThrough]
            get
            {
                return new Point(_x + _width, _y + _height);
            }
        }

        public Point Center
        {
            [DebuggerStepThrough]
            get
            {
                return new Point(_x + _width / 2, _y + _height / 2);
            }
        }

        public Point CenterLeft
        {
            [DebuggerStepThrough]
            get
            {
                return new Point(_x, _y + _height / 2);
            }
        }

        public Point CenterRight
        {
            [DebuggerStepThrough]
            get
            {
                return new Point(_x + _width, _y + _height / 2);
            }
        }

        public static Rectangle Union(params Rectangle[] rects)
        {
            Rectangle union = new Rectangle(0, 0, 0, 0);

            foreach (Rectangle rect in rects)
            {
                if (union.Width == 0 || union.Height == 0)
                {
                    union = rect;
                }
                else if (rect.Width != 0 && rect.Height != 0)
                {
                    double x = Math.Min(union.X, rect.X);
                    double y = Math.Min(union.Y, rect.Y);
                    double right = Math.Max(union.Right, rect.Right);
                    double bottom = Math.Max(union.Bottom, rect.Bottom);

                    union = new Rectangle(x, y, right - x, bottom - y);
                }
            }

            return union;
        }

        public static bool Overlap(Rectangle r1, Rectangle r2)
        {
            if (r1 == null)
            {
                throw new ArgumentNullException("r1");
            }
            if (r2 == null)
            {
                throw new ArgumentNullException("r2");
            }

            if (r1.Right < r2.Left || r2.Right < r1.Left)
            {
                return false;
            }
            if (r1.Bottom < r2.Top || r2.Bottom < r1.Top)
            {
                return false;
            }

            return true;
        }

        public static Rectangle Offset(Rectangle rect, Point offset)
        {
            return new Rectangle(rect.X + offset.X, rect.Y + offset.Y, rect.Width, rect.Height);
        }

        public static Rectangle Shrink(Rectangle rect, double by)
        {
            return new Rectangle(rect.X + by, rect.Y + by, rect.Width - by * 2, rect.Height - by * 2);
        }

        public static Rectangle Expand(Rectangle rect, double by)
        {
            return new Rectangle(rect.X - by, rect.Y - by, rect.Width + by * 2, rect.Height + by * 2);
        }

        private double _x, _y, _width, _height;
    }
    [DebuggerDisplay("Point({_x},{_y})")]
    public sealed class Point
    {
        public Point(double x, double y)
        {
            if (double.IsNaN(x))
            {
                throw new ArgumentException("Not a number.", "x");
            }
            if (double.IsNaN(y))
            {
                throw new ArgumentException("Not a number.", "y");
            }

            _x = x;
            _y = y;
        }

        public double X
        {
            get
            {
                return _x;
            }
        }

        public double Y
        {
            get
            {
                return _y;
            }
        }

        public static Point operator +(Point p, Vector v)
        {
            return new Point(p.X + v.X, p.Y + v.Y);
        }

        public static Vector operator -(Point p1, Point p2)
        {
            return new Vector(p1.X - p2.X, p1.Y - p2.Y);
        }

        private double _x, _y;
    }
    [DebuggerDisplay("Vector({_x},{_y})")]
    public sealed class Vector
    {
        public Vector(double x, double y)
        {
            if (double.IsNaN(x))
            {
                throw new ArgumentException("Not a number.", "x");
            }
            if (double.IsNaN(y))
            {
                throw new ArgumentException("Not a number.", "y");
            }

            _x = x;
            _y = y;
        }

        public Vector(Point p)
        {
            _x = p.X;
            _y = p.Y;
        }

        public double X
        {
            get
            {
                return _x;
            }
        }

        public double Y
        {
            get
            {
                return _y;
            }
        }

        public Vector Normalize(double required)
        {
            double length = Math.Sqrt(_x * X + _y * _y);

            if (length == 0)
            {
                return new Vector(0, 0);
            }
            else
            {
                return new Vector(_x * required / length, _y * required / length);
            }
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1._x + v2._x, v1._y + v2._y);
        }

        public static Vector operator /(Vector v, double f)
        {
            if (f == 0)
            {
                throw new ArgumentException("Factor may not be zero.", "f");
            }

            return new Vector(v._x / f, v._y / f);
        }

        private double _x, _y;
    }
}

namespace KellControls.VectorGraphics.Primitives
{
    public sealed class BoundsMarker : VisualItem
    {
        public BoundsMarker(Types.Rectangle rectangle)
        {
            _rectangle = rectangle;
        }

        public Types.Rectangle Rectangle
        {
            get
            {
                return _rectangle;
            }
        }

        public override void Visit(Visitor visitor)
        {
            visitor.PreVisitVisualItem(this);
            visitor.VisitBoundsMarker(this);
            visitor.PostVisitVisualItem(this);
        }

        public override VisualItem Copy()
        {
            return new BoundsMarker(_rectangle);
        }

        protected override Types.Rectangle CalculateBounds(Renderers.Renderer renderer)
        {
            return _rectangle;
        }

        private Types.Rectangle _rectangle;
    }
    public sealed class Container : VisualItem, IEnumerable<VisualItem>
    {
        public Container()
            : this(new Types.Point(0, 0), null)
        {
        }

        public Container(Types.Point offset)
            : this(offset, null)
        {
        }

        public Container(Types.Point offset, Path clip)
        {
            if (offset == null)
            {
                throw new ArgumentNullException("offset");
            }

            _offset = offset;
            _clip = clip;
        }

        public Types.Point Offset
        {
            get
            {
                return _offset;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _offset = value;

                DirtyBounds();
            }
        }

        public void AddBack(VisualItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (item.Parent != null)
            {
                throw new ArgumentException("Item is already parented.", "item");
            }

            item.Parent = this;
            _items.Add(item);
            DirtyBounds();
        }

        public void AddFront(VisualItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (item.Parent != null)
            {
                throw new ArgumentException("Item is already parented.", "item");
            }

            item.Parent = this;
            _items.Insert(0, item);
            DirtyBounds();
        }

        public void Replace(VisualItem from, VisualItem to)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }

            int index = _items.IndexOf(from);

            if (index < 0)
            {
                throw new ArgumentException("Item not found.", "from");
            }

            _items.RemoveAt(index);

            if (to != null)
            {
                _items.Insert(index, to);
            }
        }

        public override void Visit(Visitor visitor)
        {
            visitor.PreVisitVisualItem(this);
            visitor.VisitContainer(this);

            if (visitor.VisitContainerPreChildren(this))
            {
                foreach (VisualItem child in this)
                {
                    child.Visit(visitor);
                }
                visitor.VisitContainerPostChildren(this);
            }

            visitor.PostVisitVisualItem(this);
        }

        public override VisualItem Copy()
        {
            Container container = new Container(_offset, _clip);

            foreach (VisualItem child in this)
            {
                container.AddBack(child.Copy());
            }

            return container;
        }

        protected override Types.Rectangle CalculateBounds(Renderers.Renderer renderer)
        {
            List<Types.Rectangle> childBoundsList = new List<Types.Rectangle>();

            foreach (VisualItem child in this)
            {
                Types.Rectangle childBounds = child.GetBounds(renderer);

                childBounds = Types.Rectangle.Offset(childBounds, Offset);

                childBoundsList.Add(childBounds);
            }

            Types.Rectangle bounds = Types.Rectangle.Union(childBoundsList.ToArray());

            return bounds;
        }

        #region IEnumerable<VisualItem> Members

        public IEnumerator<VisualItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion

        private Types.Point _offset;
        private Path _clip;
        private List<VisualItem> _items = new List<VisualItem>();
    }
    public sealed class Path : VisualItem
    {
        #region Command

        public abstract class Command
        {
            public abstract class Visitor
            {
                public abstract void VisitMove(Move move);
                public abstract void VisitClose(Close close);
                public abstract void VisitLine(Line line);
                public abstract void VisitBezierCurve(BezierCurve curve);
                public abstract void VisitSmoothBezierCurve(SmoothBezierCurve smoothCurve);
                public abstract void VisitEllipticalArc(EllipticalArc ellipticalArc);
            }

            public abstract void Visit(Visitor visitor);
            public abstract double[] GetBoundsXCoordinates();
            public abstract double[] GetBoundsYCoordinates();
        }

        #endregion
        #region Move

        public sealed class Move : Command
        {
            public Move(Types.Point to)
            {
                if (to == null)
                {
                    throw new ArgumentNullException("to");
                }

                _to = to;
            }

            public Types.Point To
            {
                get
                {
                    return _to;
                }
            }

            public override void Visit(Visitor visitor)
            {
                visitor.VisitMove(this);
            }

            public override double[] GetBoundsXCoordinates()
            {
                return new double[] { _to.X };
            }

            public override double[] GetBoundsYCoordinates()
            {
                return new double[] { _to.Y };
            }

            private Types.Point _to;
        }

        #endregion
        #region Close

        public sealed class Close : Command
        {
            public override void Visit(Visitor visitor)
            {
                visitor.VisitClose(this);
            }

            public override double[] GetBoundsXCoordinates()
            {
                return new double[] { };
            }

            public override double[] GetBoundsYCoordinates()
            {
                return new double[] { };
            }
        }

        #endregion
        #region Line

        public sealed class Line : Command
        {
            public Line(Types.Point to)
            {
                if (to == null)
                {
                    throw new ArgumentNullException("to");
                }

                _to = to;
            }

            public Types.Point To
            {
                get
                {
                    return _to;
                }
            }

            public override void Visit(Visitor visitor)
            {
                visitor.VisitLine(this);
            }

            public override double[] GetBoundsXCoordinates()
            {
                return new double[] { _to.X };
            }

            public override double[] GetBoundsYCoordinates()
            {
                return new double[] { _to.Y };
            }

            private Types.Point _to;
        }

        #endregion
        #region BezierCurve

        public sealed class BezierCurve : Command
        {
            public BezierCurve(Types.Point c1, Types.Point c2, Types.Point to)
            {
                if (c1 == null)
                {
                    throw new ArgumentNullException("c1");
                }
                if (c2 == null)
                {
                    throw new ArgumentNullException("c2");
                }
                if (to == null)
                {
                    throw new ArgumentNullException("to");
                }

                _c1 = c1;
                _c2 = c2;
                _to = to;
            }

            public Types.Point C1
            {
                get
                {
                    return _c1;
                }
            }

            public Types.Point C2
            {
                get
                {
                    return _c2;
                }
            }

            public Types.Point To
            {
                get
                {
                    return _to;
                }
            }

            public override void Visit(Visitor visitor)
            {
                visitor.VisitBezierCurve(this);
            }

            public override double[] GetBoundsXCoordinates()
            {
                return new double[] { _to.X, _c1.X, _c2.X };
            }

            public override double[] GetBoundsYCoordinates()
            {
                return new double[] { _to.Y, _c1.Y, _c2.Y };
            }

            private Types.Point _c1, _c2, _to;
        }

        #endregion
        #region SmoothBezierCurve

        public sealed class SmoothBezierCurve : Command
        {
            public SmoothBezierCurve(Types.Point c2, Types.Point to)
            {
                if (c2 == null)
                {
                    throw new ArgumentNullException("c2");
                }
                if (to == null)
                {
                    throw new ArgumentNullException("to");
                }

                _c2 = c2;
                _to = to;
            }

            public Types.Point C2
            {
                get
                {
                    return _c2;
                }
            }

            public Types.Point To
            {
                get
                {
                    return _to;
                }
            }

            public override void Visit(Visitor visitor)
            {
                visitor.VisitSmoothBezierCurve(this);
            }

            public override double[] GetBoundsXCoordinates()
            {
                return new double[] { _to.X, _c2.X };
            }

            public override double[] GetBoundsYCoordinates()
            {
                return new double[] { _to.Y, _c2.Y };
            }

            private Types.Point _c2, _to;
        }

        #endregion
        #region EllipticalArc

        public sealed class EllipticalArc : Command
        {
            public EllipticalArc(double rx, double ry, double xAxisRotation, bool largeArcFlag, bool sweepFlag, Types.Point to)
            {
                if (to == null)
                {
                    throw new ArgumentNullException("to");
                }

                _rx = rx;
                _ry = ry;
                _xAxisRotation = xAxisRotation;
                _largeArcFlag = largeArcFlag;
                _sweepFlag = sweepFlag;
                _to = to;
            }

            public double RX
            {
                get
                {
                    return _rx;
                }
            }

            public double RY
            {
                get
                {
                    return _ry;
                }
            }

            public double XAxisRotation
            {
                get
                {
                    return _xAxisRotation;
                }
            }

            public bool LargeArcFlag
            {
                get
                {
                    return _largeArcFlag;
                }
            }

            public bool SweepFlag
            {
                get
                {
                    return _sweepFlag;
                }
            }

            public Types.Point To
            {
                get
                {
                    return _to;
                }
            }

            public override void Visit(Visitor visitor)
            {
                visitor.VisitEllipticalArc(this);
            }

            public override double[] GetBoundsXCoordinates()
            {
                return new double[] { _to.X, _to.X - _rx * 2, _to.X + _rx * 2 };
            }

            public override double[] GetBoundsYCoordinates()
            {
                return new double[] { _to.Y, _to.Y - _ry * 2, _to.Y + _ry * 2 };
            }

            private double _rx, _ry, _xAxisRotation;
            private bool _largeArcFlag, _sweepFlag;
            private Types.Point _to;
        }

        #endregion

        public Path()
            : this(new Command[] { })
        {
        }

        public Path(Command[] commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException("commands");
            }

            foreach (Command command in commands)
            {
                if (command == null)
                {
                    throw new ArgumentException("Null command found in array.", "commands");
                }

                _commands.Add(command);
            }
        }

        public Command[] Commands
        {
            get
            {
                return _commands.ToArray();
            }
        }

        public Paint.Pens.Pen Pen
        {
            get
            {
                return _pen;
            }
            set
            {
                _pen = value;
            }
        }

        public Paint.Brushes.Brush Brush
        {
            get
            {
                return _brush;
            }
            set
            {
                _brush = value;
            }
        }

        public void Add(Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            _commands.Add(command);
        }

        public override void Visit(Visitor visitor)
        {
            visitor.PreVisitVisualItem(this);
            visitor.VisitPath(this);
            visitor.PostVisitVisualItem(this);
        }

        public override VisualItem Copy()
        {
            //
            // Commands are readonly so we can safely copy references.

            Path path = new Path(_commands.ToArray());

            path.Pen = Pen;
            path.Brush = Brush;

            return path;
        }

        protected override KellControls.VectorGraphics.Types.Rectangle CalculateBounds(Renderers.Renderer renderer)
        {
            double minx = double.MaxValue, maxx = double.MinValue, miny = double.MaxValue, maxy = double.MinValue;

            foreach (Command command in Commands)
            {
                foreach (double x in command.GetBoundsXCoordinates())
                {
                    minx = Math.Min(minx, x);
                    maxx = Math.Max(maxx, x);
                }
                foreach (double y in command.GetBoundsYCoordinates())
                {
                    miny = Math.Min(miny, y);
                    maxy = Math.Max(maxy, y);
                }
            }

            if (minx == double.MaxValue)
            {
                minx = 0;
            }
            if (miny == double.MaxValue)
            {
                miny = 0;
            }

            double width = maxx - minx, height = maxy - miny;

            if (width < 0)
            {
                width = 0;
            }
            if (height < 0)
            {
                height = 0;
            }

            return new Types.Rectangle(minx, miny, width, height);
        }

        private List<Command> _commands = new List<Command>();
        private Paint.Pens.Pen _pen = new Paint.Pens.SolidPen(Paint.Color.Black, 1);
        private Paint.Brushes.Brush _brush;
    }
    public sealed class PointMarker : VisualItem
    {
        public PointMarker(Types.Point point)
        {
            if (point == null)
            {
                throw new ArgumentNullException("point");
            }

            _point = point;
        }

        public Types.Point Point
        {
            get
            {
                return _point;
            }
        }

        public override void Visit(Visitor visitor)
        {
            visitor.PreVisitVisualItem(this);
            visitor.VisitPointMarker(this);
            visitor.PostVisitVisualItem(this);
        }

        public override VisualItem Copy()
        {
            return new PointMarker(_point);
        }

        protected override Types.Rectangle CalculateBounds(Renderers.Renderer renderer)
        {
            return new Types.Rectangle(_point.X, _point.Y, 0, 0);
        }

        private Types.Point _point;
    }
    public sealed class Text : VisualItem
	{
		public enum Position
		{
			TopLeft,			TopCenter,		TopRight,
			CenterLeft,		Center,				CenterRight,
			BaseLeft,			BaseCenter,		BaseRight,
			BottomLeft,		BottomCenter,	BottomRight
		}

		[Flags]
		public enum FontStyleFlags
		{
			Normal = 0,
			Bold = 1,
			Italic = 2,
			Underline = 4
		}

		public Text( string value, Types.Point point, Position alignment )
		{
			if( value == null )
			{
				throw new ArgumentNullException( "value" );
			}
			if( point == null )
			{
				throw new ArgumentNullException( "point" );
			}

			_value = value;
			_point = point;
			_alignment = alignment;
		}

		public string Value
		{
			get
			{
				return _value;
			}
		}

		public Types.Point Point
		{
			get
			{
				return _point;
			}
		}

		public Position Alignment
		{
			get
			{
				return _alignment;
			}
		}

		public Paint.Color Color
		{
			get
			{
				return _color;
			}
			set
			{
				if( value == null )
				{
					throw new ArgumentNullException( "value" );
				}

				_color = value;
			}
		}

		public string FontFamily
		{
			get
			{
				return _fontFamily;
			}
			set
			{
				if( value == null )
				{
					throw new ArgumentNullException( "value" );
				}

				_fontFamily = value;
			}
		}

		public FontStyleFlags FontStyle
		{
			get
			{
				return _fontStyle;
			}
			set
			{
				_fontStyle = value;
			}
		}

		public double FontSizePoints
		{
			get
			{
				return _fontSizePoints;
			}
			set
			{
				if( value < 0 )
				{
					throw new ArgumentException( "Font size must be positive.", "value" );
				}

				_fontSizePoints = value;
			}
		}

		public override VisualItem Copy()
		{
			Text text = new Text( _value, _point, _alignment );

			text.Color = Color;
			text.FontFamily = FontFamily;
			text.FontStyle = FontStyle;
			text.FontSizePoints = FontSizePoints;

			return text;
		}

		public override void Visit( Visitor visitor )
		{
			visitor.PreVisitVisualItem( this );
			visitor.VisitText( this );
			visitor.PostVisitVisualItem( this );
		}

		protected override Types.Rectangle CalculateBounds( Renderers.Renderer renderer )
		{
			double width, height, baselineFromTop, x, y;

			renderer.MeasureText( this, out width, out height, out baselineFromTop );

			switch( Alignment )
			{
				case Primitives.Text.Position.TopLeft:
				case Primitives.Text.Position.CenterLeft:
				case Primitives.Text.Position.BaseLeft:
				case Primitives.Text.Position.BottomLeft:
					x = _point.X;
					break;
				case Primitives.Text.Position.TopCenter:
				case Primitives.Text.Position.Center:
				case Primitives.Text.Position.BaseCenter:
				case Primitives.Text.Position.BottomCenter:
					x = _point.X - width / 2 - 1;
					break;
				case Primitives.Text.Position.TopRight:
				case Primitives.Text.Position.CenterRight:
				case Primitives.Text.Position.BaseRight:
				case Primitives.Text.Position.BottomRight:
					x = _point.X - width;
					break;
				default:
					throw new InvalidOperationException();
			}

			switch( Alignment )
			{
				case Primitives.Text.Position.TopLeft:
				case Primitives.Text.Position.TopCenter:
				case Primitives.Text.Position.TopRight:
					y = _point.Y;
					break;
				case Primitives.Text.Position.CenterLeft:
				case Primitives.Text.Position.Center:
				case Primitives.Text.Position.CenterRight:
					y = _point.Y - height / 2;
					break;
				case Primitives.Text.Position.BaseLeft:
				case Primitives.Text.Position.BaseCenter:
				case Primitives.Text.Position.BaseRight:
					y = _point.Y - baselineFromTop;
					break;
				case Primitives.Text.Position.BottomLeft:
				case Primitives.Text.Position.BottomCenter:
				case Primitives.Text.Position.BottomRight:
					y = _point.Y - height;
					break;
				default:
					throw new InvalidOperationException();
			}

			return new KellControls.VectorGraphics.Types.Rectangle( x, y, width, height );
		}

		private string _value;
		private Types.Point _point;
		private Position _alignment;
		private Paint.Color _color = Paint.Color.Black;
		private string _fontFamily = "Arial";
		private FontStyleFlags _fontStyle;
		private double _fontSizePoints = 12;
	}
    public abstract class Visitor
    {
        protected Visitor()
        {
        }

        public virtual void PreVisitVisualItem(VisualItem visualItem)
        {
        }

        public virtual void VisitBoundsMarker(BoundsMarker boundsMarker)
        {
        }

        public virtual void VisitContainer(Container container)
        {
        }

        public virtual bool VisitContainerPreChildren(Container container)
        {
            return true;
        }

        public virtual void VisitContainerPostChildren(Container container)
        {
        }

        public virtual void VisitPath(Path path)
        {
        }

        public virtual void VisitPointMarker(PointMarker pointMarker)
        {
        }

        public virtual void VisitText(Text text)
        {
        }

        public virtual void PostVisitVisualItem(VisualItem visualItem)
        {
        }
    }
    public sealed class DelegateVisitor : Visitor
    {
        public delegate void VisitItem<ItemType>(ItemType item);

        public VisitItem<BoundsMarker> VisitBoundsMarkerDelegate;
        public VisitItem<Container> VisitContainerDelegate;
        public VisitItem<Path> VisitPathDelegate;
        public VisitItem<PointMarker> VisitPointMarkerDelegate;
        public VisitItem<Text> VisitTextDelegate;

        public override void VisitBoundsMarker(BoundsMarker boundsMarker)
        {
            if (VisitBoundsMarkerDelegate != null)
            {
                VisitBoundsMarkerDelegate(boundsMarker);
            }
        }

        public override void VisitContainer(Container container)
        {
            if (VisitContainerDelegate != null)
            {
                VisitContainerDelegate(container);
            }
        }

        public override void VisitPath(Path path)
        {
            if (VisitPathDelegate != null)
            {
                VisitPathDelegate(path);
            }
        }

        public override void VisitPointMarker(PointMarker pointMarker)
        {
            if (VisitPointMarkerDelegate != null)
            {
                VisitPointMarkerDelegate(pointMarker);
            }
        }

        public override void VisitText(Text text)
        {
            if (VisitTextDelegate != null)
            {
                VisitTextDelegate(text);
            }
        }
    }
    public abstract class VisualItem
    {
        protected VisualItem()
        {
            _style = new Styles.Style();
        }

        public Container Parent
        {
            get
            {
                return _parent;
            }
            internal set
            {
                _parent = value;
            }
        }

        public Styles.Style Style
        {
            get
            {
                return _style;
            }
        }

        public Types.Rectangle GetBounds(Renderers.Renderer renderer)
        {
            if (_bounds == null)
            {
                _bounds = CalculateBounds(renderer);

                if (_bounds == null)
                {
                    throw new InvalidOperationException();
                }
            }
            return _bounds;
        }

        public abstract void Visit(Visitor visitor);

        public abstract VisualItem Copy();

        protected abstract Types.Rectangle CalculateBounds(Renderers.Renderer renderer);

        internal void DirtyBounds()
        {
            _bounds = null;

            if (Parent != null)
            {
                Parent.DirtyBounds();
            }
        }

        private Styles.Style _style;
        private Container _parent;
        private Types.Rectangle _bounds;
    }
}

namespace KellControls.Internal
{
    [Flags]
    internal enum Coordinates
    {
        None = 0,
        X = 1,
        Y = 2,
        Width = 4,
        Height = 8
    }

    internal sealed class StaticVerticalPositioning : VerticalPositioning
    {
        internal StaticVerticalPositioning(KellTreeNodeCollection nodes, IRenderer renderer, ITreeInfo treeInfo, ITreeEvents treeEvents)
            : base(nodes, renderer, treeInfo, treeEvents)
        {
        }

        internal override double ExpansionAnimationPosition(KellTreeNode treeNode)
        {
            return TreeInfo.IsExpanded(treeNode) ? 1 : 0;
        }

        internal override bool IsAnimating()
        {
            return false;
        }

        internal override bool IsVisible(KellTreeNode treeNode)
        {
            if (_orderedNodes == null)
            {
                Calculate(null, Coordinates.None);
            }

            return _orderedNodes.Contains(treeNode);
        }

        internal override Rectangle GetNodeBounds(KellTreeNode treeNode, Coordinates required)
        {
            NodeBounds bounds;

            if (!_mapBounds.TryGetValue(treeNode, out bounds))
            {
                bounds = new NodeBounds();
                _mapBounds[treeNode] = bounds;
            }

            if (((required & Coordinates.X) != 0 && bounds.X == null)
                || ((required & Coordinates.Y) != 0 && bounds.Y == null)
                || ((required & Coordinates.Width) != 0 && bounds.Width == null)
                || ((required & Coordinates.Height) != 0 && bounds.Height == null))
            {
                Calculate(treeNode, required);
            }

            Rectangle rect = new Rectangle(bounds.X ?? 0, bounds.Y ?? 0, bounds.Width ?? 0, bounds.Height ?? 0);

            return rect;
        }

        internal override KellTreeNode[] GetNodesBetween(int top, int bottom)
        {
            if (_totalHeight == null)
            {
                Calculate(null, Coordinates.Y | Coordinates.Height);
            }
            if (top > bottom || bottom < 0 || top > _totalHeight || _orderedNodes.Count == 0)
            {
                return new KellTreeNode[] { };
            }

            int first = _orderedNodes.BinarySearch(null, new PositionComparer(_mapBounds, top));
            int last = _orderedNodes.BinarySearch(null, new PositionComparer(_mapBounds, bottom));

            if (first < 0)
            {
                first = ~first;
            }
            if (last < 0)
            {
                last = ~last;
            }

            List<KellTreeNode> nodes = new List<KellTreeNode>();

            for (int i = first; i <= last; ++i)
            {
                if (i < _orderedNodes.Count)
                {
                    nodes.Add(_orderedNodes[i]);
                }
            }

            return nodes.ToArray();
        }

        internal override KellTreeNode GetNodeAfter(KellTreeNode node)
        {
            if (_orderedNodes == null)
            {
                Calculate(null, Coordinates.None);
            }

            int index = _orderedNodes.IndexOf(node);

            if (index < _orderedNodes.Count - 1)
            {
                return _orderedNodes[index + 1];
            }
            else
            {
                return null;
            }
        }

        internal override KellTreeNode GetNodeBefore(KellTreeNode node)
        {
            if (_orderedNodes == null)
            {
                Calculate(null, Coordinates.None);
            }

            int index = _orderedNodes.IndexOf(node);

            if (index > 0)
            {
                return _orderedNodes[index - 1];
            }
            else
            {
                return null;
            }
        }

        internal override int GetTotalHeight()
        {
            if (_totalHeight == null)
            {
                Calculate(null, Coordinates.Y | Coordinates.Height);
            }

            return _totalHeight.Value;
        }

        internal override int GetMaxWidth()
        {
            if (_maxWidth == null)
            {
                Calculate(null, Coordinates.X | Coordinates.Width);
            }

            return _maxWidth.Value;
        }

        internal override void DirtyWidths()
        {
            _maxWidth = null;

            foreach (KeyValuePair<KellTreeNode, NodeBounds> kvp in _mapBounds)
            {
                kvp.Value.Width = null;
            }
        }

        internal override void SetAnimationMark(DateTime dateTime)
        {
        }

        internal StaticVerticalPositioning Copy()
        {
            StaticVerticalPositioning svp = new StaticVerticalPositioning(Nodes, Renderer, TreeInfo, TreeEvents);

            foreach (KeyValuePair<KellTreeNode, NodeBounds> kvp in _mapBounds)
            {
                svp._mapBounds[kvp.Key] = kvp.Value.Copy();
            }

            svp._maxWidth = _maxWidth;

            if (_orderedNodes != null)
            {
                svp._orderedNodes = new List<KellTreeNode>();
                svp._orderedNodes.AddRange(_orderedNodes);
            }

            svp._totalHeight = _totalHeight;

            return svp;
        }

        #region ITreeEvents Members

        public override void NodeUpdated(KellTreeNode treeNode)
        {
            using (Graphics g = TreeInfo.CreateGraphics())
            {
                NodeBounds nb;

                if (_mapBounds.TryGetValue(treeNode, out nb))
                {
                    bool needWidth = _maxWidth.HasValue;
                    Size size = Renderer.MeasureTreeNode(g, TreeInfo, treeNode, needWidth, true);

                    if (nb.Width == null || size.Width != nb.Width.Value)
                    {
                        if (needWidth)
                        {
                            nb.Width = size.Width;
                        }
                        else
                        {
                            nb.Width = null;
                        }

                        if (_maxWidth.HasValue && nb.X != null)
                        {
                            _maxWidth = Math.Max(_maxWidth.Value, nb.X.Value + nb.Width.Value + 4);
                        }
                    }

                    if (nb.Height != null && size.Height != nb.Height)
                    {
                        DirtyVerticals();
                    }
                }
            }
        }

        public override void NodeInserted(KellTreeNode treeNode)
        {
            DirtyVerticals();
        }

        public override void NodeDeleted(KellTreeNode treeNode)
        {
            DirtyVerticals();
        }

        public override void ToggleNodeExpansion(KellTreeNode treeNode)
        {
            DirtyVerticals();
        }

        public override void SelectNode(KellTreeNode treeNode)
        {
        }

        public override void Expand(KellTreeNode treeNode)
        {
            DirtyVerticals();
        }

        public override void Collapse(KellTreeNode treeNode)
        {
            DirtyVerticals();
        }

        public override void UpdateAnimations()
        {
        }

        #endregion

        private void DirtyVerticals()
        {
            _maxWidth = null;
            _totalHeight = null;
            _orderedNodes = null;
            _mapBounds.Clear();
        }

        private void Calculate(KellTreeNode stop, Coordinates required)
        {
            int ypos = 4;

            if (stop == null)
            {
                _orderedNodes = new List<KellTreeNode>();
            }

            if ((required & Coordinates.X) != 0 && (required & Coordinates.Width) != 0)
            {
                _maxWidth = 0;
            }

            using (Graphics g = TreeInfo.CreateGraphics())
            {
                CalculateCollection(g, Nodes, stop, required, ref ypos);
            }

            if (stop == null && (required & Coordinates.Y) != 0 && (required & Coordinates.Height) != 0)
            {
                _totalHeight = ypos + 4;
            }
        }

        private bool CalculateCollection(Graphics g, KellTreeNodeCollection nodes, KellTreeNode stop, Coordinates required, ref int ypos)
        {
            foreach (KellTreeNode node in nodes)
            {
                NodeBounds nb;

                if (!_mapBounds.TryGetValue(node, out nb))
                {
                    nb = new NodeBounds();
                    _mapBounds[node] = nb;
                }

                if ((required & Coordinates.X) != 0)
                {
                    if (nb.X == null)
                    {
                        nb.X = Renderer.MeasureIndent(g, TreeInfo, node);
                    }
                }
                if ((required & Coordinates.Y) != 0)
                {
                    if (nb.Y == null)
                    {
                        nb.Y = ypos;
                    }
                }
                if ((required & Coordinates.Width) != 0 || (required & Coordinates.Height) != 0)
                {
                    Size size = Renderer.MeasureTreeNode(g, TreeInfo, node, (required & Coordinates.Width) != 0, (required & Coordinates.Height) != 0);

                    if (nb.Width == null && (required & Coordinates.Width) != 0)
                    {
                        nb.Width = size.Width;
                    }
                    if (nb.Height == null && (required & Coordinates.Height) != 0)
                    {
                        nb.Height = size.Height;
                    }

                    if ((required & Coordinates.Height) != 0)
                    {
                        Debug.Assert(size.Height > 0);

                        ypos += size.Height;
                    }
                }

                if ((required & Coordinates.X) != 0 && (required & Coordinates.Width) != 0)
                {
                    _maxWidth = Math.Max(_maxWidth.Value, nb.X.Value + nb.Width.Value + 4);
                }

                if (node == stop)
                {
                    return false;
                }
                else
                {
                    if (stop == null)
                    {
                        _orderedNodes.Add(node);
                    }
                }

                if (TreeInfo.IsExpanded(node))
                {
                    if (!CalculateCollection(g, node.ChildNodes, stop, required, ref ypos))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #region TopComparer

        private sealed class PositionComparer : IComparer<KellTreeNode>
        {
            internal PositionComparer(Dictionary<KellTreeNode, NodeBounds> mapBounds, int pos)
            {
                _mapBounds = mapBounds;
                _pos = pos;
            }

            #region IComparer<KellTreeNode> Members

            public int Compare(KellTreeNode x, KellTreeNode y)
            {
                NodeBounds bounds;
                int rev = 1;

                if (x == null && y == null)
                {
                    throw new InvalidOperationException();
                }
                else if (x != null && y != null)
                {
                    throw new InvalidOperationException();
                }
                else if (x != null)
                {
                    bounds = _mapBounds[x];
                }
                else if (y != null)
                {
                    bounds = _mapBounds[y];
                    rev = -1;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                if (_pos < bounds.Y.Value)
                {
                    return rev;
                }
                else if (_pos > bounds.Y.Value + bounds.Height.Value)
                {
                    return -rev;
                }
                else
                {
                    return 0;
                }
            }

            #endregion

            private Dictionary<KellTreeNode, NodeBounds> _mapBounds;
            private int _pos;
        }

        #endregion

        #region NodeBounds

        private sealed class NodeBounds
        {
            internal int? X;
            internal int? Y;
            internal int? Width;
            internal int? Height;

            internal NodeBounds Copy()
            {
                NodeBounds nb = new NodeBounds();

                nb.X = X;
                nb.Y = Y;
                nb.Width = Width;
                nb.Height = Height;

                return nb;
            }
        }

        #endregion

        private Dictionary<KellTreeNode, NodeBounds> _mapBounds = new Dictionary<KellTreeNode, NodeBounds>();
        private int? _totalHeight;
        private int? _maxWidth;
        private List<KellTreeNode> _orderedNodes;
    }
    internal sealed class AnimatedVerticalPositioning : VerticalPositioning
    {
        internal AnimatedVerticalPositioning(KellTreeNodeCollection nodes, IRenderer renderer, ITreeInfo treeInfo, ITreeEvents treeEvents)
            : base(nodes, renderer, treeInfo, treeEvents)
        {
            _from = new StaticVerticalPositioning(nodes, renderer, treeInfo, treeEvents);
            _to = new StaticVerticalPositioning(nodes, renderer, treeInfo, treeEvents);
        }

        internal override double ExpansionAnimationPosition(KellTreeNode treeNode)
        {
            if (_treeNode == treeNode)
            {
                return TreeInfo.IsExpanded(treeNode) ? Proportion : 1 - Proportion;
            }
            else
            {
                return TreeInfo.IsExpanded(treeNode) ? 1 : 0;
            }
        }

        internal override bool IsVisible(KellTreeNode treeNode)
        {
            if (_animating)
            {
                return _from.IsVisible(treeNode) || _to.IsVisible(treeNode);
            }
            else
            {
                return _from.IsVisible(treeNode);
            }
        }

        internal override int GetMaxWidth()
        {
            if (_animating)
            {
                return GetValue(_from.GetMaxWidth(), _to.GetMaxWidth());
            }
            else
            {
                return _from.GetMaxWidth();
            }
        }

        internal override int GetTotalHeight()
        {
            if (_animating)
            {
                return GetValue(_from.GetTotalHeight(), _to.GetTotalHeight());
            }
            else
            {
                return _from.GetTotalHeight();
            }
        }

        internal override bool IsAnimating()
        {
            return _animating;
        }

        internal override Rectangle GetNodeBounds(KellTreeNode treeNode, Coordinates required)
        {
            if (!_animating)
            {
                return GetBoundsHelper(_from, treeNode, required);
            }

            Rectangle from = GetBoundsHelper(_from, treeNode, required);
            Rectangle to = GetBoundsHelper(_to, treeNode, required);

            int x = Math.Max(from.X, to.X);
            int y = GetValue(from.Y, to.Y);
            int w = Math.Max(from.Width, to.Width);
            int h = Math.Max(from.Height, to.Height);

            if (_animating)
            {
                if (_expanding)
                {
                    w = to.Width;
                }
                else
                {
                    w = from.Width;
                }
            }

            return new Rectangle(x, y, w, h);
        }

        internal override KellTreeNode[] GetNodesBetween(int top, int bottom)
        {
            List<KellTreeNode> nodes = new List<KellTreeNode>();

            if (_animating)
            {
                bottom += (int)_distance;

                int extra = Math.Abs(GetValue(0, _movement));
                int changeBottom = _changeTop + extra;
                Utility.Collections.Set<KellTreeNode> done = new Utility.Collections.Set<KellTreeNode>();
                StaticVerticalPositioning source;

                if (_expanding)
                {
                    source = _to;
                }
                else
                {
                    source = _from;
                }

                foreach (KellTreeNode tn in source.GetNodesBetween(top, _changeTop))
                {
                    if (!done.Contains(tn))
                    {
                        nodes.Add(tn);
                        done.Add(tn);
                    }
                }
                foreach (KellTreeNode tn in source.GetNodesBetween(Math.Max(top, _changeTop), Math.Min(bottom - extra, changeBottom)))
                {
                    if (!done.Contains(tn))
                    {
                        nodes.Add(tn);
                        done.Add(tn);
                    }
                }
                foreach (KellTreeNode tn in source.GetNodesBetween(changeBottom, bottom))
                {
                    if (!done.Contains(tn))
                    {
                        nodes.Add(tn);
                        done.Add(tn);
                    }
                }
            }
            else
            {
                foreach (KellTreeNode tn in _from.GetNodesBetween(top, bottom))
                {
                    nodes.Add(tn);
                }
            }

            return nodes.ToArray();
        }

        internal override void DirtyWidths()
        {
            _from.DirtyWidths();
            _to.DirtyWidths();
        }

        internal override void SetAnimationMark(DateTime dateTime)
        {
            _mark = dateTime;
        }

        internal override KellTreeNode GetNodeAfter(KellTreeNode node)
        {
            return _from.GetNodeAfter(node);
        }

        internal override KellTreeNode GetNodeBefore(KellTreeNode node)
        {
            return _from.GetNodeBefore(node);
        }

        public override void NodeUpdated(KellTreeNode treeNode)
        {
            _from.NodeUpdated(treeNode);
            _to.NodeUpdated(treeNode);
        }

        public override void NodeDeleted(KellTreeNode treeNode)
        {
            if (_animating)
            {
                _to.NodeDeleted(treeNode);
            }
            else if (TreeInfo.IsUpdatesSuspended())
            {
                _from.NodeDeleted(treeNode);
                _to.NodeDeleted(treeNode);
            }
            else
            {
                _start = DateTime.Now;
                _treeNode = treeNode;
                _expanding = false;
                _animating = true;

                _to.NodeDeleted(treeNode);

                TreeInfo.BeginAnimating();
            }
        }

        public override void NodeInserted(KellTreeNode treeNode)
        {
            if (_animating)
            {
                _to.NodeInserted(treeNode);
            }
            else if (TreeInfo.IsUpdatesSuspended())
            {
                _from.NodeInserted(treeNode);
                _to.NodeInserted(treeNode);
            }
            else
            {
                _start = DateTime.Now;
                _treeNode = treeNode;
                _expanding = true;
                _animating = true;

                _to.NodeInserted(treeNode);

                TreeInfo.BeginAnimating();
            }
        }

        public override void SelectNode(KellTreeNode treeNode)
        {
            _from.SelectNode(treeNode);
            _to.SelectNode(treeNode);
        }

        public override void Expand(KellTreeNode treeNode)
        {
            _from.Expand(treeNode);
            _to.Expand(treeNode);
        }

        public override void Collapse(KellTreeNode treeNode)
        {
            _from.Collapse(treeNode);
            _to.Collapse(treeNode);
        }

        public override void ToggleNodeExpansion(KellTreeNode treeNode)
        {
            if (_animating)
            {
                _to.ToggleNodeExpansion(treeNode);
            }
            else if (TreeInfo.IsUpdatesSuspended())
            {
                _from.ToggleNodeExpansion(treeNode);
                _to.ToggleNodeExpansion(treeNode);
            }
            else
            {
                _changeTop = GetNodeBounds(treeNode, Coordinates.Y | Coordinates.Height).Bottom;

                _start = DateTime.Now;
                _treeNode = treeNode;
                _expanding = treeNode.IsExpanded;

                _to.ToggleNodeExpansion(treeNode);

                if (!_animating)
                {
                    _animating = true;
                    TreeInfo.BeginAnimating();
                }

                _distance = Math.Abs(_from.GetTotalHeight() - _to.GetTotalHeight());
                _movement = _to.GetTotalHeight() - _from.GetTotalHeight();
            }
        }

        public override void UpdateAnimations()
        {
            if (_treeNode != null && Proportion >= 1)
            {
                if (_animating)
                {
                    TreeInfo.EndAnimating();
                    _animating = false;
                }

                _from = _to.Copy();
                _treeNode = null;
            }
        }

        private int GetValue(int from, int to)
        {
            if (from == to)
            {
                return from;
            }

            if (_expanding)
            {
                return (int)(to - _movement * (1 - Proportion));
            }
            else
            {
                return (int)(from + _movement * Proportion);
            }
        }

        private double Proportion
        {
            get
            {
                double time = 0.2;
                double secs = _mark.Subtract(_start).TotalSeconds;

                double prop = secs / time;

                if (prop < 0)
                {
                    prop = 0;
                }
                else if (prop > 1)
                {
                    prop = 1;
                }

                return prop;
            }
        }

        private Rectangle GetBoundsHelper(StaticVerticalPositioning vp, KellTreeNode tn, Coordinates required)
        {
            while (tn != null)
            {
                if (vp.IsVisible(tn))
                {
                    return vp.GetNodeBounds(tn, required);
                }

                if (tn.ParentCollection == null)
                {
                    break;
                }
                tn = tn.ParentCollection.ParentNode;
            }

            return Rectangle.Empty;
        }

        private StaticVerticalPositioning _from, _to;
        private DateTime _start;
        private KellTreeNode _treeNode;
        private bool _expanding, _animating;
        private double _distance;
        private int _movement, _changeTop;
        private DateTime _mark;
    }
    internal sealed class AnimationRequests : IDisposable
    {
        internal AnimationRequests(ITreeEvents treeEvents)
        {
            _treeEvents = treeEvents;

            _timer.Interval = 100;
            _timer.Tick += new EventHandler(_timer_Tick);
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Enabled = false;
                _timer.Tick -= new EventHandler(_timer_Tick);
                _timer.Dispose();
                _timer = null;
            }
        }

        #endregion

        internal void BeginAnimating()
        {
            ++_allCount;

            UpdateTimer();
        }

        internal void EndAnimating()
        {
            Debug.Assert(_allCount > 0);

            --_allCount;

            UpdateTimer();
        }

        internal void BeginAnimating(KellTreeNode treeNode, Rectangle subRect)
        {
            _toAdd.Add(new NodeAndSubRect(treeNode, subRect));

            UpdateTimer();
        }

        internal void EndAnimating(KellTreeNode treeNode)
        {
            _toRemove.Add(new NodeAndSubRect(treeNode, Rectangle.Empty));

            UpdateTimer();
        }

        private void DoAdd(NodeAndSubRect nodeAndSubRect)
        {
            CountAndSubRect countAndSubRect;

            if (!_nodeCounts.TryGetValue(nodeAndSubRect.KellTreeNode, out countAndSubRect))
            {
                countAndSubRect = new CountAndSubRect();
                countAndSubRect.SubRect = nodeAndSubRect.SubRect;

                _nodeCounts[nodeAndSubRect.KellTreeNode] = countAndSubRect;
            }

            ++countAndSubRect.Count;

            countAndSubRect.SubRect = Rectangle.Union(countAndSubRect.SubRect, nodeAndSubRect.SubRect);

            UpdateTimer();
        }

        private void DoRemove(NodeAndSubRect nodeAndSubRect)
        {
            CountAndSubRect countAndSubRect;

            if (_nodeCounts.TryGetValue(nodeAndSubRect.KellTreeNode, out countAndSubRect))
            {
                --countAndSubRect.Count;

                if (countAndSubRect.Count <= 0)
                {
                    _nodeCounts.Remove(nodeAndSubRect.KellTreeNode);
                }
            }

            UpdateTimer();
        }

        private void UpdateTimer()
        {
            _timer.Enabled = (_allCount > 0 || _nodeCounts.Count > 0 || _toAdd.Count > 0 || _toRemove.Count > 0);
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            foreach (NodeAndSubRect nodeAndSubRect in _toAdd)
            {
                DoAdd(nodeAndSubRect);
            }
            _toAdd.Clear();

            bool needsUpdate = false;

            if (_allCount > 0)
            {
                if (Invalidate != null)
                {
                    Invalidate(this, EventArgs.Empty);
                    needsUpdate = true;
                }
            }
            else
            {
                foreach (KeyValuePair<KellTreeNode, CountAndSubRect> kvp in _nodeCounts)
                {
                    KellTreeNode treeNode = kvp.Key;
                    CountAndSubRect countAndSubRect = kvp.Value;

                    if (InvalidateTreeNode != null)
                    {
                        InvalidateTreeNode(this, new TreeNodeRectangleEventArgs(treeNode, countAndSubRect.SubRect));
                        needsUpdate = true;
                    }
                }
            }

            if (needsUpdate && Update != null)
            {
                Update(this, EventArgs.Empty);
            }

            foreach (NodeAndSubRect nodeAndSubRect in _toRemove)
            {
                DoRemove(nodeAndSubRect);
            }
            _toRemove.Clear();

            _treeEvents.UpdateAnimations();
        }

        private sealed class NodeAndSubRect
        {
            internal NodeAndSubRect(KellTreeNode treeNode, Rectangle subRect)
            {
                KellTreeNode = treeNode;
                SubRect = subRect;
            }

            internal KellTreeNode KellTreeNode;
            internal Rectangle SubRect;
        }

        private sealed class CountAndSubRect
        {
            internal int Count;
            internal Rectangle SubRect;
        }

        public event EventHandler Invalidate;
        public event TreeNodeRectangleEventHandler InvalidateTreeNode;
        public event EventHandler Update;

        private ITreeEvents _treeEvents;
        private int _allCount;
        private Dictionary<KellTreeNode, CountAndSubRect> _nodeCounts = new Dictionary<KellTreeNode, CountAndSubRect>();
        private Timer _timer = new Timer();
        private List<NodeAndSubRect> _toAdd = new List<NodeAndSubRect>(), _toRemove = new List<NodeAndSubRect>();
    }
    internal sealed class TreeState : ITreeEvents
    {
        internal TreeState(KellTreeNodeCollection nodes, ITreeEvents treeEvents)
        {
            _nodes = nodes;
            _treeEvents = treeEvents;
        }

        internal KellTreeNode SelectedTreeNode
        {
            get
            {
                return _selectedNode;
            }
        }

        internal bool IsExpanded(KellTreeNode treeNode)
        {
            return _mapExpansionState[treeNode];
        }

        #region ITreeEvents Members

        public void NodeUpdated(KellTreeNode treeNode)
        {
        }

        public void NodeInserted(KellTreeNode treeNode)
        {
            _mapExpansionState.Add(treeNode, false);
        }

        public void NodeDeleted(KellTreeNode treeNode)
        {
            _mapExpansionState.Remove(treeNode);
        }

        public void ToggleNodeExpansion(KellTreeNode treeNode)
        {
            _mapExpansionState[treeNode] = !_mapExpansionState[treeNode];

            if (_selectedNode != null && !_mapExpansionState[treeNode])
            {
                if (IsParentOf(treeNode, _selectedNode))
                {
                    _treeEvents.SelectNode(treeNode);
                }
            }
            if (_mapExpansionState[treeNode])
                Expand(treeNode);
            else
                Collapse(treeNode);
        }

        public void Collapse(KellTreeNode treeNode)
        {
            if (Collapsing != null)
                Collapsing(this, new TreeNodeEventArgs(treeNode));
        }

        public void Expand(KellTreeNode treeNode)
        {
            if (Expanding != null)
                Expanding(this, new TreeNodeEventArgs(treeNode));
        }

        public void SelectNode(KellTreeNode treeNode)
        {
            if (_selectedNode == treeNode)
            {
                return;
            }

            if (_selectedNode != null)
            {
                _treeEvents.NodeUpdated(_selectedNode);
            }

            _selectedNode = treeNode;

            if (_selectedNode != null)
            {
                _treeEvents.NodeUpdated(_selectedNode);
            }
        }

        public void UpdateAnimations()
        {
        }

        #endregion

        private bool IsParentOf(KellTreeNode t1, KellTreeNode t2)
        {
            if (t1 == t2)
            {
                return false;
            }

            while (t2 != null)
            {
                if (t1 == t2)
                {
                    return true;
                }

                if (t2.ParentCollection == null)
                {
                    t2 = null;
                }
                else
                {
                    t2 = t2.ParentCollection.ParentNode;
                }
            }

            return false;
        }

        private KellTreeNodeCollection _nodes;
        private KellTreeNode _selectedNode;
        private Dictionary<KellTreeNode, bool> _mapExpansionState = new Dictionary<KellTreeNode, bool>();
        private ITreeEvents _treeEvents;
        public event TreeNodeEventHandler Expanding;
        public event TreeNodeEventHandler Collapsing;
    }
    internal abstract class VerticalPositioning : ITreeEvents
    {
        protected VerticalPositioning(KellTreeNodeCollection nodes, IRenderer renderer, ITreeInfo treeInfo, ITreeEvents treeEvents)
        {
            _nodes = nodes;
            _renderer = renderer;
            _treeInfo = treeInfo;
            _treeEvents = treeEvents;
        }

        internal abstract double ExpansionAnimationPosition(KellTreeNode treeNode);

        internal abstract bool IsAnimating();

        internal abstract Rectangle GetNodeBounds(KellTreeNode treeNode, Coordinates required);

        internal abstract bool IsVisible(KellTreeNode treeNode);

        internal abstract KellTreeNode[] GetNodesBetween(int top, int bottom);

        internal abstract KellTreeNode GetNodeBefore(KellTreeNode node);
        internal abstract KellTreeNode GetNodeAfter(KellTreeNode node);

        internal abstract int GetTotalHeight();
        internal abstract int GetMaxWidth();

        internal abstract void DirtyWidths();
        internal abstract void SetAnimationMark(DateTime dateTime);

        #region ITreeEvents Members

        public abstract void NodeUpdated(KellTreeNode treeNode);

        public abstract void NodeInserted(KellTreeNode treeNode);

        public abstract void NodeDeleted(KellTreeNode treeNode);

        public abstract void ToggleNodeExpansion(KellTreeNode treeNode);

        public abstract void SelectNode(KellTreeNode treeNode);

        public abstract void Expand(KellTreeNode treeNode);

        public abstract void Collapse(KellTreeNode treeNode);

        public abstract void UpdateAnimations();

        #endregion

        protected KellTreeNodeCollection Nodes
        {
            [DebuggerStepThrough]
            get
            {
                return _nodes;
            }
        }

        protected IRenderer Renderer
        {
            [DebuggerStepThrough]
            get
            {
                return _renderer;
            }
        }

        protected ITreeInfo TreeInfo
        {
            [DebuggerStepThrough]
            get
            {
                return _treeInfo;
            }
        }

        protected ITreeEvents TreeEvents
        {
            [DebuggerStepThrough]
            get
            {
                return _treeEvents;
            }
        }

        private KellTreeNodeCollection _nodes;
        private IRenderer _renderer;
        private ITreeInfo _treeInfo;
        private ITreeEvents _treeEvents;
    }
}

namespace KellControls.VectorGraphics.Styles
{
    public sealed class Lookup
    {
        public Lookup(Primitives.Container container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            Visitor visitor = new Visitor(_mapStyleToItems);

            container.Visit(visitor);
        }

        public Primitives.VisualItem[] GetVisualItems(string styleClass)
        {
            List<Primitives.VisualItem> list;

            if (_mapStyleToItems.TryGetValue(styleClass, out list))
            {
                return list.ToArray();
            }
            else
            {
                return new Primitives.VisualItem[] { };
            }
        }

        #region Visitor

        private sealed class Visitor : Primitives.Visitor
        {
            internal Visitor(Dictionary<string, List<Primitives.VisualItem>> mapStyleToItems)
            {
                _mapStyleToItems = mapStyleToItems;
            }

            public override void PreVisitVisualItem(Primitives.VisualItem visualItem)
            {
                foreach (string c in visualItem.Style.Classes)
                {
                    List<Primitives.VisualItem> items;

                    if (!_mapStyleToItems.TryGetValue(c, out items))
                    {
                        items = new List<Primitives.VisualItem>();
                        _mapStyleToItems.Add(c, items);
                    }

                    items.Add(visualItem);
                }
            }

            private Dictionary<string, List<Primitives.VisualItem>> _mapStyleToItems;
        }

        #endregion

        private Dictionary<string, List<Primitives.VisualItem>> _mapStyleToItems = new Dictionary<string, List<Primitives.VisualItem>>();
    }
    public abstract class Modifier
    {
        protected Modifier()
        {
        }

        public abstract void Apply(Renderers.Renderer renderer, Primitives.VisualItem item);
    }
    public sealed class Style
    {
        internal Style()
        {
        }

        public string[] Classes
        {
            get
            {
                return _classes.ToArray();
            }
        }

        public bool Has(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            return _classes.Contains(name);
        }

        public string GetExtra(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            string value;

            if (_extras.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        public void Add(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            _classes.Add(name);
        }

        public void AddExtra(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            _extras.Add(key, value);
        }

        public void MergeWith(Style style)
        {
            foreach (string s in style._classes)
            {
                if (!_classes.Contains(s))
                {
                    _classes.Add(s);
                }
            }

            foreach (KeyValuePair<string, string> kvp in style._extras)
            {
                _extras.Add(kvp.Key, kvp.Value);
            }
        }

        private Utility.Collections.Set<string> _classes = new Utility.Collections.Set<string>();
        private Dictionary<string, string> _extras = new Dictionary<string, string>();
    }
    public abstract class StyleSet
    {
        protected StyleSet()
        {
        }

        public void Apply(Renderers.Renderer renderer, Primitives.Container container)
        {
            foreach (KeyValuePair<string, Modifier> kvp in _modifiers)
            {
                Lookup lookup = new Lookup(container);

                string style = kvp.Key;
                Modifier modifier = kvp.Value;

                Primitives.VisualItem[] items = lookup.GetVisualItems(style);

                foreach (Primitives.VisualItem item in items)
                {
                    modifier.Apply(renderer, item);
                }
            }
        }

        protected void AddModifier(string styleClass, Modifier modifier)
        {
            if (styleClass == null)
            {
                throw new ArgumentNullException("styleClass");
            }
            if (modifier == null)
            {
                throw new ArgumentNullException("modifier");
            }

            _modifiers.Add(new KeyValuePair<string, Modifier>(styleClass, modifier));
        }

        private List<KeyValuePair<string, Modifier>> _modifiers = new List<KeyValuePair<string, Modifier>>();
    }
}

namespace KellControls.VectorGraphics.Styles.Modifiers
{
    public abstract class MarkerReplacementModifier : Modifier
    {
        protected MarkerReplacementModifier()
        {
        }

        public override void Apply(Renderers.Renderer renderer, Primitives.VisualItem item)
        {
            Primitives.DelegateVisitor visitor = new Primitives.DelegateVisitor();

            visitor.VisitBoundsMarkerDelegate = delegate(Primitives.BoundsMarker marker)
            {
                Primitives.VisualItem to = CreateItem(marker);

                if (to != null)
                {
                    to.Style.MergeWith(marker.Style);
                }

                marker.Parent.Replace(marker, to);
            };
            visitor.VisitPointMarkerDelegate = delegate(Primitives.PointMarker marker)
            {
                Primitives.VisualItem to = CreateItem(marker);

                if (to != null)
                {
                    to.Style.MergeWith(marker.Style);
                }

                marker.Parent.Replace(marker, to);
            };

            item.Visit(visitor);
        }

        protected abstract Primitives.VisualItem CreateItem(Primitives.BoundsMarker marker);
        protected abstract Primitives.VisualItem CreateItem(Primitives.PointMarker marker);
    }
    public abstract class PathModifier : Modifier
    {
        protected PathModifier()
        {
        }

        public override void Apply(Renderers.Renderer renderer, Primitives.VisualItem item)
        {
            Primitives.DelegateVisitor visitor = new Primitives.DelegateVisitor();

            visitor.VisitPathDelegate = delegate(Primitives.Path path)
            {
                Apply(renderer, path);
            };

            item.Visit(visitor);
        }

        protected abstract void Apply(Renderers.Renderer renderer, Primitives.Path path);
    }
    public class PathPaintModifier : PathModifier
    {
        public PathPaintModifier(Paint.Pens.Pen pen, Paint.Brushes.Brush brush)
        {
            _pen = pen;
            _brush = brush;
        }

        protected override void Apply(Renderers.Renderer renderer, Primitives.Path path)
        {
            path.Pen = _pen;
            path.Brush = _brush;
        }

        private Paint.Pens.Pen _pen;
        private Paint.Brushes.Brush _brush;
    }
    public class RectangleMarkerReplacementModifier : MarkerReplacementModifier
    {
        protected override Primitives.VisualItem CreateItem(Primitives.BoundsMarker marker)
        {
            Primitives.Path rect = new Primitives.Path();

            rect.Add(new Primitives.Path.Move(marker.Rectangle.TopLeft));
            rect.Add(new Primitives.Path.Line(marker.Rectangle.TopRight));
            rect.Add(new Primitives.Path.Line(marker.Rectangle.BottomRight));
            rect.Add(new Primitives.Path.Line(marker.Rectangle.BottomLeft));
            rect.Add(new Primitives.Path.Line(marker.Rectangle.TopLeft));
            rect.Add(new Primitives.Path.Close());

            return rect;
        }

        protected override Primitives.VisualItem CreateItem(Primitives.PointMarker marker)
        {
            return null;
        }
    }
    public class SoftShadowModifier : PathModifier
    {
        public SoftShadowModifier(Types.Point offset, double extent, Paint.Color color)
        {
            if (offset == null)
            {
                throw new ArgumentNullException("offset");
            }
            if (extent <= 0)
            {
                throw new ArgumentException("Extent must be greater than zero.", "extent");
            }
            if (color == null)
            {
                throw new ArgumentNullException("color");
            }

            _offset = offset;
            _extent = extent;
            _color = color;
        }

        protected override void Apply(Renderers.Renderer renderer, Primitives.Path path)
        {
            Factories.SoftShadow softShadow = new Factories.SoftShadow(renderer, _offset, _extent, _color);

            Primitives.VisualItem shadow = softShadow.Create(path);

            path.Parent.AddFront(shadow);
        }

        private Types.Point _offset;
        private double _extent;
        private Paint.Color _color;
    }
    public class TextFontModifier : TextModifier
    {
        public TextFontModifier(string fontFamily, double? fontSizeInPoints, Primitives.Text.FontStyleFlags? fontStyleFlags)
        {
            _fontFamily = fontFamily;
            _fontSizeInPoints = fontSizeInPoints;
            _fontStyleFlags = fontStyleFlags;
        }

        protected override void Apply(Renderers.Renderer renderer, Primitives.Text text)
        {
            if (_fontFamily != null)
            {
                text.FontFamily = _fontFamily;
            }

            if (_fontSizeInPoints != null)
            {
                text.FontSizePoints = _fontSizeInPoints.Value;
            }

            if (_fontStyleFlags != null)
            {
                text.FontStyle = _fontStyleFlags.Value;
            }
        }

        private string _fontFamily;
        private double? _fontSizeInPoints;
        private Primitives.Text.FontStyleFlags? _fontStyleFlags;
    }
    public abstract class TextModifier : Modifier
    {
        protected TextModifier()
        {
        }

        public override void Apply(Renderers.Renderer renderer, Primitives.VisualItem item)
        {
            Primitives.DelegateVisitor visitor = new Primitives.DelegateVisitor();

            visitor.VisitTextDelegate = delegate(Primitives.Text text)
            {
                Apply(renderer, text);
            };

            item.Visit(visitor);
        }

        protected abstract void Apply(Renderers.Renderer renderer, Primitives.Text text);
    }
}

namespace KellControls.VectorGraphics.Renderers
{
    internal partial class Canvas : UserControl
    {
        public Canvas()
        {
            InitializeComponent();

            SetStyle
                (ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.UserPaint
                , true);

            _renderer = new Renderers.GdiPlusRenderer(CreateGraphics, Renderers.GdiPlusRenderer.MarkerHandling.Display, 5);
        }

        public Renderers.GdiPlusRenderer Renderer
        {
            get
            {
                return _renderer;
            }
        }

        public Primitives.Container VectorGraphicsContainer
        {
            get
            {
                return _container;
            }
            set
            {
                _container = value;

                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_container != null)
            {
                _renderer.Render(e.Graphics, _container, new Types.Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width, e.ClipRectangle.Height));
            }
        }

        private Primitives.Container _container;
        private Renderers.GdiPlusRenderer _renderer;
    }
    partial class Canvas
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        }

        #endregion
    }
    public class GdiPlusRenderer : Renderer, IDisposable
    {
        public enum MarkerHandling
        {
            Ignore,
            Display,
            Throw
        }

        public delegate Graphics CreateGraphics();

        public GdiPlusRenderer(CreateGraphics createGraphics, MarkerHandling markerHandling, double accuracy)
        {
            if (createGraphics == null)
            {
                throw new ArgumentNullException("createGraphics");
            }
            if (accuracy <= 0)
            {
                throw new ArgumentException("Accuracy must be greater than zero.", "accuracy");
            }

            _createGraphics = createGraphics;
            _markerHandling = markerHandling;
            _accuracy = accuracy;
        }

        public void Render(Graphics g, Primitives.Container root, Types.Rectangle clip)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }
            if (clip == null)
            {
                throw new ArgumentNullException("clip");
            }

            g.SmoothingMode = SmoothingMode.HighQuality;

            PrimitivesVisitor visitor = new PrimitivesVisitor(this, g, clip);

            root.Visit(visitor);
        }

        public void Render(Graphics g, Primitives.Container root)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }

            g.SmoothingMode = SmoothingMode.HighQuality;

            PrimitivesVisitor visitor = new PrimitivesVisitor(this, g);

            root.Visit(visitor);
        }

        public MarkerHandling MarkerAction
        {
            get
            {
                return _markerHandling;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            foreach (GraphicsPath gp in _mapPathToGraphicsPath.Values)
            {
                gp.Dispose();
            }
            foreach (Font font in _mapFontDetailsToFont.Values)
            {
                font.Dispose();
            }

            _mapPathToGraphicsPath.Clear();
            _mapFontDetailsToFont.Clear();
        }

        #endregion

        public override Primitives.Path FlattenPath(Primitives.Path source)
        {
            PathCommandVisitor visitor = new PathCommandVisitor();

            foreach (Primitives.Path.Command pathCommand in source.Commands)
            {
                pathCommand.Visit(visitor);
            }

            using (GraphicsPath gp = visitor.GetGraphicsPath())
            {
                gp.Flatten();

                PointF lastPoint = PointF.Empty;

                Primitives.Path path = new Primitives.Path();

                path.Pen = source.Pen;
                path.Brush = source.Brush;

                for (int i = 0; i < gp.PointCount; ++i)
                {
                    PointF point = gp.PathPoints[i];
                    byte type = gp.PathTypes[i];
                    PointF nextPoint = point;

                    if (i < gp.PointCount - 1 && gp.PathTypes[i + 1] == 1)
                    {
                        nextPoint = gp.PathPoints[i + 1];
                    }

                    switch (type)
                    {
                        case 0:
                            {
                                path.Add(new Primitives.Path.Move(new Types.Point(point.X, point.Y)));
                                break;
                            }
                        case 1:
                            {
                                bool first = (i == 0) || gp.PathTypes[i - 1] != 1;
                                bool last = (i == gp.PointCount - 1) || gp.PathTypes[i + 1] != 1;

                                if (first || last
                                    || Math.Sqrt(Math.Pow(point.X - lastPoint.X, 2) + Math.Pow(point.Y - lastPoint.Y, 2)) > _accuracy
                                    || Math.Sqrt(Math.Pow(point.X - nextPoint.X, 2) + Math.Pow(point.Y - nextPoint.Y, 2)) > _accuracy)
                                {
                                    path.Add(new Primitives.Path.Line(new Types.Point(point.X, point.Y)));
                                    lastPoint = point;
                                }

                                break;
                            }
                        case 129:
                            {
                                path.Add(new Primitives.Path.Line(new Types.Point(point.X, point.Y)));
                                path.Add(new Primitives.Path.Close());
                                break;
                            }
                        default:
                            throw new InvalidOperationException();
                    }
                }

                return path;
            }
        }

        public override void MeasureText(Primitives.Text text, out double width, out double height, out double baselineFromTop)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            Font font = CreateFont(text);

            height = font.SizeInPoints;

            Graphics g = _createGraphics();

            g.SmoothingMode = SmoothingMode.HighQuality;

            width = g.MeasureString(text.Value, font).Width;

            double lineAscent = font.FontFamily.GetCellAscent(font.Style);
            double lineSpacing = font.FontFamily.GetLineSpacing(font.Style);

            height = font.GetHeight(g);

            baselineFromTop = height * lineAscent / lineSpacing;
        }

        private Font CreateFont(Primitives.Text text)
        {
            Font font;
            FontDetails fd = new FontDetails(text);

            if (_mapFontDetailsToFont.TryGetValue(fd, out font))
            {
                return font;
            }

            FontStyle fontStyle = FontStyle.Regular;

            if ((text.FontStyle & Primitives.Text.FontStyleFlags.Bold) != 0)
            {
                fontStyle |= FontStyle.Bold;
            }
            if ((text.FontStyle & Primitives.Text.FontStyleFlags.Italic) != 0)
            {
                fontStyle |= FontStyle.Italic;
            }
            if ((text.FontStyle & Primitives.Text.FontStyleFlags.Underline) != 0)
            {
                fontStyle |= FontStyle.Underline;
            }

            font = new Font(text.FontFamily, (float)text.FontSizePoints, fontStyle);

            _mapFontDetailsToFont.Add(fd, font);

            return font;
        }

        private GraphicsPath CreateGraphicsPath(Primitives.Path path)
        {
            GraphicsPath gp;

            if (_mapPathToGraphicsPath.TryGetValue(path, out gp))
            {
                return gp;
            }

            PathCommandVisitor visitor = new PathCommandVisitor();

            foreach (Primitives.Path.Command pathCommand in path.Commands)
            {
                pathCommand.Visit(visitor);
            }

            gp = visitor.GetGraphicsPath();

            _mapPathToGraphicsPath.Add(path, gp);

            return gp;
        }

        private Pen[] CreatePens(Paint.Pens.Pen pen)
        {
            if (pen == null)
            {
                return new Pen[] { };
            }

            PenVisitor visitor = new PenVisitor();

            pen.Visit(visitor);

            return visitor.GetPens();
        }

        private BrushStage[] CreateBrushes(Graphics graphics, Types.Rectangle bounds, Paint.Brushes.Brush brush)
        {
            if (brush == null)
            {
                return new BrushStage[] { };
            }

            BrushVisitor visitor = new BrushVisitor(graphics);

            brush.Visit(bounds, visitor);

            return visitor.GetBrushes();
        }

        #region PrimitivesVisitor

        private sealed class PrimitivesVisitor : Primitives.Visitor
        {
            internal PrimitivesVisitor(GdiPlusRenderer renderer, Graphics g, Types.Rectangle clip)
            {
                _renderer = renderer;
                _graphics = g;
                _clip = clip;
            }

            internal PrimitivesVisitor(GdiPlusRenderer renderer, Graphics g)
            {
                _renderer = renderer;
                _graphics = g;
            }

            public override void VisitBoundsMarker(Primitives.BoundsMarker boundsMarker)
            {
                if (!InClip(boundsMarker))
                {
                    return;
                }

                switch (_renderer.MarkerAction)
                {
                    case MarkerHandling.Display:
                        using (Pen pen = CreateDebugPen())
                        {
                            _graphics.DrawRectangle
                                (pen
                                , (float)boundsMarker.Rectangle.X
                                , (float)boundsMarker.Rectangle.Y
                                , (float)boundsMarker.Rectangle.Width
                                , (float)boundsMarker.Rectangle.Height);
                        }
                        break;
                    case MarkerHandling.Throw:
                        throw new InvalidOperationException("BoundsMarker found by renderer.");
                }
            }

            public override void VisitPointMarker(Primitives.PointMarker pointMarker)
            {
                if (!InClip(pointMarker))
                {
                    return;
                }

                switch (_renderer.MarkerAction)
                {
                    case MarkerHandling.Display:
                        using (Pen pen = CreateDebugPen())
                        {
                            _graphics.DrawEllipse(pen, (float)pointMarker.Point.X - 2.0f, (float)pointMarker.Point.Y - 2.0f, 4.0f, 4.0f);
                        }
                        break;
                    case MarkerHandling.Throw:
                        throw new InvalidOperationException("PointMarker found by renderer.");
                }
            }

            public override bool VisitContainerPreChildren(Primitives.Container container)
            {
                if (!InClip(container))
                {
                    return false;
                }

                _transforms.Push(_graphics.Transform);

                _graphics.TranslateTransform((float)container.Offset.X, (float)container.Offset.Y);

                return true;
            }

            public override void VisitContainerPostChildren(Primitives.Container container)
            {
                Matrix transform = _transforms.Pop();

                _graphics.Transform = transform;
            }

            public override void VisitPath(Primitives.Path path)
            {
                if (!InClip(path))
                {
                    return;
                }

                Types.Rectangle bounds = path.GetBounds(_renderer);
                Pen[] pens = _renderer.CreatePens(path.Pen);
                BrushStage[] brushes = _renderer.CreateBrushes(_graphics, bounds, path.Brush);
                GraphicsPath gp = _renderer.CreateGraphicsPath(path);

                foreach (BrushStage brush in brushes)
                {
                    _graphics.FillPath(brush.GetBrush(), gp);

                    brush.Dispose();
                }
                foreach (Pen pen in pens)
                {
                    _graphics.DrawPath(pen, gp);

                    pen.Dispose();
                }
            }

            public override void VisitText(Primitives.Text text)
            {
                if (!InClip(text))
                {
                    return;
                }

                Font font = _renderer.CreateFont(text);
                double width, height, baselineFromTop;

                _renderer.MeasureText(text, out width, out height, out baselineFromTop);

                double x, y;

                switch (text.Alignment)
                {
                    case Primitives.Text.Position.TopLeft:
                    case Primitives.Text.Position.CenterLeft:
                    case Primitives.Text.Position.BaseLeft:
                    case Primitives.Text.Position.BottomLeft:
                        x = text.Point.X;
                        break;
                    case Primitives.Text.Position.TopCenter:
                    case Primitives.Text.Position.Center:
                    case Primitives.Text.Position.BaseCenter:
                    case Primitives.Text.Position.BottomCenter:
                        x = text.Point.X - width / 2 - 1;
                        break;
                    case Primitives.Text.Position.TopRight:
                    case Primitives.Text.Position.CenterRight:
                    case Primitives.Text.Position.BaseRight:
                    case Primitives.Text.Position.BottomRight:
                        x = text.Point.X - width;
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                switch (text.Alignment)
                {
                    case Primitives.Text.Position.TopLeft:
                    case Primitives.Text.Position.TopCenter:
                    case Primitives.Text.Position.TopRight:
                        y = text.Point.Y;
                        break;
                    case Primitives.Text.Position.CenterLeft:
                    case Primitives.Text.Position.Center:
                    case Primitives.Text.Position.CenterRight:
                        y = text.Point.Y - height / 2;
                        break;
                    case Primitives.Text.Position.BaseLeft:
                    case Primitives.Text.Position.BaseCenter:
                    case Primitives.Text.Position.BaseRight:
                        y = text.Point.Y - baselineFromTop;
                        break;
                    case Primitives.Text.Position.BottomLeft:
                    case Primitives.Text.Position.BottomCenter:
                    case Primitives.Text.Position.BottomRight:
                        y = text.Point.Y - height;
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                using (StringFormat sf = new StringFormat(StringFormat.GenericTypographic))
                {
                    sf.FormatFlags |= StringFormatFlags.NoClip;
                    sf.FormatFlags |= StringFormatFlags.NoWrap;
                    sf.Trimming = StringTrimming.EllipsisCharacter;

                    using (Brush brush = new SolidBrush(GdiPlusUtility.Convert.Color(text.Color)))
                    {
                        _graphics.DrawString(text.Value, font, brush, (float)x, (float)y);
                    }
                }
            }

            private Pen CreateDebugPen()
            {
                Pen pen = new Pen(Color.Black, 1.0f);

                pen.DashStyle = DashStyle.Dash;

                return pen;
            }

            private bool InClip(Primitives.VisualItem item)
            {
                if (_clip == null)
                {
                    return true;
                }

                PointF tl = new PointF((float)_clip.Left, (float)_clip.Top);
                PointF br = new PointF((float)_clip.Right, (float)_clip.Bottom);
                PointF[] pts = new PointF[] { tl, br };

                _graphics.TransformPoints(CoordinateSpace.World, CoordinateSpace.Device, pts);

                Types.Rectangle clip = new Types.Rectangle(pts[0].X, pts[0].Y, pts[1].X - pts[0].X, pts[1].Y - pts[0].Y);

                return Types.Rectangle.Overlap(item.GetBounds(_renderer), clip);
            }

            private GdiPlusRenderer _renderer;
            private Graphics _graphics;
            private Stack<Matrix> _transforms = new Stack<Matrix>();
            private Types.Rectangle _clip;
        }

        #endregion

        #region PathCommandVisitor

        private sealed class PathCommandVisitor : Primitives.Path.Command.Visitor
        {
            public override void VisitMove(Primitives.Path.Move move)
            {
                _current = new GraphicsPath(FillMode.Winding);

                _x = _cy = move.To.X;
                _y = _cy = move.To.Y;
            }

            public override void VisitClose(Primitives.Path.Close close)
            {
                _current.CloseFigure();
                _x = _cy = 0;
                _y = _cy = 0;
            }

            public override void VisitLine(Primitives.Path.Line line)
            {
                _current.AddLine((float)_x, (float)_y, (float)line.To.X, (float)line.To.Y);
                _x = _cx = line.To.X;
                _y = _cy = line.To.Y;
            }

            public override void VisitBezierCurve(Primitives.Path.BezierCurve curve)
            {
                _current.AddBezier
                    ((float)_x, (float)_y
                    , (float)curve.C1.X, (float)curve.C1.Y
                    , (float)curve.C2.X, (float)curve.C2.Y
                    , (float)curve.To.X, (float)curve.To.Y);

                _x = curve.To.X;
                _y = curve.To.Y;
                _cx = curve.C2.X;
                _cy = curve.C2.Y;
            }

            public override void VisitSmoothBezierCurve(Primitives.Path.SmoothBezierCurve smoothCurve)
            {
                _current.AddBezier
                    ((float)_x, (float)_y
                    , (float)_cx, (float)_cy
                    , (float)smoothCurve.C2.X, (float)smoothCurve.C2.Y
                    , (float)smoothCurve.To.X, (float)smoothCurve.To.Y);

                _x = smoothCurve.To.X;
                _y = smoothCurve.To.Y;
                _cx = smoothCurve.C2.X;
                _cy = smoothCurve.C2.Y;
            }

            public override void VisitEllipticalArc(Primitives.Path.EllipticalArc ellipticalArc)
            {
                double x1 = _x;
                double y1 = _y;
                double rx = ellipticalArc.RX * 0.999;
                double ry = ellipticalArc.RY * 0.999;
                double xrot = ellipticalArc.XAxisRotation;
                bool fa = ellipticalArc.LargeArcFlag;
                bool fs = ellipticalArc.SweepFlag;
                double ex = ellipticalArc.To.X;
                double ey = ellipticalArc.To.Y;
                double cosxrot = Math.Cos(xrot);
                double sinxrot = Math.Sin(xrot);
                double mx = (x1 - ex) / 2;
                double my = (y1 - ey) / 2;
                double xd = cosxrot * mx + sinxrot * my;
                double yd = cosxrot * my - sinxrot * mx;

                if (xd != 0 || yd != 0)
                {
                    double preSqrtM = (rx * rx * ry * ry) - (rx * rx * yd * yd) - (ry * ry * xd * xd);
                    double preSqrtD = (rx * rx * yd * yd) + (ry * ry * xd * xd);
                    double preSqrt = Math.Abs(preSqrtM / preSqrtD);
                    double scale = Math.Sqrt(preSqrt);

                    if (fa == fs)
                    {
                        scale = -scale;
                    }

                    double cxd = scale * rx * yd / ry;
                    double cyd = -scale * ry * xd / rx;

                    double ccx = cosxrot * cxd - sinxrot * cyd + (x1 + ex) / 2;
                    double ccy = sinxrot * cxd + cosxrot * cyd + (y1 + ey) / 2;

                    double start = Angle(1, 0, (xd - cxd) / rx, (yd - cyd) / ry);
                    double sweep = Angle((xd - cxd) / rx, (yd - cyd) / ry, (-xd - cxd) / rx, (-yd - cyd) / ry);

                    start = 180 * start / Math.PI;
                    sweep = 180 * sweep / Math.PI;

                    if (!fs && sweep > 0)
                    {
                        sweep -= 360;
                    }
                    else if (fs && sweep < 0)
                    {
                        sweep += 360;
                    }

                    _current.AddArc((float)(ccx - rx), (float)(ccy - ry), (float)(rx * 2), (float)(ry * 2), (float)start, (float)sweep);
                }

                _x = ellipticalArc.To.X;
                _y = ellipticalArc.To.Y;
            }

            private static double Angle(double ux, double uy, double vx, double vy)
            {
                double lu = Math.Sqrt(ux * ux + uy * uy);
                double lv = Math.Sqrt(vx * vx + vy * vy);
                double a = Math.Acos((ux * vx + uy * vy) / (lu * lv));

                a *= Math.Sign(ux * vy - uy * vx);

                return a;
            }

            internal GraphicsPath GetGraphicsPath()
            {
                return _current;
            }

            private GraphicsPath _current = new GraphicsPath(FillMode.Winding);
            private double _x = 0, _y = 0, _cx = 0, _cy = 0;
        }

        #endregion

        #region PenVisitor

        private sealed class PenVisitor : Paint.Pens.PenVisitor
        {
            public override void VisitSolidPen(Paint.Pens.SolidPen solidPen)
            {
                Pen pen = new Pen(GdiPlusUtility.Convert.Color(solidPen.Color), (float)solidPen.Width);

                pen.LineJoin = LineJoin.Round;

                _pens.Add(pen);
            }

            public Pen[] GetPens()
            {
                return _pens.ToArray();
            }

            private List<Pen> _pens = new List<Pen>();
        }

        #endregion

        #region BrushVisitor

        private sealed class BrushVisitor : Paint.Brushes.BrushVisitor
        {
            internal BrushVisitor(Graphics graphics)
            {
                _graphics = graphics;
            }

            public override void VisitSolidBrush(Types.Rectangle bounds, Paint.Brushes.SolidBrush solidBrush)
            {
                Brush brush = new SolidBrush(GdiPlusUtility.Convert.Color(solidBrush.Color));

                _brushes.Add(new StandardBrushStage(brush));
            }

            public override void VisitLinearGradientBrush(Types.Rectangle bounds, Paint.Brushes.LinearGradientBrush linearGradientBrush)
            {
                double maxClipRectSize = Math.Sqrt(bounds.Width * bounds.Width + bounds.Height * bounds.Height);
                double maxLinearVectorSize = Math.Sqrt
                    (Math.Pow(linearGradientBrush.StartPoint.X - linearGradientBrush.EndPoint.X, 2)
                    + Math.Pow(linearGradientBrush.StartPoint.Y - linearGradientBrush.EndPoint.Y, 2));
                double maxSepSize = Math.Sqrt
                    (Math.Pow(linearGradientBrush.StartPoint.X - bounds.X, 2)
                    + Math.Pow(linearGradientBrush.StartPoint.Y - bounds.Y, 2));
                double extent = maxClipRectSize + maxLinearVectorSize + maxSepSize;

                if (extent == 0)
                {
                    return;
                }

                PointF linearVec = new PointF
                    ((float)(linearGradientBrush.EndPoint.X - linearGradientBrush.StartPoint.X)
                    , (float)(linearGradientBrush.EndPoint.Y - linearGradientBrush.StartPoint.Y));
                PointF vec = new PointF
                    ((float)((linearGradientBrush.EndPoint.X - linearGradientBrush.StartPoint.X) * extent / maxLinearVectorSize)
                    , (float)((linearGradientBrush.EndPoint.Y - linearGradientBrush.StartPoint.Y) * extent / maxLinearVectorSize));
                PointF ivec = new PointF
                    ((float)(-(linearGradientBrush.EndPoint.Y - linearGradientBrush.StartPoint.Y) * extent / maxLinearVectorSize)
                    , (float)((linearGradientBrush.EndPoint.X - linearGradientBrush.StartPoint.X) * extent / maxLinearVectorSize));
                PointF initialPoint = new PointF((float)linearGradientBrush.StartPoint.X, (float)linearGradientBrush.StartPoint.Y);
                PointF finalPoint = new PointF((float)linearGradientBrush.EndPoint.X, (float)linearGradientBrush.EndPoint.Y);

                if (linearGradientBrush.Render == Paint.Brushes.LinearGradientBrush.RenderHint.Normal)
                {
                    using (GraphicsPath gp = new GraphicsPath())
                    {
                        gp.StartFigure();
                        gp.AddPolygon(
                            new PointF[] {
				      initialPoint,
				      PointDiff( initialPoint, ivec ),
				      PointDiff( PointDiff( initialPoint, ivec ), vec ),
				      PointDiff( initialPoint, vec),
				      PointAdd( PointDiff( initialPoint, vec), ivec ),
				      PointAdd( initialPoint, ivec ),
				      initialPoint
				    }
                        );
                        gp.CloseFigure();

                        BrushStage brush = new ClippedStandardBrushStage(_graphics, new SolidBrush(GdiPlusUtility.Convert.Color(linearGradientBrush.StartColor)), new Region(gp));

                        _brushes.Add(brush);
                    }
                }

                List<KeyValuePair<double, Paint.Color>> steps = new List<KeyValuePair<double, Paint.Color>>();

                steps.Add(new KeyValuePair<double, Paint.Color>(0, linearGradientBrush.StartColor));
                steps.AddRange(linearGradientBrush.IntermediateColors);
                steps.Add(new KeyValuePair<double, Paint.Color>(1, linearGradientBrush.EndColor));

                if (maxLinearVectorSize > 0)
                {
                    for (int i = 0; i < steps.Count - 1; ++i)
                    {
                        double startProportion = steps[i].Key;
                        double endProportion = steps[i + 1].Key;

                        if (Math.Abs(startProportion - endProportion) < 0.001)
                        {
                            continue;
                        }

                        Paint.Color startColor = steps[i].Value;
                        Paint.Color endColor = steps[i + 1].Value;

                        PointF stepLinearVec = PointMul(linearVec, endProportion - startProportion);
                        PointF stepInitialPoint = PointAdd(initialPoint, PointMul(linearVec, startProportion));
                        PointF stepFinalPoint = PointAdd(initialPoint, PointMul(linearVec, endProportion));

                        using (GraphicsPath gp = new GraphicsPath())
                        {
                            gp.StartFigure();
                            gp.AddPolygon(
                                new PointF[] {
				      stepInitialPoint,
				      PointDiff( stepInitialPoint, ivec ),
				      PointAdd( PointDiff( stepInitialPoint, ivec ), stepLinearVec ),
				      PointAdd( stepInitialPoint, stepLinearVec ),
				      PointAdd( PointAdd( stepInitialPoint, stepLinearVec ), ivec ),
				      PointAdd( stepInitialPoint, ivec ),
				      stepInitialPoint
				    }
                            );
                            gp.CloseFigure();

                            LinearGradientBrush linearBrush = new LinearGradientBrush
                                (stepInitialPoint, stepFinalPoint, GdiPlusUtility.Convert.Color(startColor), GdiPlusUtility.Convert.Color(endColor));

                            linearBrush.WrapMode = WrapMode.TileFlipXY;

                            BrushStage brush = new ClippedStandardBrushStage(_graphics, linearBrush, new Region(gp));

                            _brushes.Add(brush);
                        }
                    }
                }

                if (linearGradientBrush.Render == Paint.Brushes.LinearGradientBrush.RenderHint.Normal)
                {
                    using (GraphicsPath gp = new GraphicsPath())
                    {
                        gp.StartFigure();
                        gp.AddPolygon(
                            new PointF[] {
				      finalPoint,
				      PointAdd( finalPoint, ivec ),
				      PointAdd( PointAdd( finalPoint, vec ), ivec ),
				      PointAdd( finalPoint, vec ),
				      PointAdd( PointDiff( finalPoint, ivec ), vec ),
				      PointDiff( finalPoint, ivec ),
				      finalPoint
				    }
                        );
                        gp.CloseFigure();

                        BrushStage brush = new ClippedStandardBrushStage(_graphics, new SolidBrush(GdiPlusUtility.Convert.Color(linearGradientBrush.EndColor)), new Region(gp));

                        _brushes.Add(brush);
                    }
                }
            }

            public override void VisitRadialGradientBrush(Types.Rectangle bounds, Paint.Brushes.RadialGradientBrush radialGradientBrush)
            {
                double dx = radialGradientBrush.InnerPoint.X - radialGradientBrush.OuterPoint.X;
                double dy = radialGradientBrush.InnerPoint.Y - radialGradientBrush.OuterPoint.Y;

                float radius = (float)Math.Sqrt(dx * dx + dy * dy);

                if (radius == 0)
                {
                    return;
                }

                using (GraphicsPath gp = new GraphicsPath())
                {
                    gp.StartFigure();
                    gp.AddEllipse((float)radialGradientBrush.InnerPoint.X - radius, (float)radialGradientBrush.InnerPoint.Y - radius, radius * 2, radius * 2);

                    Region around = new Region();

                    around.MakeInfinite();
                    around.Xor(gp);

                    SolidBrush b = new SolidBrush(GdiPlusUtility.Convert.Color(radialGradientBrush.OuterColor));

                    _brushes.Add(new ClippedStandardBrushStage(_graphics, b, around));

                    PathGradientBrush radialBrush = new PathGradientBrush(gp);

                    radialBrush.CenterColor = GdiPlusUtility.Convert.Color(radialGradientBrush.InnerColor);
                    radialBrush.CenterPoint = GdiPlusUtility.Convert.Point(radialGradientBrush.InnerPoint);
                    radialBrush.SurroundColors = new Color[] { GdiPlusUtility.Convert.Color(radialGradientBrush.OuterColor) };
                    radialBrush.WrapMode = WrapMode.Clamp;

                    _brushes.Add(new StandardBrushStage(radialBrush));
                }
            }

            public BrushStage[] GetBrushes()
            {
                return _brushes.ToArray();
            }

            private static PointF PointDiff(PointF p1, PointF p2)
            {
                return new PointF(p1.X - p2.X, p1.Y - p2.Y);
            }

            private static PointF PointAdd(PointF p1, PointF p2)
            {
                return new PointF(p1.X + p2.X, p1.Y + p2.Y);
            }

            private static PointF PointMul(PointF p, double f)
            {
                return new PointF((float)(p.X * f), (float)(p.Y * f));
            }

            private Graphics _graphics;
            private List<BrushStage> _brushes = new List<BrushStage>();
        }

        #endregion

        #region BrushStages

        private abstract class BrushStage : IDisposable
        {
            protected BrushStage()
            {
            }

            public abstract Brush GetBrush();

            protected virtual void Post()
            {
            }

            #region IDisposable Members

            public void Dispose()
            {
                Post();
            }

            #endregion
        }

        private class StandardBrushStage : BrushStage
        {
            internal StandardBrushStage(Brush brush)
            {
                _brush = brush;
            }

            public override Brush GetBrush()
            {
                return _brush;
            }

            protected override void Post()
            {
                if (_brush != null)
                {
                    _brush.Dispose();
                    _brush = null;
                }
            }

            private Brush _brush;
        }

        private sealed class ClippedStandardBrushStage : StandardBrushStage
        {
            internal ClippedStandardBrushStage(Graphics graphics, Brush brush, Region region)
                : base(brush)
            {
                _graphics = graphics;
                _region = region;
            }

            public override Brush GetBrush()
            {
                _graphics.SetClip(_region, CombineMode.Intersect);

                return base.GetBrush();
            }

            protected override void Post()
            {
                base.Post();

                if (_region != null)
                {
                    _region.Dispose();
                    _region = null;
                }

                _graphics.ResetClip();
            }

            private Graphics _graphics;
            private Region _region;
        }

        #endregion

        #region FontDetails

        private sealed class FontDetails
        {
            internal FontDetails(Primitives.Text text)
            {
                _fontFamily = text.FontFamily;
                _fontStyle = text.FontStyle;
                _fontSizePoints = text.FontSizePoints;
            }

            public override int GetHashCode()
            {
                return _fontFamily.GetHashCode() ^ _fontStyle.GetHashCode() ^ _fontSizePoints.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                FontDetails fd = obj as FontDetails;

                if (fd == null)
                {
                    return false;
                }
                else
                {
                    return _fontFamily == fd._fontFamily && _fontStyle == fd._fontStyle && _fontSizePoints == fd._fontSizePoints;
                }
            }

            private string _fontFamily = "Arial";
            private Primitives.Text.FontStyleFlags _fontStyle;
            private double _fontSizePoints = 12;
        }

        #endregion

        private CreateGraphics _createGraphics;
        private MarkerHandling _markerHandling;
        private double _accuracy;
        private Dictionary<Primitives.Path, GraphicsPath> _mapPathToGraphicsPath = new Dictionary<Primitives.Path, GraphicsPath>();
        private Dictionary<FontDetails, Font> _mapFontDetailsToFont = new Dictionary<FontDetails, Font>();
    }
    public static class Convert
    {
        public static System.Drawing.Color Color(Paint.Color color)
        {
            int r = (int)(color.Red * 255);
            int g = (int)(color.Green * 255);
            int b = (int)(color.Blue * 255);
            int a = (int)(color.Alpha * 255);

            return System.Drawing.Color.FromArgb(a, r, g, b);
        }

        public static Paint.Color Color(System.Drawing.Color color)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;
            double a = color.A / 255.0;

            return new VectorGraphics.Paint.Color(r, g, b, a);
        }

        public static Types.Rectangle Rectangle(System.Drawing.Rectangle rect)
        {
            return new VectorGraphics.Types.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static Types.Rectangle Rectangle(System.Drawing.RectangleF rect)
        {
            return new VectorGraphics.Types.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static System.Drawing.PointF Point(Types.Point p)
        {
            return new System.Drawing.PointF((float)p.X, (float)p.Y);
        }
    }
    public abstract class Renderer
    {
        protected Renderer()
        {
        }

        public abstract Primitives.Path FlattenPath(Primitives.Path path);
        public abstract void MeasureText(Primitives.Text text, out double width, out double height, out double baselineFromTop);
    }
}

namespace KellControls.VectorGraphics.Paint
{
    [DebuggerDisplay("Color(R={_r},G={_g},B={_b},A={_a})")]
    public sealed class Color
    {
        public delegate double Modify(double v);

        [DebuggerStepThrough]
        public Color(double r, double g, double b)
            : this(r, g, b, 1)
        {
        }

        public Color(Color c, double a)
            : this(c.Red, c.Green, c.Blue, a)
        {
        }

        public Color(double r, double g, double b, double a)
        {
            if (r < 0 || r > 1)
            {
                throw new ArgumentException("Color component must be between 0 and 1.", "r");
            }
            if (g < 0 || g > 1)
            {
                throw new ArgumentException("Color component must be between 0 and 1.", "r");
            }
            if (b < 0 || b > 1)
            {
                throw new ArgumentException("Color component must be between 0 and 1.", "r");
            }
            if (a < 0 || a > 1)
            {
                throw new ArgumentException("Color component must be between 0 and 1.", "r");
            }

            _r = r;
            _g = g;
            _b = b;
            _a = a;
        }

        public double Red
        {
            [DebuggerStepThrough]
            get
            {
                return _r;
            }
        }

        public double Green
        {
            [DebuggerStepThrough]
            get
            {
                return _g;
            }
        }

        public double Blue
        {
            [DebuggerStepThrough]
            get
            {
                return _b;
            }
        }

        public double Alpha
        {
            [DebuggerStepThrough]
            get
            {
                return _a;
            }
        }

        public void GetHSL(out double h, out double s, out double l)
        {
            double varMin = Math.Min(_r, Math.Min(_g, _b));
            double varMax = Math.Max(_r, Math.Max(_g, _b));
            double delMax = varMax - varMin;

            l = (varMax + varMin) / 2;

            if (delMax == 0)
            {
                h = 0;
                s = 0;
            }
            else
            {
                if (l < 0.5)
                {
                    s = delMax / (varMax + varMin);
                }
                else
                {
                    s = delMax / (2 - varMax - varMin);
                }

                double delR = (((varMax - _r) / 6) + (delMax / 2)) / delMax;
                double delG = (((varMax - _g) / 6) + (delMax / 2)) / delMax;
                double delB = (((varMax - _b) / 6) + (delMax / 2)) / delMax;

                if (_r == varMax)
                {
                    h = delB - delG;
                }
                else if (_g == varMax)
                {
                    h = (1.0 / 3.0) + delR - delB;
                }
                else if (_b == varMax)
                {
                    h = (2.0 / 3.0) + delG - delR;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                if (h < 0)
                {
                    h += 1;
                }
                if (h > 1)
                {
                    h -= 1;
                }
            }
        }

        public static Color Black
        {
            [DebuggerStepThrough]
            get
            {
                return new Color(0, 0, 0);
            }
        }

        public static Color White
        {
            [DebuggerStepThrough]
            get
            {
                return new Color(1, 1, 1);
            }
        }

        public static Color Transparent
        {
            [DebuggerStepThrough]
            get
            {
                return new Color(0, 0, 0, 0);
            }
        }

        public static Color Combine(Color c1, Color c2, double p)
        {
            if (c1 == null)
            {
                throw new ArgumentNullException("c1");
            }
            if (c2 == null)
            {
                throw new ArgumentNullException("c2");
            }

            double r = c1._r * p + c2._r * (1 - p);
            double g = c1._g * p + c2._g * (1 - p);
            double b = c1._b * p + c2._b * (1 - p);
            double a = c1._a * p + c2._a * (1 - p);

            r = Math.Min(Math.Max(r, 0), 1);
            g = Math.Min(Math.Max(g, 0), 1);
            b = Math.Min(Math.Max(b, 0), 1);
            a = Math.Min(Math.Max(a, 0), 1);

            return new Color(r, g, b, a);
        }

        public static Color ModifySaturation(Color c, Modify modify)
        {
            double h;
            double s;
            double l;

            c.GetHSL(out h, out s, out l);

            s = modify(s);

            s = Math.Min(Math.Max(0, s), 1);

            return FromHSL(h, s, l);
        }

        public static Color ModifyHue(Color c, Modify modify)
        {
            double h;
            double s;
            double l;

            c.GetHSL(out h, out s, out l);

            h = modify(h);

            return FromHSL(h, s, l);
        }

        public static Color ModifyLight(Color c, Modify modify)
        {
            double h;
            double s;
            double l;

            c.GetHSL(out h, out s, out l);

            l = modify(l);

            l = Math.Min(Math.Max(0, l), 1);

            return FromHSL(h, s, l);
        }

        public static Color ModifyHSL(Color c, Modify modifyH, Modify modifyS, Modify modifyL)
        {
            double h;
            double s;
            double l;

            c.GetHSL(out h, out s, out l);

            h = modifyH(h);
            s = modifyS(s);
            l = modifyL(l);

            s = Math.Min(Math.Max(0, s), 1);
            l = Math.Min(Math.Max(0, l), 1);

            return FromHSL(h, s, l);
        }

        public static Color FromHSL(double h, double s, double l)
        {
            double r = 0, g = 0, b = 0;
            double temp1, temp2;

            if (l == 0)
            {
                r = g = b = 0;
            }
            else
            {
                if (s == 0)
                {
                    r = g = b = l;
                }
                else
                {
                    temp2 = ((l <= 0.5) ? l * (1.0 + s) : l + s - (l * s));
                    temp1 = 2.0 * l - temp2;

                    double[] t3 = new double[] { h + 1.0 / 3.0, h, h - 1.0 / 3.0 };
                    double[] clr = new double[] { 0, 0, 0 };

                    for (int i = 0; i < 3; i++)
                    {
                        if (t3[i] < 0)
                        {
                            t3[i] += 1.0;
                        }
                        if (t3[i] > 1)
                        {
                            t3[i] -= 1.0;
                        }

                        if (6.0 * t3[i] < 1.0)
                        {
                            clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0;
                        }
                        else if (2.0 * t3[i] < 1.0)
                        {
                            clr[i] = temp2;
                        }
                        else if (3.0 * t3[i] < 2.0)
                        {
                            clr[i] = (temp1 + (temp2 - temp1) * ((2.0 / 3.0) - t3[i]) * 6.0);
                        }
                        else
                        {
                            clr[i] = temp1;
                        }
                    }

                    r = clr[0];
                    g = clr[1];
                    b = clr[2];
                }
            }

            return new Color(r, g, b);
        }

        private double _r, _g, _b, _a;
    }
}

namespace KellControls.VectorGraphics.Paint.Brushes
{
    #region BrushVisitor

    public abstract class BrushVisitor
    {
        public abstract void VisitSolidBrush(Types.Rectangle bounds, SolidBrush solidBrush);
        public abstract void VisitLinearGradientBrush(Types.Rectangle bounds, LinearGradientBrush linearGradientBrush);
        public abstract void VisitRadialGradientBrush(Types.Rectangle bounds, RadialGradientBrush radialGradientBrush);
    }

    #endregion

    #region Brush

    public abstract class Brush
    {
        protected Brush()
        {
        }

        public abstract void Visit(Types.Rectangle bounds, BrushVisitor visitor);
    }

    #endregion

    #region SolidBrush

    public sealed class SolidBrush : Brush
    {
        public SolidBrush(Color color)
        {
            if (color == null)
            {
                throw new ArgumentNullException("color");
            }

            _color = color;
        }

        public Color Color
        {
            get
            {
                return _color;
            }
        }

        public override void Visit(Types.Rectangle bounds, BrushVisitor visitor)
        {
            visitor.VisitSolidBrush(bounds, this);
        }

        private Color _color;
    }

    #endregion

    #region LinearGradientBrush

    public sealed class LinearGradientBrush : Brush
    {
        public enum RenderHint
        {
            Normal,
            NoClip
        }

        public LinearGradientBrush(Color startColor, Color endColor, Types.Point startPoint, Types.Point endPoint, params KeyValuePair<double, Color>[] intermediateColors)
            : this(startColor, endColor, startPoint, endPoint, RenderHint.Normal, intermediateColors)
        {
        }

        public LinearGradientBrush(Color startColor, Color endColor, Types.Point startPoint, Types.Point endPoint, RenderHint renderHint, params KeyValuePair<double, Color>[] intermediateColors)
        {
            if (startColor == null)
            {
                throw new ArgumentNullException("startColor");
            }
            if (endColor == null)
            {
                throw new ArgumentNullException("endColor");
            }
            if (startPoint == null)
            {
                throw new ArgumentNullException("startPoint");
            }
            if (endPoint == null)
            {
                throw new ArgumentNullException("endPoint");
            }
            if (intermediateColors == null)
            {
                throw new ArgumentNullException("intermediateColors");
            }

            foreach (KeyValuePair<double, Color> kvp in intermediateColors)
            {
                if (kvp.Key < 0 || kvp.Key > 1)
                {
                    throw new ArgumentException("Intermediate color position out-of-range.", "intermediateColors");
                }
            }

            _startColor = startColor;
            _endColor = endColor;
            _startPoint = startPoint;
            _endPoint = endPoint;
            _renderHint = renderHint;
            _intermediateColors = intermediateColors;
        }

        public Color StartColor
        {
            get
            {
                return _startColor;
            }
        }

        public Color EndColor
        {
            get
            {
                return _endColor;
            }
        }

        public Types.Point StartPoint
        {
            get
            {
                return _startPoint;
            }
        }

        public Types.Point EndPoint
        {
            get
            {
                return _endPoint;
            }
        }

        public RenderHint Render
        {
            get
            {
                return _renderHint;
            }
        }

        public KeyValuePair<double, Color>[] IntermediateColors
        {
            get
            {
                return _intermediateColors;
            }
        }

        public override void Visit(Types.Rectangle bounds, BrushVisitor visitor)
        {
            visitor.VisitLinearGradientBrush(bounds, this);
        }

        private Color _startColor, _endColor;
        private Types.Point _startPoint, _endPoint;
        private RenderHint _renderHint;
        private KeyValuePair<double, Color>[] _intermediateColors;
    }

    #endregion

    #region RadialGradientBrush

    public sealed class RadialGradientBrush : Brush
    {
        public RadialGradientBrush(Color innerColor, Color outerColor, Types.Point innerPoint, Types.Point outerPoint)
        {
            if (innerColor == null)
            {
                throw new ArgumentNullException("innerColor");
            }
            if (outerColor == null)
            {
                throw new ArgumentNullException("outerColor");
            }
            if (innerPoint == null)
            {
                throw new ArgumentNullException("innerPoint");
            }
            if (outerPoint == null)
            {
                throw new ArgumentNullException("outerPoint");
            }

            _innerColor = innerColor;
            _outerColor = outerColor;
            _innerPoint = innerPoint;
            _outerPoint = outerPoint;
        }

        public Color InnerColor
        {
            get
            {
                return _innerColor;
            }
        }

        public Color OuterColor
        {
            get
            {
                return _outerColor;
            }
        }

        public Types.Point InnerPoint
        {
            get
            {
                return _innerPoint;
            }
        }

        public Types.Point OuterPoint
        {
            get
            {
                return _outerPoint;
            }
        }

        public override void Visit(Types.Rectangle bounds, BrushVisitor visitor)
        {
            visitor.VisitRadialGradientBrush(bounds, this);
        }

        private Color _innerColor, _outerColor;
        private Types.Point _innerPoint, _outerPoint;
    }

    #endregion
}

namespace KellControls.VectorGraphics.Paint.Pens
{
    public abstract class PenVisitor
    {
        public abstract void VisitSolidPen(SolidPen solidPen);
    }

    public abstract class Pen
    {
        protected Pen()
        {
        }

        public abstract void Visit(PenVisitor visitor);
    }

    public class SolidPen : Pen
    {
        public SolidPen(Color color, double width)
        {
            if (color == null)
            {
                throw new ArgumentNullException("color");
            }
            if (width < 0)
            {
                throw new ArgumentException("Width must be non-negative.", "width");
            }

            _color = color;
            _width = width;
        }

        public Color Color
        {
            get
            {
                return _color;
            }
        }

        public double Width
        {
            get
            {
                return _width;
            }
        }

        public override void Visit(PenVisitor visitor)
        {
            visitor.VisitSolidPen(this);
        }

        private Color _color;
        private double _width;
    }
}

namespace KellControls.Utility.Control
{
    public sealed class Flag
    {
        public bool IsActive
        {
            get
            {
                lock (_lock)
                {
                    return _count > 0;
                }
            }
        }

        public IDisposable Apply()
        {
            int count;

            lock (_lock)
            {
                ++_count;

                count = _count;
            }

            if (count == 1 && Set != null)
            {
                Set(this, EventArgs.Empty);
            }

            return new Disposer(this);
        }

        private sealed class Disposer : IDisposable
        {
            internal Disposer(Flag flag)
            {
                _flag = flag;
            }

            #region IDisposable Members

            public void Dispose()
            {
                if (_flag != null)
                {
                    int count;

                    lock (_flag._lock)
                    {
                        --_flag._count;

                        count = _flag._count;
                    }

                    if (count == 0 && _flag.Reset != null)
                    {
                        _flag.Reset(_flag, EventArgs.Empty);
                    }

                    _flag = null;
                }
            }

            #endregion

            private Flag _flag;
        }

        public event EventHandler Set;
        public event EventHandler Reset;

        private int _count;
        private object _lock = new object();
    }
    public class MultipleComparer<T> : IComparer<T>
    {
        public MultipleComparer(params Comparison<T>[] comparers)
        {
            _comparers = comparers;
        }

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            foreach (Comparison<T> comparer in _comparers)
            {
                int v = comparer(x, y);

                if (v != 0)
                {
                    return v;
                }
            }

            return 0;
        }

        #endregion

        private Comparison<T>[] _comparers;
    }
    public sealed class NotFirst
    {
        public NotFirst(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            _action = action;
        }

        public void Invoke()
        {
            if (!_notfirst)
            {
                _notfirst = true;
            }
            else
            {
                _action();
            }
        }

        private bool _notfirst;
        private Action _action;
    }

    public delegate void Action();

    public sealed class Once
    {
        public Once(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            _action = action;
        }

        public void Invoke()
        {
            if (!_actioned)
            {
                _actioned = true;
                _action();
            }
        }

        private bool _actioned;
        private Action _action;
    }
}

namespace KellControls.Utility.Assemblies
{
    public sealed class ManifestResources
    {
        public ManifestResources(string baseNamespace)
            : this(baseNamespace, System.Reflection.Assembly.GetCallingAssembly())
        {
        }

        public string[] ResourceNames
        {
            get
            {
                return _assembly.GetManifestResourceNames();
            }
        }

        public ManifestResources(string baseNamespace, System.Reflection.Assembly assembly)
        {
            if (baseNamespace == null)
            {
                throw new ArgumentNullException("baseNamespace");
            }
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            _baseNamespace = baseNamespace;
            _assembly = assembly;
        }

        public System.Xml.XmlDocument GetXmlDocument(string path)
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

            using (System.IO.Stream stream = GetStream(path))
            {
                if (stream == null)
                {
                    throw new ArgumentException(string.Format("Resource '{0}' not found.", path), "path");
                }
                xmlDoc.Load(stream);
            }

            return xmlDoc;
        }

        public string GetString(string path)
        {
            using (System.IO.Stream stream = GetStream(path))
            using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }

        public System.IO.Stream GetStream(string path)
        {
            return _assembly.GetManifestResourceStream(_baseNamespace + "." + path);
        }

        public System.Drawing.Icon GetIcon(string path)
        {
            using (System.IO.Stream stream = GetStream(path))
            {
                return new System.Drawing.Icon(stream);
            }
        }

        public System.Drawing.Image GetImage(string path)
        {
            using (System.IO.Stream stream = GetStream(path))
            {
                return System.Drawing.Image.FromStream(stream);
            }
        }

        private string _baseNamespace;
        private System.Reflection.Assembly _assembly;
    }
    public class TypeIndex
    {
        public TypeIndex()
        {
        }

        public void AddAssembly(System.Reflection.Assembly assembly)
        {
            Details details = new Details();

            details.Assembly = assembly;

            foreach (Type type in assembly.GetTypes())
            {
                List<Type> types;

                if (details.TypeMap.ContainsKey(type.Name))
                {
                    types = details.TypeMap[type.Name];
                }
                else
                {
                    types = new List<Type>();
                    details.TypeMap[type.Name] = types;
                }

                types.Add(type);
            }

            _details.Add(details);
        }

        public Type[] GetTypes(string name)
        {
            List<Type> types = new List<Type>();

            foreach (Details details in _details)
            {
                if (details.TypeMap.ContainsKey(name))
                {
                    types.AddRange(details.TypeMap[name]);
                }
            }

            return types.ToArray();
        }

        public Type GetSingleType(string name)
        {
            Type[] types = GetTypes(name);

            if (types.Length == 0)
            {
                throw new Exception(string.Format("Type '{0}' not found in specified assemblies.", name));
            }
            else if (types.Length == 1)
            {
                return types[0];
            }
            else
            {
                throw new Exception(string.Format("Type '{0}' found multiple times in specified assemblies.", name));
            }
        }

        private class Details
        {
            internal System.Reflection.Assembly Assembly;
            internal Dictionary<string, List<Type>> TypeMap = new Dictionary<string, List<Type>>();
        }

        private List<Details> _details = new List<Details>();
    }
}

namespace KellControls.WinFormsUtility.Drawing
{
    using KellControls.Utility.Win32;
    using System.Drawing.Imaging;

    public static class GdiPlusEx
    {
        public enum Alignment
        {
            Left,
            Center,
            Right
        }
        public enum VAlignment
        {
            Top,
            Center,
            Bottom
        }
        public enum TextSplitting
        {
            SingleLineEllipsis,
            MultiLine
        }

        public enum Ampersands
        {
            Display,
            MakeShortcut
        }

        public static void DrawString(Graphics g, string text, Font font, Color color, Rectangle rect, TextSplitting textSplitting, Ampersands ampersands)
        {
            DrawString(g, text, font, color, rect, Alignment.Left, VAlignment.Top, textSplitting, ampersands);
        }

        public static void DrawString(Graphics g, string text, Font font, Color color, Rectangle rect, Alignment alignment, TextSplitting textSplitting, Ampersands ampersands)
        {
            DrawString(g, text, font, color, rect, Alignment.Left, VAlignment.Top, textSplitting, ampersands);
        }

        public static void DrawString(Graphics g, string text, Font font, Color color, Rectangle rect, Alignment alignment, VAlignment valignment, TextSplitting textSplitting, Ampersands ampersands)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            if (font == null)
            {
                throw new ArgumentNullException("font");
            }

            if (ampersands == Ampersands.Display)
            {
                text = text.Replace("&", "&&");
            }

            float[] txValues = g.Transform.Elements;
            IntPtr hClipRgn = g.Clip.GetHrgn(g);
            IntPtr hDC = g.GetHdc();

            Gdi.SelectClipRgn(hDC, hClipRgn);

            int oldGraphicsMode = Gdi.SetGraphicsMode(hDC, 2);
            XFORM oldXForm = new XFORM();

            Gdi.GetWorldTransform(hDC, ref oldXForm);

            XFORM newXForm = new XFORM();

            newXForm.eM11 = txValues[0];
            newXForm.eM12 = txValues[1];
            newXForm.eM21 = txValues[2];
            newXForm.eM22 = txValues[3];
            newXForm.eDx = txValues[4];
            newXForm.eDy = txValues[5];

            Gdi.SetWorldTransform(hDC, ref newXForm);

            try
            {
                IntPtr hFont = font.ToHfont();
                IntPtr hOldFont = Gdi.SelectObject(hDC, hFont);

                try
                {
                    Utility.Win32.Common.RECT r = new Utility.Win32.Common.RECT(rect);
                    User.DrawTextFlags uFormat;

                    switch (textSplitting)
                    {
                        case TextSplitting.SingleLineEllipsis:
                            uFormat
                                    = User.DrawTextFlags.DT_WORD_ELLIPSIS
                                        | User.DrawTextFlags.DT_END_ELLIPSIS;
                            break;
                        case TextSplitting.MultiLine:
                            uFormat
                                    = User.DrawTextFlags.DT_WORDBREAK;
                            break;
                        default:
                            throw new InvalidOperationException();
                    }

                    switch (alignment)
                    {
                        case Alignment.Left:
                            break;
                        case Alignment.Center:
                            uFormat
                                    = User.DrawTextFlags.DT_CENTER;
                            break;
                        case Alignment.Right:
                            uFormat
                                    = User.DrawTextFlags.DT_RIGHT;
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                    switch (valignment)
                    {
                        case VAlignment.Top:
                            break;
                        case VAlignment.Bottom:
                            uFormat |= User.DrawTextFlags.DT_BOTTOM | User.DrawTextFlags.DT_SINGLELINE;
                            break;
                        case VAlignment.Center:
                            uFormat |= User.DrawTextFlags.DT_VCENTER | User.DrawTextFlags.DT_SINGLELINE;
                            break;
                    }

                    uint bgr = (uint)((color.B << 16) | (color.G << 8) | (color.R));
                    uint oldColor = Gdi.SetTextColor(hDC, bgr);

                    try
                    {
                        BackgroundMode oldBackgroundMode = Gdi.SetBkMode(hDC, BackgroundMode.TRANSPARENT);

                        try
                        {
                            User.DrawText(hDC, text, text.Length, ref r, uFormat);
                        }
                        finally
                        {
                            Gdi.SetBkMode(hDC, oldBackgroundMode);
                        }
                    }
                    finally
                    {
                        Gdi.SetTextColor(hDC, oldColor);
                    }
                }
                finally
                {
                    Gdi.SelectObject(hDC, hOldFont);
                    Gdi.DeleteObject(hFont);
                }
            }
            finally
            {
                if (oldGraphicsMode == 1)
                {
                    oldXForm.eM11 = 1;
                    oldXForm.eM12 = 0;
                    oldXForm.eM21 = 0;
                    oldXForm.eM22 = 1;
                    oldXForm.eDx = 0;
                    oldXForm.eDx = 0;
                }

                Gdi.SetWorldTransform(hDC, ref oldXForm);
                Gdi.SetGraphicsMode(hDC, oldGraphicsMode);

                g.ReleaseHdc(hDC);

                if (hClipRgn != IntPtr.Zero)
                {
                    g.Clip.ReleaseHrgn(hClipRgn);
                }
            }
        }

        public static Size MeasureString(Graphics g, string text, Font font, int width)
        {
            Size size;
            TextDetails td = new TextDetails(text, font, width);

            if (_mapTextSizes.TryGetValue(td, out size))
            {
                return size;
            }

            IntPtr hDC = g.GetHdc();

            try
            {
                IntPtr hFont = font.ToHfont();

                try
                {
                    IntPtr hOldFont = Gdi.SelectObject(hDC, hFont);

                    try
                    {
                        Rectangle rect = new Rectangle(0, 0, width, 0);
                        Utility.Win32.Common.RECT r = new Utility.Win32.Common.RECT(rect);
                        Utility.Win32.User.DrawTextFlags uFormat = Utility.Win32.User.DrawTextFlags.DT_WORDBREAK | Utility.Win32.User.DrawTextFlags.DT_CALCRECT;

                        Utility.Win32.User.DrawText(hDC, text, text.Length, ref r, uFormat);

                        size = new Size(r.Right, r.Bottom);

                        _mapTextSizes[td] = size;

                        return size;
                    }
                    finally
                    {
                        Gdi.SelectObject(hDC, hOldFont);
                    }
                }
                finally
                {
                    Gdi.DeleteObject(hFont);
                }
            }
            finally
            {
                g.ReleaseHdc(hDC);
            }
        }

        public static void DrawRoundRect(Graphics g, Pen p, Rectangle rect, int radius)
        {
            DrawRoundRect(g, p, rect.X, rect.Y, rect.Width, rect.Height, radius);
        }

        public static void DrawRoundRect(Graphics g, Pen p, int x, int y, int width, int height, int radius)
        {
            if (width <= 0 || height <= 0)
            {
                return;
            }

            radius = Math.Min(radius, height / 2 - 1);
            radius = Math.Min(radius, width / 2 - 1);

            if (radius <= 0)
            {
                g.DrawRectangle(p, x, y, width, height);
                return;
            }

            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddLine(x + radius, y, x + width - (radius * 2), y);
                gp.AddArc(x + width - (radius * 2), y, radius * 2, radius * 2, 270, 90);
                gp.AddLine(x + width, y + radius, x + width, y + height - (radius * 2));
                gp.AddArc(x + width - (radius * 2), y + height - (radius * 2), radius * 2, radius * 2, 0, 90);
                gp.AddLine(x + width - (radius * 2), y + height, x + radius, y + height);
                gp.AddArc(x, y + height - (radius * 2), radius * 2, radius * 2, 90, 90);
                gp.AddLine(x, y + height - (radius * 2), x, y + radius);
                gp.AddArc(x, y, radius * 2, radius * 2, 180, 90);
                gp.CloseFigure();

                g.DrawPath(p, gp);
            }
        }

        public static void FillRoundRect(Graphics g, Brush b, Rectangle rect, int radius)
        {
            FillRoundRect(g, b, rect.X, rect.Y, rect.Width, rect.Height, radius);
        }

        public static void FillRoundRect(Graphics g, Brush b, int x, int y, int width, int height, int radius)
        {
            if (width <= 0 || height <= 0)
            {
                return;
            }

            radius = Math.Min(radius, height / 2);
            radius = Math.Min(radius, width / 2);

            if (radius == 0)
            {
                g.FillRectangle(b, x, y, width, height);
                return;
            }

            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddLine(x + radius, y, x + width - (radius * 2), y);
                gp.AddArc(x + width - (radius * 2), y, radius * 2, radius * 2, 270, 90);
                gp.AddLine(x + width, y + radius, x + width, y + height - (radius * 2));
                gp.AddArc(x + width - (radius * 2), y + height - (radius * 2), radius * 2, radius * 2, 0, 90);
                gp.AddLine(x + width - (radius * 2), y + height, x + radius, y + height);
                gp.AddArc(x, y + height - (radius * 2), radius * 2, radius * 2, 90, 90);
                gp.AddLine(x, y + height - (radius * 2), x, y + radius);
                gp.AddArc(x, y, radius * 2, radius * 2, 180, 90);
                gp.CloseFigure();

                g.FillPath(b, gp);
            }
        }

        public static IDisposable SaveState(Graphics g)
        {
            return new GraphicsStateDisposer(g);
        }

        public static Image MakeDisabledImage(Image source)
        {
            return MakeDisabledImage(source, SystemColors.GrayText);
        }

        public static Image MakeDisabledImage(Image source, Color greyColor)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            float w = (greyColor.R + greyColor.G + greyColor.B) / 765.0f;
            float r = greyColor.R / 255.0f / 2;
            float g = greyColor.G / 255.0f / 2;
            float b = greyColor.B / 255.0f / 2;

            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
				{
					new float[] { w, w, w, 0, 0 },
					new float[] { w, w, w, 0, 0 },
					new float[] { w, w, w, 0, 0 },
					new float[] { 0, 0, 0, 1, 0 },
					new float[] { r, g, b, 0, 1 }
				});

            ImageAttributes imageAttributes = new ImageAttributes();

            imageAttributes.SetColorMatrix(colorMatrix);

            Image disabled = (Image)source.Clone();

            using (Graphics graphics = Graphics.FromImage(disabled))
            {
                graphics.DrawImage(source, new Rectangle(0, 0, disabled.Width, disabled.Width), 0, 0, source.Width, source.Height, GraphicsUnit.Pixel, imageAttributes);
            }

            return disabled;
        }

        #region GraphicsStateDisposer

        private sealed class GraphicsStateDisposer : IDisposable
        {
            internal GraphicsStateDisposer(Graphics g)
            {
                _g = g;
                _state = _g.Save();
            }

            #region IDisposable Members

            public void Dispose()
            {
                if (_g != null)
                {
                    _g.Restore(_state);
                    _g = null;
                    _state = null;
                }
            }

            #endregion

            private Graphics _g;
            private GraphicsState _state;
        }

        #endregion

        #region TextDetails

        private sealed class TextDetails
        {
            internal TextDetails(string text, Font font, int width)
            {
                _text = text;
                _font = font;
                _width = width;
            }

            public override int GetHashCode()
            {
                return _text.GetHashCode() ^ _font.GetHashCode() ^ _width;
            }

            public override bool Equals(object obj)
            {
                TextDetails td = obj as TextDetails;

                if (td == null)
                {
                    return false;
                }

                return _text == td._text && _font.Equals(td._font) && _width == td._width;
            }

            private string _text;
            private Font _font;
            private int _width;
        }

        #endregion

        private static Dictionary<TextDetails, Size> _mapTextSizes = new Dictionary<TextDetails, Size>();
    }
    public sealed class Glass
    {
        public Glass()
        {
            if (!IsEnabled())
            {
                throw new InvalidOperationException();
            }
        }

        public bool Ignore(Control owner)
        {
            Form f = owner.FindForm();

            if (f == null)
            {
                return false;
            }

            return f.WindowState == FormWindowState.Maximized;
        }

        public static bool IsEnabled()
        {
            if (Environment.OSVersion.Version.Major < 6)
            {
                return false;
            }

            bool isGlassSupported = false;

            Utility.Win32.Dwmapi.DwmIsCompositionEnabled(ref isGlassSupported);

            return isGlassSupported;
        }

        public void ExtendGlassIntoClientArea(Form form, int top, int bottom, int left, int right)
        {
            if (!form.IsHandleCreated)
            {
                return;
            }

            Utility.Win32.Margins margins = new Utility.Win32.Margins();

            margins.Left = left;
            margins.Right = right;
            margins.Top = top;
            margins.Bottom = bottom;

            Utility.Win32.Dwmapi.DwmExtendFrameIntoClientArea(form.Handle, ref margins);
        }
    }
    public static class ColorUtil
    {
        public static Color Combine(Color c1, Color c2, double proportion)
        {
            double iprop = 1 - proportion;
            int r = (int)(c1.R * proportion + c2.R * iprop);
            int g = (int)(c1.G * proportion + c2.G * iprop);
            int b = (int)(c1.B * proportion + c2.B * iprop);

            r = Math.Min(Math.Max(0, r), 255);
            g = Math.Min(Math.Max(0, g), 255);
            b = Math.Min(Math.Max(0, b), 255);

            return Color.FromArgb(r, g, b);
        }

        public static Color ModifySaturation(Color c, double change)
        {
            double h = c.GetHue() / 360.0;
            double s = c.GetSaturation();
            double l = c.GetBrightness();

            s *= change;

            s = Math.Min(Math.Max(0, s), 1);

            return FromHSL(h, s, l);
        }

        public static Color ModifyHue(Color c, double change)
        {
            double h = c.GetHue() / 360.0;
            double s = c.GetSaturation();
            double l = c.GetBrightness();

            h += change;

            return FromHSL(h, s, l);
        }

        public static Color ModifyLight(Color c, double change)
        {
            double h = c.GetHue() / 360.0;
            double s = c.GetSaturation();
            double l = c.GetBrightness();

            l *= change;

            l = Math.Min(Math.Max(0, l), 1);

            return FromHSL(h, s, l);
        }

        public static Color FromHSL(double h, double s, double l)
        {
            double r = 0, g = 0, b = 0;
            double temp1, temp2;

            if (l == 0)
            {
                r = g = b = 0;
            }
            else
            {
                if (s == 0)
                {
                    r = g = b = l;
                }
                else
                {
                    temp2 = ((l <= 0.5) ? l * (1.0 + s) : l + s - (l * s));
                    temp1 = 2.0 * l - temp2;

                    double[] t3 = new double[] { h + 1.0 / 3.0, h, h - 1.0 / 3.0 };
                    double[] clr = new double[] { 0, 0, 0 };

                    for (int i = 0; i < 3; i++)
                    {
                        if (t3[i] < 0)
                        {
                            t3[i] += 1.0;
                        }
                        if (t3[i] > 1)
                        {
                            t3[i] -= 1.0;
                        }

                        if (6.0 * t3[i] < 1.0)
                        {
                            clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0;
                        }
                        else if (2.0 * t3[i] < 1.0)
                        {
                            clr[i] = temp2;
                        }
                        else if (3.0 * t3[i] < 2.0)
                        {
                            clr[i] = (temp1 + (temp2 - temp1) * ((2.0 / 3.0) - t3[i]) * 6.0);
                        }
                        else
                        {
                            clr[i] = temp1;
                        }
                    }

                    r = clr[0];
                    g = clr[1];
                    b = clr[2];
                }
            }

            return Color.FromArgb((int)(255 * r), (int)(255 * g), (int)(255 * b));
        }
    }
    public abstract class Animation
    {
        protected Animation()
        {
        }

        public abstract void OnPaint(Graphics g, Rectangle drawingBounds, bool running, double seconds);

        public virtual bool IsDone(double seconds)
        {
            return false;
        }

        public virtual double GetSuggestedAlpha(double seconds)
        {
            return 1;
        }
    }
}

namespace KellControls.WinFormsUtility.Win32
{
    public static class BroadcastMessages
    {
        private static void OnPowerSuspend(EventArgs e)
        {
            if (!_suspended)
            {
                _suspended = true;

                if (_powerSuspend != null)
                {
                    _powerSuspend(null, e);
                }
            }
        }

        private static void OnPowerResume(EventArgs e)
        {
            if (_suspended)
            {
                _suspended = false;

                if (_powerResume != null)
                {
                    _powerResume(null, e);
                }
            }
        }

        private static void EnsureForm()
        {
            if (_form == null)
            {
                _form = new BroadcastMessageForm();
            }
        }

        public static event EventHandler PowerSuspend
        {
            add
            {
                EnsureForm();

                _powerSuspend += value;
            }
            remove
            {
                _powerSuspend -= value;
            }
        }

        public static event EventHandler PowerResume
        {
            add
            {
                EnsureForm();

                _powerResume += value;
            }
            remove
            {
                _powerResume -= value;
            }
        }

        #region BroadcastMessageForm

        private sealed class BroadcastMessageForm : Form
        {
            internal BroadcastMessageForm()
            {
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                Location = new System.Drawing.Point(-32000, -32000);
                Name = "BroadcastMessageForm";
                Opacity = 0;
                ShowInTaskbar = false;
                StartPosition = System.Windows.Forms.FormStartPosition.Manual;

                CreateHandle();
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                if (m.Msg == (int)Utility.Win32.Messages.WM_POWERBROADCAST)
                {
                    switch ((int)m.WParam)
                    {
                        case PBT_APMSUSPEND:
                        case PBT_APMSTANDBY:
                            BroadcastMessages.OnPowerSuspend(EventArgs.Empty);
                            break;
                        case PBT_APMRESUMECRITICAL:
                        case PBT_APMRESUMESUSPEND:
                        case PBT_APMRESUMESTANDBY:
                        case PBT_APMRESUMEAUTOMATIC:
                            BroadcastMessages.OnPowerResume(EventArgs.Empty);
                            break;
                    }
                }
            }

            private const int PBT_APMSUSPEND = 0x0004;
            private const int PBT_APMSTANDBY = 0x0005;
            private const int PBT_APMRESUMECRITICAL = 0x0006;
            private const int PBT_APMRESUMESUSPEND = 0x0007;
            private const int PBT_APMRESUMESTANDBY = 0x0008;
            private const int PBT_APMRESUMEAUTOMATIC = 0x0012;
        }

        #endregion

        private static BroadcastMessageForm _form;
        private static event EventHandler _powerSuspend, _powerResume;
        private static bool _suspended;
    }
    [CLSCompliant(false)]
    public class PerPixelAlphaForm : PopupWindow
    {
        public PerPixelAlphaForm()
        {
            FormBorderStyle = FormBorderStyle.None;
        }

        protected void SetBitmap(Bitmap bitmap)
        {
            SetBitmap(bitmap, 255);
        }

        protected void SetBitmap(Bitmap bitmap, byte opacity)
        {
            if (!IsHandleCreated)
            {
                return;
            }
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");
            }

            IntPtr screenDc = Utility.Win32.User.GetDC(IntPtr.Zero);
            IntPtr memDc = Utility.Win32.Gdi.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;

            try
            {
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));  // grab a GDI handle from this GDI+ bitmap
                oldBitmap = Utility.Win32.Gdi.SelectObject(memDc, hBitmap);

                Utility.Win32.Common.SIZE size = new Utility.Win32.Common.SIZE(bitmap.Width, bitmap.Height);
                Utility.Win32.Common.POINT pointSource = new Utility.Win32.Common.POINT(0, 0);
                Utility.Win32.Common.POINT topPos = new Utility.Win32.Common.POINT(Left, Top);
                Utility.Win32.BLENDFUNCTION blend = new Utility.Win32.BLENDFUNCTION();

                blend.BlendOp = AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = opacity;
                blend.AlphaFormat = AC_SRC_ALPHA;

                Utility.Win32.User.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, ULW_ALPHA);
            }
            finally
            {
                Utility.Win32.Gdi.ReleaseDC(IntPtr.Zero, screenDc);

                if (hBitmap != IntPtr.Zero)
                {
                    Utility.Win32.Gdi.SelectObject(memDc, oldBitmap);
                    Utility.Win32.Gdi.DeleteObject(hBitmap);
                }
                Utility.Win32.Gdi.DeleteDC(memDc);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                cp.ExStyle |= (int)Utility.Win32.WindowStylesEx.WS_EX_LAYERED;

                return cp;
            }
        }

        private const Int32 ULW_COLORKEY = 0x00000001;
        private const Int32 ULW_ALPHA = 0x00000002;
        private const Int32 ULW_OPAQUE = 0x00000004;

        private const byte AC_SRC_OVER = 0x00;
        private const byte AC_SRC_ALPHA = 0x01;
    }
    [CLSCompliant(false)]
    public class PopupWindow : Form
    {
        protected PopupWindow()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = new CreateParams();

                createParams.X = Location.X;
                createParams.Y = Location.Y;
                createParams.Height = Size.Height;
                createParams.Width = Size.Width;

                createParams.ClassStyle = unchecked((int)ClassStyle);
                createParams.Parent = IntPtr.Zero;
                createParams.Style = unchecked((int)WindowStyles);
                createParams.ExStyle = unchecked((int)WindowStylesEx);

                return createParams;
            }
        }

        protected virtual Utility.Win32.ClassStyle ClassStyle
        {
            get
            {
                return 0;
            }
        }

        protected virtual Utility.Win32.WindowStyles WindowStyles
        {
            get
            {
                return Utility.Win32.WindowStyles.WS_POPUP;
            }
        }

        protected virtual Utility.Win32.WindowStylesEx WindowStylesEx
        {
            get
            {
                return
                        Utility.Win32.WindowStylesEx.WS_EX_TOOLWINDOW |
                        Utility.Win32.WindowStylesEx.WS_EX_NOACTIVATE;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                IntPtr HWND_TOPMOST = new IntPtr(-1);

                Utility.Win32.User.SetWindowPos
                    (Handle, HWND_TOPMOST, 0, 0, 0, 0
                    , Utility.Win32.SetWindowPosOptions.SWP_NOSIZE | Utility.Win32.SetWindowPosOptions.SWP_NOMOVE
                    | Utility.Win32.SetWindowPosOptions.SWP_NOACTIVATE | Utility.Win32.SetWindowPosOptions.SWP_NOREDRAW);
            }
        }

        protected override bool ShowWithoutActivation
        {
            get
            {
                return true;
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)Utility.Win32.Messages.WM_LBUTTONDOWN:
                    OnClick(EventArgs.Empty);
                    break;
            }

            if (!this.IsDisposed)
            {
                base.WndProc(ref m);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PopupWindow
            // 
            this.ClientSize = new System.Drawing.Size(292, 271);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PopupWindow";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }
    }
}

namespace KellControls.Utility.Win32.Common
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public RECT(Rectangle rect)
        {
            Left = rect.Left;
            Top = rect.Top;
            Right = rect.Right;
            Bottom = rect.Bottom;
        }

        public Rectangle Rect
        {
            get
            {
                return new Rectangle(Left, Top, Right - Left, Bottom - Top);
            }
        }

        public Point Location
        {
            get
            {
                return new Point(Left, Top);
            }
        }

        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {

        public POINT(Int32 x, Int32 y)
        {
            X = x;
            Y = y;
        }

        public Int32 X;
        public Int32 Y;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public SIZE(Int32 cx, Int32 cy)
        {
            CX = cx;
            CY = cy;
        }

        public Int32 CX;
        public Int32 CY;
    }
}

namespace KellControls.Utility.Win32
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DWM_BLURBEHIND
    {
        public int dwFlags;
        public bool fEnable;
        public System.IntPtr hRgnBlur;
        public bool fTransitionOnMaximized;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Margins
    {
        public int Left, Right, Top, Bottom;
    }

    public sealed class Dwmapi
    {
        [DllImport("dwmapi.dll")]
        public static extern void DwmIsCompositionEnabled(ref bool pfEnabled);

        [DllImport("dwmapi.dll")]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMargins);

        [DllImport("dwmapi")]
        public static extern int DwmEnableBlurBehindWindow(IntPtr hWnd, ref DWM_BLURBEHIND pBlurBehind);
    }

    public enum BackgroundMode : int
    {
        TRANSPARENT = 1,
        OPAQUE = 2
    }

    public enum TernaryRasterOperations
    {
        SRCCOPY = 0x00CC0020,
        SRCPAINT = 0x00EE0086,
        SRCAND = 0x008800C6,
        SRCINVERT = 0x00660046,
        SRCERASE = 0x00440328,
        NOTSRCCOPY = 0x00330008,
        NOTSRCERASE = 0x001100A6,
        MERGECOPY = 0x00C000CA,
        MERGEPAINT = 0x00BB0226,
        PATCOPY = 0x00F00021,
        PATPAINT = 0x00FB0A09,
        PATINVERT = 0x005A0049,
        DSTINVERT = 0x00550009,
        BLACKNESS = 0x00000042,
        WHITENESS = 0x00FF0062,
    };

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct XFORM
    {
        public float eM11;
        public float eM12;
        public float eM21;
        public float eM22;
        public float eDx;
        public float eDy;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BLENDFUNCTION
    {
        public byte BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public byte AlphaFormat;
    }

    [CLSCompliant(false)]
    [StructLayout(LayoutKind.Sequential)]
    public struct DRAWTEXTPARAMS
    {
        public uint cbSize;
        public int iTabLength;
        public int iLeftMargin;
        public int iRightMargin;
        public uint uiLengthDrawn;
    }

    [CLSCompliant(false)]
    public sealed class Gdi
    {
        [DllImport("gdi32.dll")]
        public extern static bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern int GetClipBox(IntPtr hdc, out Common.RECT lprc);

        [DllImport("gdi32.dll")]
        public static extern int GetClipRgn(IntPtr hdc, IntPtr hrgn);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public extern static IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public extern static uint SetTextColor(IntPtr hdc, uint crColor);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public extern static BackgroundMode SetBkMode(IntPtr hdc, BackgroundMode iBkMode);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public extern static int SetWorldTransform(IntPtr hdc, ref XFORM lpXform);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public extern static int GetWorldTransform(IntPtr hdc, ref XFORM lpXform);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public extern static int SetGraphicsMode(IntPtr hdc, int iMode);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public extern static IntPtr GetStockObject(int fnObject);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern int SelectClipRgn(IntPtr hdc, IntPtr hrgn);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth,
             int nHeight, IntPtr hObjSource, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);
    }
    [CLSCompliant(false)]
    public static class Kernel
    {
        [DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenEvent(uint dwDesiredAccess, bool bInheritHandle, string lpName);

        [DllImport("kernel32", SetLastError = true)]
        public static extern Int32 WaitForSingleObject(IntPtr handle, Int32 milliseconds);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalFree(IntPtr hMem);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        public static extern bool GlobalUnlock(IntPtr hMem);
    }

    public enum Messages
    {
        WM_NULL = 0x0000,
        WM_CREATE = 0x0001,
        WM_DESTROY = 0x0002,
        WM_MOVE = 0x0003,
        WM_SIZE = 0x0005,
        WM_ACTIVATE = 0x0006,
        WM_SETFOCUS = 0x0007,
        WM_KILLFOCUS = 0x0008,
        WM_ENABLE = 0x000A,
        WM_SETREDRAW = 0x000B,
        WM_SETTEXT = 0x000C,
        WM_GETTEXT = 0x000D,
        WM_GETTEXTLENGTH = 0x000E,
        WM_PAINT = 0x000F,
        WM_CLOSE = 0x0010,
        WM_QUERYENDSESSION = 0x0011,
        WM_QUIT = 0x0012,
        WM_QUERYOPEN = 0x0013,
        WM_ERASEBKGND = 0x0014,
        WM_SYSCOLORCHANGE = 0x0015,
        WM_ENDSESSION = 0x0016,
        WM_SHOWWINDOW = 0x0018,
        WM_WININICHANGE = 0x001A,
        WM_SETTINGCHANGE = 0x001A,
        WM_DEVMODECHANGE = 0x001B,
        WM_ACTIVATEAPP = 0x001C,
        WM_FONTCHANGE = 0x001D,
        WM_TIMECHANGE = 0x001E,
        WM_CANCELMODE = 0x001F,
        WM_SETCURSOR = 0x0020,
        WM_MOUSEACTIVATE = 0x0021,
        WM_CHILDACTIVATE = 0x0022,
        WM_QUEUESYNC = 0x0023,
        WM_GETMINMAXINFO = 0x0024,
        WM_PAINTICON = 0x0026,
        WM_ICONERASEBKGND = 0x0027,
        WM_NEXTDLGCTL = 0x0028,
        WM_SPOOLERSTATUS = 0x002A,
        WM_DRAWITEM = 0x002B,
        WM_MEASUREITEM = 0x002C,
        WM_DELETEITEM = 0x002D,
        WM_VKEYTOITEM = 0x002E,
        WM_CHARTOITEM = 0x002F,
        WM_SETFONT = 0x0030,
        WM_GETFONT = 0x0031,
        WM_SETHOTKEY = 0x0032,
        WM_GETHOTKEY = 0x0033,
        WM_QUERYDRAGICON = 0x0037,
        WM_COMPAREITEM = 0x0039,
        WM_GETOBJECT = 0x003D,
        WM_COMPACTING = 0x0041,
        WM_COMMNOTIFY = 0x0044,
        WM_WINDOWPOSCHANGING = 0x0046,
        WM_WINDOWPOSCHANGED = 0x0047,
        WM_POWER = 0x0048,
        WM_COPYDATA = 0x004A,
        WM_CANCELJOURNAL = 0x004B,
        WM_NOTIFY = 0x004E,
        WM_INPUTLANGCHANGEREQUEST = 0x0050,
        WM_INPUTLANGCHANGE = 0x0051,
        WM_TCARD = 0x0052,
        WM_HELP = 0x0053,
        WM_USERCHANGED = 0x0054,
        WM_NOTIFYFORMAT = 0x0055,
        WM_CONTEXTMENU = 0x007B,
        WM_STYLECHANGING = 0x007C,
        WM_STYLECHANGED = 0x007D,
        WM_DISPLAYCHANGE = 0x007E,
        WM_GETICON = 0x007F,
        WM_SETICON = 0x0080,
        WM_NCCREATE = 0x0081,
        WM_NCDESTROY = 0x0082,
        WM_NCCALCSIZE = 0x0083,
        WM_NCHITTEST = 0x0084,
        WM_NCPAINT = 0x0085,
        WM_NCACTIVATE = 0x0086,
        WM_GETDLGCODE = 0x0087,
        WM_SYNCPAINT = 0x0088,
        WM_NCMOUSEMOVE = 0x00A0,
        WM_NCMOUSELEAVE = 0x02A2,
        WM_NCLBUTTONDOWN = 0x00A1,
        WM_NCLBUTTONUP = 0x00A2,
        WM_NCLBUTTONDBLCLK = 0x00A3,
        WM_NCRBUTTONDOWN = 0x00A4,
        WM_NCRBUTTONUP = 0x00A5,
        WM_NCRBUTTONDBLCLK = 0x00A6,
        WM_NCMBUTTONDOWN = 0x00A7,
        WM_NCMBUTTONUP = 0x00A8,
        WM_NCMBUTTONDBLCLK = 0x00A9,
        WM_NCXBUTTONDOWN = 0x00AB,
        WM_NCXBUTTONUP = 0x00AC,
        WM_NCUAHDRAWCAPTION = 0x00AE,
        WM_NCUAHDRAWFRAME = 0x00AF,
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x0101,
        WM_CHAR = 0x0102,
        WM_DEADCHAR = 0x0103,
        WM_SYSKEYDOWN = 0x0104,
        WM_SYSKEYUP = 0x0105,
        WM_SYSCHAR = 0x0106,
        WM_SYSDEADCHAR = 0x0107,
        WM_KEYLAST = 0x0108,
        WM_IME_STARTCOMPOSITION = 0x010D,
        WM_IME_ENDCOMPOSITION = 0x010E,
        WM_IME_COMPOSITION = 0x010F,
        WM_IME_KEYLAST = 0x010F,
        WM_INITDIALOG = 0x0110,
        WM_COMMAND = 0x0111,
        WM_SYSCOMMAND = 0x0112,
        WM_TIMER = 0x0113,
        WM_HSCROLL = 0x0114,
        WM_VSCROLL = 0x0115,
        WM_INITMENU = 0x0116,
        WM_INITMENUPOPUP = 0x0117,
        WM_MENUSELECT = 0x011F,
        WM_MENUCHAR = 0x0120,
        WM_ENTERIDLE = 0x0121,
        WM_MENURBUTTONUP = 0x0122,
        WM_MENUDRAG = 0x0123,
        WM_MENUGETOBJECT = 0x0124,
        WM_UNINITMENUPOPUP = 0x0125,
        WM_MENUCOMMAND = 0x0126,
        WM_CTLCOLORMSGBOX = 0x0132,
        WM_CTLCOLOREDIT = 0x0133,
        WM_CTLCOLORLISTBOX = 0x0134,
        WM_CTLCOLORBTN = 0x0135,
        WM_CTLCOLORDLG = 0x0136,
        WM_CTLCOLORSCROLLBAR = 0x0137,
        WM_CTLCOLORSTATIC = 0x0138,
        WM_MOUSEMOVE = 0x0200,
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_LBUTTONDBLCLK = 0x0203,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_RBUTTONDBLCLK = 0x0206,
        WM_MBUTTONDOWN = 0x0207,
        WM_MBUTTONUP = 0x0208,
        WM_MBUTTONDBLCLK = 0x0209,
        WM_MOUSEWHEEL = 0x020A,
        WM_XBUTTONDOWN = 0x020B,
        WM_XBUTTONUP = 0x020C,
        WM_XBUTTONDBLCLK = 0x020D,
        WM_PARENTNOTIFY = 0x0210,
        WM_ENTERMENULOOP = 0x0211,
        WM_EXITMENULOOP = 0x0212,
        WM_NEXTMENU = 0x0213,
        WM_SIZING = 0x0214,
        WM_CAPTURECHANGED = 0x0215,
        WM_MOVING = 0x0216,
        WM_POWERBROADCAST = 0x0218,
        WM_DEVICECHANGE = 0x0219,
        WM_MDICREATE = 0x0220,
        WM_MDIDESTROY = 0x0221,
        WM_MDIACTIVATE = 0x0222,
        WM_MDIRESTORE = 0x0223,
        WM_MDINEXT = 0x0224,
        WM_MDIMAXIMIZE = 0x0225,
        WM_MDITILE = 0x0226,
        WM_MDICASCADE = 0x0227,
        WM_MDIICONARRANGE = 0x0228,
        WM_MDIGETACTIVE = 0x0229,
        WM_MDISETMENU = 0x0230,
        WM_ENTERSIZEMOVE = 0x0231,
        WM_EXITSIZEMOVE = 0x0232,
        WM_DROPFILES = 0x0233,
        WM_MDIREFRESHMENU = 0x0234,
        WM_IME_SETCONTEXT = 0x0281,
        WM_IME_NOTIFY = 0x0282,
        WM_IME_CONTROL = 0x0283,
        WM_IME_COMPOSITIONFULL = 0x0284,
        WM_IME_SELECT = 0x0285,
        WM_IME_CHAR = 0x0286,
        WM_IME_REQUEST = 0x0288,
        WM_IME_KEYDOWN = 0x0290,
        WM_IME_KEYUP = 0x0291,
        WM_MOUSEHOVER = 0x02A1,
        WM_MOUSELEAVE = 0x02A3,
        WM_CUT = 0x0300,
        WM_COPY = 0x0301,
        WM_PASTE = 0x0302,
        WM_CLEAR = 0x0303,
        WM_UNDO = 0x0304,
        WM_RENDERFORMAT = 0x0305,
        WM_RENDERALLFORMATS = 0x0306,
        WM_DESTROYCLIPBOARD = 0x0307,
        WM_DRAWCLIPBOARD = 0x0308,
        WM_PAINTCLIPBOARD = 0x0309,
        WM_VSCROLLCLIPBOARD = 0x030A,
        WM_SIZECLIPBOARD = 0x030B,
        WM_ASKCBFORMATNAME = 0x030C,
        WM_CHANGECBCHAIN = 0x030D,
        WM_HSCROLLCLIPBOARD = 0x030E,
        WM_QUERYNEWPALETTE = 0x030F,
        WM_PALETTEISCHANGING = 0x0310,
        WM_PALETTECHANGED = 0x0311,
        WM_HOTKEY = 0x0312,
        WM_PRINT = 0x0317,
        WM_PRINTCLIENT = 0x0318,
        WM_APPCOMMAND = 0x0319,
        WM_HANDHELDFIRST = 0x0358,
        WM_HANDHELDLAST = 0x035F,
        WM_AFXFIRST = 0x0360,
        WM_AFXLAST = 0x037F,
        WM_PENWINFIRST = 0x0380,
        WM_PENWINLAST = 0x038F,
        WM_APP = 0x8000,
        WM_USER = 0x0400,
        WM_MOUSEFIRST = 0x0200,
        WM_MOUSELAST = 0x020D
    }
    public static class Uac
    {
        public static bool IsSupported
        {
            get
            {
                return Environment.OSVersion.Version.Major >= 6;
            }
        }

        public class ShieldButton : Button
        {
            public ShieldButton()
            {
                FlatStyle = FlatStyle.System;
            }

            protected override void OnHandleCreated(EventArgs e)
            {
                base.OnHandleCreated(e);

                if (Uac.IsSupported)
                {
                    SendMessage(new HandleRef(this, this.Handle), BCM_SETSHIELD, IntPtr.Zero, new IntPtr(1));
                }
            }

            private const int BS_COMMANDLINK = 0x0000000E;
            private const uint BCM_SETNOTE = 0x00001609;
            private const uint BCM_SETSHIELD = 0x0000160C;

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            private static extern IntPtr SendMessage(HandleRef hWnd, UInt32 Msg, IntPtr wParam, string lParam);

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            private static extern IntPtr SendMessage(HandleRef hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

            private string _noteText = string.Empty;
        }
    }
    [CLSCompliant(false)]
    public static class User
    {
        [Flags]
        public enum DrawTextFlags : uint
        {
            DT_TOP = 0x0,
            DT_LEFT = 0x0,
            DT_CENTER = 0x1,
            DT_RIGHT = 0x2,
            DT_VCENTER = 0x4,
            DT_BOTTOM = 0x8,
            DT_WORDBREAK = 0x10,
            DT_SINGLELINE = 0x20,
            DT_EXPANDTABS = 0x40,
            DT_TABSTOP = 0x80,
            DT_NOCLIP = 0x100,
            DT_EXTERNALLEADING = 0x200,
            DT_CALCRECT = 0x400,
            DT_NOPREFIX = 0x800,
            DT_INTERNAL = 0x1000,
            DT_EDITCONTROL = 0x2000,
            DT_PATH_ELLIPSIS = 0x4000,
            DT_END_ELLIPSIS = 0x8000,
            DT_MODIFYSTRING = 0x10000,
            DT_RTLREADING = 0x20000,
            DT_WORD_ELLIPSIS = 0x40000,
            DT_NOFULLWIDTHCHARBREAK = 0x80000,
            DT_HIDEPREFIX = 0x100000,
            DT_PREFIXONLY = 0x200000
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public extern static bool DestroyIcon(IntPtr handle);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public extern static int DrawText(IntPtr hDC, string lpString, int nCount, ref Common.RECT lpRect, DrawTextFlags uFormat);

        [DllImport("user32.dll")]
        public static extern int DrawTextEx(IntPtr hdc, StringBuilder lpchText, int cchText, ref Common.RECT lprc, Win32.User.DrawTextFlags dwDTFormat, ref DRAWTEXTPARAMS lpDTParams);
        [DllImport("user32.dll")]
        public static extern int DrawTextEx(IntPtr hdc, StringBuilder lpchText, int cchText, ref Common.RECT lprc, Win32.User.DrawTextFlags dwDTFormat, IntPtr lpDTParams);

        [DllImport("user32.dll")]
        public extern static IntPtr SetActiveWindow(IntPtr handle);

        [DllImport("user32.dll")]
        public static extern uint RegisterWindowMessage(string lpString);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, Messages wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Common.POINT pptDst, ref Common.SIZE psize, IntPtr hdcSrc, ref Common.POINT pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void DisableProcessWindowsGhosting();

        [DllImport("user32.dll")]
        public static extern IntPtr GetDCEx(IntPtr hwnd, IntPtr hrgnclip, uint fdwOptions);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern Int32 SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, Int32 x, Int32 y, Int32 cx, Int32 cy, UInt32 uFlags);

        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr rectUpdate, IntPtr hrgnUpdate, RedrawWindowOptions flags);

        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr hWnd, ref Common.RECT rectUpdate, IntPtr hrgnUpdate, RedrawWindowOptions flags);

        [DllImport("user32.dll")]
        public static extern bool PeekMessage(ref System.Windows.Forms.Message msg, IntPtr hwnd, int msgMin, int msgMax, int remove);

        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, Utility.Win32.Messages Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        public static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SetWindowPosOptions uFlags);

        [DllImport("user32.dll")]
        public static extern int ShowWindow(IntPtr hWnd, ShowWindowOptions nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out Common.RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out Common.RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr GetFocus();
    }
    public static class UxTheme
    {
        [DllImport("uxtheme.dll")]
        public static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);
    }
    [Flags]
    [CLSCompliant(false)]
    public enum WindowStyles : uint
    {
        WS_OVERLAPPED = 0x00000000,
        WS_POPUP = 0x80000000,
        WS_CHILD = 0x40000000,
        WS_MINIMIZE = 0x20000000,
        WS_VISIBLE = 0x10000000,
        WS_DISABLED = 0x08000000,
        WS_CLIPSIBLINGS = 0x04000000,
        WS_CLIPCHILDREN = 0x02000000,
        WS_MAXIMIZE = 0x01000000,
        WS_BORDER = 0x00800000,
        WS_DLGFRAME = 0x00400000,
        WS_VSCROLL = 0x00200000,
        WS_HSCROLL = 0x00100000,
        WS_SYSMENU = 0x00080000,
        WS_THICKFRAME = 0x00040000,
        WS_GROUP = 0x00020000,
        WS_TABSTOP = 0x00010000,

        WS_MINIMIZEBOX = 0x00020000,
        WS_MAXIMIZEBOX = 0x00010000,

        WS_CAPTION = WS_BORDER | WS_DLGFRAME,
        WS_TILED = WS_OVERLAPPED,
        WS_ICONIC = WS_MINIMIZE,
        WS_SIZEBOX = WS_THICKFRAME,
        WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,

        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
        WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
        WS_CHILDWINDOW = WS_CHILD,
    }

    [Flags]
    [CLSCompliant(false)]
    public enum WindowStylesEx : uint
    {
        WS_EX_DLGMODALFRAME = 0x00000001,
        WS_EX_NOPARENTNOTIFY = 0x00000004,
        WS_EX_TOPMOST = 0x00000008,
        WS_EX_ACCEPTFILES = 0x00000010,
        WS_EX_TRANSPARENT = 0x00000020,
        WS_EX_MDICHILD = 0x00000040,
        WS_EX_TOOLWINDOW = 0x00000080,
        WS_EX_WINDOWEDGE = 0x00000100,
        WS_EX_CLIENTEDGE = 0x00000200,
        WS_EX_CONTEXTHELP = 0x00000400,
        WS_EX_RIGHT = 0x00001000,
        WS_EX_LEFT = 0x00000000,
        WS_EX_RTLREADING = 0x00002000,
        WS_EX_LTRREADING = 0x00000000,
        WS_EX_LEFTSCROLLBAR = 0x00004000,
        WS_EX_RIGHTSCROLLBAR = 0x00000000,
        WS_EX_CONTROLPARENT = 0x00010000,
        WS_EX_STATICEDGE = 0x00020000,
        WS_EX_APPWINDOW = 0x00040000,

        WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE),
        WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST),

        WS_EX_LAYERED = 0x00080000,

        WS_EX_NOINHERITLAYOUT = 0x00100000,
        WS_EX_LAYOUTRTL = 0x00400000,

        WS_EX_COMPOSITED = 0x02000000,
        WS_EX_NOACTIVATE = 0x08000000,
    }

    [Flags]
    public enum ClassStyle
    {
        CS_DROPSHADOW = 0x00020000
    }

    public enum RegionValue
    {
        ERROR = 0,
        NULLREGION = 1,
        SIMPLEREGION = 2,
        COMPLEXREGION = 3,
        RGN_AND = 1
    }

    public enum NCHITTEST
    {
        HTTRANSPARENT = -1,
        HTNOWHERE = 0,
        HTCLIENT = 1,
        HTCAPTION = 2,
        HTSYSMENU = 3,
        HTGROWBOX = 4,
        HTSIZE = HTGROWBOX,
        HTMENU = 5,
        HTHSCROLL = 6,
        HTVSCROLL = 7,
        HTMINBUTTON = 8,
        HTMAXBUTTON = 9,
        HTLEFT = 10,
        HTRIGHT = 11,
        HTTOP = 12,
        HTTOPLEFT = 13,
        HTTOPRIGHT = 14,
        HTBOTTOM = 15,
        HTBOTTOMLEFT = 16,
        HTBOTTOMRIGHT = 17,
        HTBORDER = 18,
        HTREDUCE = HTMINBUTTON,
        HTZOOM = HTMAXBUTTON,
        HTSIZEFIRST = HTLEFT,
        HTSIZELAST = HTBOTTOMRIGHT,
        HTOBJECT = 19,
        HTCLOSE = 20,
        HTHELP = 21,
    }

    [Flags]
    public enum SetWindowPosOptions
    {
        SWP_NOSIZE = 0x0001,
        SWP_NOMOVE = 0x0002,
        SWP_NOZORDER = 0x0004,
        SWP_NOREDRAW = 0x0008,
        SWP_NOACTIVATE = 0x0010,
        SWP_FRAMECHANGED = 0x0020,	/* The frame changed: send WM_NCCALCSIZE */
        SWP_SHOWWINDOW = 0x0040,
        SWP_HIDEWINDOW = 0x0080,
        SWP_NOCOPYBITS = 0x0100,
        SWP_NOOWNERZORDER = 0x0200,	/* Don't do owner Z ordering */
        SWP_NOSENDCHANGING = 0x0400		/* Don't send WM_WINDOWPOSCHANGING */
    }

    [Flags()]
    public enum DCX
    {
        DCX_CACHE = 0x2,
        DCX_CLIPCHILDREN = 0x8,
        DCX_CLIPSIBLINGS = 0x10,
        DCX_EXCLUDERGN = 0x40,
        DCX_EXCLUDEUPDATE = 0x100,
        DCX_INTERSECTRGN = 0x80,
        DCX_INTERSECTUPDATE = 0x200,
        DCX_LOCKWINDOWUPDATE = 0x400,
        DCX_NORECOMPUTE = 0x100000,
        DCX_NORESETATTRS = 0x4,
        DCX_PARENTCLIP = 0x20,
        DCX_VALIDATE = 0x200000,
        DCX_WINDOW = 0x1,
    }

    public enum ShowWindowOptions
    {
        SW_HIDE = 0,
        SW_SHOWNORMAL = 1,
        SW_NORMAL = 1,
        SW_SHOWMINIMIZED = 2,
        SW_SHOWMAXIMIZED = 3,
        SW_MAXIMIZE = 3,
        SW_SHOWNOACTIVATE = 4,
        SW_SHOW = 5,
        SW_MINIMIZE = 6,
        SW_SHOWMINNOACTIVE = 7,
        SW_SHOWNA = 8,
        SW_RESTORE = 9,
        SW_SHOWDEFAULT = 10,
        SW_FORCEMINIMIZE = 11
    }

    [Flags]
    public enum RedrawWindowOptions
    {
        RDW_INVALIDATE = 0x0001,
        RDW_INTERNALPAINT = 0x0002,
        RDW_ERASE = 0x0004,
        RDW_VALIDATE = 0x0008,
        RDW_NOINTERNALPAINT = 0x0010,
        RDW_NOERASE = 0x0020,
        RDW_NOCHILDREN = 0x0040,
        RDW_ALLCHILDREN = 0x0080,
        RDW_UPDATENOW = 0x0100,
        RDW_ERASENOW = 0x0200,
        RDW_FRAME = 0x0400,
        RDW_NOFRAME = 0x0800
    }

    public enum SystemCommands
    {
        SC_SIZE = 0xF000,
        SC_MOVE = 0xF010,
        SC_MINIMIZE = 0xF020,
        SC_MAXIMIZE = 0xF030,
        SC_MAXIMIZE2 = 0xF032,	// fired from double-click on caption
        SC_NEXTWINDOW = 0xF040,
        SC_PREVWINDOW = 0xF050,
        SC_CLOSE = 0xF060,
        SC_VSCROLL = 0xF070,
        SC_HSCROLL = 0xF080,
        SC_MOUSEMENU = 0xF090,
        SC_KEYMENU = 0xF100,
        SC_ARRANGE = 0xF110,
        SC_RESTORE = 0xF120,
        SC_RESTORE2 = 0xF122,	// fired from double-click on caption
        SC_TASKLIST = 0xF130,
        SC_SCREENSAVE = 0xF140,
        SC_HOTKEY = 0xF150,

        SC_DEFAULT = 0xF160,
        SC_MONITORPOWER = 0xF170,
        SC_CONTEXTHELP = 0xF180,
        SC_SEPARATOR = 0xF00F
    }

    [Flags]
    public enum PeekMessageOptions
    {
        PM_NOREMOVE = 0x0000,
        PM_REMOVE = 0x0001,
        PM_NOYIELD = 0x0002
    }

    public enum SizingOptions
    {
        WMSZ_LEFT = 1,
        WMSZ_RIGHT = 2,
        WMSZ_TOP = 3,
        WMSZ_TOPLEFT = 4,
        WMSZ_TOPRIGHT = 5,
        WMSZ_BOTTOM = 6,
        WMSZ_BOTTOMLEFT = 7,
        WMSZ_BOTTOMRIGHT = 8
    }

    [StructLayout(LayoutKind.Sequential)]
    [CLSCompliant(false)]
    public struct WINDOWPOS
    {
        public IntPtr hwnd;
        public IntPtr hWndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public uint flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    [CLSCompliant(false)]
    public struct NCCALCSIZE_PARAMS
    {
        /// <summary>
        /// Contains the new coordinates of a window that has been moved or resized, that is, it is the proposed new window coordinates.
        /// </summary>
        public Common.RECT rectProposed;
        /// <summary>
        /// Contains the coordinates of the window before it was moved or resized.
        /// </summary>
        public Common.RECT rectBeforeMove;
        /// <summary>
        /// Contains the coordinates of the window's client area before the window was moved or resized.
        /// </summary>
        public Common.RECT rectClientBeforeMove;
        /// <summary>
        /// Pointer to a WINDOWPOS structure that contains the size and position values specified in the operation that moved or resized the window.
        /// </summary>
        public WINDOWPOS lpPos;
    }

    public enum INPUTTYPE
    {
        MOUSE = 0,
        KEYBOARD = 1,
        HARDWARE = 2
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public int mouseData;
        public int dwFlags;
        public int time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KEYBDINPUT
    {
        public short wVk;
        public short wScan;
        public int dwFlags;
        public int time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HARDWAREINPUT
    {
        public int uMsg;
        public short wParamL;
        public short wParamH;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct INPUT
    {
        [FieldOffset(0)]
        public INPUTTYPE type;
        [FieldOffset(4)]
        public MOUSEINPUT mi;
        [FieldOffset(4)]
        public KEYBDINPUT ki;
        [FieldOffset(4)]
        public HARDWAREINPUT hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPLACEMENT
    {
        public int length;
        public int flags;
        public ShowWindowOptions showCmd;
        public Common.POINT ptMinPosition;
        public Common.POINT ptMaxPosition;
        public Common.RECT rcNormalPosition;

        public static WINDOWPLACEMENT Default
        {
            get
            {
                WINDOWPLACEMENT result = new WINDOWPLACEMENT();

                result.length = Marshal.SizeOf(result);

                return result;
            }
        }
    }
}

namespace KellControls.VectorGraphics.Factories
{
    public sealed class Ellipse
    {
        public Primitives.Path Create(Types.Rectangle rect)
        {
            return Create(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public Primitives.Path Create(double x, double y, double width, double height)
        {
            Primitives.Path path = new Primitives.Path();

            path.Add(new Primitives.Path.Move(new Types.Point(x + width / 2, y)));
            path.Add(new Primitives.Path.EllipticalArc(width / 2, height / 2, 0, false, true, new Types.Point(x + width, y + height / 2)));
            path.Add(new Primitives.Path.EllipticalArc(width / 2, height / 2, 0, false, true, new Types.Point(x + width / 2, y + height)));
            path.Add(new Primitives.Path.EllipticalArc(width / 2, height / 2, 0, false, true, new Types.Point(x, y + height / 2)));
            path.Add(new Primitives.Path.EllipticalArc(width / 2, height / 2, 0, false, true, new Types.Point(x + width / 2, y)));

            path.Add(new Primitives.Path.Close());

            return path;
        }
    }
    public sealed class SoftShadow : Shadow
    {
        public SoftShadow(Renderers.Renderer renderer, Types.Point offset, double extent, Paint.Color color)
        {
            if (renderer == null)
            {
                throw new ArgumentNullException("renderer");
            }
            if (offset == null)
            {
                throw new ArgumentNullException("offset");
            }
            if (extent <= 0)
            {
                throw new ArgumentException("Extent must be greater than zero.", "extent");
            }
            if (color == null)
            {
                throw new ArgumentNullException("color");
            }

            _renderer = renderer;
            _offset = offset;
            _extent = extent;
            _color = color;
        }

        public override Primitives.VisualItem Create(Primitives.Path source)
        {
            Primitives.Container container = new Primitives.Container(_offset);
            PathCommandVisitor visitor = new PathCommandVisitor(this);

            source = _renderer.FlattenPath(source);

            foreach (Primitives.Path.Command command in source.Commands)
            {
                command.Visit(visitor);
            }

            foreach (Primitives.Path path in visitor.Paths)
            {
                container.AddFront(path);
            }

            return container;
        }

        private sealed class PathCommandVisitor : Primitives.Path.Command.Visitor
        {
            internal PathCommandVisitor(SoftShadow softShadow)
            {
                _softShadow = softShadow;
                _innerPath.Pen = null;
                _innerPath.Brush = new Paint.Brushes.SolidBrush(_softShadow._color);
            }

            internal Primitives.Path[] Paths
            {
                get
                {
                    Primitives.Path[] paths = new Primitives.Path[_parts.Count + 1];

                    _parts.CopyTo(paths, 0);
                    paths[paths.Length - 1] = _innerPath;

                    return paths;
                }
            }

            public override void VisitMove(Primitives.Path.Move move)
            {
                _pos = move.To;
                _estartpos = _epos = null;
                _startpos = _lastpos = _pos;
                _startev = null;

                _innerPath.Add(new Primitives.Path.Move(move.To));
            }

            public override void VisitClose(Primitives.Path.Close close)
            {
                Types.Vector lv = _startpos - _lastpos;
                Types.Vector ev = new Types.Vector(lv.Y, -lv.X).Normalize(_softShadow._extent);

                if (_estartpos != null)
                {
                    CreateFiller(ev);
                }

                Primitives.Path part = new KellControls.VectorGraphics.Primitives.Path();

                part.Add(new Primitives.Path.Move(_lastpos + ev));
                part.Add(new Primitives.Path.Line(_startpos + ev));
                part.Add(new Primitives.Path.Line(_startpos));
                part.Add(new Primitives.Path.Line(_lastpos));
                part.Add(new Primitives.Path.Line(_lastpos + ev));
                part.Add(new Primitives.Path.Close());

                part.Pen = null;

                Types.Point gstart = _startpos, gend = _startpos + ev;

                part.Brush = new Paint.Brushes.LinearGradientBrush
                    (_softShadow._color, Paint.Color.Transparent, gstart, gend, Paint.Brushes.LinearGradientBrush.RenderHint.NoClip);

                _parts.Add(part);

                if (_estartpos == null)
                {
                    _estartpos = _pos + ev;
                }

                _lastpos = _pos = _startpos;
                _epos = _pos + ev;

                _innerPath.Add(new Primitives.Path.Line(_startpos));

                CreateFiller(_startev);

                _innerPath.Add(new Primitives.Path.Close());
            }

            public override void VisitLine(Primitives.Path.Line line)
            {
                Types.Vector lv = line.To - _lastpos;
                Types.Vector ev = new Types.Vector(lv.Y, -lv.X).Normalize(_softShadow._extent);

                if (_epos != null)
                {
                    CreateFiller(ev);
                }

                Primitives.Path part = new KellControls.VectorGraphics.Primitives.Path();

                part.Add(new Primitives.Path.Move(_lastpos + ev));
                part.Add(new Primitives.Path.Line(line.To + ev));
                part.Add(new Primitives.Path.Line(line.To));
                part.Add(new Primitives.Path.Line(_lastpos));
                part.Add(new Primitives.Path.Line(_lastpos + ev));
                part.Add(new Primitives.Path.Close());

                part.Pen = null;

                Types.Point gstart = line.To, gend = line.To + ev;

                part.Brush = new Paint.Brushes.LinearGradientBrush
                    (_softShadow._color, Paint.Color.Transparent, gstart, gend, Paint.Brushes.LinearGradientBrush.RenderHint.NoClip);

                _parts.Add(part);

                if (_estartpos == null)
                {
                    _estartpos = _pos + ev;
                }
                if (_startev == null)
                {
                    _startev = ev;
                }

                _lastpos = _pos = line.To;
                _epos = _pos + ev;

                _innerPath.Add(new Primitives.Path.Line(line.To));
            }

            public override void VisitEllipticalArc(Primitives.Path.EllipticalArc ellipticalArc)
            {
                _lastpos = ellipticalArc.To;
            }

            public override void VisitBezierCurve(Primitives.Path.BezierCurve curve)
            {
                _lastpos = curve.To;
            }

            public override void VisitSmoothBezierCurve(Primitives.Path.SmoothBezierCurve smoothCurve)
            {
                _lastpos = smoothCurve.To;
            }

            private void CreateFiller(Types.Vector ev2)
            {
                Types.Vector ev1 = _epos - _pos;
                Types.Vector lv1 = _lastpos - _pos;
                Types.Vector lv2 = (_lastpos + ev2) - _epos;

                Primitives.Path part = new KellControls.VectorGraphics.Primitives.Path();

                part.Add(new Primitives.Path.Move(_lastpos));

                part.Add(new Primitives.Path.Line(_lastpos + ev2));
                part.Add(new Primitives.Path.EllipticalArc(_softShadow._extent, _softShadow._extent, 0, false, false, _epos));

                part.Add(new Primitives.Path.Line(_lastpos));

                part.Add(new Primitives.Path.Close());

                part.Pen = null;

                Types.Point gstart = _pos + (_lastpos - _pos) / 2, gend = gstart + (ev1 + ev2).Normalize(_softShadow._extent * 2 / 3);

                part.Brush = new Paint.Brushes.RadialGradientBrush
                    (_softShadow._color, Paint.Color.Transparent, gstart, gend);

                _parts.Add(part);

                _innerPath.Add(new Primitives.Path.Line(_lastpos));
            }

            private SoftShadow _softShadow;
            private Types.Point _pos, _epos, _lastpos, _estartpos, _startpos;
            private List<Primitives.Path> _parts = new List<Primitives.Path>();
            private Primitives.Path _innerPath = new Primitives.Path();
            private Types.Vector _startev;
        }

        private Renderers.Renderer _renderer;
        private Types.Point _offset;
        private Paint.Color _color;
        private double _extent;
    }
    public sealed class GlossyBrush
    {
        public GlossyBrush()
        {
        }

        public GlossyBrush(Paint.Color lightener)
        {
            if (lightener == null)
            {
                throw new ArgumentNullException("lightener");
            }

            _lightener = lightener;
        }

        public Paint.Brushes.Brush Create(Paint.Color color, double top, double bottom)
        {
            return Create(color, Paint.Color.Combine(color, _lightener, 0.2), top, bottom);
        }

        public Paint.Brushes.Brush Create(Paint.Color color, double top, double bottom, double proportion)
        {
            return Create(color, Paint.Color.Combine(color, _lightener, 0.2), top, bottom, proportion);
        }

        public Paint.Brushes.Brush Create(Paint.Color color, Paint.Color lighterColor, double top, double bottom)
        {
            return Create(color, lighterColor, top, bottom, 0.3);
        }

        public Paint.Brushes.Brush Create(Paint.Color color, Paint.Color lighterColor, double top, double bottom, double proportion)
        {
            Paint.Color white = new Paint.Color(_lightener, color.Alpha);
            Paint.Color topStart = Paint.Color.Combine(color, white, 0.4);
            Paint.Color topEnd = Paint.Color.Combine(color, white, 0.7);
            Paint.Color bottomStart = color;
            Paint.Color bottomEnd = lighterColor;

            return new Paint.Brushes.LinearGradientBrush
                (topStart, bottomEnd
                , new Types.Point(0, top), new Types.Point(0, bottom)
                , Paint.Brushes.LinearGradientBrush.RenderHint.NoClip
                , new KeyValuePair<double, Paint.Color>[] { new KeyValuePair<double, Paint.Color>(proportion, topEnd), new KeyValuePair<double, Paint.Color>(proportion, bottomStart) });
        }

        private Paint.Color _lightener = Paint.Color.White;
    }
    public sealed class HardShadow : Shadow
    {
        public HardShadow(Types.Point offset, Paint.Color color)
        {
            if (offset == null)
            {
                throw new ArgumentNullException("offset");
            }
            if (color == null)
            {
                throw new ArgumentNullException("color");
            }

            _offset = offset;
            _color = color;
        }

        public override Primitives.VisualItem Create(Primitives.Path source)
        {
            Primitives.Container container = new Primitives.Container(_offset);

            Primitives.Path shadow = (Primitives.Path)source.Copy();

            shadow.Pen = null;
            shadow.Brush = new Paint.Brushes.SolidBrush(_color);

            container.AddBack(shadow);

            return container;
        }

        private Types.Point _offset;
        private Paint.Color _color;
    }
    public sealed class Rectangle
    {
        public Primitives.Path Create(Types.Rectangle rect)
        {
            return Create(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public Primitives.Path Create(double x, double y, double width, double height)
        {
            Primitives.Path path = new Primitives.Path();

            path.Add(new Primitives.Path.Move(new Types.Point(x, y)));
            path.Add(new Primitives.Path.Line(new Types.Point(x + width, y)));
            path.Add(new Primitives.Path.Line(new Types.Point(x + width, y + height)));
            path.Add(new Primitives.Path.Line(new Types.Point(x, y + height)));
            path.Add(new Primitives.Path.Line(new Types.Point(x, y)));
            path.Add(new Primitives.Path.Close());

            return path;
        }
    }
    public sealed class RoundedRectangle
    {
        [Flags]
        public enum Corners
        {
            TopLeft = 1,
            TopRight = 2,
            BottomRight = 4,
            BottomLeft = 8,
            All = TopLeft | TopRight | BottomRight | BottomLeft
        }

        public Primitives.Path Create(Types.Rectangle rect, double radius)
        {
            return Create(rect.X, rect.Y, rect.Width, rect.Height, radius, Corners.All);
        }

        public Primitives.Path Create(Types.Rectangle rect, double radius, Corners corners)
        {
            return Create(rect.X, rect.Y, rect.Width, rect.Height, radius, corners);
        }

        public Primitives.Path Create(double x, double y, double width, double height, double radius)
        {
            return Create(x, y, width, height, radius, Corners.All);
        }

        public Primitives.Path Create(double x, double y, double width, double height, double radius, Corners corners)
        {
            bool topLeft = (corners & Corners.TopLeft) != 0;
            bool topRight = (corners & Corners.TopRight) != 0;
            bool bottomLeft = (corners & Corners.BottomLeft) != 0;
            bool bottomRight = (corners & Corners.BottomRight) != 0;

            Primitives.Path path = new Primitives.Path();

            path.Add(new Primitives.Path.Move(new Types.Point(x + (topLeft ? radius : 0), y)));
            path.Add(new Primitives.Path.Line(new Types.Point(x + width - (topRight ? radius : 0), y)));

            if (topRight)
            {
                path.Add(new Primitives.Path.EllipticalArc(radius, radius, 0, false, true, new Types.Point(x + width, y + radius)));
            }

            path.Add(new Primitives.Path.Line(new Types.Point(x + width, y + height - (bottomRight ? radius : 0))));

            if (bottomRight)
            {
                path.Add(new Primitives.Path.EllipticalArc(radius, radius, 0, false, true, new Types.Point(x + width - radius, y + height)));
            }

            path.Add(new Primitives.Path.Line(new Types.Point(x + (bottomLeft ? radius : 0), y + height)));

            if (bottomLeft)
            {
                path.Add(new Primitives.Path.EllipticalArc(radius, radius, 0, false, true, new Types.Point(x, y + height - radius)));
            }

            path.Add(new Primitives.Path.Line(new Types.Point(x, y + (topLeft ? radius : 0))));

            if (topLeft)
            {
                path.Add(new Primitives.Path.EllipticalArc(radius, radius, 0, false, true, new Types.Point(x + radius, y)));
            }

            path.Add(new Primitives.Path.Close());

            return path;
        }
    }
    public abstract class Shadow
    {
        protected Shadow()
        {
        }

        public abstract Primitives.VisualItem Create(Primitives.Path source);

        public void Apply(Primitives.Container container)
        {
            List<Primitives.Path> sources = new List<Primitives.Path>();
            Primitives.DelegateVisitor visitor = new Primitives.DelegateVisitor();

            visitor.VisitPathDelegate = delegate(Primitives.Path path)
            {
                sources.Add(path);
            };

            container.Visit(visitor);

            foreach (Primitives.Path source in sources)
            {
                container.AddFront(Create(source));
            }
        }
    }
    public class Transparency
    {
        public void Apply(Primitives.Container container, double multiplier)
        {
            Visitor visitor = new Visitor(multiplier);

            container.Visit(visitor);
        }

        private sealed class Visitor : Primitives.Visitor
        {
            internal Visitor(double multiplier)
            {
                _multiplier = multiplier;
            }

            public override void VisitPath(Primitives.Path path)
            {
                base.VisitPath(path);

                PenVisitor penVisitor = new PenVisitor(_multiplier);
                BrushVisitor brushVisitor = new BrushVisitor(_multiplier);

                if (path.Pen != null)
                {
                    path.Pen.Visit(penVisitor);
                    path.Pen = penVisitor.NewPen;
                }
                if (path.Brush != null)
                {
                    path.Brush.Visit(new Types.Rectangle(0, 0, 0, 0), brushVisitor);
                    path.Brush = brushVisitor.NewBrush;
                }
            }

            public override void VisitText(Primitives.Text text)
            {
                base.VisitText(text);

                text.Color = new Paint.Color(text.Color, text.Color.Alpha * _multiplier);
            }

            private double _multiplier;
        }

        #region PenVisitor

        private sealed class PenVisitor : Paint.Pens.PenVisitor
        {
            internal PenVisitor(double multiplier)
            {
                _multiplier = multiplier;
            }

            internal Paint.Pens.Pen NewPen
            {
                get
                {
                    return _newPen;
                }
            }

            public override void VisitSolidPen(Paint.Pens.SolidPen solidPen)
            {
                _newPen = new Paint.Pens.SolidPen(new Paint.Color(solidPen.Color, solidPen.Color.Alpha * _multiplier), solidPen.Width);
            }

            private double _multiplier;
            private Paint.Pens.Pen _newPen;
        }

        #endregion

        #region BrushVisitor

        private sealed class BrushVisitor : Paint.Brushes.BrushVisitor
        {
            internal BrushVisitor(double multiplier)
            {
                _multiplier = multiplier;
            }

            internal Paint.Brushes.Brush NewBrush
            {
                get
                {
                    return _newBrush;
                }
            }

            public override void VisitSolidBrush(Types.Rectangle bounds, Paint.Brushes.SolidBrush solidBrush)
            {
                _newBrush = new Paint.Brushes.SolidBrush(new Paint.Color(solidBrush.Color, solidBrush.Color.Alpha * _multiplier));
            }

            public override void VisitLinearGradientBrush(Types.Rectangle bounds, Paint.Brushes.LinearGradientBrush linearGradientBrush)
            {
                List<KeyValuePair<double, Paint.Color>> intermediates = new List<KeyValuePair<double, Paint.Color>>();

                foreach (KeyValuePair<double, Paint.Color> kvp in linearGradientBrush.IntermediateColors)
                {
                    intermediates.Add(new KeyValuePair<double, Paint.Color>(kvp.Key, new Paint.Color(kvp.Value, kvp.Value.Alpha * _multiplier)));
                }

                _newBrush = new Paint.Brushes.LinearGradientBrush
                    (new Paint.Color(linearGradientBrush.StartColor, linearGradientBrush.StartColor.Alpha * _multiplier)
                    , new Paint.Color(linearGradientBrush.EndColor, linearGradientBrush.EndColor.Alpha * _multiplier)
                    , linearGradientBrush.StartPoint, linearGradientBrush.EndPoint, intermediates.ToArray());
            }

            public override void VisitRadialGradientBrush(Types.Rectangle bounds, Paint.Brushes.RadialGradientBrush radialGradientBrush)
            {
                _newBrush = new VectorGraphics.Paint.Brushes.RadialGradientBrush
                    (new Paint.Color(radialGradientBrush.InnerColor, radialGradientBrush.InnerColor.Alpha * _multiplier)
                    , new Paint.Color(radialGradientBrush.OuterColor, radialGradientBrush.OuterColor.Alpha * _multiplier)
                    , radialGradientBrush.InnerPoint, radialGradientBrush.OuterPoint);
            }

            private double _multiplier;
            private Paint.Brushes.Brush _newBrush;
        }

        #endregion
    }
}

namespace KellControls.WinFormsUtility.Events
{
    public static class ApplicationFailure
    {
        public interface IReportContext
        {
            void OnException(Exception e);
            string GenerateFilename();
            string[] GetFilenames();
        }

        public delegate void Send(string report);

        public static void Hook(IReportContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _context = context;

            System.Windows.Forms.Application.ThreadException +=
                    new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException +=
                    new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        public static void LogFailure(Exception e, string extra)
        {
            if (e == null)
            {
                try
                {
                    throw new InvalidOperationException();
                }
                catch (Exception fake)
                {
                    e = fake;
                }
            }

            ProcessException(e, extra, false);
        }

        public static bool HaveUnsentReports
        {
            get
            {
                return _context != null && _context.GetFilenames().Length > 0;
            }
        }

        private static void ProcessException(Exception e, string extra, bool abort)
        {
            string filename = _context.GenerateFilename();

            using (XmlTextWriter tw = new XmlTextWriter(filename, Encoding.UTF8))
            {
                CreateErrorReport(tw, e, extra);
            }

            if (abort)
            {
                _context.OnException(e);

                System.Windows.Forms.Application.Exit();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                System.Windows.Forms.Application.ThreadException -=
                        new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            }
            catch
            {
            }

            ProcessException(e.Exception, null, true);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException -=
                        new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            }
            catch
            {
            }

            ProcessException((Exception)e.ExceptionObject, null, true);
        }

        private static void CreateErrorReport(XmlTextWriter tw, Exception exception, string extra)
        {
            string callstack = Callstack(exception);
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] callstackBytes = new System.Text.UnicodeEncoding().GetBytes(callstack);
            byte[] callstackMD5Bytes = md5.ComputeHash(callstackBytes);
            string callstackMD5 = DumpBytes(callstackMD5Bytes);

            tw.Formatting = Formatting.Indented;

            tw.WriteStartDocument();
            tw.WriteStartElement("CrashReport");

            tw.WriteStartElement("Product");
            tw.WriteString(Application.ProductName);
            tw.WriteEndElement();

            tw.WriteStartElement("Version");
            tw.WriteString(Application.ProductVersion);
            tw.WriteEndElement();

            tw.WriteStartElement("StackHash");
            tw.WriteString(callstackMD5);
            tw.WriteEndElement();

            tw.WriteStartElement("Time");
            tw.WriteString(DateTime.UtcNow.ToString(System.Globalization.CultureInfo.InvariantCulture));
            tw.WriteEndElement();

            tw.WriteStartElement("Stack");
            tw.WriteString(callstack);
            tw.WriteEndElement();

            if (!string.IsNullOrEmpty(extra))
            {
                tw.WriteStartElement("Extra");
                tw.WriteString(extra);
                tw.WriteEndElement();
            }

            tw.WriteEndElement();
        }

        private static string DumpBytes(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte b in bytes)
            {
                int low = b & 0xf;
                int high = (b & 0xf0) >> 4;

                sb.Append("0123456789abcdef"[low]);
                sb.Append("0123456789abcdef"[high]);
            }

            return sb.ToString();
        }

        private static string Callstack(Exception exception)
        {
            StringBuilder sb = new StringBuilder();

            while (exception != null)
            {
                sb.Append(exception.ToString());
                sb.Append(Environment.NewLine);

                exception = exception.InnerException;
            }

            sb.Append(Environment.NewLine + Environment.NewLine);

            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();

            sb.Append(st.ToString());
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }

        private static IReportContext _context;
    }
    public static class MenuLoop
    {
        public static bool InMenuLoop
        {
            get
            {
                return _inMenuLoop;
            }
        }

        public static void NotifyEnterMenuLoop()
        {
            _inMenuLoop = true;
        }

        public static void NotifyExitMenuLoop()
        {
            _inMenuLoop = false;
        }

        private static bool _inMenuLoop;
    }
    public enum EventResult
    {
        Done,
        Defer
    }

    public interface IEvent
    {
        EventResult Invoke();
    }

    internal partial class EventQueue : Component
    {
        public EventQueue()
        {
            InitializeComponent();
        }

        public EventQueue(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void Add(IEvent ev)
        {
            if (ev == null)
            {
                throw new ArgumentNullException("ev");
            }

            lock (_lock)
            {
                if (!_events.Contains(ev))
                {
                    _events.Enqueue(ev);
                }
            }
        }

        public bool Enabled
        {
            get
            {
                return _timer.Enabled;
            }
            set
            {
                _timer.Enabled = value;
            }
        }

        public void Flush()
        {
            for (; ; )
            {
                if (!FlushOne())
                {
                    return;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (_timer != null)
            {
                _timer.Tick -= new System.EventHandler(_timer_Tick);
                _timer.Dispose();
                _timer = null;
            }

            _events.Clear();

            base.Dispose(disposing);
        }

        private bool FlushOne()
        {
            if (_activeFlag.IsActive)
            {
                throw new InvalidOperationException("Cannot flush the event queue from an event handler.");
            }

            using (_activeFlag.Apply())
            {
                IEvent ev = null;

                lock (_lock)
                {
                    if (_events.Count > 0)
                    {
                        ev = _events.Peek();
                    }
                }

                if (ev == null)
                {
                    return false;
                }
                else
                {
                    EventResult er = ev.Invoke();

                    switch (er)
                    {
                        case EventResult.Done:
                            lock (_lock)
                            {
                                Queue<IEvent> newEvents = new Queue<IEvent>();

                                while (_events.Count > 0)
                                {
                                    IEvent ne = _events.Dequeue();

                                    if (ne != ev)
                                    {
                                        newEvents.Enqueue(ne);
                                    }
                                }

                                _events = newEvents;
                            }
                            break;
                        case EventResult.Defer:
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }
            }

            return true;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (!_activeFlag.IsActive)
            {
                FlushOne();
            }
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._timer = new System.Windows.Forms.Timer(this.components);
            // 
            // _timer
            // 
            this._timer.Interval = 200;
            this._timer.Tick += new System.EventHandler(this._timer_Tick);

        }

        #endregion

        private object _lock = new object();
        private Queue<IEvent> _events = new Queue<IEvent>();
        private Utility.Control.Flag _activeFlag = new Utility.Control.Flag();
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Timer _timer;
    }
    public delegate void Action();

    internal partial class DelayedAction : Component
    {
        public DelayedAction()
        {
            InitializeComponent();
        }

        public DelayedAction(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void ApplyImmediate()
        {
            _timer.Stop();

            OnApply(EventArgs.Empty);
        }

        public void ApplyLater(int milliseconds)
        {
            ApplyLater(milliseconds, null);
        }

        public void ApplyLater(int milliseconds, Action action)
        {
            _action = action;

            if (milliseconds <= 0)
            {
                ApplyImmediate();
                return;
            }

            _timer.Interval = milliseconds;

            _start = DateTime.Now;
            _timer.Start();
        }

        protected virtual void OnApply(EventArgs e)
        {
            if (_action != null)
            {
                _action();
                _action = null;
            }

            if (Apply != null)
            {
                Apply(this, e);
            }
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            double ms = DateTime.Now.Subtract(_start).TotalMilliseconds;

            if (ms < _timer.Interval)
            {
                return;
            }

            _timer.Stop();

            OnApply(EventArgs.Empty);
        }

        public event EventHandler Apply;

        private DateTime _start;
        private Action _action;
    }
    partial class DelayedAction
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._timer = new System.Windows.Forms.Timer(this.components);
            // 
            // _timer
            // 
            this._timer.Tick += new System.EventHandler(this._timer_Tick);

        }

        #endregion

        private System.Windows.Forms.Timer _timer;
    }
}

namespace KellControls.VectorGraphics.Renderers.GdiPlusUtility
{
    public static class Convert
    {
        public static System.Drawing.Color Color(Paint.Color color)
        {
            int r = (int)(color.Red * 255);
            int g = (int)(color.Green * 255);
            int b = (int)(color.Blue * 255);
            int a = (int)(color.Alpha * 255);

            return System.Drawing.Color.FromArgb(a, r, g, b);
        }

        public static Paint.Color Color(System.Drawing.Color color)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;
            double a = color.A / 255.0;

            return new VectorGraphics.Paint.Color(r, g, b, a);
        }

        public static Types.Rectangle Rectangle(System.Drawing.Rectangle rect)
        {
            return new VectorGraphics.Types.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static Types.Rectangle Rectangle(System.Drawing.RectangleF rect)
        {
            return new VectorGraphics.Types.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static System.Drawing.PointF Point(Types.Point p)
        {
            return new System.Drawing.PointF((float)p.X, (float)p.Y);
        }
    }
}

namespace KellControls.WinFormsGloss.Controls
{
    public static class SuperToolTipManager
    {
        static SuperToolTipManager()
        {
            _timer.Interval = 500;
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Enabled = true;
        }

        public static void ShowToolTip(Drawing.ColorTable colorTable, SuperToolTipInfo info, Control owner, Point p)
        {
            ShowToolTip(colorTable, info, owner, p, false);
        }

        public static void ShowToolTip(Drawing.ColorTable colorTable, SuperToolTipInfo info, Control owner, Point p, bool balloon)
        {
            if (_suppressCount != 0 || WinFormsUtility.Events.MenuLoop.InMenuLoop)
            {
                return;
            }

            if (_existing != null)
            {
                if (object.Equals(_existing.Info, info))
                {
                    return;
                }
                else
                {
                    _existing.Close();
                    _existing = null;
                }
            }

            _mousePoint = Control.MousePosition;

            _existing = new SuperToolTip(colorTable, info, p, balloon);

            _existing.Show(owner);
        }

        public static void CloseToolTip()
        {
            if (_existing != null)
            {
                _existing.Close();
                _existing = null;
            }
        }

        public static void SuppressToolTips()
        {
            ++_suppressCount;
            CloseToolTip();
        }

        public static void AllowToolTips()
        {
            --_suppressCount;
        }

        private static void _timer_Tick(object sender, EventArgs e)
        {
            if (_mousePoint != Control.MousePosition)
            {
                CloseToolTip();
            }
        }

        private static Timer _timer = new Timer();
        private static SuperToolTip _existing;
        private static Point _mousePoint;
        private static int _suppressCount;
    }

    [CLSCompliant(false)]
    public sealed class SuperToolTip : WinFormsUtility.Win32.PerPixelAlphaForm
    {
        public SuperToolTip(Drawing.ColorTable colorTable, SuperToolTipInfo info, Point p, bool balloon)
        {
            _colorTable = colorTable;
            _info = info;
            _balloon = balloon;

            using (Graphics g = CreateGraphics())
            {
                Size size = GetSize(g, info);

                _width = size.Width;
                _height = size.Height;
            }

            Position(p);

            Setup();
        }

        public static Size GetSize(Graphics g, SuperToolTipInfo info)
        {
            int width, height;

            using (Font font = new Font(SystemFonts.DialogFont, FontStyle.Bold))
            {
                width = Math.Max(200, WinFormsUtility.Drawing.GdiPlusEx.MeasureString(g, info.Title, font, int.MaxValue).Width + 8);
            }

            height = WinFormsUtility.Drawing.GdiPlusEx.MeasureString(g, info.Title, SystemFonts.DialogFont, width - _bodyIndent).Height;
            height += WinFormsUtility.Drawing.GdiPlusEx.MeasureString(g, info.Description, SystemFonts.DialogFont, width - _bodyIndent).Height;

            width += _border * 2;
            height += _border * 2 + _titleSep;

            return new Size(width, height);
        }

        public SuperToolTipInfo Info
        {
            get
            {
                return _info;
            }
        }

        public new double Opacity
        {
            get
            {
                return _opacity;
            }
            set
            {
                _opacity = value;

                Setup();
            }
        }

        public void Position(Point p)
        {
            Rectangle bounds = new Rectangle(p.X - _xoff, p.Y - _yoff, _width + 20 + _xoff, _height + 20 + _yoff);
            Rectangle screen = SystemInformation.WorkingArea;
            Point mousePos = Control.MousePosition;

            if (!screen.Contains(Control.MousePosition))
            {
                screen = SystemInformation.VirtualScreen;
            }

            if (bounds.Left < screen.Left)
            {
                bounds.X = screen.Left + 4;
            }
            if (bounds.Top < screen.Top)
            {
                bounds.Y = screen.Top + 4;
            }
            if (bounds.Right > screen.Right)
            {
                bounds.X = screen.Right - bounds.Width - 4;
            }
            if (bounds.Bottom > screen.Bottom)
            {
                bounds.Y = Math.Min(screen.Bottom, mousePos.Y) - bounds.Height - 4;
            }

            Bounds = bounds;
        }

        public static void Render(Graphics g, Rectangle clip, SuperToolTipInfo info, int width, int height, bool balloon, Drawing.ColorTable colorTable)
        {
            VectorGraphics.Primitives.Container container;

            Render(g, clip, info, width, height, balloon, colorTable, out container);
        }

        private static void Render(Graphics g, Rectangle clip, SuperToolTipInfo info, int width, int height, bool balloon, Drawing.ColorTable colorTable, out VectorGraphics.Primitives.Container container)
        {
            VectorGraphics.Types.Rectangle clipRect = VectorGraphics.Renderers.GdiPlusUtility.Convert.Rectangle(clip);
            VectorGraphics.Renderers.GdiPlusRenderer renderer = new VectorGraphics.Renderers.GdiPlusRenderer
                (delegate
                {
                    return g;
                }, KellControls.VectorGraphics.Renderers.GdiPlusRenderer.MarkerHandling.Ignore, 5);

            container = CreateContainer(renderer, width, height, balloon, colorTable);

            g.TranslateTransform(_xoff, _yoff);
            renderer.Render(g, container, clipRect);

            int titleHeight = WinFormsUtility.Drawing.GdiPlusEx.MeasureString(g, info.Title, SystemFonts.DialogFont, width - _bodyIndent).Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            Rectangle titleRect = new Rectangle(rect.X + _border, rect.Y + _border, rect.Width - _border * 2, titleHeight);
            Rectangle bodyRect = new Rectangle(rect.X + _border + _bodyIndent, rect.Y + titleHeight + _border + _titleSep, rect.Width - _border * 2 - _bodyIndent, rect.Height - titleHeight - _border * 2 - _titleSep);

            using (Font font = new Font(SystemFonts.DialogFont, FontStyle.Bold))
            {
                WinFormsUtility.Drawing.GdiPlusEx.DrawString
                    (g, info.Title, font, colorTable.TextColor, titleRect
                    , WinFormsUtility.Drawing.GdiPlusEx.TextSplitting.MultiLine, WinFormsUtility.Drawing.GdiPlusEx.Ampersands.Display);
            }

            WinFormsUtility.Drawing.GdiPlusEx.DrawString
                (g, info.Description, SystemFonts.DialogFont, colorTable.TextColor
                , bodyRect, WinFormsUtility.Drawing.GdiPlusEx.TextSplitting.MultiLine, WinFormsUtility.Drawing.GdiPlusEx.Ampersands.Display);
        }

        private void Setup()
        {
            BufferedGraphicsContext bgc = BufferedGraphicsManager.Current;

            bgc.MaximumBuffer = new Size(_width - 10, _height - 10);

            using (BufferedGraphics bg = bgc.Allocate(CreateGraphics(), new Rectangle(_xoff + 7, _yoff + 3, _width - 10, _height - 10)))
            {
                Graphics g = bg.Graphics;
                VectorGraphics.Primitives.Container container;

                Render(g, new Rectangle(0, 0, _width + 20, _height + 20), _info, _width, _height, _balloon, _colorTable, out container);

                Bitmap bitmapArgb = new Bitmap(_width + 30, _height + 30, PixelFormat.Format32bppArgb);

                using (Graphics ng = Graphics.FromImage(bitmapArgb))
                {
                    VectorGraphics.Renderers.GdiPlusRenderer nrenderer = new VectorGraphics.Renderers.GdiPlusRenderer
                        (delegate
                        {
                            return ng;
                        }, KellControls.VectorGraphics.Renderers.GdiPlusRenderer.MarkerHandling.Ignore, 5);

                    ng.TranslateTransform(_xoff, _yoff);
                    nrenderer.Render(ng, container, new VectorGraphics.Types.Rectangle(0, 0, _width + 30, _height + 30));

                    ng.TranslateTransform(_xoff + 4, _yoff + 4);
                    bg.Render(ng);
                }

                byte opacity = (byte)(_opacity * 255);

                SetBitmap(bitmapArgb, opacity);
            }
        }

        private static VectorGraphics.Primitives.Container CreateContainer(VectorGraphics.Renderers.Renderer renderer, int width, int height, bool balloon, Drawing.ColorTable colorTable)
        {
            VectorGraphics.Paint.Color primaryColor = VectorGraphics.Renderers.GdiPlusUtility.Convert.Color(colorTable.PrimaryColor);
            VectorGraphics.Paint.Color lightener = VectorGraphics.Renderers.GdiPlusUtility.Convert.Color(colorTable.GlossyLightenerColor);
            VectorGraphics.Paint.Color borderColor = VectorGraphics.Paint.Color.Combine(primaryColor, lightener, 0.5);
            VectorGraphics.Paint.Color gradientStartColor = VectorGraphics.Paint.Color.Combine(primaryColor, lightener, 0.05);
            VectorGraphics.Paint.Color gradientEndColor = VectorGraphics.Paint.Color.Combine(primaryColor, lightener, 0.2);

            VectorGraphics.Factories.RoundedRectangle roundRectFactory = new VectorGraphics.Factories.RoundedRectangle();
            VectorGraphics.Factories.SoftShadow softShadowFactory = new VectorGraphics.Factories.SoftShadow
                (renderer, new VectorGraphics.Types.Point(1, 1), 3, new VectorGraphics.Paint.Color(0, 0, 0, 0.3));

            VectorGraphics.Types.Rectangle mainRect = new VectorGraphics.Types.Rectangle(0, 0, width, height);
            VectorGraphics.Primitives.Container container = new VectorGraphics.Primitives.Container();

            double radius = 3;
            VectorGraphics.Primitives.Path shape = roundRectFactory.Create(mainRect, radius);

            if (balloon)
            {
                shape = new VectorGraphics.Primitives.Path();

                shape.Add(new VectorGraphics.Primitives.Path.Move(new VectorGraphics.Types.Point(mainRect.X + radius, mainRect.Y)));
                shape.Add(new VectorGraphics.Primitives.Path.Line(new VectorGraphics.Types.Point(mainRect.X + radius * 2, mainRect.Y)));
                shape.Add(new VectorGraphics.Primitives.Path.Line(new VectorGraphics.Types.Point(mainRect.X - radius, mainRect.Y - radius * 7)));
                shape.Add(new VectorGraphics.Primitives.Path.Line(new VectorGraphics.Types.Point(mainRect.X + radius * 9, mainRect.Y)));
                shape.Add(new VectorGraphics.Primitives.Path.Line(new VectorGraphics.Types.Point(mainRect.X + mainRect.Width - radius, mainRect.Y)));
                shape.Add(new VectorGraphics.Primitives.Path.EllipticalArc(radius, radius, 0, false, true, new VectorGraphics.Types.Point(mainRect.X + mainRect.Width, mainRect.Y + radius)));
                shape.Add(new VectorGraphics.Primitives.Path.Line(new VectorGraphics.Types.Point(mainRect.X + mainRect.Width, mainRect.Y + mainRect.Height - radius)));
                shape.Add(new VectorGraphics.Primitives.Path.EllipticalArc(radius, radius, 0, false, true, new VectorGraphics.Types.Point(mainRect.X + mainRect.Width - radius, mainRect.Y + mainRect.Height)));
                shape.Add(new VectorGraphics.Primitives.Path.Line(new VectorGraphics.Types.Point(mainRect.X + radius, mainRect.Y + mainRect.Height)));
                shape.Add(new VectorGraphics.Primitives.Path.EllipticalArc(radius, radius, 0, false, true, new VectorGraphics.Types.Point(mainRect.X, mainRect.Y + mainRect.Height - radius)));
                shape.Add(new VectorGraphics.Primitives.Path.Line(new VectorGraphics.Types.Point(mainRect.X, mainRect.Y + radius)));
                shape.Add(new VectorGraphics.Primitives.Path.EllipticalArc(radius, radius, 0, false, true, new VectorGraphics.Types.Point(mainRect.X + radius, mainRect.Y)));
                shape.Add(new VectorGraphics.Primitives.Path.Close());
            }
            else
            {
                shape = roundRectFactory.Create(mainRect, 3);
            }

            shape.Pen = new VectorGraphics.Paint.Pens.SolidPen(borderColor, 1);
            shape.Brush = new VectorGraphics.Paint.Brushes.LinearGradientBrush(gradientStartColor, gradientEndColor, mainRect.TopLeft, mainRect.BottomLeft);

            container.AddBack(shape);

            softShadowFactory.Apply(container);

            return container;
        }

        private const int _xoff = 10, _yoff = 20;
        private const int _border = 8;
        private const int _bodyIndent = 10;
        private const int _titleSep = 10;

        private int _width, _height;
        private SuperToolTipInfo _info;
        private Drawing.ColorTable _colorTable = new Drawing.WindowsThemeColorTable();
        private double _opacity = 1;
        private bool _balloon;
    }

    #region SuperToolTipInfo

    public sealed class SuperToolTipInfo
    {
        public SuperToolTipInfo(string title, string description)
        {
            if (title == null)
            {
                throw new ArgumentNullException("title");
            }
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            _title = title;
            _description = description;
        }

        public string Title
        {
            get
            {
                return _title;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        public override int GetHashCode()
        {
            return _title.GetHashCode() ^ _description.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            SuperToolTipInfo info = obj as SuperToolTipInfo;

            if (info == null)
            {
                return false;
            }

            return Title == info.Title && Description == info.Description;
        }

        private string _title;
        private string _description;
    }

    #endregion
}