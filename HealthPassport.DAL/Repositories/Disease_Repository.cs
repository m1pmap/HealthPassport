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

        public bool Add_Item(Disease entity)
        {
            try
            {
                _db.Diseases.Add(entity);
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

        public List<Disease> Get_AllItems()
        {
            return _db.Diseases.ToList();
        }

        public bool Update_Item(Disease entity)
        {
            try
            {
                Disease updateDisease = _db.Diseases.FirstOrDefault(e => e.DiseaseId == entity.DiseaseId);

                if (updateDisease != null)
                {
                    if(updateDisease.Name != entity.Name)
                        updateDisease.Name = entity.Name;

                    if(updateDisease.StardDiseaseDate != entity.StardDiseaseDate)
                        updateDisease.StardDiseaseDate = entity.StardDiseaseDate;

                    if(updateDisease.EndDiseaseDate != entity.EndDiseaseDate)
                        updateDisease.EndDiseaseDate = entity.EndDiseaseDate;
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
