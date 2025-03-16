using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Repositories
{
    public class Subunit_Repository : ISubunit
    {
        private readonly ApplicationDbContext _db;
        public Subunit_Repository(ApplicationDbContext db) 
        {
            _db = db;
        }
        public List<Subunit> GetAllSubunits()
        {
            return _db.Subunits.ToList();
        }

        public int Get_SubunitIdByName(string subunitName)
        {
            return _db.Subunits.FirstOrDefault(jt => jt.SubunitName == subunitName).SubunitId;
        }

        public string Get_SubunitNameById(int id)
        {
            return _db.Subunits.FirstOrDefault(jt => jt.SubunitId == id).SubunitName;

        }
    }
}
