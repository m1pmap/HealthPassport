using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Models
{
    public class FamilyStatus
    {
        public int FamilyStatusId { get; set; }
        public string Status { get; set; }
        public DateTime StartFamilyDate { get; set; }
        public DateTime EndFamilyDate { get; set;}

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
