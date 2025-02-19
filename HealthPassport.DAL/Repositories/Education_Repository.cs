using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Repositories
{
    public class Education_Repository : IEducation
    {
        private readonly ApplicationDbContext _db;
        public Education_Repository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool Add_Education(Education education)
        {
            try
            {
                _db.Educations.Add(education);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete_Education(int id)
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

        public bool Update_Education(Education education)
        {
            try
            {
                Education updateEducation = _db.Educations.FirstOrDefault(e => e.EducationId == education.EducationId);

                if (updateEducation != null)
                {
                    updateEducation.EducationType = education.EducationType;
                    updateEducation.EducationInstitution = education.EducationInstitution;
                    updateEducation.Date = education.Date;
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
