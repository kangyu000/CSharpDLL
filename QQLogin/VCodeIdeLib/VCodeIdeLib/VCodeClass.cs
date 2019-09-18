using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;

namespace VCodeIdeLib
{
    public static class VCodeClass
    {
        ///命令功能:通过上传验证码图片字节到服务器进行验证码识别，方便多线程发送 
        ///b:上传验证码图片字节集
        ///len:上传验证码图片字节集长度
        ///strVcodeUser：联众账号
        ///strVcodePass：联众密码
        ///成功返回->验证码结果|!|打码工人；后台没点数了返回:No Money! ；未注册返回:No Reg! ；上传验证码失败:Error:Put Fail!  ；识别超时了:Error:TimeOut!  ；上传无效验证码:Error:empty picture!  
        [DllImport("FastVerCode.dll")]
        private static extern string RecByte(byte[] b, int len, string strVcodeUser, string strVcodePass);

        /// <summary>
        /// 验证码识别
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        public static string SreamToString(Bitmap bmp, string UserName, string PassWord) 
        {
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] photo_byte = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(photo_byte, 0, Convert.ToInt32(ms.Length));
            //bmp.Dispose();
            System.Diagnostics.Debug.WriteLine(photo_byte.Length);
            string returnMess = RecByte(photo_byte, photo_byte.Length, UserName, PassWord);
            if (returnMess.Equals("No Money!"))
            {
                return "ERROR!!点数不足";
            }
            else if (returnMess.Equals("No Reg!"))
            {
                return "ERROR!!没有注册";
            }
            else if (returnMess.Equals("Error:Put Fail!"))
            {
                return "ERROR!!上传验证码失败";
            }
            else if (returnMess.Equals("Error:TimeOut!"))
            {
                return "ERROR!!识别超时";
            }
            else if (returnMess.Equals("Error:empty picture!"))
            {
                return "ERROR!!上传无效验证码";
            }
            else
            {
                return returnMess.Split('|')[0];
            }
        }
    }
}
