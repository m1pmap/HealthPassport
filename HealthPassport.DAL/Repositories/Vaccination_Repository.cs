using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Repositories
{
    public class Vaccination_Repository : IVaccination
    {
        private readonly ApplicationDbContext _db;
        public Vaccination_Repository(ApplicationDbContext db) 
        {
            _db = db;
        }
        public bool Add_Vaccination(Vaccination newVaccination)
        {
            try
            {
                _db.Vaccinations.Add(newVaccination);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete_Vaccination(int id)
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

        public bool Update_Vaccination(Vaccination vaccination)
        {
            try
            {
                Vaccination updateVaccination = _db.Vaccinations.FirstOrDefault(e => e.VaccinationId == vaccination.VaccinationId);

                if (updateVaccination != null)
                {
                    updateVaccination.Name = vaccination.Name;
                    updateVaccination.Date = vaccination.Date;
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
