using HealthPassport.DAL.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthPassport.Models
{
    public class FamilyStatus_ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _FamilyStatusId { get; set; }
        public int FamilyStatusId
        {
            get => _FamilyStatusId;
            set
            {
                if (_FamilyStatusId != value)
                {
                    _FamilyStatusId = value;
                    OnPropertyChanged(nameof(FamilyStatusId));
                }
            }
        }
        private string _Status { get; set; }
        public string Status
        {
            get => _Status;
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }
        private string _StartFamilyDate { get; set; }
        public string StartFamilyDate
        {
            get => _StartFamilyDate;
            set
            {
                if (_StartFamilyDate != value)
                {
                    _StartFamilyDate = value;
                    OnPropertyChanged(nameof(StartFamilyDate));
                }
            }
        }
        private string _EndFamilyDate { get; set; }
        public string EndFamilyDate
        {
            get => _EndFamilyDate;
            set
            {
                if (_EndFamilyDate != value)
                {
                    _EndFamilyDate = value;
                    OnPropertyChanged(nameof(EndFamilyDate));
                }
            }
        }

        public static explicit operator FamilyStatus_ViewModel(FamilyStatus familyStatus)
        {
            FamilyStatus_ViewModel newFamilyStatusVM = new FamilyStatus_ViewModel
            {
                FamilyStatusId = familyStatus.FamilyStatusId,
                Status = familyStatus.Status,
                StartFamilyDate = familyStatus.StartFamilyDate.ToString("dd.MM.yyyy"),
            };

            if(familyStatus.EndFamilyDate == DateTime.MinValue)
            {
                newFamilyStatusVM.EndFamilyDate = "-";
            }
            else
            {
                newFamilyStatusVM.EndFamilyDate = familyStatus.EndFamilyDate.ToString("dd.MM.yyyy");
            }

            return newFamilyStatusVM;
        }
    }
}
