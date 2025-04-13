using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.BLL.Interfaces
{
    public interface IGetProcessing<TEntity> where TEntity : class
    {
        public List<TEntity> Get_AllItems();
        public string Get_MainParamById(int id);
        public int Get_IdByMainParam(string mainParam);
    }
}
