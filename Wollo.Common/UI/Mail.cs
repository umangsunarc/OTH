using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using Wollo.Entities.Models;
using Wollo.Base.LocalResource;

namespace Wollo.Common.UI
{
    public static class Mail
    {
        //ViewModels.PortalUser objPortalUser(first argument)
        /// <summary>
        /// Send mail to single user
        /// </summary>
        /// <param name="objPortalUser"></param>
        /// <param name="Subject"></param>
        /// <param name="Message"></param>
        public static int sendMail(Wollo.Entities.Models.User user, string Subject, string Message)
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
                user.user_name+" ", Message, DateTime.UtcNow.Year);
                m.IsBodyHtml = true;
                using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
                {
                    smtp.Send(m);
                }
            }
            return 0;
        }

        //public static int sendMailToUser(ViewModels.RegisterRequest registration, string Subject, string Message)
        //{
        //    string htmlMessage;
        //    htmlMessage = Properties.Resources.EmailTemplate.ToString();
        //    htmlMessage = htmlMessage.Replace("&gt;", ">");
        //    htmlMessage = htmlMessage.Replace("&lt;", "<");
        //    htmlMessage = htmlMessage.Replace("\r", "");
        //    htmlMessage = htmlMessage.Replace("\n", "");
        //    htmlMessage = htmlMessage.Replace("\t", "");
        //    htmlMessage = HttpUtility.HtmlDecode(htmlMessage);
        //    using (System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage())
        //    {
        //        m.To.Add(new System.Net.Mail.MailAddress(registration.Email));
        //        m.Subject = Subject;
        //        m.Body = string.Format(htmlMessage,
        //        registration.Title.Text + " " + registration.LastName, Message, DateTime.UtcNow.Year);
        //        m.IsBodyHtml = true;
        //        using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
        //        {
        //            smtp.Send(m);
        //        }
        //    }
        //    return 0;
        //}

        ///// <summary>
        ///// Send mail to multiple users
        ///// </summary>
        ///// <param name="userInfo"></param>
        ///// <param name="Subject"></param>
        ///// <param name="Message"></param>
        //public static void sendMailMultiple(List<PortalUser> userInfo, string Subject, string Message)
        //{
        //    string htmlMessage;

        //    string addressList = String.Empty;
        //    if (userInfo.Count > 0)
        //    {
        //        foreach (var user in userInfo)
        //        {
        //            addressList += user.Email + ",";
        //        }
        //        addressList = addressList.Substring(0, (addressList.Length - 1));
        //        htmlMessage = Properties.Resources.EmailTemplate.ToString();
        //        htmlMessage = htmlMessage.Replace("&gt;", ">");
        //        htmlMessage = htmlMessage.Replace("&lt;", "<");
        //        htmlMessage = htmlMessage.Replace("\r", "");
        //        htmlMessage = htmlMessage.Replace("\n", "");
        //        htmlMessage = htmlMessage.Replace("\t", "");
        //        htmlMessage = HttpUtility.HtmlDecode(htmlMessage);
        //        using (System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage())
        //        {
        //            m.To.Add(addressList);
        //            m.Subject = Subject;
        //            m.Body = string.Format(htmlMessage, "User", Message, DateTime.UtcNow.Year);
        //            m.IsBodyHtml = true;
        //            using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
        //            {
        //                smtp.Send(m);
        //            }
        //        }
        //    }
        //}
    }
}

