using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Interfaces
{
    public interface IJob
    {
        bool Add_Job(Job newJob);
        bool Delete_Job(int id);
        bool Update_Job(Job newJob);
    }
}
