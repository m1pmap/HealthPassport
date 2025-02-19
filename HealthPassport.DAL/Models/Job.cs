using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Models
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }
        public string Subunit {  get; set; }
        public double WorkingRate {  get; set; }
        public DateTime StartWorkingDate {  get; set; }
        public DateTime EndWorkingDate {  get; set; }
        public int EmployeeId { get; set; }
        public int JobTypeId { get; set; }

        public virtual JobType JobType { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
