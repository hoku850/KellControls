using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;
using System.Configuration;
using System.Data.Common;
using System.Collections;
using System.IO;
using KellControls.KellTable.Models;
using System.Runtime.InteropServices;

namespace KellControls
{
    /// <summary>
    /// 本控件默认有外键的表对应的主表中的显示字段名为“Name”，如若不是，则要设置RelatedDisplayColumnName
    /// 默认主键字段名为“ID"，如若不是，则要设置KeyFieldName
    /// 默认主键字段为数字类型(true)，如若不是，则要设置IsKeyFieldNumeric(=false)
    /// </summary>
    public partial class KellRecordEditor : UserControl
    {
        string id;
        bool externalOneRecord;
        bool externalListSource;
        private bool bindKellTable;
        string externalRecordFile;
        string externalListFile;
        int currentRowIndex = -1;
        int currentPageIndex = 1;
        int currentPageSize = 10;
        string keyFieldName = "ID";
        bool isKeyFieldNumeric = true;
        string relatedDisplayColumnName = "Name";
        List<FieldInfo> record = new List<FieldInfo>();

        public class PopupSettingArgs
        {
            bool cancel;

            public bool Cancel
            {
                get { return cancel; }
            }
            string popupType;

            public string PopupType
            {
                get { return popupType; }
            }
            FieldInfo field;

            public FieldInfo Field
            {
                get { return field; }
            }

            public PopupSettingArgs(bool Cancel, string PopupType, FieldInfo Field)
            {
                cancel = Cancel;
                popupType = PopupType;
                field = Field;
            }
        }
        public delegate void PopupSettingHandler(object sender, PopupSettingArgs e);
        public event PopupSettingHandler Popup;

        private void OnPopup(object sender, PopupSettingArgs e)
        {
            if (Popup != null)
                Popup(sender, e);
        }

        public List<FieldInfo> Record
        {
            get { return record; }
        }
        DataSet datasource = new DataSet();
        IDataAdapter da;
        DbCommandBuilder cmdBuilder;
        //IDbCommand cmd;
        //IDbConnection conn;
        Color orgColor = SystemColors.Control;
        Color changedColor = Color.LightGray;
        public const string RecordExtensionName = "rec";

        /// <summary>
        /// 表的外键信息，0列为主键表名称，1列为主键列名，2列为外键列名，3列为级联更新状态（0和1）,4列为级联删除状态（0和1）
        /// </summary>
        DataTable fkTable;
        DataBaseContext dbContext;
        string connString;
        bool saved;
        int titleWidth = 100;
        Font titleFont = new Font("宋体", 9F, FontStyle.Bold);
        public delegate void FieldValueChangeHandler(object sender, FieldInfo e);
        public event FieldValueChangeHandler FieldValueChanged;
        string TempFile;

        const string ValueControlName = "ValueControl";

        const string QueryFkInfoSql = "SELECT 主键表名称=object_name(b.rkeyid),主键列名=(SELECT name FROM syscolumns WHERE colid=b.rkey AND id=b.rkeyid),外键列名=(SELECT name FROM syscolumns WHERE colid=b.fkey AND id=b.fkeyid),级联更新=ObjectProperty(a.id,'CnstIsUpdateCascade'),级联删除=ObjectProperty(a.id,'CnstIsDeleteCascade') FROM sysobjects a join sysforeignkeys b on a.id=b.constid join sysobjects c on a.parent_obj=c.id WHERE a.xtype='F' AND c.xtype='U' and object_name(b.fkeyid)=";

        const string QueryPkInfoSql = "SELECT 主键列名=(SELECT name FROM syscolumns WHERE colid=b.rkey AND id=b.rkeyid),外键表名称=object_name(b.fkeyid),外键列名=(SELECT name FROM syscolumns WHERE colid=b.fkey AND id=b.fkeyid),级联更新=ObjectProperty(a.id,'CnstIsUpdateCascade'),级联删除=ObjectProperty(a.id,'CnstIsDeleteCascade') FROM sysobjects a join sysforeignkeys b on a.id=b.constid join sysobjects c on a.parent_obj=c.id WHERE a.xtype='F' AND c.xtype='U' and object_name(b.rkeyid)=";
        private Table table;

