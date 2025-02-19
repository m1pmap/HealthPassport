using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Repositories
{
    public class Job_Repository : IJob
    {
        private readonly ApplicationDbContext _db;
        public Job_Repository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool Add_Job(Job newJob)
        {
            try
            {
                _db.Jobs.Add(newJob);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete_Job(int id)
        {
            try
            {
                Job? dbJob = _db.Jobs.FirstOrDefault(e => e.JobId == id);
                if (dbJob != null)
                {
                    _db.Jobs.Remove(dbJob);
                    _db.SaveChanges();

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Update_Job(Job newJob)
        {
            try
            {
                Job? updateJob = _db.Jobs.FirstOrDefault(j => j.JobId == newJob.JobId);

                if (updateJob != null)
                {
                    updateJob.Subunit = newJob.Subunit;
                    updateJob.WorkingRate = newJob.WorkingRate;
                    updateJob.StartWorkingDate = newJob.StartWorkingDate;
                    updateJob.EndWorkingDate = newJob.EndWorkingDate;
                    updateJob.JobTypeId = newJob.JobTypeId;
                    _db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
