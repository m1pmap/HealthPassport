using HealthPassport.BLL.Interfaces;
using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.BLL.Services
{
    public class EducationProcessing_Service : IEducationProcessing
    {
        private readonly IEducation _educationRepository;
        private readonly IEducationLevel _educationLevelRepository;
        public EducationProcessing_Service(IEducation educationRepository, IEducationLevel educationLevelRepository) 
        {
            _educationRepository = educationRepository;
            _educationLevelRepository = educationLevelRepository;
        }
        public bool Add_Education(Education education)
        {
            return _educationRepository.Add_Education(education);
        }

        public bool Delete_Education(int id)
        {
            return _educationRepository.Delete_Education(id);
        }

        public List<EducationLevel> GetAllEducationLevels()
        {
            return _educationLevelRepository.GetAllEducationLevels();
        }

        public int Get_EducationLevelIdByName(string educationLevelName)
        {
            return _educationLevelRepository.Get_EducationLevelIdByName(educationLevelName);
        }

        public string Get_EducationLevelNameById(int id)
        {
            return _educationLevelRepository.Get_EducationLevelNameById(id);

        }

        public bool Update_Education(Education education)
        {
            return _educationRepository.Update_Education(education);
        }
    }
}
