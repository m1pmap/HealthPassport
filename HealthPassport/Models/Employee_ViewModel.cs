using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.Models
{
    public class Employee_ViewModel : INotifyPropertyChanged
    {
        private int _EmployeeId = 0;
        public int EmployeeId
        {
            get => _EmployeeId;
            set
            {
                if (_EmployeeId != value)
                {
                    _EmployeeId = value;
                    OnPropertyChanged(nameof(EmployeeId));
                }
            }
        }
        private string _FIO { get; set; } = string.Empty;
        public string FIO
        {
            get => _FIO;
            set
            {
                if (_FIO != value)
                {
                    _FIO = value;
                    OnPropertyChanged(nameof(FIO));
                }
            }
        }

        private string _Job { get; set; } = string.Empty;
        public string Job
        {
            get => _Job;
            set
            {
                if (_Job != value)
                {
                    _Job = value;
                    OnPropertyChanged(nameof(Job));
                }
            }
        }

        private string _Education { get; set; } = string.Empty;
        public string Education
        {
            get => _Education;
            set
            {
                if (_Education != value)
                {
                    _Education = value;
                    OnPropertyChanged(nameof(Education));
                }
            }
        }

        private string _Birthday { get; set; } = string.Empty;
        public string Birthday
        {
            get => _Birthday;
            set
            {
                if (_Birthday != value)
                {
                    _Birthday = value;
                    OnPropertyChanged(nameof(Birthday));
                }
            }
        }

        private string _FamilyStatus { get; set; } = string.Empty;
        public string FamilyStatus
        {
            get => _FamilyStatus;
            set
            {
                if (_FamilyStatus != value)
                {
                    _FamilyStatus = value;
                    OnPropertyChanged(nameof(FamilyStatus));
                }
            }
        }

        private string _MailAdress { get; set; } = string.Empty;
        public string MailAdress
        {
            get => _MailAdress;
            set
            {
                if (_MailAdress != value)
                {
                    _MailAdress = value;
                    OnPropertyChanged(nameof(MailAdress));
                }
            }
        }
        private byte[] _Photo { get; set; } = null;

        public byte[] Photo
        {
            get => _Photo;
            set
            {
                if (_Photo != value)
                {
                    _Photo = value;
                    OnPropertyChanged(nameof(Photo));
                }
            }
        }

        private bool _isCurrentUser { get; set; } = false;
        public bool IsCurrentUser
        {
            get => _isCurrentUser;
            set
            {
                if (_isCurrentUser != value)
                {
                    _isCurrentUser = value;
                    OnPropertyChanged(nameof(IsCurrentUser));
                }
            }
        }

        public List<Disease_ViewModel> Diseases { get; set; } = new List<Disease_ViewModel>();
        public List<Vaccination_ViewModel> Vaccinations { get; set; } = new List<Vaccination_ViewModel>();
        public List<Job_ViewModel> Jobs { get; set; } = new List<Job_ViewModel>();
        public List<FamilyStatus_ViewModel> FamilyStatuses { get; set; } = new List<FamilyStatus_ViewModel>();
        public List<AntropologicalResearch_ViewModel> AntropologicalResearches { get; set; } = new List<AntropologicalResearch_ViewModel>();
        public List<Education_ViewModel> Educations { get; set; } = new List<Education_ViewModel>();


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static explicit operator Employee_ViewModel(Employee employee)
        {
            Employee_ViewModel NewEmployeeVM = new Employee_ViewModel
            {
                EmployeeId = employee.EmployeeId,
                FIO = employee.FIO,
                Birthday = employee.Birthday.ToString("dd.MM.yyyy"),
                MailAdress = employee.MailAdress,
                Photo = employee.Photo,
            };

            //Подгрузка заболеваний
            foreach (var disease in employee.Diseases)
            {
                NewEmployeeVM.Diseases.Add((Disease_ViewModel)disease);
            }

            NewEmployeeVM.Diseases.Reverse();

            //Подгрузка прививок
            foreach (var vaccination in employee.Vaccinations)
            {
                NewEmployeeVM.Vaccinations.Add((Vaccination_ViewModel)vaccination);
            }
            NewEmployeeVM.Vaccinations.Reverse();

            //Подгрузка семейных статусов
            if (employee.FamilyStatuses.Count > 0)
            {
                foreach (var familyStatus in employee.FamilyStatuses)
                {
                    NewEmployeeVM.FamilyStatuses.Add((FamilyStatus_ViewModel)familyStatus);
                }

                NewEmployeeVM.FamilyStatuses.Reverse();

                FamilyStatus lastFamilyStatus = employee.FamilyStatuses.FirstOrDefault();
                if(lastFamilyStatus.StartFamilyDate > lastFamilyStatus.EndFamilyDate)
                {
                    NewEmployeeVM.FamilyStatus = "Женат";
                }
                else
                {
                    NewEmployeeVM.FamilyStatus = "Разведён";
                }
            }
            else
            {
                NewEmployeeVM.FamilyStatus = "Холост";
            }

            //Подгрузка антропологических исследований
            foreach (var antropologicakResearch in employee.AntropologicalResearches)
            {
                NewEmployeeVM.AntropologicalResearches.Add((AntropologicalResearch_ViewModel)antropologicakResearch);
            }
            NewEmployeeVM.AntropologicalResearches.Reverse();


            //Подгрузка образований
            if (employee.Educations.Count > 0)
            {
                foreach (var education in employee.Educations)
                {
                    NewEmployeeVM.Educations.Add((Education_ViewModel)education);
                }
                NewEmployeeVM.Education.Reverse();
                NewEmployeeVM.Education = NewEmployeeVM.Educations[0].EducationType;
            }
            else
            {
                NewEmployeeVM.Education = "Без образования";
            }

            //Подгрузка должностей
            if (employee.Jobs.Count > 0)
            {
                foreach (var job in employee.Jobs)
                {
                    NewEmployeeVM.Jobs.Add((Job_ViewModel)job);
                }
                NewEmployeeVM.Job.Reverse();
                NewEmployeeVM.Job = NewEmployeeVM.Jobs[0].JobName;
            }

            return NewEmployeeVM;
        }
    }
}
