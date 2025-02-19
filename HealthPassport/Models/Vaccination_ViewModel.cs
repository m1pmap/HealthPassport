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
    public class Vaccination_ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _VaccinationId { get; set; }
        public int VaccinationId
        {
            get => _VaccinationId;
            set
            {
                if (_VaccinationId != value)
                {
                    _VaccinationId = value;
                    OnPropertyChanged(nameof(VaccinationId));
                }
            }
        }
        private string _Name { get; set; } = string.Empty;
        public string Name
        {
            get => _Name;
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged(nameof(Name));
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

        public static explicit operator Vaccination_ViewModel(Vaccination vaccination)
        {
            return new Vaccination_ViewModel
            {
                VaccinationId = vaccination.VaccinationId,
                Name = vaccination.Name,
                Date = vaccination.Date.ToString("dd.MM.yyyy")
            };
        }
    }
}