        public KellRecordEditor()
        {
            InitializeComponent();
            TempFile = Path.GetTempFileName();
            orgColor = this.BackColor;
            connString = ConfigurationManager.ConnectionStrings["connString"] == null ? "" : ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        /// <summary>
        ///  获取指定表的外键信息，0列为主键表名称，1列为主键列名，2列为外键列名，3列为级联更新状态（0和1）,4列为级联删除状态（0和1）
        /// </summary>
        /// <param name="dataBaseContext">数据库上下文对象</param>
        /// <returns></returns>
        public static DataTable GetFkInfo(DataBaseContext dataBaseContext)
        {
            string sql = QueryFkInfoSql + "'" + dataBaseContext.TableName + "'";
            return GetDataSource(sql, dataBaseContext.Connection, dataBaseContext.Command);
        }

        /// <summary>
        /// 获取指定表的主键信息，0列为主键列名，1列为外键表名称，2列为外键列名，3列为级联更新状态（0和1）,4列为级联删除状态（0和1）
        /// </summary>
        /// <param name="dataBaseContext">数据库上下文对象</param>
        /// <returns></returns>
        public static DataTable GetPkInfo(DataBaseContext dataBaseContext)
        {
            if (dataBaseContext == null)
                return null;

            string sql = QueryPkInfoSql + "'" + dataBaseContext.TableName + "'";
            return GetDataSource(sql, dataBaseContext.Connection, dataBaseContext.Command);
        }

        /// <summary>
        /// 获取或设置数据库上下文
        /// </summary>
        public DataBaseContext DbContext
        {
            get { return dbContext; }
            set { dbContext = value; }
        }

        /// <summary>
        /// 获取当前的记录是否为外部独立的数据行（即非数据库中的记录）
        /// </summary>
        public bool ExternalOneRecord
        {
            get { return externalOneRecord; }
        }

        /// <summary>
        /// 获取当前的记录是否为外部独立的数据文件（即非数据库中的记录）
        /// </summary>
        public bool ExternalListSource
        {
            get { return externalListSource; }
        }

        /// <summary>
        /// 获取或者设置当前显示的记录的索引，利用此属性的set可以实现记录“定位到...”功能
        /// </summary>
        public int CurrentRowIndex
        {
            get { return currentRowIndex; }
            set { 
                currentRowIndex = value;
                LoadCurrentRecord();
            }
        }

        /// <summary>
        /// 获取或设置当前分页索引值
        /// </summary>
        public int CurrentPageIndex
        {
            get { return currentPageIndex; }
            set { currentPageIndex = value; }
        }

        /// <summary>
        /// 获取或设置当前分页索引值
        /// </summary>
        public int CurrentPageSize
        {
            get { return currentPageSize; }
            set { currentPageSize = value; }
        }

        public Color OriginalColor
        {
            get { return orgColor; }
        }

        public Color ChangedColor
        {
            get { return changedColor; }
            set { changedColor = value; }
        }

        public string RelatedDisplayColumnName
        {
            get { return relatedDisplayColumnName; }
            set { relatedDisplayColumnName = value; }
        }

        public string KeyFieldName
        {
            get { return keyFieldName; }
            set { keyFieldName = value; }
        }

        public bool IsKeyFieldNumeric
        {
            get { return isKeyFieldNumeric; }
            set { isKeyFieldNumeric = value; }
        }

        public string ID
        {
            get { return id; }
        }

        public int TitleWidth
        {
            get
            {
                return titleWidth;
            }
            set
            {
                titleWidth = value;
            }
        }

        public Font TitleFont
        {
            get { return titleFont; }
            set { titleFont = value; }
        }

        private void LinkOther(FieldInfo fi, int y, DataBaseContext dataBaseContext)
        {
            ComboBox cbb = new ComboBox();
            cbb.Name = ValueControlName;
            cbb.DropDownStyle = ComboBoxStyle.DropDownList;
            cbb.Width = titleWidth;
            cbb.ContextMenuStrip = contextMenuStrip1;
            List<LinkComboItem> list = GetDictionary(fi.LinkTable.Value, dataBaseContext.Connection, dataBaseContext.Command);
            if (list != null)
            {
                BindComboBox(cbb, list);
                if (list.Count > 0)
                {
                    int index = GetListIndex(list, fi.FieldValue);
                    if (index > -1 && index < cbb.Items.Count)
                        cbb.SelectedIndex = index;
                }
            }
            cbb.Tag = fi;
            cbb.SelectedIndexChanged += new EventHandler(cbb_SelectedIndexChanged);
            cbb.Left = titleWidth + 2;
            cbb.Top = y;
            this.Controls.Add(cbb);
        }

        private void BindComboBox(ComboBox cbb, List<LinkComboItem> list)
        {
            cbb.Items.Clear();
            foreach (LinkComboItem item in list)
            {
                cbb.Items.Add(item);
            }
        }

        private int GetRowIndex(DataTable dt, int columnIndex, object fieldValue)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    object cell = dt.Rows[i][columnIndex];
                    if (cell.ToString() == fieldValue.ToString())
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private int GetListIndex(List<LinkComboItem> list, object fieldValue)
        {
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Value.ToString() == fieldValue.ToString())
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private void SetComboBox(ComboBox cbb, FieldInfo fieldInfo)
        {
            if (cbb != null && cbb.Items.Count > 0)
            {
                for (int i = 0; i < cbb.Items.Count; i++)
                {
                    LinkComboItem item = (LinkComboItem)cbb.Items[i];
                    if (item.Value.ToString() == fieldInfo.FieldValue.ToString())
                    {
                        cbb.SelectedIndex = i;
                        cbb.Tag = fieldInfo;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定conn,cmd,sql所确定的数据集中的第一个结果集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="isStoredProceduce"></param>
        /// <returns></returns>
        public static DataTable GetDataSource(string sql, IDbConnection conn, IDbCommand cmd, bool isStoredProceduce = false)
        {
            if (cmd == null || conn == null)
                return null;

            DataTable result = new DataTable();
            cmd.CommandText = sql;
            if (isStoredProceduce)
                cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            try
            {
                int fieldCount = reader.FieldCount;
                for (int i = 0; i < fieldCount; i++)
                {
                    result.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                }
                while (reader.Read())
                {
                    DataRow row = result.NewRow();
                    for (int i = 0; i < fieldCount; i++)
                    {
                        row[i] = reader[i];
                    }
                    result.Rows.Add(row);
                }
            }
            catch (DbException e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                reader.Close();
            }
            return result;
        }

        /// <summary>
        /// 获取指定conn,cmd,sql所确定的数据集中的第一个结果集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="isStoredProceduce"></param>
        /// <returns></returns>
        private List<LinkComboItem> GetDictionary(string sql, IDbConnection conn, IDbCommand cmd, bool isStoredProceduce = false)
        {
            if (cmd == null || conn == null)
                return null;

            List<LinkComboItem> result = new List<LinkComboItem>();
            cmd.CommandText = sql;
            if (isStoredProceduce)
                cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            try
            {
                int fieldCount = reader.FieldCount;
                while (reader.Read())
                {
                    result.Add(new LinkComboItem() { Value = reader[0], Text = reader[1] == null ? "" : reader[1].ToString() });
                }
            }
            catch (DbException e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                reader.Close();
            }
            return result;
        }

        private void LoadControls()
        {
            int y = 0;
            if (this.Controls.Count > 0)
                this.Controls.Clear();
            int i=0;
            foreach (FieldInfo fi in record)
            {
                switch (fi.ValueType.Name.ToLower())
                {
                    case "string":
                        this.Controls.Add(new Label() { Text = fi.FieldName, Font = titleFont, TextAlign = ContentAlignment.MiddleRight, AutoSize = false, AutoEllipsis = true, Width = titleWidth, Top = y });
                        if (fi.IsLink)
                        {
                            LinkOther(fi, y, this.dbContext);
                        }
                        else
                        {
                            TextBox tb = new TextBox() { Name = ValueControlName, Text = fi.FieldValue.ToString(), Tag = fi, ContextMenuStrip = contextMenuStrip1 };
                            tb.TextChanged += new EventHandler(tb_TextChanged);
                            tb.Left = titleWidth + 2;
                            tb.Top = y;
                            this.Controls.Add(tb);
                        }
                        break;
                    case "boolean":
                        CheckBox cb = new CheckBox() { Name = ValueControlName, Text = fi.FieldName, AutoSize = true, Font = titleFont, TextAlign = ContentAlignment.MiddleRight, Checked = Convert.ToBoolean(fi.FieldValue), Tag = fi, ContextMenuStrip = contextMenuStrip1 };
                        cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
                        cb.Left = titleWidth + 2;
                        cb.Top = y;
                        this.Controls.Add(cb);
                        break;
                    case "datetime":
                        this.Controls.Add(new Label() { Text = fi.FieldName, Font = titleFont, TextAlign = ContentAlignment.MiddleRight, AutoSize = false, AutoEllipsis = true, Width = titleWidth, Top = y });
                        DateTimePicker dtp = new DateTimePicker() { Name = ValueControlName, Value = Convert.ToDateTime(fi.FieldValue), Tag = fi, Width = 140, ContextMenuStrip = contextMenuStrip1 };
                        dtp.ShowUpDown = true;
                        dtp.Format = DateTimePickerFormat.Custom;
                        dtp.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                        dtp.ValueChanged += new EventHandler(dtp_ValueChanged);
                        dtp.Left = titleWidth + 2;
                        dtp.Top = y;
                        this.Controls.Add(dtp);
                        break;
                    case "image":
                        this.Controls.Add(new Label() { Text = fi.FieldName, Font = titleFont, TextAlign = ContentAlignment.MiddleRight, AutoSize = false, AutoEllipsis = true, Width = titleWidth, Top = y });
                        ImageBox ib = new ImageBox() { Name = ValueControlName, Tag = fi, ContextMenuStrip = contextMenuStrip1 };
                        Image img = (Image)fi.FieldValue;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            ib.ImportImageByStream(ms);
                        }
                        ib.ImageChanged += new ImageBox.ImageChangedHandler(ib_ImageChanged);
                        ib.Left = titleWidth + 2;
                        ib.Top = y;
                        this.Controls.Add(ib);
                        break;
                    case "color":
                        this.Controls.Add(new Label() { Text = fi.FieldName, Font = titleFont, TextAlign = ContentAlignment.MiddleRight, AutoSize = false, AutoEllipsis = true, Width = titleWidth, Top = y });
                        ColorBox clb = new ColorBox() { Name = ValueControlName, Color = (Color)fi.FieldValue, Tag = fi, ContextMenuStrip = contextMenuStrip1 };
                        clb.ColorChanged += new ColorBox.ColorChangedHandler(clb_ColorChanged);
                        clb.Left = titleWidth + 2;
                        clb.Top = y;
                        this.Controls.Add(clb);
                        break;
                    case "int16":
                        this.Controls.Add(new Label() { Text = fi.FieldName, Font = titleFont, TextAlign = ContentAlignment.MiddleRight, AutoSize = false, AutoEllipsis = true, Width = titleWidth, Top = y });
                        NumericUpDown nud = new NumericUpDown() { Name = ValueControlName, Value = (short)fi.FieldValue, Maximum = 32767, Minimum = -32768, Tag = fi, Width = 100, ContextMenuStrip = contextMenuStrip1 };
                        nud.ValueChanged += new EventHandler(nud_ValueChanged);
                        nud.Left = titleWidth + 2;
                        nud.Top = y;
                        this.Controls.Add(nud);
                        break;
                    case "uint16":
                        this.Controls.Add(new Label() { Text = fi.FieldName, Font = titleFont, TextAlign = ContentAlignment.MiddleRight, AutoSize = false, AutoEllipsis = true, Width = titleWidth, Top = y });
                        NumericUpDown nud1 = new NumericUpDown() { Name = ValueControlName, Value = (ushort)fi.FieldValue, Maximum = 65535, Minimum = 0, Tag = fi, Width = 100, ContextMenuStrip = contextMenuStrip1 };
                        nud1.ValueChanged += new EventHandler(nud_ValueChanged);
                        nud1.Left = titleWidth + 2;
                        nud1.Top = y;
                        this.Controls.Add(nud1);
                        break;
                    case "int32":
                        this.Controls.Add(new Label() { Text = fi.FieldName, Font = titleFont, TextAlign = ContentAlignment.MiddleRight, AutoSize = false, AutoEllipsis = true, Width = titleWidth, Top = y });
                        if (fi.IsLink)
                        {
                            LinkOther(fi, y, this.dbContext);
                        }
                        else
                        {
                            NumericUpDown nud2 = new NumericUpDown() { Name = ValueControlName, Value = (int)fi.FieldValue, Maximum = 2147483647, Minimum = -2147483648, Tag = fi, Width = 100, ContextMenuStrip = contextMenuStrip1 };
                            nud2.ValueChanged += new EventHandler(nud_ValueChanged);
                            nud2.Left = titleWidth + 2;
                            nud2.Top = y;
                            this.Controls.Add(nud2);
                        }
                        break;
                    case "uint32":
                        this.Controls.Add(new Label() { Text = fi.FieldName, Font = titleFont, TextAlign = ContentAlignment.MiddleRight, AutoSize = false, AutoEllipsis = true, Width = titleWidth, Top = y });
                        if (fi.IsLink)
                        {
                            LinkOther(fi, y, this.dbContext);
                        }
                        else
                        {
                            NumericUpDown nud3 = new NumericUpDown() { Name = ValueControlName, Value = (uint)fi.FieldValue, Maximum = 4294967294, Minimum = 0, Tag = fi, Width = 100, ContextMenuStrip = contextMenuStrip1 };
                            nud3.ValueChanged += new EventHandler(nud_ValueChanged);
                            nud3.Left = titleWidth + 2;
                            nud3.Top = y;
                            this.Controls.Add(nud3);
                        }
                        break;
                    case "byte":
                        this.Controls.Add(new Label() { Text = fi.FieldName, Font = titleFont, TextAlign = ContentAlignment.MiddleRight, AutoSize = false, AutoEllipsis = true, Width = titleWidth, Top = y });
                        NumericUpDown nud4 = new NumericUpDown() { Name = ValueControlName, Value = (byte)fi.FieldValue, Maximum = 255, Minimum = 0, Tag = fi, Width = 100, ContextMenuStrip = contextMenuStrip1 };
                        nud4.ValueChanged += new EventHandler(nud_ValueChanged);
                        nud4.Left = titleWidth + 2;
                        nud4.Top = y;
                        this.Controls.Add(nud4);
                        break;
                    case "sbyte":
                        this.Controls.Add(new Label() { Text = fi.FieldName, Font = titleFont, TextAlign = ContentAlignment.MiddleRight, AutoSize = false, AutoEllipsis = true, Width = titleWidth, Top = y });
                        NumericUpDown nud5 = new NumericUpDown() { Name = ValueControlName, Value = (sbyte)fi.FieldValue, Maximum = 127, Minimum = -128, Tag = fi, Width = 100, ContextMenuStrip = contextMenuStrip1 };
                        nud5.ValueChanged += new EventHandler(nud_ValueChanged);
                        nud5.Left = titleWidth + 2;
                        nud5.Top = y;
                        this.Controls.Add(nud5);
                        break;
                    case "int64":
                        this.Controls.Add(new Label() { Text = fi.FieldName, Font = titleFont, TextAlign = ContentAlignment.MiddleRight, AutoSize = false, AutoEllipsis = true, Width = titleWidth, Top = y });
                        NumericUpDown nud6 = new NumericUpDown() { Name = ValueControlName, Value = (long)fi.FieldValue, Maximum = 9999999999999999999, Minimum = -999999999999999999, Tag = fi, Width = 100, ContextMenuStrip = contextMenuStrip1 };
                        nud6.ValueChanged += new EventHandler(nud_ValueChanged);
                        nud6.Left = titleWidth + 2;
                        nud6.Top = y;
                        this.Controls.Add(nud6);
                        break;
                    case "uint64":
                        this.Controls.Add(new Label() { Text = fi.FieldName, Font = titleFont, TextAlign = ContentAlignment.MiddleRight, AutoSize = false, AutoEllipsis = true, Width = titleWidth, Top = y });
                        NumericUpDown nud7 = new NumericUpDown() { Name = ValueControlName, Value = (ulong)fi.FieldValue, Maximum = 9999999999999999999, Minimum = 0, Tag = fi, Width = 100, ContextMenuStrip = contextMenuStrip1 };
                        nud7.ValueChanged += new EventHandler(nud_ValueChanged);
                        nud7.Left = titleWidth + 2;
                        nud7.Top = y;
                        this.Controls.Add(nud7);
                        break;
                    case "single":
                        this.Controls.Add(new Label() { Text = fi.FieldName, Font = titleFont, TextAlign = ContentAlignment.MiddleRight, AutoSize = false, AutoEllipsis = true, Width = titleWidth, Top = y });
                        TextBox tb1 = new TextBox() { Name = ValueControlName, Text = Convert.ToSingle(fi.FieldValue).ToString(), Tag = fi, ContextMenuStrip = contextMenuStrip1 };
                        tb1.TextChanged += new EventHandler(tb_TextChanged);
                        tb1.Left = titleWidth + 2;
                        tb1.Top = y;
                        this.Controls.Add(tb1);
                        break;
                    case "double":
                        this.Controls.Add(new Label() { Text = fi.FieldName, Font = titleFont, TextAlign = ContentAlignment.MiddleRight, AutoSize = false, AutoEllipsis = true, Width = titleWidth, Top = y });
                        TextBox tb2 = new TextBox() { Name = ValueControlName, Text = Convert.ToDouble(fi.FieldValue).ToString(), Tag = fi, ContextMenuStrip = contextMenuStrip1 };
                        tb2.TextChanged += new EventHandler(tb_TextChanged);
                        tb2.Left = titleWidth + 2;
                        tb2.Top = y;
                        this.Controls.Add(tb2);
                        break;
                    case "decimal":
                        this.Controls.Add(new Label() { Text = fi.FieldName, Font = titleFont, TextAlign = ContentAlignment.MiddleRight, AutoSize = false, AutoEllipsis = true, Width = titleWidth, Top = y });
                        TextBox tb3 = new TextBox() { Name = ValueControlName, Text = Convert.ToDecimal(fi.FieldValue).ToString(), Tag = fi, ContextMenuStrip = contextMenuStrip1 };
                        tb3.TextChanged += new EventHandler(tb_TextChanged);
                        tb3.Left = titleWidth + 2;
                        tb3.Top = y;
                        this.Controls.Add(tb3);
                        break;
                }
                i++;
                y += 25;
            }
        }

        void cbb_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbb = (ComboBox)sender;
            if (cbb.SelectedIndex > -1)
            {
                FieldInfo fi = (FieldInfo)cbb.Tag;
                fi.FieldValue = ((LinkComboItem)cbb.Items[cbb.SelectedIndex]).Value;
                //fi.FieldValue = cbb.SelectedValue;
                OnFieldValueChanged(fi);
            }
        }

        void nud_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown tb = (NumericUpDown)sender;
            FieldInfo fi = (FieldInfo)tb.Tag;
            fi.FieldValue = tb.Value;
            OnFieldValueChanged(fi);
        }

        void clb_ColorChanged(object sender, Color e)
        {
            ColorBox clb = (ColorBox)sender;
            FieldInfo fi = (FieldInfo)clb.Tag;
            fi.FieldValue = e;
            OnFieldValueChanged(fi);
        }

        void dtp_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            FieldInfo fi = (FieldInfo)dtp.Tag;
            fi.FieldValue = dtp.Value;
            OnFieldValueChanged(fi);
        }

        void cb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            FieldInfo fi = (FieldInfo)cb.Tag;
            fi.FieldValue = cb.Checked;
            OnFieldValueChanged(fi);
        }

