using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace MusicInfoLib
{
    public static class GetMusicListClsss
    {
        public static List<string> GetMusicList(string MusicName, string SingerName)
        {
            List<string> MusicUrl = new List<string>();
            string URL = string.Empty;
            if (SingerName != "")
            {
                URL = "http://box.zhangmen.baidu.com/x?op=12&count=1&title=" + MusicName + "$$" + SingerName + "$$$";
            }
            else
            {
                URL = "http://box.zhangmen.baidu.com/x?op=12&count=1&title=" + MusicName + "$$";
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
            HttpWebResponse Response = (HttpWebResponse)req.GetResponse();
            Stream stream = Response.GetResponseStream();
            StreamReader sr = new StreamReader(stream, Encoding.UTF8);
            string result = sr.ReadToEnd();
            while (result.Contains("</decode>"))
            {
                try
                {
                    string STR = result.Substring(0, result.IndexOf("</decode>") + 9);

                    string HeadUrl = STR.Substring(STR.IndexOf("<![CDATA[") + 9);
                    HeadUrl = HeadUrl.Substring(0, HeadUrl.IndexOf("$]]>"));
                    HeadUrl = HeadUrl.Substring(0, HeadUrl.LastIndexOf("/") + 1);

                    string EndUrl = STR.Substring(STR.IndexOf("<decode>"));
                    EndUrl = EndUrl.Substring(EndUrl.IndexOf("<![CDATA[") + 9);
                    EndUrl = EndUrl.Substring(0, EndUrl.IndexOf("]]>"));

                    string Url = HeadUrl + EndUrl;
                    MusicUrl.Add(Url);
                    result = result.Replace(STR, "");
                }
                catch
                {
                    break;
                }
            }
            return MusicUrl;
        }
    }
}
