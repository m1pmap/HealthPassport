using HealthPassport.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthPassport.BLL.Services
{
    public class JsonSerializer_Service : IJsonSerializer
    {
        public T Deserialize<T>(string filepath)
        {
            string jsonText = File.ReadAllText(filepath);
            T obj = JsonSerializer.Deserialize<T>(jsonText);
            return obj;
        }

        public bool Serialize<T>(T obj, string filepath)
        {
            try
            {
                string jsonText = JsonSerializer.Serialize(obj);
                File.WriteAllText(filepath, jsonText);
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}
