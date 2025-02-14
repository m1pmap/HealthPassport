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
        public string _FIO { get; set; } = string.Empty;
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

        public string _Job { get; set; } = string.Empty;
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

        public string _Education { get; set; } = string.Empty;
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

        public string _Birthday { get; set; } = string.Empty;
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

        public string _FamilyStatus { get; set; } = string.Empty;
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

        public string _MailAdress { get; set; } = string.Empty;
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
        public byte[] _Photo { get; set; } = null;

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static explicit operator Employee_ViewModel(Employee employee)
        {
            return new Employee_ViewModel
            {
                EmployeeId = employee.EmployeeId,
                FIO = employee.FIO,
                Job = employee.Job,
                Education = employee.Education,
                Birthday = employee.Birthday.ToString("dd.MM.yyyy"),
                FamilyStatus = employee.FamilyStatus,
                MailAdress = employee.MailAdress,
                Photo = employee.Photo,
            };
        }
    }
}
