using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Interfaces
{
    public interface IAntropologicalResearch
    {
        bool Add_AntropologicalResearch(AntropologicalResearch antropologicalResearch);
        bool Delete_AntropologicalResearch(int id);
        bool Update_AntropologicalResearch(AntropologicalResearch antropologicalResearch);
    }
}
