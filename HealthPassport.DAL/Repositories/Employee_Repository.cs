using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using Microsoft.EntityFrameworkCore;
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

        public List<Employee> Get_AllEmployees()
        {
            return _db.Employees.ToList();
        }

        public Employee Connect_EmployeeInformation(Employee employee)
        {
            try
            {
                Employee connectedEmployee = new Employee();

                connectedEmployee = _db.Employees
                            .Include(e => e.Diseases)
                            .Include(e => e.Vaccinations)
                            .Include(e => e.FamilyStatuses)
                            .Include(e => e.Educations)
                            .Include(e => e.AntropologicalResearches)
                            .Include(e => e.Jobs)
                                .ThenInclude(j => j.JobType)
                            .Include(e => e.Jobs)
                                .ThenInclude(j => j.Subunit)
                            .FirstOrDefault(u => u.EmployeeId == employee.EmployeeId);

                return connectedEmployee;
            }
            catch
            {
                return employee;
            }
        }

        public bool Add_Item(Employee entity)
        {
            try
            {
                _db.Employees.Add(entity);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update_Item(Employee entity)
        {
            try
            {
                var updateEmployee = _db.Employees.FirstOrDefault(e => e.EmployeeId == entity.EmployeeId);

                if (updateEmployee != null)
                {
                    if(updateEmployee.FIO != entity.FIO)
                        updateEmployee.FIO = entity.FIO;

                    if(updateEmployee.Birthday != entity.Birthday)
                        updateEmployee.Birthday = entity.Birthday;

                    if(updateEmployee.Photo != entity.Photo)
                        updateEmployee.Photo = entity.Photo;

                    if (updateEmployee.MailAdress != entity.MailAdress)
                        updateEmployee.MailAdress = entity.MailAdress;
                    _db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public bool Delete_Item(int id)
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

        public JobType Get_EmployeeJobType(int id)
        {
            try
            {
                Employee? dbEmployee = _db.Employees.FirstOrDefault(e => e.EmployeeId == id);
                if (dbEmployee != null)
                {
                    dbEmployee = Connect_EmployeeInformation(dbEmployee);
                    return dbEmployee.Jobs.ToList()[dbEmployee.Jobs.Count - 1].JobType;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public void SetCurrentEmployeeId(int id)
        {
            _db.CurrentEmployeeid = id;
        }
    }
}
