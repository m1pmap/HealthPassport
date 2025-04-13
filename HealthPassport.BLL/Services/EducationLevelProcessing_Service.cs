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
    public class EducationLevelProcessing_Service : IGetProcessing<EducationLevel>
    {
        private readonly IGetRepository<EducationLevel> _educationLevelRepository;
        public EducationLevelProcessing_Service(IGetRepository<EducationLevel> educationLevelRepository)
        {
            _educationLevelRepository = educationLevelRepository;
        }

        public List<EducationLevel> Get_AllItems()
        {
            return _educationLevelRepository.Get_AllItems();
        }

        public int Get_IdByMainParam(string mainParam)
        {
            return _educationLevelRepository.Get_IdByMainParam(mainParam);
        }

        public string Get_MainParamById(int id)
        {
            return _educationLevelRepository.Get_MainParamById(id);
        }
    }
}
