using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Repositories
{
    public class FamilyStatus_Repository : IFamilyStatus
    {
        private readonly ApplicationDbContext _db;
        public FamilyStatus_Repository(ApplicationDbContext db) 
        {
            _db = db;
        }
        public bool Add_FamilyStatus(FamilyStatus familyStatus)
        {
            try
            {
                _db.FamilyStatuses.Add(familyStatus);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete_FamilyStatus(int id)
        {
            try
            {
                FamilyStatus? dbfamilyStatus= _db.FamilyStatuses.FirstOrDefault(fs => fs.FamilyStatusId == id);
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

        public bool Update_FamilyStatus(FamilyStatus familyStatus)
        {
            try
            {
                FamilyStatus ?updateFamilyStatus = _db.FamilyStatuses.FirstOrDefault(fs => fs.FamilyStatusId == familyStatus.FamilyStatusId);

                if (updateFamilyStatus != null)
                {
                    updateFamilyStatus.Status = familyStatus.Status;
                    updateFamilyStatus.StartFamilyDate = familyStatus.StartFamilyDate;
                    updateFamilyStatus.EndFamilyDate = familyStatus.EndFamilyDate;
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
