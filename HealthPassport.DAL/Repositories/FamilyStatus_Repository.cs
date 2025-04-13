using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Repositories
{
    public class FamilyStatus_Repository : ICudRepository<FamilyStatus>
    {
        private readonly ApplicationDbContext _db;
        public FamilyStatus_Repository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public bool Add_Item(FamilyStatus entity)
        {
            try
            {
                _db.FamilyStatuses.Add(entity);
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
                FamilyStatus? dbfamilyStatus = _db.FamilyStatuses.FirstOrDefault(fs => fs.FamilyStatusId == id);
                if (dbfamilyStatus != null)
                {
                    _db.FamilyStatuses.Remove(dbfamilyStatus);
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

        public bool Update_Item(FamilyStatus entity)
        {
            try
            {
                FamilyStatus? updateFamilyStatus = _db.FamilyStatuses.FirstOrDefault(fs => fs.FamilyStatusId == entity.FamilyStatusId);

                if (updateFamilyStatus != null)
                {
                    if(updateFamilyStatus.Status != entity.Status)
                        updateFamilyStatus.Status = entity.Status;

                    if(updateFamilyStatus.StartFamilyDate != entity.StartFamilyDate)
                        updateFamilyStatus.StartFamilyDate = entity.StartFamilyDate;

                    if(updateFamilyStatus.EndFamilyDate != entity.EndFamilyDate)
                        updateFamilyStatus.EndFamilyDate = entity.EndFamilyDate;
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