        void ib_ImageChanged(object sender, Image e)
        {
            ImageBox ib = (ImageBox)sender;
            FieldInfo fi = (FieldInfo)ib.Tag;
            fi.FieldValue = e;
            OnFieldValueChanged(fi);
        }

        void tb_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            FieldInfo fi = (FieldInfo)tb.Tag;
            fi.FieldValue = tb.Text;
            OnFieldValueChanged(fi);
        }

        private void OnFieldValueChanged(FieldInfo e)
        {
            if (!ExternalOneRecord)
            {
                DataRow dr = null;
                dr = datasource.Tables[0].Rows[currentRowIndex];
                //if (!IsKeyFieldNumeric)
                //    dr = datasource.Tables[0].Select(keyFieldName + "='" + this.ID + "'")[0];
                //else
                //    dr = datasource.Tables[0].Select(keyFieldName + "=" + this.ID)[0];
                int columnIndex = datasource.Tables[0].Columns.IndexOf(e.FieldName);
                if (columnIndex > -1)
                {
                    try
                    {
                        object[] newValues = new object[dr.ItemArray.Length];
                        Array.Copy(dr.ItemArray, newValues, newValues.Length);
                        newValues[columnIndex] = e.FieldValue;
                        dr.ItemArray = newValues;
                        this.saved = false;
                        this.BackColor = changedColor;
                        if (FieldValueChanged != null)
                            FieldValueChanged(this, e);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                //UpdateRecord(e);//不需要，因为内存中已经更新了！
                this.saved = false;
                this.BackColor = changedColor;
                if (FieldValueChanged != null)
                    FieldValueChanged(this, e);
            }
        }

        private void UpdateRecord(FieldInfo e)
        {
            foreach (FieldInfo fi in this.record)
            {
                if (fi.FieldName == e.FieldName)
                {
                    fi.FieldValue = e.FieldValue;
                }
            }
        }

        /// <summary>
        /// 清除当前显示的记录（只会清空当前内存中的对象，不影响到数据源，且通过Undo()方法可以恢复）
        /// </summary>
        public void ClearRecord()
        {
            this.record.Clear();
            this.Controls.Clear();
        }

        public void LoadRecordFromFile(string filepath)
        {
            if (File.Exists(filepath))
            {
                if (filepath.Substring(filepath.LastIndexOf(".") + 1).ToLower() == RecordExtensionName)
                {
                    try
                    {
                        this.externalOneRecord = true;
                        this.externalRecordFile = filepath;
                        this.record = DeserializeRecord(filepath);
                        LoadControls();
                        StoreLastRecord();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("从文件载入记录出错：" + e.Message);
                    }
                }
                else
                {
                    MessageBox.Show("请载入rec文件！");
                }
            }
        }

        public void SaveRecordToFile(string filepath)
        {
            if (filepath.Substring(filepath.LastIndexOf(".") + 1).ToLower() == RecordExtensionName)
            {
                SerializeRecord(filepath);
            }
            else
            {
                MessageBox.Show("请保存为rec文件！");
            }
        }

        public string ConnString
        {
            get { return connString; }
            set { connString = value; }
        }

        public bool Saved
        {
            get { return saved; }
        }

        /// <summary>
        /// 无界面显示的绑定
        /// </summary>
        /// <param name="dt"></param>
        public void BindDataTable(DataTable dt)
        {
            if (dt != null)
            {
                this.externalOneRecord = false;
                this.datasource.Tables.Clear();
                this.datasource.Tables.Add(dt);
                if (this.datasource.Tables[0].Rows.Count > 0)
                    LoadFirstRecord();
            }
        }

        public void BindKellTable(Table table)
        {
            if (table != null && table.TableModel != null && table.ColumnModel != null)
            {
                if (table.Tag != null)
                {
                    if (table.Tag is DataBaseContext)
                    {
                        this.dbContext = (DataBaseContext)table.Tag;
                        this.fkTable = GetFkInfo(this.dbContext);
                    }
                    else
                    {
                        externalRecordFile = table.Tag.ToString();
                        externalListSource = true;
                    }
                }
                this.table = table;
                this.bindKellTable = true;
                this.externalOneRecord = false;
                this.datasource.Tables.Clear();
                DataTable dt = KellTable.Utility.ConvertToDataTable(table);
                this.datasource.Tables.Add(dt);
                if (this.datasource.Tables[0].Rows.Count > 0)
                    LoadFirstRecord();
            }
        }

        public void BindDataGridView(DataGridView dgv)
        {
            if (dgv != null && dgv.DataSource != null)
            {
                if (dgv.Tag != null)
                {
                    if (dgv.Tag is DataBaseContext)
                    {
                        this.dbContext = (DataBaseContext)dgv.Tag;
                        this.fkTable = GetFkInfo(this.dbContext);
                    }
                    else
                    {
                        externalListFile = dgv.Tag.ToString();
                        externalListSource = true;
                    }
                }
                this.externalOneRecord = false;
                this.datasource.Tables.Clear();
                if (dgv.DataSource is DataTable)
                    this.datasource.Tables.Add((DataTable)dgv.DataSource);
                else if (dgv.DataSource is DataView)
                    this.datasource.Tables.Add((dgv.DataSource as DataView).Table);
                else if (dgv.DataSource is IListSource)
                    this.datasource.Tables.Add((dgv.DataSource as IListSource).ToDataTable());
                else if (dgv.DataSource is IEnumerable)
                    this.datasource.Tables.Add((dgv.DataSource as IEnumerable).ToDataTable());
                if (this.datasource.Tables[0].Rows.Count > 0)
                    LoadFirstRecord();
            }
        }

        /// <summary>
        /// 无界面显示的绑定
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="selectString"></param>
        /// <param name="tableName"></param>
        /// <param name="keyFieldName"></param>
        public void BindDataSet(DataSourceType dst, string selectString, string tableName, string keyFieldName = "ID")
        {
            this.externalOneRecord = false;
            if (!string.IsNullOrEmpty(keyFieldName))
                this.keyFieldName = keyFieldName;
            GetAdapter(dst, selectString, tableName);
        }

        private void GetAdapter(DataSourceType dst, string selectString, string tableName)
        {
            if (string.IsNullOrEmpty(connString))
                return;

            da = new SqlDataAdapter(selectString, connString);
            IDbCommand cmd = new SqlCommand();
            IDbConnection conn = new SqlConnection(connString);
            cmdBuilder = new SqlCommandBuilder((SqlDataAdapter)da);
            switch (dst)
            {
                case DataSourceType.OleDB:
                    da = new OleDbDataAdapter(selectString, connString);
                    cmd = new OleDbCommand();
                    conn = new OleDbConnection(connString);
                    cmdBuilder = new OleDbCommandBuilder((OleDbDataAdapter)da);
                    break;
                case DataSourceType.OrcDB:
                    da = new OracleDataAdapter(selectString, connString);
                    cmd = new OracleCommand();
                    conn = new OracleConnection(connString);
                    cmdBuilder = new OracleCommandBuilder((OracleDataAdapter)da);
                    break;
                case DataSourceType.ODBC:
                    da = new OdbcDataAdapter(selectString, connString);
                    cmd = new OdbcCommand();
                    conn = new OdbcConnection(connString);
                    cmdBuilder = new OdbcCommandBuilder((OdbcDataAdapter)da);
                    break;
                default:
                case DataSourceType.SqlDB:
                    break;
            }
            this.dbContext = new DataBaseContext(tableName, conn, cmd);
            this.fkTable = GetFkInfo(this.dbContext);
            da.FillSchema(datasource, SchemaType.Source);
            da.Fill(datasource);
            if (datasource.Tables.Count > 0 && datasource.Tables[0].Rows.Count > 0)
            {
                //datasource.Tables[0].TableName = this.TableName;
                LoadFirstRecord();
                //string firstId = datasource.Tables[0].Rows[0][keyFieldName].ToString();
                //bool flag = LoadRecord(firstId);
                //if (flag) currentRowIndex = 0;
            }
        }

        public void BindOneRecord(List<FieldInfo> record)
        {
            try
            {
                this.externalOneRecord = true;
                this.record = record;
                LoadControls();
                StoreLastRecord();
            }
            catch (Exception e)
            {
                MessageBox.Show("绑定指定的记录失败：" + e.Message);
            }
        }

        public bool IsChanged
        {
            get
            {
                return datasource.HasChanges(DataRowState.Modified);
            }
        }

        public bool NewRecord(List<FieldInfo> model)
        {
            try
            {
                this.externalOneRecord = true;
                this.record = model;
                LoadControls();
                StoreLastRecord();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("新建记录失败：" + e.Message);
                return false;
            }
        }

        public bool LoadRecord(string Id)
        {
            this.externalOneRecord = false;
            bool flag = false;
            if (datasource.Tables.Count > 0 && datasource.Tables[0].Rows.Count > 0)
            {
                try
                {
                    DataRow dr = null;
                    bool find = false;
                    for (int r = 0; r < datasource.Tables[0].Rows.Count; r++)
                    {
                        if (datasource.Tables[0].Rows[r][keyFieldName].ToString() == Id)
                        {
                            currentRowIndex = r;
                            find = true;
                            break;
                        }
                    }
                    if (find)
                    {
                        dr = datasource.Tables[0].Rows[currentRowIndex];
                    }
                    else
                    {
                        MessageBox.Show("找不到主键为 [" + Id + "] 的记录！");
                        return false;
                    }
                    //if (!IsKeyFieldNumeric)
                    //    dr = datasource.Tables[0].Select(keyFieldName + "='" + Id + "'")[0];
                    //else
                    //    dr = datasource.Tables[0].Select(keyFieldName + "=" + Id)[0];
                    List<int> hideColIndex = new List<int>();
                    for (int i = 0; i < datasource.Tables[0].Columns.Count; i++)
                    {
                        if (datasource.Tables[0].Columns[i].AutoIncrement || datasource.Tables[0].Columns[i].ReadOnly)
                            hideColIndex.Add(i);
                    }

                    record.Clear();

                    //读取磁盘中记录数据
                    //ConstraintCollection cc = datasource.Tables[0].Constraints;

                    for (int i = 0; i < datasource.Tables[0].Columns.Count; i++)
                    {
                        if (!hideColIndex.Contains(i))
                        {
                            FieldInfo fi = new FieldInfo(datasource.Tables[0].Columns[i].ColumnName, dr.ItemArray[i], datasource.Tables[0].Columns[i].DataType);
                            //if (cc.Count > 0)
                            if (fkTable != null && fkTable.Rows.Count > 0)
                            {
                                //foreach (Constraint c in cc)
                                //{
                                for (int j = 0; j < fkTable.Rows.Count; j++)
                                {
                                    //if (c is UniqueConstraint)//主键（唯一性约束）
                                    //if (c is ForeignKeyConstraint)//外键
                                    //{
                                    //ForeignKeyConstraint fk = (ForeignKeyConstraint)c;
                                    //string relatedTableName = fk.RelatedTable.TableName;
                                    if (fkTable.Rows[j][2].ToString() == fi.FieldName)
                                    //if (fk.Columns.Contains<DataColumn>(datasource.Tables[0].Columns[i], new DataColumnComparer()))
                                    {
                                        string relatedTableName = fkTable.Rows[j][0].ToString();
                                        string relatedColumnName = fkTable.Rows[j][1].ToString();
                                        fi.IsLink = true;
                                        fi.LinkTable = new KeyValuePair<string, string>(fi.FieldName, "select " + relatedColumnName + "," + this.RelatedDisplayColumnName + " from " + relatedTableName);
                                        //fi.LinkTable.Add(fi.FieldName, new LinkSqlValueDisplay() { SqlText = "select " + relatedColumnName + "," + this.RelatedDisplayColumnName + " from " + relatedTableName, ValueText = relatedColumnName, DisplayText = this.RelatedDisplayColumnName });
                                        //Func<DataColumn, bool> predicate = dc =>
                                        //{
                                        //    return dc.ColumnName == fi.FieldName;
                                        //};
                                        //DataColumn d = fk.RelatedColumns.Single<DataColumn>(predicate);
                                        //fi.LinkTable.Add(fi.FieldName, new LinkSqlValueDisplay() { SqlText = "select " + d.ColumnName + " from " + relatedTableName, ValueText = d.ColumnName, DisplayText = this.RelatedDisplayColumnName });
                                    }
                                }
                            }
                            record.Add(fi);
                        }
                    }
                    LoadControls();
                    this.id = Id;
                    flag = true;
                    StoreLastRecord();
                }
                catch (Exception e)
                {
                    MessageBox.Show("找不到主键为 [" + Id + "] 的记录！\n" + e.Message);
                }
            }
            return flag;
        }

        public bool LoadNextRecord()
        {
            this.externalOneRecord = false;
            bool flag = false;
            if (datasource.Tables.Count > 0 && datasource.Tables[0].Rows.Count > 0)
            {
                if (currentRowIndex > datasource.Tables[0].Rows.Count - 2)
                {
                    MessageBox.Show("已经到达末记录！");
                    return false;
                }
                currentRowIndex++;
                try
                {
                    DataRow dr = datasource.Tables[0].Rows[currentRowIndex];
                    List<int> hideColIndex = new List<int>();
                    for (int i = 0; i < datasource.Tables[0].Columns.Count; i++)
                    {
                        if (datasource.Tables[0].Columns[i].AutoIncrement || datasource.Tables[0].Columns[i].ReadOnly)
                            hideColIndex.Add(i);
                    }

                    record.Clear();

                    //读取磁盘中记录数据
                    //ConstraintCollection cc = datasource.Tables[0].Constraints;

                    for (int i = 0; i < datasource.Tables[0].Columns.Count; i++)
                    {
                        if (!hideColIndex.Contains(i))
                        {
                            FieldInfo fi = new FieldInfo(datasource.Tables[0].Columns[i].ColumnName, dr.ItemArray[i], datasource.Tables[0].Columns[i].DataType);
                            //if (cc.Count > 0)
                            if (fkTable != null && fkTable.Rows.Count > 0)
                            {
                                //foreach (Constraint c in cc)
                                //{
                                for (int j = 0; j < fkTable.Rows.Count; j++)
                                {
                                    //if (c is UniqueConstraint)//主键（唯一性约束）
                                    //if (c is ForeignKeyConstraint)//外键
                                    //{
                                    //ForeignKeyConstraint fk = (ForeignKeyConstraint)c;
                                    //string relatedTableName = fk.RelatedTable.TableName;
                                    if (fkTable.Rows[j][2].ToString() == fi.FieldName)
                                    //if (fk.Columns.Contains<DataColumn>(datasource.Tables[0].Columns[i], new DataColumnComparer()))
                                    {
                                        string relatedTableName = fkTable.Rows[j][0].ToString();
                                        string relatedColumnName = fkTable.Rows[j][1].ToString();
                                        fi.IsLink = true;
                                        fi.LinkTable = new KeyValuePair<string, string>(fi.FieldName, "select " + relatedColumnName + "," + this.RelatedDisplayColumnName + " from " + relatedTableName);
                                        //fi.LinkTable.Add(fi.FieldName, new LinkSqlValueDisplay() { SqlText = "select " + relatedColumnName + "," + this.RelatedDisplayColumnName + " from " + relatedTableName, ValueText = relatedColumnName, DisplayText = this.RelatedDisplayColumnName });
                                        //Func<DataColumn, bool> predicate = dc =>
                                        //{
                                        //    return dc.ColumnName == fi.FieldName;
                                        //};
                                        //DataColumn d = fk.RelatedColumns.Single<DataColumn>(predicate);
                                        //fi.LinkTable.Add(fi.FieldName, new LinkSqlValueDisplay() { SqlText = "select " + d.ColumnName + " from " + relatedTableName, ValueText = d.ColumnName, DisplayText = this.RelatedDisplayColumnName });
                                    }
                                }
                            }
                            record.Add(fi);
                        }
                    }
                    LoadControls();
                    if (datasource.Tables[0].Columns.Contains(keyFieldName))
                        this.id = dr[keyFieldName].ToString();
                    flag = true;
                    StoreLastRecord();
                }
                catch (Exception e)
                {
                    MessageBox.Show("找不到下一条的记录！\n" + e.Message);
                }
            }
            return flag;
        }

        public bool LoadPreviousRecord()
        {
            this.externalOneRecord = false;
            bool flag = false;
            if (datasource.Tables.Count > 0 && datasource.Tables[0].Rows.Count > 0)
            {
                if (currentRowIndex < 1)
                {
                    MessageBox.Show("已经到达首记录！");
                    return false;
                }
                currentRowIndex--;
                try
                {
                    DataRow dr = datasource.Tables[0].Rows[currentRowIndex];
                    List<int> hideColIndex = new List<int>();
                    for (int i = 0; i < datasource.Tables[0].Columns.Count; i++)
                    {
                        if (datasource.Tables[0].Columns[i].AutoIncrement || datasource.Tables[0].Columns[i].ReadOnly)
                            hideColIndex.Add(i);
                    }

                    record.Clear();

                    //读取磁盘中记录数据
                    //ConstraintCollection cc = datasource.Tables[0].Constraints;

                    for (int i = 0; i < datasource.Tables[0].Columns.Count; i++)
                    {
                        if (!hideColIndex.Contains(i))
                        {
                            FieldInfo fi = new FieldInfo(datasource.Tables[0].Columns[i].ColumnName, dr.ItemArray[i], datasource.Tables[0].Columns[i].DataType);
                            //if (cc.Count > 0)
                            if (fkTable != null && fkTable.Rows.Count > 0)
                            {
                                //foreach (Constraint c in cc)
                                //{
                                for (int j = 0; j < fkTable.Rows.Count; j++)
                                {
                                    //if (c is UniqueConstraint)//主键（唯一性约束）
                                    //if (c is ForeignKeyConstraint)//外键
                                    //{
                                    //ForeignKeyConstraint fk = (ForeignKeyConstraint)c;
                                    //string relatedTableName = fk.RelatedTable.TableName;
                                    if (fkTable.Rows[j][2].ToString() == fi.FieldName)
                                    //if (fk.Columns.Contains<DataColumn>(datasource.Tables[0].Columns[i], new DataColumnComparer()))
                                    {
                                        string relatedTableName = fkTable.Rows[j][0].ToString();
                                        string relatedColumnName = fkTable.Rows[j][1].ToString();
                                        fi.IsLink = true;
                                        fi.LinkTable = new KeyValuePair<string, string>(fi.FieldName, "select " + relatedColumnName + "," + this.RelatedDisplayColumnName + " from " + relatedTableName);
                                        //fi.LinkTable.Add(fi.FieldName, new LinkSqlValueDisplay() { SqlText = "select " + relatedColumnName + "," + this.RelatedDisplayColumnName + " from " + relatedTableName, ValueText = relatedColumnName, DisplayText = this.RelatedDisplayColumnName });
                                        //Func<DataColumn, bool> predicate = dc =>
                                        //{
                                        //    return dc.ColumnName == fi.FieldName;
                                        //};
                                        //DataColumn d = fk.RelatedColumns.Single<DataColumn>(predicate);
                                        //fi.LinkTable.Add(fi.FieldName, new LinkSqlValueDisplay() { SqlText = "select " + d.ColumnName + " from " + relatedTableName, ValueText = d.ColumnName, DisplayText = this.RelatedDisplayColumnName });
                                    }
                                }
                            }
                            record.Add(fi);
                        }
                    }
                    LoadControls();
                    if (datasource.Tables[0].Columns.Contains(keyFieldName))
                        this.id = dr[keyFieldName].ToString();
                    flag = true;
                    StoreLastRecord();
                }
                catch (Exception e)
                {
                    MessageBox.Show("找不到上一条的记录！\n" + e.Message);
                }
            }
            return flag;
        }

        public bool LoadFirstRecord()
        {
            this.externalOneRecord = false;
            bool flag = false;
            if (datasource.Tables.Count > 0 && datasource.Tables[0].Rows.Count > 0)
            {
                currentRowIndex = 0;
                try
                {
                    DataRow dr = datasource.Tables[0].Rows[currentRowIndex];
                    List<int> hideColIndex = new List<int>();
                    for (int i = 0; i < datasource.Tables[0].Columns.Count; i++)
                    {
                        if (datasource.Tables[0].Columns[i].AutoIncrement || datasource.Tables[0].Columns[i].ReadOnly)
                            hideColIndex.Add(i);
                    }

                    record.Clear();

                    //读取磁盘中记录数据
                    //ConstraintCollection cc = datasource.Tables[0].Constraints;

                    for (int i = 0; i < datasource.Tables[0].Columns.Count; i++)
                    {
                        if (!hideColIndex.Contains(i))
                        {
                            FieldInfo fi = new FieldInfo(datasource.Tables[0].Columns[i].ColumnName, dr.ItemArray[i], datasource.Tables[0].Columns[i].DataType);
                            //if (cc.Count > 0)
                            if (fkTable != null && fkTable.Rows.Count > 0)
                            {
                                //foreach (Constraint c in cc)
                                //{
                                for (int j = 0; j < fkTable.Rows.Count; j++)
                                {
                                    //if (c is UniqueConstraint)//主键（唯一性约束）
                                    //if (c is ForeignKeyConstraint)//外键
                                    //{
                                    //ForeignKeyConstraint fk = (ForeignKeyConstraint)c;
                                    //string relatedTableName = fk.RelatedTable.TableName;
                                    if (fkTable.Rows[j][2].ToString() == fi.FieldName)
                                    //if (fk.Columns.Contains<DataColumn>(datasource.Tables[0].Columns[i], new DataColumnComparer()))
                                    {
                                        string relatedTableName = fkTable.Rows[j][0].ToString();
                                        string relatedColumnName = fkTable.Rows[j][1].ToString();
                                        fi.IsLink = true;
                                        fi.LinkTable = new KeyValuePair<string, string>(fi.FieldName, "select " + relatedColumnName + "," + this.RelatedDisplayColumnName + " from " + relatedTableName);
                                        //fi.LinkTable.Add(fi.FieldName, new LinkSqlValueDisplay() { SqlText = "select " + relatedColumnName + "," + this.RelatedDisplayColumnName + " from " + relatedTableName, ValueText = relatedColumnName, DisplayText = this.RelatedDisplayColumnName });
                                        //Func<DataColumn, bool> predicate = dc =>
                                        //{
                                        //    return dc.ColumnName == fi.FieldName;
                                        //};
                                        //DataColumn d = fk.RelatedColumns.Single<DataColumn>(predicate);
                                        //fi.LinkTable.Add(fi.FieldName, new LinkSqlValueDisplay() { SqlText = "select " + d.ColumnName + " from " + relatedTableName, ValueText = d.ColumnName, DisplayText = this.RelatedDisplayColumnName });
                                    }
                                }
                            }
                            record.Add(fi);
                        }
                    }
                    LoadControls();
                    if (datasource.Tables[0].Columns.Contains(keyFieldName))
                        this.id = dr[keyFieldName].ToString();
                    flag = true;
                    StoreLastRecord();
                }
                catch (Exception e)
                {
                    MessageBox.Show("找不到首条的记录！\n" + e.Message);
                }
            }
            return flag;
        }

        public bool LoadLastRecord()
        {
            this.externalOneRecord = false;
            bool flag = false;
            if (datasource.Tables.Count > 0 && datasource.Tables[0].Rows.Count > 0)
            {
                currentRowIndex = datasource.Tables[0].Rows.Count - 1;
                try
                {
                    DataRow dr = datasource.Tables[0].Rows[currentRowIndex];
                    List<int> hideColIndex = new List<int>();
                    for (int i = 0; i < datasource.Tables[0].Columns.Count; i++)
                    {
                        if (datasource.Tables[0].Columns[i].AutoIncrement || datasource.Tables[0].Columns[i].ReadOnly)
                            hideColIndex.Add(i);
                    }

                    record.Clear();

                    //读取磁盘中记录数据
                    //ConstraintCollection cc = datasource.Tables[0].Constraints;

                    for (int i = 0; i < datasource.Tables[0].Columns.Count; i++)
                    {
                        if (!hideColIndex.Contains(i))
                        {
                            FieldInfo fi = new FieldInfo(datasource.Tables[0].Columns[i].ColumnName, dr.ItemArray[i], datasource.Tables[0].Columns[i].DataType);
                            //if (cc.Count > 0)
                            if (fkTable != null && fkTable.Rows.Count > 0)
                            {
                                //foreach (Constraint c in cc)
                                //{
                                for (int j = 0; j < fkTable.Rows.Count; j++)
                                {
                                    //if (c is UniqueConstraint)//主键（唯一性约束）
                                    //if (c is ForeignKeyConstraint)//外键
                                    //{
                                    //ForeignKeyConstraint fk = (ForeignKeyConstraint)c;
                                    //string relatedTableName = fk.RelatedTable.TableName;
                                    if (fkTable.Rows[j][2].ToString() == fi.FieldName)
                                    //if (fk.Columns.Contains<DataColumn>(datasource.Tables[0].Columns[i], new DataColumnComparer()))
                                    {
                                        string relatedTableName = fkTable.Rows[j][0].ToString();
                                        string relatedColumnName = fkTable.Rows[j][1].ToString();
                                        fi.IsLink = true;
                                        fi.LinkTable = new KeyValuePair<string, string>(fi.FieldName, "select " + relatedColumnName + "," + this.RelatedDisplayColumnName + " from " + relatedTableName);
                                        //fi.LinkTable.Add(fi.FieldName, new LinkSqlValueDisplay() { SqlText = "select " + relatedColumnName + "," + this.RelatedDisplayColumnName + " from " + relatedTableName, ValueText = relatedColumnName, DisplayText = this.RelatedDisplayColumnName });
                                        //Func<DataColumn, bool> predicate = dc =>
                                        //{
                                        //    return dc.ColumnName == fi.FieldName;
                                        //};
                                        //DataColumn d = fk.RelatedColumns.Single<DataColumn>(predicate);
                                        //fi.LinkTable.Add(fi.FieldName, new LinkSqlValueDisplay() { SqlText = "select " + d.ColumnName + " from " + relatedTableName, ValueText = d.ColumnName, DisplayText = this.RelatedDisplayColumnName });
                                    }
                                }
                            }
                            record.Add(fi);
                        }
                    }
                    LoadControls();
                    if (datasource.Tables[0].Columns.Contains(keyFieldName))
                        this.id = dr[keyFieldName].ToString();
                    flag = true;
                    StoreLastRecord();
                }
                catch (Exception e)
                {
                    MessageBox.Show("找不到末条的记录！\n" + e.Message);
                }
            }
            return flag;
        }

        public bool LoadCurrentRecord()
        {
            this.externalOneRecord = false;
            bool flag = false;
            if (datasource.Tables.Count > 0 && datasource.Tables[0].Rows.Count > 0)
            {
                try
                {
                    DataRow dr = datasource.Tables[0].Rows[currentRowIndex];
                    List<int> hideColIndex = new List<int>();
                    for (int i = 0; i < datasource.Tables[0].Columns.Count; i++)
                    {
                        if (datasource.Tables[0].Columns[i].AutoIncrement || datasource.Tables[0].Columns[i].ReadOnly)
                            hideColIndex.Add(i);
                    }

                    record.Clear();

                    //读取磁盘中记录数据
                    //ConstraintCollection cc = datasource.Tables[0].Constraints;

                    for (int i = 0; i < datasource.Tables[0].Columns.Count; i++)
                    {
                        if (!hideColIndex.Contains(i))
                        {
                            FieldInfo fi = new FieldInfo(datasource.Tables[0].Columns[i].ColumnName, dr.ItemArray[i], datasource.Tables[0].Columns[i].DataType);
                            //if (cc.Count > 0)
                            if (fkTable != null && fkTable.Rows.Count > 0)
                            {
                                //foreach (Constraint c in cc)
                                //{
                                for (int j = 0; j < fkTable.Rows.Count; j++)
                                {
                                    //if (c is UniqueConstraint)//主键（唯一性约束）
                                    //if (c is ForeignKeyConstraint)//外键
                                    //{
                                    //ForeignKeyConstraint fk = (ForeignKeyConstraint)c;
                                    //string relatedTableName = fk.RelatedTable.TableName;
                                    if (fkTable.Rows[j][2].ToString() == fi.FieldName)
                                    //if (fk.Columns.Contains<DataColumn>(datasource.Tables[0].Columns[i], new DataColumnComparer()))
                                    {
                                        string relatedTableName = fkTable.Rows[j][0].ToString();
                                        string relatedColumnName = fkTable.Rows[j][1].ToString();
                                        fi.IsLink = true;
                                        fi.LinkTable = new KeyValuePair<string, string>(fi.FieldName, "select " + relatedColumnName + "," + this.RelatedDisplayColumnName + " from " + relatedTableName);
                                        //fi.LinkTable.Add(fi.FieldName, new LinkSqlValueDisplay() { SqlText = "select " + relatedColumnName + "," + this.RelatedDisplayColumnName + " from " + relatedTableName, ValueText = relatedColumnName, DisplayText = this.RelatedDisplayColumnName });
                                        //Func<DataColumn, bool> predicate = dc =>
                                        //{
                                        //    return dc.ColumnName == fi.FieldName;
                                        //};
                                        //DataColumn d = fk.RelatedColumns.Single<DataColumn>(predicate);
                                        //fi.LinkTable.Add(fi.FieldName, new LinkSqlValueDisplay() { SqlText = "select " + d.ColumnName + " from " + relatedTableName, ValueText = d.ColumnName, DisplayText = this.RelatedDisplayColumnName });
                                    }
                                }
                            }
                            record.Add(fi);
                        }
                    }
                    LoadControls();
                    if (datasource.Tables[0].Columns.Contains(keyFieldName))
                        this.id = dr[keyFieldName].ToString();
                    flag = true;
                    StoreLastRecord();
                }
                catch (Exception e)
                {
                    MessageBox.Show("找不到首条的记录！\n" + e.Message);
                }
            }
            return flag;
        }

        private void StoreLastRecord()
        {
            //List<FieldInfo> lastRecord = new List<FieldInfo>();
            //foreach (FieldInfo fi in record)
            //{
            //    FieldInfo field = new FieldInfo(fi.FieldName, fi.FieldValue, fi.ValueType);
            //    field.IsLink = fi.IsLink;
            //    if (fi.IsLink)
            //    {
            //        field.LinkTable = new KeyValuePair<string, string>(fi.LinkTable.Key, fi.LinkTable.Value);
            //    }
            //    lastRecord.Add(field);
            //}
            //以上方法在内存中保存，第一次可以撤销，但第二次以后就会跟随最新的更改，于是就无法撤销了，只好采用下面这个持久性存储的办法来解决：
            SerializeRecord();
        }

        void SerializeRecord()
        {
            SerializeRecord(TempFile);
        }

        void SerializeRecord(string filepath)
        {
            using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bf.Serialize(fs, record);
            }
        }

        List<FieldInfo> DeserializeRecord()
        {
            return DeserializeRecord(TempFile);
        }

        List<FieldInfo> DeserializeRecord(string filepath)
        {
            List<FieldInfo> last = null;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                last = (List<FieldInfo>)bf.Deserialize(fs);
            }
            return last;
        }

