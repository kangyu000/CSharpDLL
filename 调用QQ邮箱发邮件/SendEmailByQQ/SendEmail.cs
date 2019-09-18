using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Net;

namespace SendEmailByQQ
{
    public static class SendEmail
    {
        public static int AttachmentNumber = 0;//附件索引

        #region 填写邮件内容

        /// <summary>
        /// 发件人、收件人、抄送、主题、正文、附件、用户名、密码
        /// </summary>
        /// <param name="From">发件人</param>
        /// <param name="To">收件人</param>
        /// <param name="CC">抄送</param>
        /// <param name="Subject">主题</param>
        /// <param name="Body">正文</param>
        /// <param name="Attachment">附件</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Pwd">密码</param>
        /// <returns></returns>
        public static string SendMail(string From, List<string> To, List<string> CC, string Subject, string Body, List<string> Attachment, string UserName, string Pwd, string smtp)
        {
            string SendMessage;

            //发件人邮箱 
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(From);

            //收件人邮箱 
            for (int i = 0; i < To.Count; i++)
            {
                mailMessage.To.Add(To[i]);
            }

            //抄送       
            if (CC != null)
            {
                foreach (string cc in CC)
                {
                    if (cc != string.Empty)
                    {
                        mailMessage.CC.Add(cc);
                    }
                }
            }

            //主题   
            mailMessage.Subject = Subject;

            //正文          
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = true;

            //附件
            if (Attachment.Count > 0)
            {
                FileInfo file = new FileInfo(Attachment[AttachmentNumber]);
                FileStream fs = new FileStream(Attachment[AttachmentNumber], FileMode.Open, FileAccess.Read);
                if (fs.Length > (long)int.MaxValue)
                {

                }
                else
                {
                    Attachment attach = new Attachment(Attachment[AttachmentNumber]);
                    mailMessage.Attachments.Add(attach);
                    attach.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                }
            }
            SendMessage = SetSmtpClient(mailMessage, UserName, Pwd, smtp);
            return SendMessage;
        }
        #endregion

        #region 发送邮件
        private static string SetSmtpClient(MailMessage mail, string UserName, string Pwd, string smtp)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.EnableSsl = false;
                smtpClient.Host = smtp;       //smtp服务器地址     
                //smtpClient.Port = 587;        //端口号     
                smtpClient.Credentials = new NetworkCredential(UserName, Pwd);
                smtpClient.Send(mail);
                return "发送完成！";
            }
            catch (Exception ee)
            {
                return ee.Message.ToString();
            }
        }
        #endregion

    }
}
