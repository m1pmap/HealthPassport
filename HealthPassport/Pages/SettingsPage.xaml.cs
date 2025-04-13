using HealthPassport.BLL.Interfaces;
using HealthPassport.BLL.Services;
using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using HealthPassport.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HealthPassport.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Window
    {
        private readonly ISubunitProcessing _subunitProcessingService;
        private readonly IJobTypeProcessing _jobTypeProcessingService;
        private readonly IAuditLogProcessing _auditLogProcessingService;

        public ObservableCollection<Subunit> subunits = new();
        public ObservableCollection<JobType> jobTypes = new();
        public ObservableCollection<AuditLog> auditLogs = new();
        public SettingsPage(ISubunitProcessing subunitProcessingService, IJobTypeProcessing jobTypeProcessingService, IAuditLogProcessing auditLogProcessingService)
        {
            InitializeComponent();

            _subunitProcessingService = subunitProcessingService;
            _jobTypeProcessingService = jobTypeProcessingService;
            _auditLogProcessingService = auditLogProcessingService;

            subunit_dataGrid.ItemsSource = subunits;
            jobType_dataGrid.ItemsSource = jobTypes;
            auditLogs_dataGrid.ItemsSource = auditLogs;
        }

        private void addSubunit_button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(subunitName_textBox.Text))
            {
                Subunit newSubunit = new Subunit
                {
                    SubunitName = subunitName_textBox.Text
                };

                if (_subunitProcessingService.Add_Item(newSubunit))
                {
                    subunits.Add(newSubunit);
                    subunit_dataGrid.ScrollIntoView(newSubunit);
                    subunit_dataGrid.SelectedItem = newSubunit;
                    subunitName_textBox.Text = string.Empty;

                    UpdateMainWindowSubunits();
                }
                else
                    MessageBox.Show("Подразделение не добавлено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Введите наименование подразделения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void removeSubunit_button_Click(object sender, RoutedEventArgs e)
        {
            Subunit selectedSubunit = (Subunit)subunit_dataGrid.SelectedItem;
            if(selectedSubunit != null)
            {
                if (_subunitProcessingService.Delete_Item(selectedSubunit.SubunitId))
                {
                    subunits.Remove(selectedSubunit);
                    UpdateMainWindowSubunits();
                }
                else
                    MessageBox.Show("Подразделение не удалено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var subunitsList = _subunitProcessingService.Get_AllItems();
            foreach (var subunit in subunitsList)
                subunits.Add(subunit);

            var jobTypesList = _jobTypeProcessingService.Get_AllItems();
            foreach (var jobType in jobTypesList)
                jobTypes.Add(jobType);

            var auditLogsList = _auditLogProcessingService.Get_AllItems();
            for (int i = auditLogsList.Count - 1; i >= 0; i--)
                auditLogs.Add(auditLogsList[i]);
        }

        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void subunit_dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var selectedItem = (Subunit)subunit_dataGrid.SelectedItem;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!_subunitProcessingService.Update_Item(selectedItem))
                    MessageBox.Show("Подразделение не обновлено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    UpdateMainWindowSubunits();
            }), DispatcherPriority.Background);
        }

        private void UpdateMainWindowSubunits()
        {
            MainWindow window = (MainWindow)this.Owner;
            window.NewSubunit_comboBox.ItemsSource = _subunitProcessingService.Get_AllItems();
        }

        private void UpdateMainWindowJobTypes()
        {
            MainWindow window = (MainWindow)this.Owner;
            window.NewJob_comboBox.ItemsSource = _jobTypeProcessingService.Get_AllItems();
        }

        private void addJobType_button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(jobTypeName_textBox.Text))
            {
                JobType newJobType = new JobType
                {
                    JobName = jobTypeName_textBox.Text,
                    isCanAddRows = false,
                    isCanEditItems = false,
                    isCanSendMessages = false
                };

                if (_jobTypeProcessingService.Add_Item(newJobType))
                {
                    jobTypes.Add(newJobType);
                    jobType_dataGrid.ScrollIntoView(newJobType);
                    jobType_dataGrid.SelectedItem = newJobType;
                    jobTypeName_textBox.Text = string.Empty;

                    UpdateMainWindowJobTypes();
                }
                else
                    MessageBox.Show("Должность не добавлено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Введите наименование должности.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void removeJobType_button_Click(object sender, RoutedEventArgs e)
        {
            JobType selectedJobType = (JobType)jobType_dataGrid.SelectedItem;
            if (selectedJobType != null)
            {
                if (_jobTypeProcessingService.Delete_Item(selectedJobType.JobTypeId))
                {
                    jobTypes.Remove(selectedJobType);
                    UpdateMainWindowJobTypes();
                }
                else
                    MessageBox.Show("Должность не удалено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void jobType_dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var selectedItem = (JobType)jobType_dataGrid.SelectedItem;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!_jobTypeProcessingService.Update_Item(selectedItem))
                    MessageBox.Show("Должность не обновлена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    UpdateMainWindowJobTypes();
            }), DispatcherPriority.Background);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            auditLogMoreInfoHeight.Height = new GridLength(1, GridUnitType.Star);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            auditLogMoreInfoHeight.Height = new GridLength(0, GridUnitType.Star);
        }

        private void auditLogs_dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            AuditLog selectedAuditLog = (AuditLog)auditLogs_dataGrid.SelectedItem;
            if(selectedAuditLog != null)
            {
                changedColumns_textBox.Text = selectedAuditLog.ChangedColumns;
                changedElementId_textBox.Text = selectedAuditLog.PrimaryKey;
                oldValue_textBox.Text = selectedAuditLog.OldValues;
                newValue_textBox.Text = selectedAuditLog.NewValues;
            }
        }
    }
}
