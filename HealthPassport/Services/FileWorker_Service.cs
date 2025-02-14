using HealthPassport.BLL.Interfaces;
using HealthPassport.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.Services
{
    public class FileWorker_Service : IFileWorker
    {
        private readonly IJsonSerializer _jsonSerializer;
        public FileWorker_Service(IJsonSerializer jsonSerializer) 
        {
            _jsonSerializer = jsonSerializer;
        }

        public T ReadJson<T>(string jsonPath)
        {
            return _jsonSerializer.Deserialize<T>(jsonPath);
        }

        public T ReadJsonFileDialog<T>()
        {
            throw new NotImplementedException();
        }

        public bool WriteJson<T>(T obj, string jsonPath)
        {
            return _jsonSerializer.Serialize(obj, jsonPath);
        }

        public bool WriteJsonFileDialog<T>(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
