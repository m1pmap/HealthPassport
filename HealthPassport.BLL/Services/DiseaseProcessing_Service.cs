using HealthPassport.BLL.Interfaces;
using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using HealthPassport.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.BLL.Services
{
    public class DiseaseProcessing_Service : IDiseaseProcessing
    {
        private readonly IDisease _diseaseRepository;
        public DiseaseProcessing_Service(IDisease diseaseRepository) 
        {
            _diseaseRepository = diseaseRepository;
        }
        public bool Add_Disease(Disease newDisease)
        {
            return _diseaseRepository.Add_Disease(newDisease);
        }

        public bool Delete_Disease(int id)
        {
            return _diseaseRepository.Delete_Disease(id);
        }

        public bool Update_Disease(Disease newDisease)
        {
            return _diseaseRepository.Update_Disease(newDisease);
        }
    }
}
