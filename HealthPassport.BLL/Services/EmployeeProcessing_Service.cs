using HealthPassport.BLL.Interfaces;
using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.BLL.Services
{
    public class EmployeeProcessing_Service : IEmployeeProcessing
    {
        private readonly IEmployee _employeeRepository;
        public EmployeeProcessing_Service(IEmployee employeeRepository) 
        {
            _employeeRepository = employeeRepository;
        }
        public bool Add_Employee(Employee employee)
        {
            return _employeeRepository.Add_Employee(employee);
        }

        public Employee CheckEmployeeInDb(string mail)
        {
            List<Employee> allEmployees = _employeeRepository.Get_AllEmployees();
            return allEmployees.FirstOrDefault(e => e.MailAdress == mail);
        }

        public bool Delete_Employee(int id)
        {
            return _employeeRepository.Delete_Employee(id);
        }

        public List<Employee> Get_AllEmployees()
        {
            return _employeeRepository.Get_AllEmployees();
        }

        public bool Update_employee(Employee employee)
        {
            return _employeeRepository.Update_Employee(employee);
        }

        public Employee Connect_EmployeeInformation(Employee employee)
        {
            return _employeeRepository.Connect_EmployeeInformation(employee);
        }

    }
}
