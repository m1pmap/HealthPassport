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
    public class VaccinationProcessing_Service : ICudProcessing<Vaccination>
    {
        private readonly ICudRepository<Vaccination> _vaccinationRepository;
        public VaccinationProcessing_Service(ICudRepository<Vaccination> vaccinationRepository) 
        {
            _vaccinationRepository = vaccinationRepository;
        }

        public bool Add_Item(Vaccination entity)
        {
            return _vaccinationRepository.Add_Item(entity);
        }

        public bool Delete_Item(int id)
        {
            return _vaccinationRepository.Delete_Item(id);
        }

        public bool Update_Item(Vaccination entity)
        {
            return _vaccinationRepository.Update_Item(entity);
        }
    }
}
