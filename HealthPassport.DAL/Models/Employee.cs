using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        [MaxLength(200)]
        public string FIO { get; set; } = string.Empty;
        [MaxLength(150)]
        public string Job { get; set; } = string.Empty;
        [MaxLength(120)]
        public string Education { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }
        [MaxLength(50)]
        public string FamilyStatus { get; set; } = string.Empty;
        [MaxLength(50)]
        public string MailAdress {  get; set; } = string.Empty;
        public byte[] Photo { get; set; } = null;
    }
}
