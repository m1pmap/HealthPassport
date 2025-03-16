using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Interfaces
{
    public interface IEducationLevel
    {
        List<EducationLevel> GetAllEducationLevels();
        string Get_EducationLevelNameById(int id);
        int Get_EducationLevelIdByName(string educationLevelName);
    }
}
