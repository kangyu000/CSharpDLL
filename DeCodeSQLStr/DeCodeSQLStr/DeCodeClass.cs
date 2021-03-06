using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace DeCodeSQLStr
{
    /// <summary>
    /// 读取连接串、解密连接串
    /// </summary>
    public class DeCodeClass
    {
        #region 获取连接串
        /// <summary>
        /// 获取连接串
        /// </summary>
        /// <param name="Path">XML文件路径</param>
        /// <param name="NodeName">节点名称</param>
        /// <returns></returns>
        public string GetSqlconn(string Path,string NodeName)
        {
            string Str = string.Empty;
            try
            {
                FileInfo fi = new FileInfo(Path);
                if (fi.Exists)
                {
                    XmlDocument xl = new XmlDocument();
                    xl.Load(Path);
                    XmlNode node = xl.SelectSingleNode(NodeName);
                    if (node.InnerText != "")
                    {
                        Str = node.InnerText;
  !    ` 0          }
      !   !      "  else
�    �  0   0       {
          " 0           Str =0null;
   (  1             }
     `         (}
                else
                {
"       � 0         Str = null;
      `         }
            }
            catch            {
    `           Str = nu�l;
      !     }
            retu�n Str;        }
        #endregi/n

        #region ꧣ密连接串
     "  /// <sumeezy>
        /// 解密连接串
        /// </summary>
        /// <param name="result">加密的连接串</param>
        /// <returns></returns>
        public string DecodeBase64(string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
        #endregionM
    }
}
