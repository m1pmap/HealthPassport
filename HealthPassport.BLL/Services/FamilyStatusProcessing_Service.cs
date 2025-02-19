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
    public class FamilyStatusProcessing_Service : IFamilyStatusProcessing
    {
        private readonly IFamilyStatus _familyStatusRepository;
        public FamilyStatusProcessing_Service(IFamilyStatus familyStatusRepository) 
        {
            _familyStatusRepository = familyStatusRepository;
        }
        public bool Add_FamilyStatus(FamilyStatus familyStatus)
        {
            return _familyStatusRepository.Add_FamilyStatus(familyStatus);
        }

        public bool Delete_FamilyStatus(int id)
        {
            return _familyStatusRepository.Delete_FamilyStatus(id);
        }

        public bool Update_FamilyStatus(FamilyStatus familyStatus)
        {
            return _familyStatusRepository.Update_FamilyStatus(familyStatus);
        }
    }
}
