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
    public class EducationLevel_Repository : IGetRepository<EducationLevel>
    {
        private readonly ApplicationDbContext _db;
        public EducationLevel_Repository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public List<EducationLevel> Get_AllItems()
        {
            return _db.EducationLevels.ToList();
        }

        public int Get_IdByMainParam(string mainParam)
        {
            return _db.EducationLevels.FirstOrDefault(el => el.EducationLevelName == mainParam).EducationLevelId;
        }

        public string Get_MainParamById(int id)
        {
            return _db.EducationLevels.FirstOrDefault(el => el.EducationLevelId == id).EducationLevelName;
        }
    }
}
