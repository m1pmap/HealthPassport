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
        public EducationProcessing_Service(IEducation educationRepository) 
        {
            _educationRepository = educationRepository;
        }
        public bool Add_Education(Education education)
        {
            return _educationRepository.Add_Education(education);
        }

        public bool Delete_Education(int id)
        {
            return _educationRepository.Delete_Education(id);
        }

        public bool Update_Education(Education education)
        {
            return _educationRepository.Update_Education(education);
        }
    }
}
