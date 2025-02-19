using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.BLL.Interfaces
{
    public interface IFamilyStatusProcessing
    {
        bool Add_FamilyStatus(FamilyStatus familyStatus);
        bool Delete_FamilyStatus(int id);
        bool Update_FamilyStatus(FamilyStatus familyStatus);
    }
}
