using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace WebPageHtml
{
    /// <summary>
    /// 获取网站源码
    /// </summary>
    public static class GETWebPage
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        [DllImport("sensapi.dll")]
        private extern static bool IsNetworkAlive(out int connectionDescription);

        #region 判断网络连接是否畅通
        /// <summary>
        /// 判断网络连接是否畅通
        /// </summary>
        /// <returns></returns>
        public static bool ISConn()
        {
            int flags;//上网方式 
            bool m_bOnline = true;//是否在线 
            m_bOnline = InternetGetConnectedState(out flags, 0);
            if (m_bOnline)//在线   
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 传入url（string）、字符编码格式（string）（uft-8、gb2312），返回网页源码（string）
        /// <summary>
        /// 传入url（string）、字符编码格式（string）（uft-8、gb2312），返回网页源码（string）
        /// </summary>
        /// <param name="Url">要抓取的网址</param>
        /// <param name="Encod">字符编码格式</param>
        /// <param name="Timeout">延迟时间</param>
        /// <returns>网页源代码</returns>
        public static string GetHtml(string Url, string Encod, int Timeout)
        {
            string HTML = null;
            if (ISConn() == true)
            {
                try
                {
                    Stream strmPage = null;
                    StreamReader srPage = null;
                    HttpWebRequest wrqPage = (HttpWebRequest)WebRequest.Create(Url);//根据制定的URL字符构造一个网络请求  
                    if (Timeout > 0)
                    {
                        wrqPage.Timeout = Timeout;
                    }
                    WebResponse wrpPage = wrqPage.GetResponse();//获取网络相应      
                    strmPage = wrpPage.GetResponseStream();//获取网络相应的数据流        
                    srPage = new StreamReader(strmPage, System.Text.Encoding.GetEncoding(Encod));//将获取的数据流构造为一个StreamReader，用来读取流的内容         
                    HTML = srPage.ReadToEnd(); //使用StreamReader读取到流的末尾，并将读取的内容存储到HTML变量中       
                    strmPage.Close();
                }
                catch
                {

                }
            }
            else
            {
                HTML = "网络未连接！";
            }
            return HTML;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static string GetHtml(string Url)
        {
            string HTML = string.Empty;
            try
            {
                Stream strmPage = null;
                StreamReader srPage = null;
                HttpWebRequest wrqPage = (HttpWebRequest)WebRequest.Create(Url);//根据制定的URL字符构造一个网络请求  
                WebResponse wrpPage = wrqPage.GetResponse();//获取网络相应      
                strmPage = wrpPage.GetResponseStream();//获取网络相应的数据流        
                srPage = new StreamReader(strmPage, System.Text.Encoding.Default);//将获取的数据流构造为一个StreamReader，用来读取流的内容         
                HTML = srPage.ReadToEnd(); //使用StreamReader读取到流的末尾，并将读取的内容存储到HTML变量中       
                strmPage.Close();
            }
            catch
            {

            }
            return HTML;
        }

        /// <summary>
        /// 使用代理获取网页源码
        /// </summary>
        /// <param name="URL">链接</param>
        /// <param name="IP">IP</param>
        /// <param name="PORT">端口</param>
        /// <param name="Enc">编码格式</param>
        /// <returns></returns>
        public static string GetHtml(string URL, string IP, string PORT, string Enc)
        {
            string HTML = string.Empty;
            try
            {
                using (WebClient wc = new WebClient())
                {
                    WebProxy wp = new WebProxy(IP + ":" + PORT, true);
                    wc.Proxy = wp; //指定代理

                    wc.Encoding = Encoding.GetEncoding(Enc);
                    HTML = wc.DownloadString(URL);
                }
            }
            catch
            {

            }
            return HTML;
        }
    }
}