        private string GetColumnsString(DataColumn[] dcs)
        {
            StringBuilder sb = new StringBuilder();
            if (dcs.Length > 0)
            {
                sb.Append(dcs[0].ColumnName);
                for (int i = 1; i < dcs.Length; i++)
                {
                    sb.Append(", " + dcs[i].ColumnName);
                }
            }
            return sb.ToString();
        }

        public void Save()
        {
            try
            {
                if (!externalOneRecord && IsChanged && da != null)
                {
                    //保存到磁盘中
                    DataSet cgDS = datasource.GetChanges(DataRowState.Modified);
                    da.Update(cgDS);
                    //注意：以下方法是错误的！！！
                    //datasource.AcceptChanges();
                    //da.Update(datasource);
                }
                else
                {
                    if (externalListSource)
                    {
                        if (File.Exists(externalListFile))
                        {
                            List<string> list = new List<string>();
                            using (FileStream fs = new FileStream(externalListFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                using (StreamReader sr = new StreamReader(fs))
                                {
                                    int i = 0;
                                    while (!sr.EndOfStream)
                                    {
                                        string s = sr.ReadLine();

                                        if (i == currentRowIndex + currentPageSize * (currentPageIndex - 1))
                                        {
                                            string newValue = this.record[0].FieldValue.ToString();
                                            list.Add(newValue);
                                        }
                                        else
                                        {
                                            list.Add(s);
                                        }
                                        i++;
                                    }
                                }
                            }
                            using (FileStream fs1 = new FileStream(externalListFile, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
                            {
                                using (StreamWriter sw = new StreamWriter(fs1))
                                {
                                    foreach (string s in list)
                                    {
                                        sw.WriteLine(s);
                                    }
                                }
                            }
                            UpdateDataTable();
                        }
                    }
                    else if (bindKellTable)
                    {
                        UpdateKellTable();
                    }
                    else if (externalOneRecord)
                    { }
                    else if (File.Exists(externalRecordFile))
                    {
                        SerializeRecord(externalRecordFile);
                    }
                }
                this.saved = true;
                this.BackColor = orgColor;
            }
            catch (Exception e)
            {
                MessageBox.Show("保存出错：" + e.Message);
            }
        }

        private void UpdateKellTable()
        {
            KellTable.Utility.LoadDataTable(this.table, datasource.Tables[0]);
        }

        private void UpdateDataTable()
        {
            List<string> list = new List<string>();
            using (StreamReader sr = File.OpenText(externalListFile))
            {
                while (!sr.EndOfStream)
                {
                    list.Add(sr.ReadLine());
                }
            }
            this.datasource.Tables.Clear();
            this.datasource.Tables.Add(list.ToDataTable());
        }

        public void Undo()
        {
            try
            {
                if (!externalOneRecord && IsChanged)
                {
                    datasource.RejectChanges();
                }
                RestoreRecord();
                this.saved = true;
                this.BackColor = orgColor;
            }
            catch (Exception e)
            {
                MessageBox.Show("撤销时出错：" + e.Message);
            }
        }

        private void RestoreRecord()
        {
            Control[] cs = this.Controls.Find(ValueControlName, false);
            List<FieldInfo> lastRecord = DeserializeRecord();
            for (int i = 0; i < cs.Length; i++)
            {
                SetControlValue(cs[i], lastRecord[i]);
            }
        }

        private void SetControlValue(Control control, FieldInfo fieldInfo)
        {
            switch (fieldInfo.ValueType.Name.ToLower())
            {
                case "string":
                    if (fieldInfo.IsLink)
                    {
                        ComboBox cbb = (ComboBox)control;
                        SetComboBox(cbb, fieldInfo);
                    }
                    else
                    {
                        ((TextBox)control).Text = fieldInfo.FieldValue.ToString();
                        ((TextBox)control).Tag = fieldInfo;
                    }
                    break;
                case "boolean":
                    CheckBox cb = (CheckBox)control;
                    cb.Checked = Convert.ToBoolean(fieldInfo.FieldValue);
                    cb.Tag = fieldInfo;
                    break;
                case "datetime":
                    DateTimePicker dtp = (DateTimePicker)control;
                    dtp.Value = Convert.ToDateTime(fieldInfo.FieldValue);
                    dtp.Tag = fieldInfo;
                    break;
                case "image":
                    ImageBox ib = (ImageBox)control;
                    ib.Tag = fieldInfo;
                    Image img = (Image)fieldInfo.FieldValue;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        ib.ImportImageByStream(ms);
                    }
                    break;
                case "color":
                    ColorBox clb = (ColorBox)control;
                    clb.Color = (Color)fieldInfo.FieldValue;
                    clb.Tag = fieldInfo;
                    break;
                case "int16":
                    NumericUpDown nud = (NumericUpDown)control;
                    nud.Value = (short)fieldInfo.FieldValue;
                    nud.Tag = fieldInfo;
                    break;
                case "uint16":
                    NumericUpDown nud1 = (NumericUpDown)control;
                    nud1.Value = (ushort)fieldInfo.FieldValue;
                    nud1.Tag = fieldInfo;
                    break;
                case "int32":
                    if (fieldInfo.IsLink)
                    {
                        ComboBox cbb = (ComboBox)control;
                        SetComboBox(cbb, fieldInfo);
                    }
                    else
                    {
                        NumericUpDown nud2 = (NumericUpDown)control;
                        nud2.Value = (int)fieldInfo.FieldValue;
                        nud2.Tag = fieldInfo;
                    }
                    break;
                case "uint32":
                    if (fieldInfo.IsLink)
                    {
                        ComboBox cbb = (ComboBox)control;
                        SetComboBox(cbb, fieldInfo);
                    }
                    else
                    {
                        NumericUpDown nud3 = (NumericUpDown)control;
                        nud3.Value = (uint)fieldInfo.FieldValue;
                        nud3.Tag = fieldInfo;
                    }
                    break;
                case "byte":
                    NumericUpDown nud4 = (NumericUpDown)control;
                    nud4.Value = (byte)fieldInfo.FieldValue;
                    nud4.Tag = fieldInfo;
                    break;
                case "sbyte":
                    NumericUpDown nud5 = (NumericUpDown)control;
                    nud5.Value = (sbyte)fieldInfo.FieldValue;
                    nud5.Tag = fieldInfo;
                    break;
                case "int64":
                    NumericUpDown nud6 = (NumericUpDown)control;
                    nud6.Value = (long)fieldInfo.FieldValue;
                    nud6.Tag = fieldInfo;
                    break;
                case "uint64":
                    NumericUpDown nud7 = (NumericUpDown)control;
                    nud7.Value = (ulong)fieldInfo.FieldValue;
                    nud7.Tag = fieldInfo;
                    break;
                case "single":
                case "double":
                case "decimal":
                    ((TextBox)control).Text = fieldInfo.FieldValue.ToString();
                    ((TextBox)control).Tag = fieldInfo;
                    break;
            }
        }

        private void 添加字段ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FieldInfoEditor fe = new FieldInfoEditor(record);
            if (fe.ShowDialog() == DialogResult.OK)
            {
                record.Add(fe.Field);
                LoadControls();
                OnPopup(this, new PopupSettingArgs(false, "添加字段", fe.Field));
            }
            else
            {
                OnPopup(this, new PopupSettingArgs(true, "添加字段", null));
            }
        }

        private void 插入字段ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control c;
            int index = GetCurrentFieldIndex(out c);
            FieldInfoEditor fe = new FieldInfoEditor(record);
            if (fe.ShowDialog() == DialogResult.OK)
            {
                record.Insert(index, fe.Field);
                LoadControls();
                OnPopup(c, new PopupSettingArgs(false, "插入字段", fe.Field));
            }
            else
            {
                OnPopup(c, new PopupSettingArgs(true, "插入字段", null));
            }
        }

