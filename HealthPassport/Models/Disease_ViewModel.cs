using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.Models
{
    public class Disease_ViewModel : INotifyPropertyChanged
    {
        private int _DiseaseId = 0;
        public int DiseaseId
        {
            get => _DiseaseId;
            set
            {
                if (_DiseaseId != value)
                {
                    _DiseaseId = value;
                    OnPropertyChanged(nameof(DiseaseId));
                }
            }
        }

        private string _Name = string.Empty;
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
        private string _StardDiseaseDate { get; set; }
        public string StardDiseaseDate
        {
            get => _StardDiseaseDate;
            set
            {
                if (_StardDiseaseDate != value)
                {
                    _StardDiseaseDate = value;
                    OnPropertyChanged(nameof(StardDiseaseDate));
                }
            }
        }
        private string _EndDiseaseDate { get; set; }
        public string EndDiseaseDate
        {
            get => _EndDiseaseDate;
            set
            {
                if (_EndDiseaseDate != value)
                {
                    _EndDiseaseDate = value;
                    OnPropertyChanged(nameof(EndDiseaseDate));
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public static explicit operator Disease_ViewModel(Disease disease)
        {
            return new Disease_ViewModel
            {
                DiseaseId = disease.DiseaseId,
                Name = disease.Name,
                StardDiseaseDate = disease.StardDiseaseDate.ToString("dd.MM.yyyy"),
                EndDiseaseDate = disease.EndDiseaseDate.ToString("dd.MM.yyyy"),
            };
        }
    }
}
