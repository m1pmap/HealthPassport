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
    public class JobProcessing_Service : IJobProcessing
    {
        private readonly IJob _jobRepository;
        private readonly IJobType _jobTypeRepository;
        private readonly ISubunit _subunitRepository;
        public JobProcessing_Service(IJob jobRepository,
            IJobType jobTypeRepository,
            ISubunit subunitRepository)
        {
            _jobRepository = jobRepository;
            _jobTypeRepository = jobTypeRepository;
            _subunitRepository = subunitRepository;
        }
        public bool Add_Job(Job newJob)
        {
            return _jobRepository.Add_Job(newJob);
        }

        public bool Delete_Job(int id)
        {
            return _jobRepository.Delete_Job(id);
        }

        public List<JobType> GetAllJobTypes()
        {
            return _jobTypeRepository.GetAllJobTypes();
        }

        public List<Subunit> GetAllSubunits()
        {
            return _subunitRepository.GetAllSubunits();
        }

        public string Get_JobNameById(int id)
        {
            return _jobTypeRepository.Get_JobNameById(id);
        }

        public int Get_JobTypeIdByName(string jobName)
        {
            return _jobTypeRepository.Get_JobTypeIdByName(jobName);

        }

        public int Get_SubunitIdByName(string subunitName)
        {
            return _subunitRepository.Get_SubunitIdByName(subunitName);
        }

        public string Get_SubunitNameById(int id)
        {
            return _subunitRepository.Get_SubunitNameById(id);
        }

        public bool Update_Job(Job newJob)
        {
            return _jobRepository.Update_Job(newJob);
        }
    }
}
