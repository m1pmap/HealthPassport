using HealthPassport.BLL.Interfaces;
using System.Windows;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HealthPassport.DAL.Repositories;
using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using Autofac;

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
    }
}
