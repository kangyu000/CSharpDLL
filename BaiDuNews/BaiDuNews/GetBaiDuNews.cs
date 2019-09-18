using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace BaiDuNews
{
    public static class GetBaiDuNews
    {
        public static string GetHtml(string Url, string Encod)
        {
            string HTML = null;
            try
            {
                Stream strmPage = null;
                StreamReader srPage = null;
                HttpWebRequest wrqPage = (HttpWebRequest)WebRequest.Create(Url);//根据制定的URL字符构造一个网络请求  
                WebResponse wrpPage = wrqPage.GetResponse();//获取网络相应      
                strmPage = wrpPage.GetResponseStream();//获取网络相应的数据流        
                srPage = new StreamReader(strmPage, System.Text.Encoding.GetEncoding(Encod));//将获取的数据流构造为一个StreamReader，用来读取流的内容         
                HTML = srPage.ReadToEnd(); //使用StreamReader读取到流的末尾，并将读取的内容存储到HTML变量中       
                strmPage.Close();
            }
            catch (Exception ee)
            {

            }
            return HTML;
        }
        /// <summary>
        /// 抓取百度新闻
        /// </summary>
        /// <param name="Key">搜索关键字</param>
        /// <param name="Encod">网页编码格式（gb2312，utf-8等）</param>
        /// <param name="page">获取的页数</param>
        /// <returns></returns>
        public static List<List<string>> GetBaiNews(string Key, string Encod, int page)//抓取百度新闻
        {
            string Page = (page - 1).ToString() + "0";
            List<List<string>> News = new List<List<string>>();
            List<string> ListHtml = new List<string>();//每个新闻的源文件
            List<string> NewsName = new List<string>();//新闻名称
            List<string> NewsURL = new List<string>();//新闻链接
            List<string> Source = new List<string>();//新闻来源和时间
            if (Key.Trim() != "")
            {
                string Url = "http://news.baidu.com/ns?bt=0&et=0&si=&rn=10&tn=news&ie=" + Encod + "&ct=1&word=" + Key + "&pn=" + Page + "&cl=3";
                string HtML = GetHtml(Url, Encod);
                HtML = HtML.Substring(HtML.IndexOf("<script language=\"javascript\">"));
                HtML = HtML.Substring(0, HtML.IndexOf("<div"));
                while (HtML.Contains("<script language=\"javascript\">"))
                {
                    string listHTML = HtML.Substring(0, HtML.IndexOf("</script>") + 9);
                    HtML = HtML.Substring(HtML.IndexOf("</script>") + 9);
                    ListHtml.Add(listHTML);
                }

                for (int i = 0; i < ListHtml.Count; i++)
                {
                    string listHTML = ListHtml[i].ToString();
                    string source = listHTML.Substring(listHTML.IndexOf("source: '") + 9);
                    source = source.Substring(0, source.IndexOf("',"));
                    string name = listHTML.Substring(listHTML.IndexOf("title: '") + 8);
                    name = name.Substring(0, name.IndexOf("url") - 3);
                    name = name.Replace("<font color=#C60A00>", "");
                    name = name.Replace("</font>", "");
                    name = name.Replace("&quot;", "\"");
                    
                    if (NewsName.Contains(name) == false)
                    {
                        Source.Add(source);
                        NewsName.Add(name);
                        string url = listHTML.Substring(listHTML.IndexOf("url: '") + 6);
                        url = url.Substring(0, url.IndexOf(",") - 1);
                        url = url.Replace(@"\", "");
                        NewsURL.Add(url);
                    }
                }
            }
            else 
            {

            }
            News.Add(NewsName);
            News.Add(NewsURL);
            News.Add(Source);
            return News;
        }
    }
}
