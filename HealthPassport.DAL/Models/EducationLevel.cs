using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Models
{
    public class EducationLevel
    {
        public int EducationLevelId {  get; set; }
        public string EducationLevelName { get; set; }

        public virtual ICollection<Education> Educations { get; set; } = new List<Education>();
    }
}
