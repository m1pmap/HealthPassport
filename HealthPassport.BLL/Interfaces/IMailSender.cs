using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.BLL.Interfaces
{
    public interface IMailSender
    {
        public bool SendEmailCode(string mailTo, string subject, string text);
        public string GenerateAlphaNumericCode(int length);
        public bool IsValidEmail(string email);
        bool SendFromAdminMailMessage(string mailTo, string subject, string text);
    }
}
