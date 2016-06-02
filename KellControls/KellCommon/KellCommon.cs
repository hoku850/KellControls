using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;

namespace KellControls
{
    public class KellCommon
    {
        static string p = "5201314";

        public static string IniFile
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + "\\config.ini";
            }
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public static string ReadAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static ConnectionStringSettings ReadConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name];
        }

        public static string GetRegCodeByHardDiskSN(string HDSN)
        {
            if (string.IsNullOrEmpty(HDSN))
                return string.Empty;

            string pwd = HDSN[0].ToString();
            for (int i = 1; i < HDSN.Length; i++)
            {
                if (i < 8)
                    pwd += p[i - 1].ToString() + HDSN[i].ToString();
                else
                    pwd += HDSN[i].ToString();
            }
            return pwd;
        }

        public static string ReadIni(string section, string key, string iniFullpath)
        {
            if (!File.Exists(iniFullpath))
            {
                using (StreamWriter sw = File.CreateText(iniFullpath))
                {
                    sw.WriteLine("[" + section + "]");
                    sw.WriteLine(key + "=");
                }
            }
            StringBuilder sb = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", sb, 255, iniFullpath);
            return sb.ToString();
        }

        public static void WriteIni(string section, string key, string value, string iniFullpath)
        {
            if (!File.Exists(iniFullpath))
            {
                using (StreamWriter sw = File.CreateText(iniFullpath))
                {
                    sw.WriteLine("[" + section + "]");
                    sw.WriteLine(key + "=" + value);
                }
            }
            WritePrivateProfileString(section, key, value, iniFullpath);
        }
    }
}
