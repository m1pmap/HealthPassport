using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.BLL.Interfaces
{
    public interface IDiseaseProcessing : ICudProcessing<Disease>
    {
        public List<Disease> Get_AllItems();
    }
}
