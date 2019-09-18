using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Drawing;
using System.IO;
using System.Collections;

namespace ShareLib
{
    /// <summary>
    /// QQ空间（微博分享类）
    /// </summary>
    public static class QQShare
    {
        /// <summary>
        /// QQ登陆Post参数
        /// </summary>
         const string str = "http://ptlogin2.qq.com/login?u={0}&p={1}&verifycode={2}&aid=549000912&u1=http%3A%2F%2Fqzs.qq.com%2Fqzone%2Fv5%2Floginsucc.html%3Fpara%3Dizone&h=1&ptredirect=0&ptlang=2052&from_ui=1&dumy=&low_login_enable=0&regmaster=&fp=loginerroralert&action=4-6-1378107872656&mibao_css=&t=1&g=1&js_ver=10042&js_type=1&login_sig=rNpVncXOl8VYllT9O3-xM8VsoHgzJdGoSPJVkQyMbkP2Q0H0xFf4Ltv83VXGwHJp";

        /// <summary>
        /// 分享到QQ空间Post参数
        /// </summary>
         const string Str = "where=1&entryuin={0}&spaceuin={1}&title={2}&summary={3}&token=1885659264&sendparam=&description=&type=4&url={4}&site=&to=&share2weibo=1&fupdate=1&notice=1";

        static CookieContainer cc = new CookieContainer();

        #region 获取QQ验证码
        /// <summary>
        /// 获取QQ验证码
        /// </summary>
        /// <param name="QQNumber">QQ账号</param>
        /// <returns></returns>
        public static Bitmap GetVCodeImage(string QQNumber) 
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://captcha.qq.com/getimage?uin=" + QQNumber + "&aid=549000912&0.5260701394024645");
            req.Accept = "*/*";
            req.Referer = "http://ui.ptlogin2.qq.com/cgi-bin/login?hide_title_bar=1&low_login=0&qlogin_auto_login=1&no_verifyimg=1&link_target=blank&appid=549000912&style=12&target=self&s_url=http%3A//qzs.qq.com/qzone/v5/loginsucc.html?para=izone&pt_qr_app=%CA%D6%BB%FAQQ%BF%D5%BC%E4&pt_qr_link=http%3A//z.qzone.com/download.html&self_regurl=http%3A//qzs.qq.com/qzone/v6/reg/index.html&pt_qr_help_link=http%3A//z.qzone.com/download.html";
            req.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
            req.Host = "captcha.qq.com";
            req.CookieContainer = cc;
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream sr = res.GetResponseStream();
            Bitmap bitmap = new Bitmap(sr, false);
            //pictureBox1.Image = (Image)bitmap;
            return bitmap;
        }
        #endregion

        #region QQ登陆
        /// <summary>
        /// QQ登陆
        /// </summary>
        /// <param name="QQNumber">QQ账号</param>
        /// <param name="QQPwd">QQ密码</param>
        /// <param name="VCode">验证码</param>
        /// <returns></returns>
        public static string QQLogin(string QQNumber,string QQPwd,string VCode) 
        {
            string ss = PasswordHelper.GetPassword(QQNumber, QQPwd,VCode);
            string url = string.Format(str, QQNumber, ss, VCode);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Accept = "*/*";
            req.Referer = "http://ui.ptlogin2.qq.com/cgi-bin/login?hide_title_bar=1&low_login=0&qlogin_auto_login=1&no_verifyimg=1&link_target=blank&appid=549000912&style=12&target=self&s_url=http%3A//qzs.qq.com/qzone/v5/loginsucc.html?para=izone&pt_qr_app=%CA%D6%BB%FAQQ%BF%D5%BC%E4&pt_qr_link=http%3A//z.qzone.com/download.html&self_regurl=http%3A//qzs.qq.com/qzone/v6/reg/index.html&pt_qr_help_link=http%3A//z.qzone.com/download.html";
            req.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
            req.Host = "ptlogin2.qq.com";
            req.CookieContainer = cc;
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
            string strResult1 = sr.ReadToEnd();
            return strResult1;
        }
        #endregion

        #region 分享内容到QQ空间
        /// <summary>
        /// 分享内容到QQ空间
        /// </summary>
        /// <param name="QQNumber">QQ账号</param>
        /// <param name="Title">标题</param>
        /// <param name="Body">内容</param>
        /// <param name="Url">链接</param>
        /// <returns></returns>
        public static string QZoneShare(string QQNumber,string Title,string Body,string Url) 
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshareadd_url?g_tk=" + GetG_tk(GetAllCookies(cc)) + "");
            req.Accept = "*/*";
            req.Referer = "http://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?to=&where=1&url=www.qq.com";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";
            req.Host = "sns.qzone.qq.com";
            req.Method = "POST";
            string Data = string.Format(Str, QQNumber, QQNumber, System.Web.HttpUtility.UrlEncode(Title, Encoding.UTF8).ToString().ToUpper(), System.Web.HttpUtility.UrlEncode(Body, Encoding.UTF8).ToString().ToUpper(), System.Web.HttpUtility.UrlEncode(Url, Encoding.UTF8).ToString().ToUpper());
            byte[] PostByte = Encoding.UTF8.GetBytes(Data);
            req.ContentLength = PostByte.Length;
            req.CookieContainer = cc;
            Stream requestStream = req.GetRequestStream();
            requestStream.Write(PostByte, 0, PostByte.Length);
            HttpWebResponse wrp = (HttpWebResponse)req.GetResponse();
            string str = new StreamReader(wrp.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            return str;
        }
        #endregion

        private static int GetG_tk(string str)
        {
            int hash = 5381;
            for (int i = 0; i < str.Length; i++)
                hash += (hash << 5) + (int)str.Substring(i, 1).ToCharArray()[0];
            hash = hash & 0x7fffffff;

            return hash;
        }

        private static string GetAllCookies(CookieContainer cc)
        {
            List<Cookie> lstCookies = new List<Cookie>();
            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cc, new object[] { });
            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies) lstCookies.Add(c);
            }
            string skey = string.Empty;
            foreach (Cookie c in lstCookies)
            {
                if (c.Name == "skey")
                {
                    skey = c.Value;
                    break;
                }
            }
            return skey;
        }
    }
}
