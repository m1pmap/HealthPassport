using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.BLL.Interfaces
{
    public interface IVaccinationProcessing
    {
        bool Add_Vaccination(Vaccination vaccination);
        bool Delete_Vaccination(int id);
        bool Update_Vaccination(Vaccination vaccination);
    }
}
