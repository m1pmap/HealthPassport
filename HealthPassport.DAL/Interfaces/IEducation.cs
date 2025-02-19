using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Interfaces
{
    public interface IEducation
    {
        bool Add_Education(Education education);
        bool Delete_Education(int id);
        bool Update_Education(Education education);
    }
}
