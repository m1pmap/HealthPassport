using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Interfaces
{
    public interface IDisease
    {
        bool Add_Disease(Disease disease);
        bool Delete_Disease(int id);
        bool Update_Disease(Disease disease);
    }
}
