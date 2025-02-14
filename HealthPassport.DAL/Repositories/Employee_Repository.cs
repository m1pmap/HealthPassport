using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Repositories
{
    public class Employee_Repository : IEmployee
    {
        private readonly ApplicationDbContext _db;

        public Employee_Repository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public bool Add_Employee(Employee employee)
        {
            try
            {
                _db.Employees.Add(employee);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete_Employee(int id)
        {
            try
            {
                Employee? dbEmployee = _db.Employees.FirstOrDefault(e => e.EmployeeId == id);
                if (dbEmployee != null)
                {
                    _db.Employees.Remove(dbEmployee);
                    _db.SaveChanges();

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public List<Employee> Get_AllEmployees()
        {
            return _db.Employees.ToList();
        }

        public bool Update_Employee(Employee newEmployee)
        {
            try
            {
                var updateEmployee = _db.Employees.FirstOrDefault(e => e.EmployeeId == newEmployee.EmployeeId);

                if (updateEmployee != null)
                {
                    updateEmployee.FIO = newEmployee.FIO;
                    updateEmployee.Job = newEmployee.Job;
                    updateEmployee.Education = newEmployee.Education;
                    updateEmployee.Birthday = newEmployee.Birthday;
                    updateEmployee.FamilyStatus = newEmployee.FamilyStatus;
                    updateEmployee.Photo = newEmployee.Photo;
                    _db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {

                return false; 
            }
        }
    }
}
