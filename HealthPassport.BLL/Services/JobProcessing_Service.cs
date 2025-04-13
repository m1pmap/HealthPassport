using HealthPassport.BLL.Interfaces;
using HealthPassport.DAL;
using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.BLL.Services
{
    public class JobProcessing_Service : ICudProcessing<Job>
    {
        private readonly ICudRepository<Job> _jobRepository;
        public JobProcessing_Service(ICudRepository<Job> jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public bool Add_Item(Job entity)
        {
            return _jobRepository.Add_Item(entity);
        }

        public bool Delete_Item(int id)
        {
            return _jobRepository.Delete_Item(id);
        }
        public bool Update_Item(Job entity)
        {
            return _jobRepository.Update_Item(entity);
        }
    }
}
