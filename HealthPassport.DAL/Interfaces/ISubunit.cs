using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Interfaces
{
    public interface ISubunit
    {
        public List<Subunit> GetAllSubunits();
        public int Get_SubunitIdByName(string subunitName);
        public string Get_SubunitNameById(int id);
    }
}
