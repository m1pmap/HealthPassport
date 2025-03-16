using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Models
{
    public class Subunit
    {
        [Key]
        public int SubunitId { get; set; }
        public string SubunitName { get; set; }


        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
