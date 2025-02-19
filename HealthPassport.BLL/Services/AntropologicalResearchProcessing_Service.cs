using HealthPassport.BLL.Interfaces;
using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.BLL.Services
{
    public class AntropologicalResearchProcessing_Service : IAntropologicalResearchProcessing
    {
        private readonly IAntropologicalResearch _antropologicalResearchService;
        public AntropologicalResearchProcessing_Service(IAntropologicalResearch antropologicalResearchService)
        {
            _antropologicalResearchService = antropologicalResearchService;
        }
        public bool Add_AntropologicalResearch(AntropologicalResearch antropologicalResearch)
        {
            return _antropologicalResearchService.Add_AntropologicalResearch(antropologicalResearch);
        }

        public bool Delete_AntropologicalResearch(int id)
        {
            return _antropologicalResearchService.Delete_AntropologicalResearch(id);
        }

        public bool Update_AntropologicalResearch(AntropologicalResearch antropologicalResearch)
        {
            return _antropologicalResearchService.Update_AntropologicalResearch(antropologicalResearch);
        }
    }
}
