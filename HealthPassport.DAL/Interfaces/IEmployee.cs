using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Interfaces
{
    public interface IEmployee : ICudRepository<Employee>
    {
        List<Employee> Get_AllEmployees();
        Employee Connect_EmployeeInformation(Employee employee);
        JobType Get_EmployeeJobType(int id);
        void SetCurrentEmployeeId(int id);
    }
}
