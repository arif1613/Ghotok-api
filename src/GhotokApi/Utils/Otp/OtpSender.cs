using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using GhotokApi.Models.RequestModels;

namespace GhotokApi.Utils.Otp
{
    public class OtpSender : IOtpSender
    {
        public Task SendOtpMobileMessage(OtpRequestModel model, string otp)
        {
            throw new NotImplementedException();
        }

        public Task SendOtpEmailMessage(OtpRequestModel model, string otp)
        {
            return Task.Run(() =>
            {
                using var message = new MailMessage();
                message.To.Add(new MailAddress("arif1613@yahoo.com", "Arif Anwarul"));
                message.From = new MailAddress("arif1613@gmail.com", "Ghotok Api");
                //message.CC.Add(new MailAddress("cc@email.com", "CC Name"));
                //message.Bcc.Add(new MailAddress("bcc@email.com", "BCC Name"));
                message.Subject = "Ghotok Registration Code";
                message.Body = $"Your Ghotok Code is: {otp}";
                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential("arif1613@gmail.com", "aa1613++");
                    client.EnableSsl = true;
                    client.Send(message);
                }
            });

        }
    }
}
