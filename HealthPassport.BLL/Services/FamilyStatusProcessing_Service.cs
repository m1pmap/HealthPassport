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
    public class FamilyStatusProcessing_Service : ICudProcessing<FamilyStatus>
    {
        private readonly ICudRepository<FamilyStatus> _familyStatusRepository;
        public FamilyStatusProcessing_Service(ICudRepository<FamilyStatus> familyStatusRepository) 
        {
            _familyStatusRepository = familyStatusRepository;
        }

        public bool Add_Item(FamilyStatus entity)
        {
            return _familyStatusRepository.Add_Item(entity);
        }

        public bool Delete_Item(int id)
        {
            return _familyStatusRepository.Delete_Item(id);

        }

        public bool Update_Item(FamilyStatus entity)
        {
            return _familyStatusRepository.Update_Item(entity);
        }
    }
}
