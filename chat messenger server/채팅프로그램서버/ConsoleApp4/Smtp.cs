using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class Smtp:SmtpClient
    {
        public Smtp(string emailAddr, int _num)
        {
            this.Host = "smtp.gmail.com";
            this.Port = 587;
            this.UseDefaultCredentials = false;
            this.EnableSsl = true;
            this.DeliveryMethod = SmtpDeliveryMethod.Network;
            this.Credentials = new System.Net.NetworkCredential("your email id", "your email password");
            MailAddress from = new MailAddress("your email id", "send name", System.Text.Encoding.UTF8);
            MailAddress to = new MailAddress(emailAddr);
            MailMessage message1 = new MailMessage(from, to);
            message1.Body = "인증번호: " + _num;
            //string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
            // message1.Body += Environment.NewLine + someArrows;
            message1.BodyEncoding = System.Text.Encoding.UTF8;
            message1.Subject = "test message 2";
            message1.SubjectEncoding = System.Text.Encoding.UTF8;
            try
            {
                this.Send(message1);
                message1.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("이메일인증요청" + emailAddr);
        }
    }
}
