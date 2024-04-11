using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Net;

/// <summary>
/// MailUtil 的摘要描述
/// </summary>
public class MailUtil
{
    public void MailTo(string toMail, string subject, string body)
    {
        SendMail(toMail, "", "", subject, body);
    }
    public void MailTo(string toMail, string ccMail, string subject, string body)
    {
        SendMail(toMail, ccMail, "", subject, body);
    }
    public void MailTo(string toMail, string ccMail, string bccMail, string subject, string body)
    {
        SendMail(toMail, ccMail, bccMail, subject, body);
    }

    public void SendMail(string toMail, string ccMail, string bccMail, string subject, string body)
    {
        if (ConfigurationManager.AppSettings["MailStatus"] == "open")
        {
			// 創建 SmtpClient
			string smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
			SmtpClient client = new SmtpClient(smtpServer);
			client.Port = 587;
			client.EnableSsl = true; // 啟用 SSL/TLS 加密
			
			// 使用者認證資訊
			string username = ConfigurationManager.AppSettings["SmtpUserName"];
			string password = ConfigurationManager.AppSettings["SmtpPw"];

			// 設置用戶名和密碼
			client.Credentials = new NetworkCredential(username, password);
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			
			// Mail Information
			MailMessage message = new MailMessage();
            message.From = new MailAddress(username, ConfigurationManager.AppSettings["MailSender"]);
            //TO
            string[] toAddr = toMail.Split(',');
            for (int i = 0; i < toAddr.Length; i++)
            {
                if (!string.IsNullOrEmpty(toAddr[i]))
                {
                    message.To.Add(new MailAddress(toAddr[i]));
                }
            }

            //CC
            string[] ccAddr = ccMail.Split(',');
            for (int i = 0; i < ccAddr.Length; i++)
            {
                if (!string.IsNullOrEmpty(ccAddr[i]))
                {
                    message.CC.Add(new MailAddress(ccAddr[i]));
                }
            }

            //BCC
            string[] bccAddr = bccMail.Split(',');
            for (int i = 0; i < bccAddr.Length; i++)
            {
                if (!string.IsNullOrEmpty(bccAddr[i]))
                {
                    message.Bcc.Add(new MailAddress(bccAddr[i]));
                }
            }



			body += "<br><br><br>※本信件由系統自動發送，請勿直接回覆";
			message.Subject = subject;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.Body = body;
            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.UTF8;

            client.Send(message);
        }
    }
}