using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Repositories
{
    public class EducationLevel_Repository : IEducationLevel
    {
        private readonly ApplicationDbContext _db;
        public EducationLevel_Repository(ApplicationDbContext db) 
        {
            _db = db;
        }
        public List<EducationLevel> GetAllEducationLevels()
        {
            return _db.EducationLevels.ToList();
        }

        public int Get_EducationLevelIdByName(string educationLevelName)
        {
            return _db.EducationLevels.FirstOrDefault(el => el.EducationLevelName == educationLevelName).EducationLevelId;
        }

        public string Get_EducationLevelNameById(int id)
        {
            return _db.EducationLevels.FirstOrDefault(el => el.EducationLevelId == id).EducationLevelName;
        }
    }
}
