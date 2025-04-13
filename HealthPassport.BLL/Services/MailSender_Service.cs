using HealthPassport.BLL.Interfaces;
using System.Windows;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HealthPassport.DAL.Repositories;
using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;

namespace HealthPassport.BLL.Services
{
    public class MailSender_Service : IMailSender
    {
        private readonly string mailFrom = "dok_koks@mail.ru";
        private readonly string pass = "TKZB28r34gSTNVmY7DeW";

        public bool SendEmailCode(string mailTo, string subject, string text)
        {
            try
            {
                int currentYear = DateTime.Now.Year;

                MailMessage message = new MailMessage();
                message.From = new MailAddress(mailFrom);
                message.To.Add(mailTo);
                message.Subject = subject;
                message.IsBodyHtml = true;

                //HTML-стили для красивого оформления письма
                string htmlBody = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            text-align: center;
                            padding: 20px;
                        }}
                        .container {{
                            background: white;
                            padding: 20px;
                            border-radius: 10px;
                            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
                            display: inline-block;
                            text-align: center;
                        }}
                        .title {{
                            font-size: 24px;
                            font-weight: bold;
                            color: #3cb371;
                        }}
                        .code {{
                            font-size: 28px;
                            font-weight: bold;
                            color: #ffffff;
                            background: #3cb371;
                            padding: 10px 20px;
                            display: inline-block;
                            border-radius: 5px;
                            margin-top: 15px;
                        }}
                        .footer {{
                            font-size: 12px;
                            color: gray;
                            margin-top: 20px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='title'>🔐 Код подтверждения</div>
                        <div class='code'> {text}</div>
                        <div class='footer'>Если вы не запрашивали этот код, просто проигнорируйте письмо.<br>© {currentYear} HealthPassport.</div>
                    </div>
                </body>
                </html>";

                message.Body = htmlBody;

                SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                smtp.Credentials = new NetworkCredential(mailFrom, pass);
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Timeout = 120000;
                smtp.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка отправки сообщения: " + ex.Message, ex);
            }
        }

        public string GenerateAlphaNumericCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool SendFromAdminMailMessage(string mailTo, string subject, string text)
        {
            try
            {
                int currentYear = DateTime.Now.Year;

                MailMessage message = new MailMessage();
                message.From = new MailAddress(mailFrom);
                message.To.Add(mailTo);
                message.Subject = "HealthPassport";
                message.IsBodyHtml = true;

                //HTML-стили для красивого оформления письма
                string htmlBody = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f8f9fa;
                            color: #2c3e50;
                            text-align: left;
                            padding: 20px;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: auto;
                            padding: 25px;
                            border-radius: 8px;
                            background-color: #ffffff;
                            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
                        }}
                        .header {{
                            font-size: 22px;
                            font-weight: bold;
                            color: #34495e;
                            margin-bottom: 20px;
                            border-bottom: 2px solid #3b82f6;
                            padding-bottom: 10px;
                        }}
                        .message {{
                            font-size: 16px;
                            line-height: 1.6;
                            color: #444;
                            margin-bottom: 20px;
                        }}
                        .footer {{
                            font-size: 12px;
                            color: #888;
                            margin-top: 20px;
                            border-top: 1px solid #ddd;
                            padding-top: 10px;
                            text-align: center;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>📢 {subject}</div>
                        <div class='message'>
                            Добрый день, {text}
            
                        </div>
                        <div class='footer'>
                            Данное письмо отправлено автоматически, пожалуйста, не отвечайте на него.<br>
                            © {currentYear} HealthPassport.
                        </div>
                    </div>
                </body>
                </html>";

                message.Body = htmlBody;

                SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                smtp.Credentials = new NetworkCredential(mailFrom, pass);
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Timeout = 120000;
                smtp.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка отправки сообщения: " + ex.Message, ex);
            }
        }
    }
}
