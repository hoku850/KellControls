using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace KellControls
{
    /// <summary>
    /// 皮肤类型枚举（后续可更新）
    /// </summary>
    public enum SkinType
    {
        Default,
        Calmness,
        CalmnessColor1,
        CalmnessColor2,
        DeepCyan,
        DeepGreen,
        DeepOrange,
        DiamondBlue,
        DiamondGreen,
        Eighteen,
        EighteenColor1,
        EighteenColor2,
        Emerald,
        EmeraldColor1,
        EmeraldColor2,
        EmeraldColor3,
        GlassBrown,
        GlassGreen,
        GlassOrange,
        Longhorn,
        MacOS,
        Midsummer,
        MidsummerColor1,
        MidsummerColor2,
        MidsummerColor3,
        MP10,
        MSN,
        OneBlue,
        OneCyan,
        OneGreen,
        OneOrange,
        Page,
        PageColor1,
        PageColor2,
        RealOne,
        Silver,
        SilverColor1,
        SilverColor2,
        SportsBlack,
        SportsBlue,
        SportsCyan,
        SportsGreen,
        SportsOrange,
        SteelBlack,
        SteelBlue,
        vista1,
        vista1_green,
        Vista2_color1,
        Vista2_color2,
        Vista2_color3,
        Vista2_color4,
        Vista2_color5,
        Vista2_color6,
        Vista2_color7,
        Warm,
        WarmColor1,
        WarmColor2,
        WarmColor3,
        Wave,
        WaveColor1,
        WaveColor2,
        XPBlue,
        XPGreen,
        XPOrange,
        XPSilver
    }
    /// <summary>
    /// 皮肤释放静态类
    /// </summary>
    public class SkinExtractor
    {
        /// <summary>
        /// 将程序集中嵌入的指定皮肤释放到指定的路径下，成功即返回全路径，失败则返回空字符串string.Empty
        /// </summary>
        /// <param name="runPath"></param>
        /// <param name="skin"></param>
        /// <returns></returns>
        public static string ExtractToFile(string runPath, SkinType skin)
        {
            if (!Directory.Exists(runPath))
                return string.Empty;
            if (!runPath.EndsWith("\\"))
                runPath += "\\";
            string skinFile = runPath + skin.ToString() + ".ssk";
            try
            {
                Assembly ass = Assembly.LoadFrom("Resources.dll");
                using (Stream s = ass.GetManifestResourceStream(@"KellControls.Resources.Resources.Skin." + skin.ToString() + ".ssk"))
                {
                    using (FileStream fs = new FileStream(skinFile, FileMode.Create, FileAccess.Write))
                    {
                        s.CopyTo(fs);
                        fs.Flush();
                    }
                }
                return skinFile;
            }
            catch
            {
                if (File.Exists(skinFile))
                    File.Delete(skinFile);
                return string.Empty;
            }
        }
    }
}
