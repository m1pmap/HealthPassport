using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Repositories
{
    public class Vaccination_Repository : ICudRepository<Vaccination>
    {
        private readonly ApplicationDbContext _db;
        public Vaccination_Repository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public bool Add_Item(Vaccination entity)
        {
            try
            {
                _db.Vaccinations.Add(entity);
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
                Vaccination? dbVaccination = _db.Vaccinations.FirstOrDefault(e => e.VaccinationId == id);
                if (dbVaccination != null)
                {
                    _db.Vaccinations.Remove(dbVaccination);
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

        public bool Update_Item(Vaccination entity)
        {
            try
            {
                Vaccination updateVaccination = _db.Vaccinations.FirstOrDefault(e => e.VaccinationId == entity.VaccinationId);

                if (updateVaccination != null)
                {
                    if(updateVaccination.Name != entity.Name)
                        updateVaccination.Name = entity.Name;
                    
                    if(updateVaccination.Date != entity.Date)
                        updateVaccination.Date = entity.Date;
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
