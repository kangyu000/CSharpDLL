using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace ShareLib
{
    /// <summary>
    /// 人人分享类
    /// </summary>
    public static class RenRenShareClass
    {
        static CookieContainer cc = new CookieContainer();
        static string RenRenUserID = string.Empty;
        static string _rtk = string.Empty;
        static string requestToken = string.Empty;

        #region 人人登陆
        /// <summary>
        /// 人人登陆
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Pwd">密码</param>
        public static string LoginRenRen(string UserName, string Pwd)
        {
            string logUrl = "http://passport.renren.com/PLogin.do";
            string hostUrl = "http://www.renren.com/SysHome.dom";
            string postdata = "email=" + UserName + "&password=" + Pwd + "&origURL=" + hostUrl + "&domain=renren.com";
            string Cookiesstr = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            request = (HttpWebRequest)WebRequest.Create(logUrl);
            request.Method = "POST";
            request.Referer = "http://www.renren.com/SysHome.do";
            request.ContentType = "application/x-www-form-urlencoded";
            request.AllowAutoRedirect = false;
            byte[] postdatabtyes = Encoding.UTF8.GetBytes(postdata);
            request.ContentLength = postdatabtyes.Length;
            request.CookieContainer = cc;
            request.KeepAlive = true;
            Stream requeststream = request.GetRequestStream();
            requeststream.Write(postdatabtyes, 0, postdatabtyes.Length);
            requeststream.Close();
            response = (HttpWebResponse)request.GetResponse();
            response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);
            CookieCollection cook = response.Cookies;
            string strcoook = request.CookieContainer.GetCookieHeader(request.RequestUri);
            Cookiesstr = strcoook;
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string content = sr.ReadToEnd();
            response.Close();

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://www.renren.com");
            req.CookieContainer = cc;
            HttpWebResponse wrp = (HttpWebResponse)req.GetResponse();
            string str = new StreamReader(wrp.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            _rtk = str.Substring(str.IndexOf("get_check_x") + 12);
            _rtk = _rtk.Substring(0, _rtk.IndexOf(","));
            _rtk = _rtk.Replace("'", "");

            requestToken = str.Substring(str.IndexOf("get_check") + 10);
            requestToken = requestToken.Substring(0, requestToken.IndexOf(","));
            requestToken = requestToken.Replace("'", "");

            return content;
        }
        #endregion

        #region 人人分享方法
        /// <summary>
        /// 人人分享方法
        /// </summary>
        /// <param name="PingJia">个人评价</param>
        /// <param name="Url">分享链接</param>
        /// <param name="Title">标题</param>
        /// <param name="Summary">内容</param>
        public static string ShareRenRen(string PingJia, string Url, string Title, string Summary)
        {
            HttpWebRequest req2 = (HttpWebRequest)WebRequest.Create("http://widget.renren.com/dialog/share/dfln");
            req2.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, */*";
            req2.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
            req2.Host = "widget.renren.com";
            req2.CookieContainer = cc;
            HttpWebResponse wrp = (HttpWebResponse)req2.GetResponse();
            string str = new StreamReader(wrp.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            RenRenUserID = str.Substring(str.IndexOf("\"uid\"") + 6);
            RenRenUserID = RenRenUserID.Substring(0, RenRenUserID.IndexOf(","));

            HttpWebRequest req1 = (HttpWebRequest)WebRequest.Create("http://shell.renren.com/" + RenRenUserID + "/share");
            req1.Accept = "*/*";
            req1.Referer = "http://shell.renren.com/ajaxproxy.htm";
            req1.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
            req1.Host = "shell.renren.com";
            req1.Method = "POST";
            req1.ContentType = "application/x-www-form-urlencoded";
            req1.CookieContainer = cc;
            string postdata = "comment=" + System.Web.HttpUtility.UrlEncode(PingJia, Encoding.UTF8) + "&link=" + System.Web.HttpUtility.UrlEncode(Url, Encoding.UTF8) + "&type=6&url=" + System.Web.HttpUtility.UrlEncode(Url, Encoding.UTF8) + "&thumbUrl=&meta=%2522%2522&nothumb=off&title=" + System.Web.HttpUtility.UrlEncode(Title, Encoding.UTF8) + "&summary=" + System.Web.HttpUtility.UrlEncode(Summary, Encoding.UTF8) + "&hostid=" + RenRenUserID + "&requestToken=" + requestToken + "&_rtk=" + _rtk + "&channel=renren";
            byte[] postdatabtyes = Encoding.UTF8.GetBytes(postdata);
            req1.ContentLength = postdatabtyes.Length;
            Stream requestStream = req1.GetRequestStream();
            requestStream.Write(postdatabtyes, 0, postdatabtyes.Length);
            wrp = (HttpWebResponse)req1.GetResponse();
            str = new StreamReader(wrp.GetResponseStream(), Encoding.UTF8).ReadToEnd();

            return str;
        }
        #endregion
    }
}
