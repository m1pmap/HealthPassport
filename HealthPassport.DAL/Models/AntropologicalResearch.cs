using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Models
{
    public class AntropologicalResearch
    {
        public int AntropologicalResearchId { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public DateTime Date { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
