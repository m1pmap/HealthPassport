using HealthPassport.BLL.Interfaces;
using HealthPassport.BLL.Services;
using HealthPassport.DAL.Interfaces;
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
    public class Job_ViewModel : INotifyPropertyChanged
    {
        public static IServiceProvider Provider { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _JobId { get; set; }
        public int JobId
        {
            get => _JobId;
            set
            {
                if (_JobId != value)
                {
                    _JobId = value;
                    OnPropertyChanged(nameof(JobId));
                }
            }
        }
        private string _JobName { get; set; }
        public string JobName
        {
            get => _JobName;
            set
            {
                if (_JobName != value)
                {
                    _JobName = value;
                    OnPropertyChanged(nameof(JobName));
                }
            }
        }

        private string _Subunit { get; set; }
        public string Subunit
        {
            get => _Subunit;
            set
            {
                if (_Subunit != value)
                {
                    _Subunit = value;
                    OnPropertyChanged(nameof(Subunit));
                }
            }
        }

        private double _WorkingRate { get; set; }
        public double WorkingRate
        {
            get => _WorkingRate;
            set
            {
                if (_WorkingRate != value)
                {
                    _WorkingRate = value;
                    OnPropertyChanged(nameof(WorkingRate));
                }
            }
        }

        private string _StartWorkingDate { get; set; }
        public string StartWorkingDate
        {
            get => _StartWorkingDate;
            set
            {
                if (_StartWorkingDate != value)
                {
                    _StartWorkingDate = value;
                    OnPropertyChanged(nameof(StartWorkingDate));
                }
            }
        }
        private string _EndWorkingDate { get; set; }
        public string EndWorkingDate
        {
            get => _EndWorkingDate;
            set
            {
                if (_EndWorkingDate != value)
                {
                    _EndWorkingDate = value;
                    OnPropertyChanged(nameof(EndWorkingDate));
                }
            }
        }

        public static explicit operator Job_ViewModel(Job job)
        {
            var jobProcessingService = ServiceLocator.Provider.GetService<IJobProcessing>();

            Job_ViewModel newJobVM = new Job_ViewModel
            {
                JobId = job.JobId,
                JobName = jobProcessingService.Get_JobNameById(job.JobTypeId),
                Subunit = job.Subunit,
                WorkingRate = job.WorkingRate,
                StartWorkingDate = job.StartWorkingDate.ToString("dd.MM.yyyy"),
            };

            if (job.EndWorkingDate == DateTime.MinValue)
            {
                newJobVM.EndWorkingDate = "-";
            }
            else
            {
                newJobVM.EndWorkingDate = job.EndWorkingDate.ToString("dd.MM.yyyy");
            }

            return newJobVM;
        }
    }
}
