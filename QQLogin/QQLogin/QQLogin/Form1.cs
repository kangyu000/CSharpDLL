using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;

namespace QQLogin
{
    public partial class Form1 : Form
    {
        ///命令功能:通过上传验证码图片字节到服务器进行验证码识别，方便多线程发送 
        ///b:上传验证码图片字节集
        ///len:上传验证码图片字节集长度
        ///strVcodeUser：联众账号
        ///strVcodePass：联众密码
        ///成功返回->验证码结果|!|打码工人；后台没点数了返回:No Money! ；未注册返回:No Reg! ；上传验证码失败:Error:Put Fail!  ；识别超时了:Error:TimeOut!  ；上传无效验证码:Error:empty picture!  
        [DllImport("FastVerCode.dll")]
        private static extern string RecByte(byte[] b, int len, string strVcodeUser, string strVcodePass);
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "2480002728";
            textBox2.Text = "233185510876";
            GetVCode();
        }
        CookieContainer cc = new CookieContainer();
        private void GetVCode() 
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://captcha.qq.com/getimage?uin="+textBox1.Text+"&aid=549000912&0.9356742896384528");
            req.Accept = "*/*";
            req.Referer = "http://ui.ptlogin2.qq.com/cgi-bin/login?hide_title_bar=1&low_login=0&qlogin_auto_login=1&no_verifyimg=1&link_target=blank&appid=549000912&style=12&target=self&s_url=http%3A//qzs.qq.com/qzone/v5/loginsucc.html?para=izone&pt_qr_app=%CA%D6%BB%FAQQ%BF%D5%BC%E4&pt_qr_link=http%3A//z.qzone.com/download.html&self_regurl=http%3A//qzs.qq.com/qzone/v6/reg/index.html&pt_qr_help_link=http%3A//z.qzone.com/download.html";
            req.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
            req.Host = "captcha.qq.com";
            req.CookieContainer = cc;
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream sr = res.GetResponseStream();
            Bitmap bitmap = new Bitmap(sr, false);
            pictureBox1.Image = (Image)bitmap;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            GetVCode();
        }
        const string str = "http://ptlogin2.qq.com/login?u={0}&p={1}&verifycode={2}&aid=549000912&u1=http%3A%2F%2Fqzs.qq.com%2Fqzone%2Fv5%2Floginsucc.html%3Fpara%3Dizone&h=1&ptredirect=0&ptlang=2052&from_ui=1&dumy=&low_login_enable=0&regmaster=&fp=loginerroralert&action=4-6-1378107872656&mibao_css=&t=1&g=1&js_ver=10042&js_type=1&login_sig=rNpVncXOl8VYllT9O3-xM8VsoHgzJdGoSPJVkQyMbkP2Q0H0xFf4Ltv83VXGwHJp";
        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string s = Class1.GetPassword(textBox1.Text, textBox2.Text, textBox3.Text);
            string ss = PasswordHelper.GetPassword(textBox1.Text, textBox2.Text, textBox3.Text);
            string url = string.Format(str, textBox1.Text, ss, textBox3.Text);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Accept = "*/*";
            req.Referer = "http://ui.ptlogin2.qq.com/cgi-bin/login?hide_title_bar=1&low_login=0&qlogin_auto_login=1&no_verifyimg=1&link_target=blank&appid=549000912&style=12&target=self&s_url=http%3A//qzs.qq.com/qzone/v5/loginsucc.html?para=izone&pt_qr_app=%CA%D6%BB%FAQQ%BF%D5%BC%E4&pt_qr_link=http%3A//z.qzone.com/download.html&self_regurl=http%3A//qzs.qq.com/qzone/v6/reg/index.html&pt_qr_help_link=http%3A//z.qzone.com/download.html";
            req.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
            req.Host = "ptlogin2.qq.com";
            req.CookieContainer = cc;
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string strResult1 = sr.ReadToEnd();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            Bitmap bmp = (Bitmap)pictureBox1.Image;
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] photo_byte = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(photo_byte, 0, Convert.ToInt32(ms.Length));
            bmp.Dispose();
            System.Diagnostics.Debug.WriteLine(photo_byte.Length);
            string returnMess = RecByte(photo_byte, photo_byte.Length, "bingogo", "61523050");
            if (returnMess.Equals("No Money!"))
            {
                MessageBox.Show("点数不足", "友情提示");
            }
            else if (returnMess.Equals("No Reg!"))
            {
                MessageBox.Show("没有注册", "友情提示");
            }
            else if (returnMess.Equals("Error:Put Fail!"))
            {
                MessageBox.Show("上传验证码失败", "友情提示");
            }
            else if (returnMess.Equals("Error:TimeOut!"))
            {
                MessageBox.Show("识别超时", "友情提示");
            }
            else if (returnMess.Equals("Error:empty picture!"))
            {
                MessageBox.Show("上传无效验证码", "友情提示");
            }
            else
            {
                textBox3.Text = returnMess.Split('|')[0];
                string ss = returnMess.Split('|')[2];
                MessageBox.Show("识别成功", "友情提示");
            }
        }
    }
}
