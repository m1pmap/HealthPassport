using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.Interfaces
{
    public interface IFileWorker
    {
        public bool WriteJsonFileDialog<T>(T obj);
        public T ReadJsonFileDialog<T>();
        public bool WriteJson<T>(T obj, string jsonPath);
        public T ReadJson<T>(string jsonPath);
    }
}
