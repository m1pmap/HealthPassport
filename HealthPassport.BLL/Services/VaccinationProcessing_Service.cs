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
    public class VaccinationProcessing_Service : IVaccinationProcessing
    {
        private readonly IVaccination _vaccinationRepository;
        public VaccinationProcessing_Service(IVaccination vaccinationRepository) 
        {
            _vaccinationRepository = vaccinationRepository;
        }
        public bool Add_Vaccination(Vaccination newVaccination)
        {
            return _vaccinationRepository.Add_Vaccination(newVaccination);
        }

        public bool Delete_Vaccination(int id)
        {
            return _vaccinationRepository.Delete_Vaccination(id);
        }

        public bool Update_Vaccination(Vaccination vaccination)
        {
            return _vaccinationRepository.Update_Vaccination(vaccination);
        }
    }
}
