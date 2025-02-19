using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Interfaces
{
    public interface IVaccination
    {
        bool Add_Vaccination(Vaccination newVaccination);
        bool Delete_Vaccination(int id);
        bool Update_Vaccination(Vaccination vaccination);
    }
}
