using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Forum.Helpers
{
    public class MailHelper
    {
        IOptions<ReadAppSettings> _appSettings;

        public MailHelper(IOptions<ReadAppSettings> appSettings)
        {
            _appSettings = appSettings;
        }


        public bool SendMail(string ToAddress, string Msg, string Subject, bool isBodyHtml = true)
        {
            if (string.IsNullOrEmpty(ToAddress))
                return false;

            Msg += "<br/>Note:Please do not reply to this e~mail address because its a virtual address.";
            MailMessage mailMsg = new MailMessage(_appSettings.Value.SMTPSenderAddress, ToAddress);
            mailMsg.Priority = MailPriority.High;
            {
                var withBlock = mailMsg;
                withBlock.IsBodyHtml = isBodyHtml;
                withBlock.Subject = Subject;
                withBlock.Body = Msg;
            }
            try
            {
                SmtpClient client = new SmtpClient(_appSettings.Value.SMTPServerHost, _appSettings.Value.SMTPServerPort);

                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential(_appSettings.Value.SMTPServerUsername, _appSettings.Value.SMTPServerPassword);            

                client.DeliveryMethod = SmtpDeliveryMethod.Network;                
                client.Send(mailMsg);
                
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                //throw new ApplicationException("Error while attempting to send a notification to the specified email addres. Please contact your administrator or check your mail server settings.");
            }
        }

        public Task SendMailAsync(string ToAddress, string Msg, string Subject, bool isBodyHtml = true)
        {
           
            Msg += "<br/>Note:Please do not reply to this e~mail address because its a virtual address.";
            MailMessage mailMsg = new MailMessage(_appSettings.Value.SMTPSenderAddress, ToAddress);
            mailMsg.Priority = MailPriority.High;
            {
                var withBlock = mailMsg;
                withBlock.IsBodyHtml = isBodyHtml;
                withBlock.Subject = Subject;
                withBlock.Body = Msg;
            }
            try
            {
                SmtpClient client = new SmtpClient(_appSettings.Value.SMTPServerHost, _appSettings.Value.SMTPServerPort);
                client.Send(mailMsg);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw ex;
                //throw new ApplicationException("Error while attempting to send a notification to the specified email addres. Please contact your administrator or check your mail server settings.");
            }
        }


    }
}
