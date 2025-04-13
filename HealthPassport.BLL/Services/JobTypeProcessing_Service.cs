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
    public class JobTypeProcessing_Service : IJobTypeProcessing
    {
        private readonly IJobType _jobTypeRepository;

        public JobTypeProcessing_Service(IJobType jobTypeRepository)
        {
            _jobTypeRepository = jobTypeRepository;
        }

        public bool Add_Item(JobType entity)
        {
            return _jobTypeRepository.Add_Item(entity);
        }

        public bool Delete_Item(int id)
        {
            return _jobTypeRepository.Delete_Item(id);
        }

        public List<JobType> Get_AllItems()
        {
            return _jobTypeRepository.Get_AllItems();
        }

        public int Get_IdByMainParam(string mainParam)
        {
            return _jobTypeRepository.Get_IdByMainParam(mainParam);
        }

        public string Get_MainParamById(int id)
        {
            return _jobTypeRepository.Get_MainParamById(id);
        }

        public bool Update_Item(JobType entity)
        {
            return _jobTypeRepository.Update_Item(entity);
        }
    }
}
