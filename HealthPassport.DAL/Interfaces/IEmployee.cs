using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Interfaces
{
    public interface IEmployee
    {
        bool Add_Employee(Employee employee);
        List<Employee> Get_AllEmployees();
        bool Delete_Employee(int id);
        bool Update_Employee(Employee employee);
        Employee Connect_EmployeeInformation(Employee employee);
    }
}
