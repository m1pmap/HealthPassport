using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Repositories
{
    public class AuditLog_Repository : IAuditLog
    {
        private readonly ApplicationDbContext _db;
        public AuditLog_Repository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool Add_Item(AuditLog entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete_Item(int id)
        {
            try
            {
                AuditLog? dbAuditLog = _db.AuditLogs.FirstOrDefault(e => e.AuditLogId == id);
                if (dbAuditLog != null)
                {
                    _db.AuditLogs.Remove(dbAuditLog);
                    _db.SaveChanges();

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public List<AuditLog> Get_AllItems()
        {
            return _db.AuditLogs.ToList();
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
