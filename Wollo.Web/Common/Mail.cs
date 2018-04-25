using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using Wollo.Entities.Models;
using Wollo.Base.LocalResource;

namespace Wollo.Web.Common
{
    public class Mail
    {
        //ViewModels.PortalUser objPortalUser(first argument)
        /// <summary>
        /// Send mail to single user
        /// </summary>
        /// <param name="objPortalUser"></param>
        /// <param name="Subject"></param>
        /// <param name="Message"></param>
        public static string sendMail(Wollo.Entities.Models.User user, string Subject, string Message)
        {
            string htmlMessage;
            htmlMessage = Resource.EmailTemplate.ToString();
            htmlMessage = htmlMessage.Replace("&gt;", ">");
            htmlMessage = htmlMessage.Replace("&lt;", "<");
            htmlMessage = htmlMessage.Replace("\r", "");
            htmlMessage = htmlMessage.Replace("\n", "");
            htmlMessage = htmlMessage.Replace("\t", "");
            htmlMessage = HttpUtility.HtmlDecode(htmlMessage);
            using (System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage())
            {
                m.To.Add(new System.Net.Mail.MailAddress(user.email_address));
                m.Subject = Subject;
                m.Body = string.Format(htmlMessage,
                user.user_name + " ", Message, DateTime.UtcNow.Year);
                m.IsBodyHtml = true;
                try
                {
                    using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
                    {
                        smtp.Send(m);
                        return "success";
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.InnerException.Message.ToString();
                    return message;
                }

            }
        }

        /// <summary>
        /// Send mail to multiple users
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="Subject"></param>
        /// <param name="Message"></param>
        public static string sendMailMultiple(List<Wollo.Entities.Models.User> userInfo,Wollo.Entities.Models.User sender, string Subject, string Message)
        {
            string htmlMessage;
            string message = string.Empty;
            string addressList = String.Empty;
            if (userInfo.Count > 0)
            {
                foreach (var user in userInfo)
                {
                    addressList += user.email_address + ",";
                }
                addressList = addressList.Substring(0, (addressList.Length - 1));
                htmlMessage = Resource.EmailTemplate.ToString();
                htmlMessage = htmlMessage.Replace("&gt;", ">");
                htmlMessage = htmlMessage.Replace("&lt;", "<");
                htmlMessage = htmlMessage.Replace("\r", "");
                htmlMessage = htmlMessage.Replace("\n", "");
                htmlMessage = htmlMessage.Replace("\t", "");
                htmlMessage = HttpUtility.HtmlDecode(htmlMessage);
                using (System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage())
                {
                    m.To.Add(new System.Net.Mail.MailAddress(sender.email_address));
                    m.Bcc.Add(addressList);
                    m.Subject = Subject;
                    m.Body = string.Format(htmlMessage, "User", Message, DateTime.UtcNow.Year);
                    m.IsBodyHtml = true;
                    try
                    {
                        using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
                        {
                            smtp.Send(m);
                        }
                        message= "Success";
                    }
                    catch (Exception ex)
                    {
                        message= ex.InnerException.Message.ToString();
                    }
                }
            }
            return message;
        }
    }
}