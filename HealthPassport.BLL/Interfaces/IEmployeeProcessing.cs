using HealthPassport.DAL.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.BLL.Interfaces
{
    public interface IEmployeeProcessing
    {
        bool Add_Employee(Employee employee);
        List<Employee> Get_AllEmployees();
        bool Delete_Employee(int id);
        bool Update_employee(Employee employee);
        Employee CheckEmployeeInDb(string mail);
    }
}
