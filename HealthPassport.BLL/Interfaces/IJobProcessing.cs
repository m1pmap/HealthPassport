using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.BLL.Interfaces
{
    public interface IJobProcessing
    {
        bool Add_Job(Job newJob);
        bool Delete_Job(int id);
        bool Update_Job(Job newJob);

        List<JobType> GetAllJobTypes();
        public string Get_JobNameById(int id);
        public int Get_JobTypeIdByName(string jobName);

        List<Subunit> GetAllSubunits();
        public string Get_SubunitNameById(int id);
        public int Get_SubunitIdByName(string subunitName);
    }
}
