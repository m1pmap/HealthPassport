using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.BLL.Interfaces
{
    public interface IJsonSerializer
    {
        public bool Serialize<T>(T obj, string filepath);
        public T Deserialize<T>(string filepath);
    }
}
