using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Frp.NetWork
{
    /// <summary>
    /// INI配置文件读写
    /// </summary>
    public class IniConfig
    {
        /// <summary>
        /// ini 文件路径
        /// </summary>
        private static string strFilePath = System.Environment.CurrentDirectory + @"\Frps\frps.ini";

        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">节点名称[如[TypeName]]</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filepath);

        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="def">值</param>
        /// <param name="retval">stringbulider对象</param>
        /// <param name="size">字节大小</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        /// <summary>
        /// 写入ini配置信息
        /// </summary>
        /// <param name="node">节点名称</param>
        /// <param name="dic">配置信息</param>
        public static void WriteIniInfo(string node,Dictionary<string, object> dic)
        {
            foreach (var item in dic)
            {
                WritePrivateProfileString(node, item.Key.PadRight(35), item.Value?.ToString(), strFilePath);
            }
        }

        /// <summary>
        /// 读取ini配置信息
        /// </summary>
        /// <param name="node">读取节点</param>
        /// <param name="key">配置节点</param>
        /// <returns></returns>
        public static string ReadIniInfo(string node,string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(node, key, "", temp, 255, strFilePath);
            return i == 0 ? null : temp.ToString();
        }

        /// <summary>
        /// 清除node节点配置信息
        /// </summary>
        /// <param name="node">节点名称</param>
        public static void ClearIniNode(string node)
        {
            WritePrivateProfileString(node, null,null, strFilePath);
        }
    }
}