        private int GetCurrentFieldIndex(out Control c)
        {
            c = contextMenuStrip1.SourceControl;
            int index = c.Top / 25;
            return index;
        }

        private void 删除字段ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control c;
            int index = GetCurrentFieldIndex(out c);
            if (MessageBox.Show("确定删除该字段[" + record[index].FieldName + "]？", "删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                record.RemoveAt(index);
                LoadControls();
                OnPopup(c, new PopupSettingArgs(false, "删除字段", record[index]));
            }
            else
            {
                OnPopup(c, new PopupSettingArgs(true, "删除字段", null));
            }
        }
    }

    [Serializable]
    public class FieldInfo
    {
        string fieldName;

        /// <summary>
        /// 注意：保证在一个记录中不要出现相同的FieldName
        /// </summary>
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }
        object fieldValue;

        public object FieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }
        Type valueType;

        public Type ValueType
        {
            get { return valueType; }
            set { valueType = value; }
        }
        bool isLink;

        public bool IsLink
        {
            get { return isLink; }
            set { isLink = value; }
        }
        KeyValuePair<string, string> linkTable = new KeyValuePair<string, string>();

        public KeyValuePair<string, string> LinkTable
        {
            get { return linkTable; }
            set { linkTable = value; }
        }

        /// <summary>
        /// 注意：保证在一个记录中不要出现相同的name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public FieldInfo(string name, object value)
        {
            this.fieldName = name;
            this.fieldValue = value;
            this.valueType = typeof(string);
        }

        /// <summary>
        /// 注意：保证在一个记录中不要出现相同的name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        public FieldInfo(string name, object value, Type valueType)
        {
            this.fieldName = name;
            this.fieldValue = value;
            this.valueType = valueType;
        }

        public override string ToString()
        {
            return this.fieldName;
        }
    }

    public enum DataSourceType
    {
        OleDB,
        SqlDB,
        OrcDB,
        ODBC
    }

    public struct LinkSqlValueDisplay
    {
        public string SqlText;
        public string ValueText;
        public string DisplayText;
    }

    public class DataColumnComparer : IEqualityComparer<DataColumn>
    {

        #region IEqualityComparer<DataColumn> 成员

        public bool Equals(DataColumn x, DataColumn y)
        {
            return x.Table.TableName == y.Table.TableName && x.ColumnName == y.ColumnName;
        }

        public int GetHashCode(DataColumn obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }

    public class LinkComboItem
    {
        object value;

        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public override string ToString()
        {
            return this.Text;
        }
    }

    public static class Common
    {
        /// <summary>
        /// 获取分页后的数据表
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex">从1开始的页号</param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static DataTable GetDataTableByPaging(DataTable source, int pageIndex, int pageSize)
        {
            DataTable dest = new DataTable();
            if (pageIndex > 0 && pageSize > 0)
            {
                if (source != null && source.Rows.Count > 0)
                {
                    dest = source.Clone();
                    int count = source.Rows.Count / pageSize;
                    int lastNum = source.Rows.Count % pageSize;
                    int len = pageSize;
                    if (pageIndex == count + 1)
                    {
                        len = lastNum;
                    }
                    int start = pageSize * (pageIndex - 1);
                    for (int i = start; i < start + len; i++)
                    {
                        DataRow dr = dest.NewRow();
                        dr.ItemArray = source.Rows[i].ItemArray;
                        dest.Rows.Add(dr);
                    }
                }
            }
            else
            {
                MessageBox.Show("pageIndex 和 pageSize 都必须大于零！");
            }
            return dest;
        }

        /// <summary>
        /// 获取指定数据源下的查询结果
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="connString"></param>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static DataTable GetDataTableBySql(DataSourceType dst, string connString, string sql, out IDbConnection conn, out IDbCommand cmd)
        {
            conn = null;
            cmd = null;

            if (string.IsNullOrEmpty(connString))
                return null;

            if (string.IsNullOrEmpty(sql))
                return null;

            cmd = new SqlCommand();
            conn = new SqlConnection(connString);
            DataSet ds = new DataSet();

            IDataAdapter da = new SqlDataAdapter(sql, connString);
            cmd = new SqlCommand();
            conn = new SqlConnection(connString);
            switch (dst)
            {
                case DataSourceType.OleDB:
                    da = new OleDbDataAdapter(sql, connString);
                    cmd = new OleDbCommand();
                    conn = new OleDbConnection(connString);
                    break;
                case DataSourceType.OrcDB:
                    da = new OracleDataAdapter(sql, connString);
                    cmd = new OracleCommand();
                    conn = new OracleConnection(connString);
                    break;
                case DataSourceType.ODBC:
                    da = new OdbcDataAdapter(sql, connString);
                    cmd = new OdbcCommand();
                    conn = new OdbcConnection(connString);
                    break;
                default:
                case DataSourceType.SqlDB:
                    break;
            }
            da.FillSchema(ds, SchemaType.Source);
            da.Fill(ds);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return new DataTable();
        }

        public static byte[] RawSerialize(object anything)
        {
            int rawsize = Marshal.SizeOf(anything);
            byte[] rawdata = new byte[rawsize];
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            try
            {
                Marshal.StructureToPtr(anything, buffer, false);
                Marshal.Copy(buffer, rawdata, 0, rawsize);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
            return rawdata;
        }

        public static object RawDeserialize(byte[] rawdata, Type type)
        {
            object anything = null;
            int rawsize = Marshal.SizeOf(rawdata);
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            try
            {
                anything = Marshal.PtrToStructure(buffer, type);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
            return anything;
        }
    }

    public class DataBaseContext
    {
        string tableName;

        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }
        IDbConnection conn;

        public IDbConnection Connection
        {
            get { return conn; }
            set { conn = value; }
        }
        IDbCommand cmd;

        public IDbCommand Command
        {
            get { return cmd; }
            set { cmd = value; }
        }

        public DataBaseContext(string tableName, IDbConnection conn, IDbCommand cmd)
        {
            this.tableName = tableName;
            this.conn = conn;
            this.cmd = cmd;
        }
    }
    
    public static class IListSourceExtensions
    {
        public static DataTable ToDataTable(this IListSource source)
        {
            DataTable dt = new DataTable();

            if (source.ContainsListCollection)
            {
                DataColumn dc = new DataColumn("ColumnName");
                IList list = source.GetList();
                IEnumerator ie = list.GetEnumerator();
                bool getType = false;
                while (ie.MoveNext())
                {
                    if (!getType)
                    {
                        dc.DataType = ie.Current.GetType();
                        dt.Columns.Add(dc);
                        getType = true;
                    }
                    DataRow row = dt.NewRow();
                    row.ItemArray = new object[1] { ie.Current };
                    dt.Rows.Add(row);
                }
            }

            return dt;
        }
    }

    public static class IEnumerableExtensions
    {
        public static DataTable ToDataTable(this IEnumerable source)
        {
            DataTable dt = new DataTable();

            DataColumn dc = new DataColumn("ColumnName");
            IEnumerator ie = source.GetEnumerator();
            bool getType = false;
            while (ie.MoveNext())
            {
                if (!getType)
                {
                    dc.DataType = ie.Current.GetType();
                    dt.Columns.Add(dc);
                    getType = true;
                }
                DataRow row = dt.NewRow();
                row.ItemArray = new object[1] { ie.Current };
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}