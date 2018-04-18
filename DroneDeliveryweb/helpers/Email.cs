using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EASendMail;

namespace DroneDeliveryweb.helpers
{
    public class Email
    {
        public static void Sendmail(string to, string body, string subject)
        {
            

            SmtpMail omail = new SmtpMail("TryIt");
            SmtpClient oSmtp = new SmtpClient();

            omail.From = "droneydelivery@gmail.com";
            omail.To = to;
            omail.Subject = subject;
            omail.TextBody = body;
            SmtpServer oserver = new SmtpServer("smtp.gmail.com");

            oserver.Port = 587;
            oserver.ConnectType = SmtpConnectType.ConnectSSLAuto;

            oserver.User = "droneydelivery@gmail.com";
            oserver.Password = "Dronedelivery";
            try
            {
                oSmtp.SendMail(oserver, omail);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}