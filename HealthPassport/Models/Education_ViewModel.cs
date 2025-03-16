using HealthPassport.BLL.Interfaces;
using HealthPassport.DAL.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.Models
{
    public class Education_ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _EducationId { get; set; }
        public int EducationId
        {
            get => _EducationId;
            set
            {
                if (_EducationId != value)
                {
                    _EducationId = value;
                    OnPropertyChanged(nameof(EducationId));
                }
            }
        }
        private string _EducationType { get; set; }
        public string EducationType
        {
            get => _EducationType;
            set
            {
                if (_EducationType != value)
                {
                    _EducationType = value;
                    OnPropertyChanged(nameof(EducationType));
                }
            }
        }
        private string _EducationInstitution { get; set; }
        public string EducationInstitution
        {
            get => _EducationInstitution;
            set
            {
                if (_EducationInstitution != value)
                {
                    _EducationInstitution = value;
                    OnPropertyChanged(nameof(EducationInstitution));
                }
            }
        }
        private string _Date { get; set; }
        public string Date
        {
            get => _Date;
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

        public static explicit operator Education_ViewModel(Education education)
        {
            var educationProcessingService = ServiceLocator.Provider.GetService<IEducationProcessing>();

            return new Education_ViewModel
            {
                EducationId = education.EducationId,
                EducationType = educationProcessingService.Get_EducationLevelNameById(education.EducationLevelId),
                EducationInstitution = education.EducationInstitution,
                Date = education.Date.ToString("dd.MM.yyyy"),
            };
        }
    }
}
