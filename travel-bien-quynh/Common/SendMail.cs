using travel_bien_quynh.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using ZstdSharp.Unsafe;

namespace travel_bien_quynh.CoTravel_bien_quynh
{
    public class SendMail
    {
        public string FROM { get; private set; }
        public string FROMNAME { get; private set; }
        public string HOST { get; private set; }
        public int PORT { get; private set; }
        public string SMTP_USERNAME { get; private set; }
        public string SMTP_PASSWORD { get; private set; }
        public bool IsSystem { get; private set; }
        private readonly IEmailConfiguration _emailConfiguration;
        //send
        private static string[] listMail = {
                    "novaerp07@na.com.vn",
                    "novaerp03@na.com.vn",
                    "novaerp04@na.com.vn",
                    "novaerp05@na.com.vn",
                    "novaerp06@na.com.vn",
                    "mca1@na.com.vn",
                    "mca2@na.com.vn",
                    "mca3@na.com.vn",
                    "mca4@na.com.vn",
                    "mca5@na.com.vn",
                    "mca6@na.com.vn",
                    "mca7@na.com.vn",
                    "mca8@na.com.vn",
                    "mca9@na.com.vn",
                    "mca10@na.com.vn"
                };

        private static string SMTPUser = listMail[randInt()];
        private static string SMTPPassword = "moingaymotniemvui@556";//Utils.GetSetting<string>(MAIL_PASSWORD, MAIL_PASS);

