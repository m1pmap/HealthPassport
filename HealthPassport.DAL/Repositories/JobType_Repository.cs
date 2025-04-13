using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public bool Add_Item(JobType entity)
        {
            try
            {
                _db.JobTypes.Add(entity);
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
                JobType? dbJobType = _db.JobTypes.FirstOrDefault(e => e.JobTypeId == id);
                if (dbJobType != null)
                {
                    _db.JobTypes.Remove(dbJobType);
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

        public List<JobType> Get_AllItems()
        {
            return _db.JobTypes.ToList();
        }

        public int Get_IdByMainParam(string mainParam)
        {
            return _db.JobTypes.FirstOrDefault(jt => jt.JobName == mainParam).JobTypeId;
        }

        public string Get_MainParamById(int id)
        {
            return _db.JobTypes.FirstOrDefault(jt => jt.JobTypeId == id).JobName;
        }

        public bool Update_Item(JobType entity)
        {
            try
            {
                JobType? updateJobType = _db.JobTypes.FirstOrDefault(j => j.JobTypeId == entity.JobTypeId);

                if (updateJobType != null)
                {
                    if(updateJobType.JobName != entity.JobName)
                        updateJobType.JobName = entity.JobName;

                    if(updateJobType.isCanSendMessages != entity.isCanSendMessages)
                        updateJobType.isCanSendMessages = entity.isCanSendMessages;

                    if(updateJobType.isCanAddRows != entity.isCanAddRows)
                        updateJobType.isCanAddRows = entity.isCanAddRows;

                    if(updateJobType.isCanEditItems != entity.isCanEditItems)
                        updateJobType.isCanEditItems = entity.isCanEditItems;
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
