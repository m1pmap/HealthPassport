using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Repositories
{
    public class Disease_Repository : IDisease
    {
        private readonly ApplicationDbContext _db;
        public Disease_Repository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public bool Add_Disease(Disease disease)
        {
            try
            {
                _db.Diseases.Add(disease);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete_Disease(int id)
        {
            try
            {
                Disease? dbDisease = _db.Diseases.FirstOrDefault(e => e.DiseaseId == id);
                if (dbDisease != null)
                {
                    _db.Diseases.Remove(dbDisease);
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

        public bool Update_Disease(Disease newDisease)
        {
            try
            {
                Disease updateDisease = _db.Diseases.FirstOrDefault(e => e.DiseaseId == newDisease.DiseaseId);

                if (updateDisease != null)
                {
                    updateDisease.Name = newDisease.Name;
                    updateDisease.StardDiseaseDate = newDisease.StardDiseaseDate;
                    updateDisease.EndDiseaseDate = newDisease.EndDiseaseDate;
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
