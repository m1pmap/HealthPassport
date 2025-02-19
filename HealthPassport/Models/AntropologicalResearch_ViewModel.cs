using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.Models
{
    public class AntropologicalResearch_ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _AntropologicalResearchId { get; set; }
        public int AntropologicalResearchId
        {
            get => _AntropologicalResearchId;
            set
            {
                if (_AntropologicalResearchId != value)
                {
                    _AntropologicalResearchId = value;
                    OnPropertyChanged(nameof(AntropologicalResearchId));
                }
            }
        }

        private double _Weight { get; set; }
        public double Weight
        {
            get => _Weight;
            set
            {
                if (_Weight != value)
                {
                    _Weight = value;
                    OnPropertyChanged(nameof(Weight));
                }
            }
        }

        private double _Height { get; set; }
        public double Height
        {
            get => _Height;
            set
            {
                if (_Height != value)
                {
                    _Height = value;
                    OnPropertyChanged(nameof(Height));
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

        public static explicit operator AntropologicalResearch_ViewModel(AntropologicalResearch antropologicalResearch)
        {
            return new AntropologicalResearch_ViewModel
            {
                AntropologicalResearchId = antropologicalResearch.AntropologicalResearchId,
                Weight = antropologicalResearch.Weight,
                Height = antropologicalResearch.Height,
                Date = antropologicalResearch.Date.ToString("dd.MM.yyyy"),
            };
        }
    }
}
