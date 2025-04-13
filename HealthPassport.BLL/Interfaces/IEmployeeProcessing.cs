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
    public interface IEmployeeProcessing : ICudProcessing<Employee>
    {
        List<Employee> Get_AllEmployees();
        Employee CheckEmployeeInDb(string mail);
        Employee Connect_EmployeeInformation(Employee employee);
        JobType Get_EmployeeJobType(int id);
        void SetCurrentEmployeeId(int id);
    }
}
