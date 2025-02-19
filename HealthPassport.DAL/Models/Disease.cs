using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Models
{
    public class Disease
    {
        public int DiseaseId { get; set; }
        [MaxLength(60)]
        public string Name{ get; set; } = string.Empty;
        public DateTime StardDiseaseDate{ get; set; }
        public DateTime EndDiseaseDate{ get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
