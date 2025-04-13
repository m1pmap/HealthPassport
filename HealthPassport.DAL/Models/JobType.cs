using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Models
{
    public class JobType
    {
        [Key]
        public int JobTypeId { get; set; }
        public string JobName { get; set; }
        public bool isCanAddRows { get; set; } = false;
        public bool isCanSendMessages { get; set; } = false;
        public bool isCanEditItems { get; set; } = false;


        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
