using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Repositories
{
    public class AntropologicalResearch_Repository : ICudRepository<AntropologicalResearch>
    {
        private readonly ApplicationDbContext _db;
        public AntropologicalResearch_Repository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Add_Item(AntropologicalResearch entity)
        {
            try
            {
                _db.AntropologicalResearches.Add(entity);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete_Item(int id)
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

        public bool Update_Item(AntropologicalResearch entity)
        {
            try
            {
                AntropologicalResearch updateAntropologicalResearch = _db.AntropologicalResearches.FirstOrDefault(ar => ar.AntropologicalResearchId == entity.AntropologicalResearchId);

                if (updateAntropologicalResearch != null)
                {

                    if (updateAntropologicalResearch.Weight != entity.Weight)
                        updateAntropologicalResearch.Weight = entity.Weight;

                    if(updateAntropologicalResearch.Height != entity.Height)
                        updateAntropologicalResearch.Height = entity.Height;

                    if(updateAntropologicalResearch.Date != entity.Date)
                        updateAntropologicalResearch.Date = entity.Date;
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
