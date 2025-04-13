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

        public Employee CheckEmployeeInDb(string mail)
        {
            List<Employee> allEmployees = _employeeRepository.Get_AllEmployees();
            return allEmployees.FirstOrDefault(e => e.MailAdress == mail);
        }

        public List<Employee> Get_AllEmployees()
        {
            return _employeeRepository.Get_AllEmployees();
        }

        public Employee Connect_EmployeeInformation(Employee employee)
        {
            return _employeeRepository.Connect_EmployeeInformation(employee);
        }

        public bool Add_Item(Employee entity)
        {
            return _employeeRepository.Add_Item(entity);
        }

        public bool Update_Item(Employee entity)
        {
            return _employeeRepository.Update_Item(entity);
        }

        public bool Delete_Item(int id)
        {
            return _employeeRepository.Delete_Item(id);
        }

        public JobType Get_EmployeeJobType(int id)
        {
            return _employeeRepository.Get_EmployeeJobType(id);
        }

        public void SetCurrentEmployeeId(int id)
        {
            _employeeRepository.SetCurrentEmployeeId(id);
        }
    }
}
