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


        public bool SendMail(string ToAddress, string Msg, string Subject, bool isBodyHtml = false)
        {
            if (string.IsNullOrEmpty(ToAddress))
                return false;

            Msg += "Note:Please do not reply to this e~mail address because its a virtual address.";
            MailMessage mailMsg = new MailMessage(_appSettings.Value.SenderAddress, ToAddress);
            mailMsg.Priority = MailPriority.High;
            {
                var withBlock = mailMsg;
                withBlock.IsBodyHtml = isBodyHtml;
                withBlock.Subject = Subject;
                withBlock.Body = Msg;
            }
            try
            {
                SmtpClient client = new SmtpClient(_appSettings.Value.SmtpServer, _appSettings.Value.SmtpPort);
                client.Send(mailMsg);
                
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                //throw new ApplicationException("Error while attempting to send a notification to the specified email addres. Please contact your administrator or check your mail server settings.");
            }
        }

        public Task SendMailAsync(string ToAddress, string Msg, string Subject, bool isBodyHtml = false)
        {
           
            Msg += "Note:Please do not reply to this e~mail address because its a virtual address.";
            MailMessage mailMsg = new MailMessage(_appSettings.Value.SenderAddress, ToAddress);
            mailMsg.Priority = MailPriority.High;
            {
                var withBlock = mailMsg;
                withBlock.IsBodyHtml = isBodyHtml;
                withBlock.Subject = Subject;
                withBlock.Body = Msg;
            }
            try
            {
                SmtpClient client = new SmtpClient(_appSettings.Value.SmtpServer, _appSettings.Value.SmtpPort);
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
