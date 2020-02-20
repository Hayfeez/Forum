using System;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Forum.CommonClasses
{
    public class MailHelper
    {
        IOptions<ReadSMTPSettings> _smtpSettings;

        public MailHelper(IOptions<ReadSMTPSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings;
        }


        public bool SendMail(string ToAddress, string Msg, string Subject, bool CopyHR, bool isBodyHtml)
        {
            if (string.IsNullOrEmpty(ToAddress))
                return false;

            Msg += "Note:Please do not reply to this e~mail address because its a virtual address.";
            MailMessage mailMsg = new MailMessage(_smtpSettings.Value.SenderAddress, ToAddress);
            mailMsg.Priority = MailPriority.High;
            {
                var withBlock = mailMsg;
                withBlock.IsBodyHtml = isBodyHtml;
                withBlock.Subject = Subject;
                withBlock.Body = Msg;
            }
            try
            {
                SmtpClient client = new SmtpClient(_smtpSettings.Value.SmtpServer, _smtpSettings.Value.SmtpPort);
                client.Send(mailMsg);
                
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                //throw new ApplicationException("Error while attempting to send a notification to the specified email addres. Please contact your administrator or check your mail server settings.");
            }
        }


    }
}
