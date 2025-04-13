using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.BLL.Interfaces
{
    public interface ICudProcessing<TEntity> where TEntity : class
    {
        bool Add_Item(TEntity entity);
        bool Update_Item(TEntity entity);
        bool Delete_Item(int id);
    }
}
