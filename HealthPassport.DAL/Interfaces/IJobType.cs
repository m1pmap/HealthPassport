using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Interfaces
{
    public interface IJobType
    {
        public List<JobType> GetAllJobTypes();
        public int Get_JobTypeIdByName(string jobName);
        public string Get_JobNameById(int id);
    }
}
