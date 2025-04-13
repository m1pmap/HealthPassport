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
    public class EducationProcessing_Service : ICudProcessing<Education>
    {
        private readonly ICudRepository<Education> _educationRepository;
        public EducationProcessing_Service(ICudRepository<Education> educationRepository) 
        {
            _educationRepository = educationRepository;
        }

        public bool Add_Item(Education entity)
        {
            return _educationRepository.Add_Item(entity);
        }

        public bool Delete_Item(int id)
        {
            return _educationRepository.Delete_Item(id);
        }

        public bool Update_Item(Education entity)
        {
            return _educationRepository.Update_Item(entity);
        }
    }
}
