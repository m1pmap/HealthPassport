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
    public class AuditLogProcessing_Service : IAuditLogProcessing
    {
        private readonly IAuditLog _auditLogRepository;

        public AuditLogProcessing_Service(IAuditLog auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }
        public bool Add_Item(AuditLog entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete_Item(int id)
        {
            return _auditLogRepository.Delete_Item(id);
        }

        public List<AuditLog> Get_AllItems()
        {
            return _auditLogRepository.Get_AllItems();
        }

        public int Get_IdByMainParam(string mainParam)
        {
            throw new NotImplementedException();
        }

        public string Get_MainParamById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update_Item(AuditLog entity)
        {
            throw new NotImplementedException();
        }
    }
}
