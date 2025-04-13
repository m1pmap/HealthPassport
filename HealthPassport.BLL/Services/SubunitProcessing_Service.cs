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
    public class SubunitProcessing_Service : ISubunitProcessing
    {
        private readonly ISubunit _subunitRepository;

        public SubunitProcessing_Service(ISubunit subunitRepository)
        {
            _subunitRepository = subunitRepository;
        }

        public bool Add_Item(Subunit entity)
        {
            return _subunitRepository.Add_Item(entity);
        }

        public bool Delete_Item(int id)
        {
            return _subunitRepository.Delete_Item(id);
        }

        public List<Subunit> Get_AllItems()
        {
            return _subunitRepository.Get_AllItems();
        }

        public int Get_IdByMainParam(string mainParam)
        {
            return _subunitRepository.Get_IdByMainParam(mainParam);
        }

        public string Get_MainParamById(int id)
        {
            return _subunitRepository.Get_MainParamById(id);
        }

        public bool Update_Item(Subunit entity)
        {
            return _subunitRepository.Update_Item(entity);
        }
    }
}
