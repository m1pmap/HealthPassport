using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Repositories
{
    public class AntropologicalResearch_Repository : IAntropologicalResearch
    {
        private readonly ApplicationDbContext _db;
        public AntropologicalResearch_Repository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool Add_AntropologicalResearch(AntropologicalResearch antropologicalResearch)
        {
            try
            {
                _db.AntropologicalResearches.Add(antropologicalResearch);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete_AntropologicalResearch(int id)
        {
            try
            {
                AntropologicalResearch? dbAntropologicalResearch = _db.AntropologicalResearches.FirstOrDefault(e => e.AntropologicalResearchId == id);
                if (dbAntropologicalResearch != null)
                {
                    _db.AntropologicalResearches.Remove(dbAntropologicalResearch);
                    _db.SaveChanges();

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Update_AntropologicalResearch(AntropologicalResearch antropologicalResearch)
        {
            try
            {
                AntropologicalResearch updateAntropologicalResearch= _db.AntropologicalResearches.FirstOrDefault(ar => ar.AntropologicalResearchId== antropologicalResearch.AntropologicalResearchId);

                if (updateAntropologicalResearch != null)
                {
                    updateAntropologicalResearch.Weight = antropologicalResearch.Weight;
                    updateAntropologicalResearch.Height = antropologicalResearch.Height;
                    updateAntropologicalResearch.Date = antropologicalResearch.Date;
                    _db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
