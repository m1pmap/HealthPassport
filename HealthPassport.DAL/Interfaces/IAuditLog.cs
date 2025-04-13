using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Interfaces
{
    public interface IAuditLog : ICudRepository<AuditLog>, IGetRepository<AuditLog>
    {
    }
}
