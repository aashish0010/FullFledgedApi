using FullFledgedModel;
using FullFledgedRepository.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullFledgedRepository.Repository
{
    public class SendMailRepo : IMailServiceInterface
    {
        private readonly MailSetting _mailSetting;
        public SendMailRepo(IOptions<MailSetting> options)
        {
            _mailSetting = options.Value;
        }
        public async Task SendEmailAsync(SendMail sendMail)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSetting.Mail);
            email.To.Add(MailboxAddress.Parse(sendMail.ToEmail));
            email.Subject = sendMail.Subject;
            var builder = new BodyBuilder();
            //if (sendMail.Attachment != null)
            //{
            //    byte[] fileBytes;
            //    foreach (var files in sendMail.Attachment)
            //    {
            //        if (files.Length > 0)
            //        {
            //            using (var ms = new MemoryStream())
            //            {
            //                files.CopyTo(ms);
            //                fileBytes = ms.ToArray();
            //            }
            //            builder.Attachments.Add(files.FileName, fileBytes, ContentType.Parse(files.ContentType));
            //        }
            //    }
            //}
            builder.HtmlBody = sendMail.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSetting.Mail, _mailSetting.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
    }
