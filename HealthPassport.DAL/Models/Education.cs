using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Models
{
    public class Education
    {
        public int EducationId { get; set; }
        [MaxLength(100)]
        public string EducationInstitution { get; set; }
        public DateTime Date {  get; set; }

        public int EmployeeId { get; set; }
        public int EducationLevelId { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual EducationLevel EducationLevel { get; set; }

    }
}
