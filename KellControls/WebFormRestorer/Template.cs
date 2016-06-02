using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace KellControls
{
    /// <summary>
    /// 页面结构
    /// </summary>
    [Serializable]
    public struct PageUtility
    {
        /// <summary>
        /// 当前页面所在的Url
        /// </summary>
        public string URL;
        /// <summary>
        /// 当前页面中的表单列表
        /// </summary>
        public FormUtility Form;
    }

    /// <summary>
    /// 表单元素结构
    /// </summary>
    [Serializable]
    public struct ItemUtility
    {
        /// <summary>
        /// 表单元素的名值对
        /// </summary>
        public KeyValuePair<string, string> NameValue;
    }

    /// <summary>
    /// 表单类
    /// </summary>
    [Serializable]
    public class FormUtility : ICloneable
    {
        List<ItemUtility> items;

        /// <summary>
        /// 表单类构造函数
        /// </summary>
        public FormUtility(List<ItemUtility> items)
        {
            this.items = items;
        }

        /// <summary>
        /// 添加一个表单元素
        /// </summary>
        public void AddItem(ItemUtility item)
        {
            items.Add(item);
        }

        /// <summary>
        /// 添加一个表单元素
        /// </summary>
        public void AddItem(string name, string value)
        {
            ItemUtility item = new ItemUtility();
            item.NameValue = new KeyValuePair<string, string>(name, value);
            items.Add(item);
        }

        /// <summary>
        /// 设置指定索引处的表单元素属性
        /// </summary>
        public void SetItem(int index, ItemUtility item)
        {
            items[index] = item;
        }

        /// <summary>
        /// 设置指定索引处的表单元素属性
        /// </summary>
        public void SetItem(int index, string name, string value)
        {
            ItemUtility item = new ItemUtility();
            item.NameValue = new KeyValuePair<string, string>(name, value);
            items[index] = item;
        }

        /// <summary>
        /// 修改指定索引处的表单元素的值
        /// </summary>
        public void SetItem(int index, string value)
        {
            ItemUtility item = new ItemUtility();
            item.NameValue = new KeyValuePair<string, string>(items[index].NameValue.Key, value);
            items[index] = item;
        }

        /// <summary>
        /// 获取表单中指定索引处的元素名
        /// </summary>
        public string GetName(int index)
        {
            return items[index].NameValue.Key;
        }

        /// <summary>
        /// 获取表单中指定名称的元素值的列表
        /// </summary>
        public string[] GetValues(string name)
        {
            List<string> values = new List<string>();
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].NameValue.Key == name)
                    values.Add(items[i].NameValue.Value);
            }
            return values.ToArray();
        }

        /// <summary>
        /// 获取当前表单对象中的所有元素列表的副本
        /// </summary>
        public ItemUtility[] GetDictionary()
        {
            List<ItemUtility> kvs = new List<ItemUtility>();
            for (int i = 0; i < items.Count; i++)
            {
                ItemUtility item = new ItemUtility();
                item.NameValue = new KeyValuePair<string, string>(items[i].NameValue.Key, items[i].NameValue.Value);
                kvs.Add(item);
            }
            return kvs.ToArray();
        }

        /// <summary>
        /// 获取当前表单对象中的所有元素的列表
        /// </summary>
        public List<ItemUtility> Items
        {
            get
            {
                return items;
            }
        }

        /// <summary>
        /// 移除指定名称的表单元素
        /// </summary>
        public void Remove(string name)
        {
            for (int i = items.Count - 1; i > -1; i--)
            {
                if (items[i].NameValue.Key == name)
                    items.RemoveAt(i);
            }
        }

        /// <summary>
        /// 在指定索引处的表单元素
        /// </summary>
        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        /// <summary>
        /// 清除表单元素
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }

        #region ICloneable 成员

        /// <summary>
        /// 克隆一个表单
        /// </summary>
        public object Clone()
        {
            FormUtility clone = new FormUtility(this.items);
            ItemUtility[] dic = this.GetDictionary();
            foreach (ItemUtility d in dic)
            {
                clone.AddItem(d);
            }
            return clone;
        }

        #endregion
    }

    /// <summary>
    /// 表单模板类
    /// </summary>
    public class Template
    {
        Guid id;

        /// <summary>
        /// 当前表单模板的ID
        /// </summary>
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        string name;

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        PageUtility page;

        /// <summary>
        /// 获取或设置当前表单模板的页面
        /// </summary>
        public PageUtility Page
        {
            get { return page; }
            set { page = value; }
        }

        /// <summary>
        /// 表单模板的构造函数
        /// </summary>
        public Template()
        {
            id = Guid.NewGuid();
            page = new PageUtility();
        }

        /// <summary>
        /// 表单模板的构造函数
        /// </summary>
        public Template(Guid guid)
        {
            id = guid;
            page = new PageUtility();
        }

        /// <summary>
        /// 保存当前模板到指定的文件
        /// </summary>
        public void SaveTemplate(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, page);
            }
        }

        /// <summary>
        /// 将当前的模板转化为字节数组
        /// </summary>
        public byte[] GetTemplateBytes()
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, page);
            byte[] data = ms.ToArray();
            ms.Close();
            return data;
        }

        /// <summary>
        /// 将序列化字节数组转化为当前模板的页面列表
        /// </summary>
        public void SetTemplateBy(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryFormatter bf = new BinaryFormatter();
            page = (PageUtility)bf.Deserialize(ms);
            ms.Close();
        }

        /// <summary>
        /// 从文件载入模板(模板中含有的数据即载入)
        /// </summary>
        public void LoadTemplate(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                page = (PageUtility)bf.Deserialize(fs);
            }
            //PageUtility p = page;
            //FormUtility fu = p.Form;
            //FormUtility form = (FormUtility)fu.Clone();
            //ItemUtility[] items = form.GetDictionary();
            //for (int i = 0; i < items.Length; i++)
            //{
            //    ItemUtility iu = items[i];
            //    form.SetItem(i, iu);
            //}
        }
    }
}