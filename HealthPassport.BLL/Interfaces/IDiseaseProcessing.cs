using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.BLL.Interfaces
{
    public interface IDiseaseProcessing
    {
        bool Add_Disease(Disease newDisease);
        bool Delete_Disease(int id);
        bool Update_Disease(Disease newDisease);
    }
}
