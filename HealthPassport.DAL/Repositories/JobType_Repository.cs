using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Repositories
{
    public class JobType_Repository : IJobType
    {
        private readonly ApplicationDbContext _db;
        public JobType_Repository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public List<JobType> GetAllJobTypes()
        {
            return _db.JobTypes.ToList();
        }

        public string Get_JobNameById(int id)
        {
            return _db.JobTypes.FirstOrDefault(jt => jt.JobTypeId == id).JobName;
        }

        public int Get_JobTypeIdByName(string jobName)
        {
            return _db.JobTypes.FirstOrDefault(jt => jt.JobName == jobName).JobTypeId;
        }
    }
}
