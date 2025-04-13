using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Repositories
{
    public class Job_Repository : ICudRepository<Job>
    {
        private readonly ApplicationDbContext _db;
        public Job_Repository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Add_Item(Job entity)
        {
            try
            {
                _db.Jobs.Add(entity);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete_Item(int id)
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

        public bool Update_Item(Job entity)
        {
            try
            {
                Job? updateJob = _db.Jobs.FirstOrDefault(j => j.JobId == entity.JobId);

                if (updateJob != null)
                {
                    if(updateJob.SubunitId != entity.SubunitId)
                        updateJob.SubunitId = entity.SubunitId;

                    if(updateJob.WorkingRate != entity.WorkingRate)
                        updateJob.WorkingRate = entity.WorkingRate;

                    if(updateJob.StartWorkingDate != entity.StartWorkingDate)
                        updateJob.StartWorkingDate = entity.StartWorkingDate;

                    if(updateJob.EndWorkingDate != entity.EndWorkingDate)
                        updateJob.EndWorkingDate = entity.EndWorkingDate;

                    if(updateJob.JobTypeId != entity.JobTypeId)
                        updateJob.JobTypeId = entity.JobTypeId;
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
