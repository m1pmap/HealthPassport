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
    public class AntropologicalResearchProcessing_Service : ICudProcessing<AntropologicalResearch>
    {
        private readonly ICudRepository<AntropologicalResearch> _antropologicalResearchService;
        public AntropologicalResearchProcessing_Service(ICudRepository<AntropologicalResearch> antropologicalResearchService)
        {
            _antropologicalResearchService = antropologicalResearchService;
        }

        public bool Add_Item(AntropologicalResearch entity)
        {
            return _antropologicalResearchService.Add_Item(entity);
        }

        public bool Delete_Item(int id)
        {
            return _antropologicalResearchService.Delete_Item(id);
        }

        public bool Update_Item(AntropologicalResearch entity)
        {
            return _antropologicalResearchService.Update_Item(entity);
        }
    }
}
