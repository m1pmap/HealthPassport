using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Interfaces
{
    public interface IGetRepository<TEntity> where TEntity : class
    {
        public List<TEntity> Get_AllItems();
        public string Get_MainParamById(int id);
        public int Get_IdByMainParam(string mainParam);
    }
}
