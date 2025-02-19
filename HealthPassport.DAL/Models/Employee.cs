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
        public DateTime Birthday { get; set; }
        [MaxLength(50)]
        public string MailAdress {  get; set; } = string.Empty;
        public byte[] Photo { get; set; } = null;

        public virtual ICollection<Disease> Diseases { get; set; } = new List<Disease>();
        public virtual ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();
        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
        public virtual ICollection<FamilyStatus> FamilyStatuses { get; set; } = new List<FamilyStatus>();
        public virtual ICollection<AntropologicalResearch> AntropologicalResearches { get; set; } = new List<AntropologicalResearch>();
        public virtual ICollection<Education> Educations { get; set; } = new List<Education>();
    }
}
