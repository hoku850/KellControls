using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace KellControls
{
    /// <summary>
    /// 此类是代码模板，不要调用！主要用于配置文件中的连接字符串的加密和解密
    /// </summary>
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0")]
    internal sealed class Appconfig : global::System.Configuration.
ApplicationSettingsBase
    {
        static string defaultConnectionString = "Data Source=61.147.122.162;Initial Catalog=kell;Persist Security Info=True;Max Pool Size=1000;User ID=kell;Password=118051";
        public Appconfig(string connectionString)
        {
            defaultConnectionString = connectionString;
        }
        private static Appconfig defaultInstance = ((Appconfig)(global::System.
Configuration.ApplicationSettingsBase.Synchronized(new Appconfig(defaultConnectionString))));
        public static Appconfig Default
        {
            get
            {
                return defaultInstance;
            }
        }
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.
Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute
("Data Source=61.147.122.162;Initial Catalog=kell;Persist Security Info=False;Max Pool Size=1000;User ID=kell;Password=118051")]
        public string ConnectionString
        {
            get
            {
                DesUtility des = new DesUtility();
                return des.Decrypt(this["ConnectionString"].ToString());
            }
            set
            {
                DesUtility des = new DesUtility();
                string encode = des.Encrypt(value);
                base["ConnectionString"] = encode;
                this.SetKeyValue("connectionString", encode);
            }
        }
        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="AppKey"></param>
        /// <param name="AppValue"></param>         
        private void SetKeyValue(string AppKey, string AppValue)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(System.Windows.Forms.Application.ExecutablePath + ".config");
            XmlNode xNode;
            XmlElement xElem1;
            XmlElement xElem2;
            xNode = xDoc.SelectSingleNode("//connectionStrings");
            xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@name='" +
AppKey + "']");
            if (xElem1 != null) xElem1.SetAttribute("connectionString", AppValue);
            else
            {
                xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("name", AppKey);
                xElem2.SetAttribute("connectionString", AppValue);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(System.Windows.Forms.Application.ExecutablePath + ".config");
        }
    }
}