        public SendMail(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        //public string sendMailByAmazonSDK(string receiverAddress, string subject, string htmlBody, string textBody)
        //{
        //    string msg = String.Empty;
        //    string emailSend = System.Configuration.ConfigurationManager.AppSettings["FROM"];

        //    using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1))
        //    {
        //        var sendRequest = new SendEmailRequest
        //        {
        //            Source = emailSend,
        //            Destination = new Destination
        //            {
        //                ToAddresses =
        //                new List<string> { receiverAddress },
        //            },
        //            Message = new Amazon.SimpleEmail.Model.Message
        //            {
        //                Subject = new Content(subject),
        //                Body = new Body
        //                {
        //                    Html = new Content
        //                    {
        //                        Charset = "UTF-8",
        //                        Data = htmlBody
        //                    },
        //                    Text = new Content
        //                    {
        //                        Charset = "UTF-8",
        //                        Data = textBody
        //                    }
        //                }
        //            },
        //            // If you are not using a configuration set, comment
        //            // or remove the following line
        //            //ConfigurationSetName = configSet
        //        };
        //        try
        //        {
        //            Console.WriteLine("Sending email using Amazon SES...");
        //            var response = client.SendEmail(sendRequest);
        //            Console.WriteLine("The email was sent successfully.");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("The email was not sent.");
        //            Console.WriteLine("Error message: " + ex.Message);
        //            return ex.Message;
        //        }
        //    }
        //    return msg;
        //}

        public string sendMailBySmtp(string TO, string CC, string SUBJECT, string BODY)
        {
            string msg = "";
            if (_emailConfiguration.IsSystem == false)
            {
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                message.From = new MailAddress(_emailConfiguration.SmtpUsername, _emailConfiguration.FromName);
                message.Subject = SUBJECT;
                message.Body = BODY;

                string[] s_To = TO.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                message.To.Clear();
                message.CC.Clear();
                foreach (var to in s_To)
                {
                    if (message.To.Where(x => x.Address == to).Count() == 0)//cmt
                    {
                        message.To.Add(to.ToString());
                    }
                }
                string[] s_CC = CC.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var cc in s_CC)
                {
                    if (cc.ToString() != "")
                    {
                        message.CC.Add(cc.ToString());
                    }
                }

                //leave as it is even if you are not sending HTML message
                message.IsBodyHtml = true;

                //set the priority of the mail message to normal
                message.Priority = MailPriority.Normal;

                message.SubjectEncoding = Encoding.UTF8;
                message.BodyEncoding = Encoding.UTF8;
                // Comment or delete the next line if you are not using a configuration set
                //message.Headers.Add("X-SES-CONFIGURATION-SET", CONFIGSET);

                using (var client = new SmtpClient(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort))
                {
                    // Pass SMTP credentials
                    client.Credentials = new NetworkCredential(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                    // Enable SSL encryption
                    client.EnableSsl = true;

                    // Try to send the message. Show status in console.
                    try
                    {
                        Console.WriteLine("Attempting to send email...");
                        client.Send(message);
                        Console.WriteLine("Email sent!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("The email was not sent.");
                        Console.WriteLine("Error message: " + ex.Message);
                        return ex.Message;
                    }
                }
            }
            else
            {
                int sendResult = Send(TO, SUBJECT, BODY, out msg, out string mailSender);
            }
            return msg;
        }

        public static int randInt()
        {
            // Usually this can be a field rather than a method variable
            Random rand = new Random();

            // nextInt is normally exclusive of the top value,
            // so add 1 to make it inclusive
            int dice = rand.Next(0, 14);

            return dice;
        }

        public int Send(string to, string subject, string body, out string msg, out string mailSender)
        {
            return Send(new[] { to }, null, null, subject, body, out msg, out mailSender);
        }

        public int Send(string sender, string[] to, string subject, string content, out string msg, out string mailSender)
        {
            return Send(to, null, null, subject, content, out msg, out mailSender);
        }

        public int Send(string[] to, string[] cc, string subject, string content, out string msg, out string mailSender)
        {
            return Send(to, cc, null, subject, content, out msg, out mailSender);
        }

        public int Send(string[] tolist, string[] ccList, string[] bccList, string subject, string body, out string msg, out string mailSender)
        {
            try
            {
                //string SMTPUser = Utils.GetSetting<string>(MAIL_SENDER, MAIL_ADDRESS);
                //string SMTPPassword = Utils.GetSetting<string>(MAIL_PASSWORD, MAIL_PASS);
                SMTPUser = listMail[randInt()];
                //Now instantiate a new instance of MailMessage
                using (var mail = new MailMessage())
                {
                    //set the sender address of the mail message
                    mail.From = new MailAddress(SMTPUser, "Mobile CRM");

                    //set the recepient addresses of the mail message
                    if (tolist != null && tolist.Any())
                        foreach (var to in tolist)
                        {
                            if (to != null)
                            {
                                string to1 = to.Trim();
                                if (to.Trim().EndsWith(","))
                                {
                                    to1 = to.Substring(0, to.Length - 1);
                                }
                                if (to1.Trim().Length > 0 && mail.To.Where(x => x.Address == to1).Count() == 0)
                                {
                                    mail.To.Add(to1.Trim());
                                }
                            }
                        }

                    //set the recepient addresses of the mail message
                    if (ccList != null && ccList.Any())
                        foreach (var cc in ccList)
                        {
                            if (cc != null)
                            {
                                string cc1 = cc.Trim();
                                if (cc.Trim().EndsWith(","))
                                {
                                    cc1 = cc.Substring(0, cc.Length - 1);
                                }
                                if (cc1.Trim().Length > 0)
                                {
                                    mail.CC.Add(cc1.Trim());
                                }
                            }
                        }

                    if (bccList != null && bccList.Any())
                        foreach (var bcc in bccList)
                        {
                            if (bcc != null)
                            {
                                mail.Bcc.Add(bcc);
                            }
                        }

                    //set the subject of the mail message
                    mail.Subject = subject;

                    //set the body of the mail message
                    mail.Body = body;

                    //leave as it is even if you are not sending HTML message
                    mail.IsBodyHtml = true;

                    //set the priority of the mail message to normal
                    mail.Priority = MailPriority.Normal;

                    mail.SubjectEncoding = Encoding.UTF8;
                    mail.BodyEncoding = Encoding.UTF8;

                    //instantiate a new instance of SmtpClient
                    using (var smtp = new SmtpClient())
                    {
                        //if you are using your smtp server, then change your host like "smtp.yourdomain.com"

                        smtp.Host = "smtp.gmail.com";

                        //chnage your port for your host
                        smtp.Port = 587; //or you can also use port# 587

                        //provide smtp credentials to authenticate to your account
                        smtp.Credentials = new System.Net.NetworkCredential(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                        //if you are using secure authentication using SSL/TLS then "true" else "false"
                        smtp.EnableSsl = true;

                        smtp.Send(mail);

                        msg = string.Empty;
                        mailSender = _emailConfiguration.SmtpUsername;
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.Instance.Info(string.Format("Send email exception: {0}\t\n {1}", ex.Message, subject));
                msg = ex.Message;
                mailSender = SMTPUser;
                return 0;
            }
        }

        public static int Send(string[] tolist, string[] ccList, string subject, string body, string[] Attachfile)
        {
            try
            {
                //string SMTPUser = Utils.GetSetting<string>(MAIL_SENDER, MAIL_ADDRESS);
                //string SMTPPassword = Utils.GetSetting<string>(MAIL_PASSWORD, MAIL_PASS);
                SMTPUser = listMail[randInt()];
                //Now instantiate a new instance of MailMessage
                using (var mail = new MailMessage())
                {
                    //set the sender address of the mail message
                    mail.From = new MailAddress(SMTPUser, "NOVAON HRM");

                    //set the recepient addresses of the mail message

                    if (tolist != null && tolist.Any())
                        foreach (var to in tolist)
                        {
                            if (to != null)
                            {
                                string to1 = to;
                                if (to.Trim().EndsWith(","))
                                {
                                    to1 = to.Substring(0, to.Length - 1);
                                }
                                if (to1.Trim().Length > 0)
                                {
                                    mail.To.Add(to1);
                                }
                            }
                        }

                    //set the recepient addresses of the mail message
                    if (ccList != null && ccList.Any())
                        foreach (var cc in ccList)
                        {
                            if (cc != null)
                            {
                                string cc1 = cc;
                                if (cc.Trim().EndsWith(","))
                                {
                                    cc1 = cc.Substring(0, cc.Length - 1);
                                }
                                if (cc1.Trim().Length > 0)
                                {
                                    mail.CC.Add(cc1);
                                }
                            }
                        }

                    //set the subject of the mail message
                    mail.Subject = subject;

                    //set the body of the mail message
                    mail.Body = body;

                    //leave as it is even if you are not sending HTML message
                    mail.IsBodyHtml = true;

                    //set the priority of the mail message to normal
                    mail.Priority = MailPriority.Normal;

                    mail.SubjectEncoding = Encoding.UTF8;
                    mail.BodyEncoding = Encoding.UTF8;

                    //mail.Attachments.Add(new Attachment(_ms, "ABC-Certificate.Pdf", "application/pdf"));
                    System.Net.Mail.Attachment attachment;
                    foreach (var file in Attachfile)
                    {
                        if (file != null && file.Length > 1)
                        {
                            attachment = new System.Net.Mail.Attachment(file);
                            mail.Attachments.Add(attachment);
                        }
                    }

                    //instantiate a new instance of SmtpClient
                    using (var smtp = new SmtpClient())
                    {
                        //if you are using your smtp server, then change your host like "smtp.yourdomain.com"
                        smtp.Host = "smtp.gmail.com";

                        //chnage your port for your host
                        smtp.Port = 587; //or you can also use port# 587

                        //provide smtp credentials to authenticate to your account
                        smtp.Credentials = new System.Net.NetworkCredential(SMTPUser, SMTPPassword);

                        //if you are using secure authentication using SSL/TLS then "true" else "false"
                        smtp.EnableSsl = true;

                        smtp.Send(mail);

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.Instance.Info(string.Format("Send email exception: {0}\t\n {1}", ex.Message, subject));
                return 0;
            }
            finally
            {
                foreach (var file in Attachfile)
                {
                    if (file != null && file.Length > 1)
                    {
                        if (System.IO.File.Exists(file))
                        {
                            System.IO.File.Delete(file);
                        }
                    }
                }
            }
        }
    }
}
