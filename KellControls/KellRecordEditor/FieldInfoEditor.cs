using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KellControls
{
    public partial class FieldInfoEditor : Form
    {
        public FieldInfoEditor(List<FieldInfo> record)
        {
            InitializeComponent();
            this.record = record;
            string newName = "字段";
            int i = 1;
            foreach (FieldInfo fi in this.record)
            {
                if (fi.FieldName == newName + i)
                {
                    i++;
                }
            }
            textBox1.Text = newName + i;
        }

        public FieldInfo Field;
        List<FieldInfo> record;

        private void FieldInfoEditor_Load(object sender, EventArgs e)
        {
            object[] items = new object[] { "String", "Boolean", "DateTime", "Image", "Color", "Int16", "UInt16", "Int32", "UInt32", "Byte", "SByte", "Int64", "UInt64", "Single", "Double", "Decimal"};
            comboBox1.Items.AddRange(items);
            comboBox1.SelectedIndex = 0;
            Field = new FieldInfo(textBox1.Text.Trim(), "");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (FieldInfo fi in record)
            {
                if (fi.FieldName.ToLower() == textBox1.Text.Trim().ToLower())
                {
                    DialogResult dr = MessageBox.Show("确定允许字段名字相同？", "字段重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == System.Windows.Forms.DialogResult.OK)
                    {
                        break;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            object defaultValue;
            Type type = GetTypeByName(comboBox1.Text, out defaultValue);
            Field = new FieldInfo(textBox1.Text.Trim(), defaultValue, type);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            //this.Close();//有了上面那句，这句就可以免了！
        }

        private Type GetTypeByName(string typeName, out object defaultValue)
        {
            Type type = typeof(string);
            defaultValue = "";
            switch (typeName)
            {
                case "Boolean":
                    type = typeof(Boolean);
                    defaultValue = false;
                    break;
                case "DateTime":
                    type = typeof(DateTime);
                    defaultValue = DateTime.Now;
                    break;
                case "Image":
                    type = typeof(Image);
                    Image img = new Bitmap(21, 21);
                    Graphics.FromImage(img).Clear(Color.White);
                    defaultValue = img;
                    break;
                case "Color":
                    type = typeof(Color);
                    defaultValue = Color.White;
                    break;
                case "Int16":
                    type = typeof(Int16);
                    defaultValue = 0; 
                    break;
                case "UInt16":
                    type = typeof(UInt16);
                    defaultValue = 0; 
                    break;
                case "Int32":
                    type = typeof(Int32);
                    defaultValue = 0; 
                    break;
                case "UInt32":
                    type = typeof(UInt32);
                    defaultValue = 0; 
                    break;
                case "Byte":
                    type = typeof(Byte);
                    defaultValue = 0; 
                    break;
                case "SByte":
                    type = typeof(SByte);
                    defaultValue = 0; 
                    break;
                case "Int64":
                    type = typeof(Int64);
                    defaultValue = 0; 
                    break;
                case "UInt64":
                    type = typeof(UInt64);
                    defaultValue = 0; 
                    break;
                case "Single":
                    type = typeof(Single);
                    defaultValue = 0.0; 
                    break;
                case "Double":
                    type = typeof(Double);
                    defaultValue = 0.0; 
                    break;
                case "Decimal":
                    type = typeof(Decimal);
                    defaultValue = 0.0; 
                    break;
                case "String":
                default:
                    break;
            }
            return type;
        }
    }
}
