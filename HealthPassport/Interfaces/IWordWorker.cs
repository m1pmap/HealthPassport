﻿using HealthPassport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Interfaces
{
    public interface IWordWorker
    {
        void exportEmployeeInfo(Employee_ViewModel employee);
        void exportEmployeeDiseases(Employee_ViewModel employee);
        void exportEmployeeVaccinations(Employee_ViewModel employee);
        void exportEmployeeAntropologicalResearches(Employee_ViewModel employee);
        void exportEmployeeEducations(Employee_ViewModel employee);
        void exportEmployeeJobs(Employee_ViewModel employee);
        void exportEmployeeFamilyStatuses(Employee_ViewModel employee);
    }
}
