using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace MusicInfoLib
{
    public static class GetLrcClass
    {
        public static string GetLrc(string MusicName) 
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://geci.me/api/lyric/" + MusicName);
            HttpWebResponse Response = (HttpWebResponse)req.GetResponse();
            Stream stream = Response.GetResponseStream();
            StreamReader sr = new StreamReader(stream, Encoding.UTF8);
            string result = sr.ReadToEnd();
            if (result.Contains("http://s.geci.me/lrc/"))
            {
                result = result.Substring(result.IndexOf("http://s.geci.me/lrc/"));
                result = result.Substring(0, result.IndexOf("\""));
                req = (HttpWebRequest)WebRequest.Create(result);
                Response = (HttpWebResponse)req.GetResponse();
                stream = Response.GetResponseStream();
                sr = new StreamReader(stream, Encoding.UTF8);
                result = sr.ReadToEnd();
            }
            else 
            {
                result = string.Empty;
            }
            return result;
        }

        public static string GetLrc(string MusicName, string SingerName) 
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://geci.me/api/lyric/" + MusicName + "/" + SingerName);
            HttpWebResponse Response = (HttpWebResponse)req.GetResponse();
            Stream stream = Response.GetResponseStream();
            StreamReader sr = new StreamReader(stream, Encoding.UTF8);
            string result = sr.ReadToEnd();
            if (result.Contains("http://s.geci.me/lrc/"))
            {
                result = result.Substring(result.IndexOf("http://s.geci.me/lrc/"));
                result = result.Substring(0, result.IndexOf("\""));
                req = (HttpWebRequest)WebRequest.Create(result);
                Response = (HttpWebResponse)req.GetResponse();
                stream = Response.GetResponseStream();
                sr = new StreamReader(stream, Encoding.UTF8);
                result = sr.ReadToEnd();
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }
    }
}
