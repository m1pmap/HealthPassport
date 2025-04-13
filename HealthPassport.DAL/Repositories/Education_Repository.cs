using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Repositories
{
    public class Education_Repository : ICudRepository<Education>
    {
        private readonly ApplicationDbContext _db;
        public Education_Repository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Add_Item(Education entity)
        {
            try
            {
                _db.Educations.Add(entity);
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
                Education? dbEducation = _db.Educations.FirstOrDefault(e => e.EducationId == id);
                if (dbEducation != null)
                {
                    _db.Educations.Remove(dbEducation);
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

        public bool Update_Item(Education entity)
        {
            try
            {
                Education updateEducation = _db.Educations.FirstOrDefault(e => e.EducationId == entity.EducationId);

                if (updateEducation != null)
                {
                    if(updateEducation.EducationLevelId != entity.EducationLevelId)
                        updateEducation.EducationLevelId = entity.EducationLevelId;

                    if(updateEducation.EducationInstitution != entity.EducationInstitution)
                        updateEducation.EducationInstitution = entity.EducationInstitution;

                    if(updateEducation.Date != entity.Date)
                        updateEducation.Date = entity.Date;
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
