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

        public bool Add_Item(Disease entity)
        {
            return _diseaseRepository.Add_Item(entity);
        }

        public bool Delete_Item(int id)
        {
            return _diseaseRepository.Delete_Item(id);
        }

        public List<Disease> Get_AllItems()
        {
            return _diseaseRepository.Get_AllItems();
        }

        public bool Update_Item(Disease entity)
        {
            return _diseaseRepository.Update_Item(entity);
        }
    }
}
