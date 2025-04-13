using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Repositories
{
    public class Subunit_Repository : ISubunit
    {
        private readonly ApplicationDbContext _db;
        public Subunit_Repository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public bool Add_Item(Subunit entity)
        {
            try
            {
                _db.Subunits.Add(entity);
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
                Subunit? dbSubunit = _db.Subunits.FirstOrDefault(e => e.SubunitId == id);
                if (dbSubunit != null)
                {
                    _db.Subunits.Remove(dbSubunit);
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

        public List<Subunit> Get_AllItems()
        {
            return _db.Subunits.ToList();
        }

        public int Get_IdByMainParam(string mainParam)
        {
            return _db.Subunits.FirstOrDefault(jt => jt.SubunitName == mainParam).SubunitId;
        }

        public string Get_MainParamById(int id)
        {
            return _db.Subunits.FirstOrDefault(jt => jt.SubunitId == id).SubunitName;

        }

        public bool Update_Item(Subunit entity)
        {
            try
            {
                Subunit? updateSubunit = _db.Subunits.FirstOrDefault(j => j.SubunitId == entity.SubunitId);

                if (updateSubunit != null)
                {
                    if(updateSubunit.SubunitId != entity.SubunitId)
                        updateSubunit.SubunitId = entity.SubunitId;

                    if(updateSubunit.SubunitName != entity.SubunitName)
                        updateSubunit.SubunitName = entity.SubunitName;
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
