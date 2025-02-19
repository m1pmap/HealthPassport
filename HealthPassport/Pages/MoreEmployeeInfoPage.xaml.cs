using HealthPassport.BLL.Interfaces;
using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using HealthPassport.Models;
using HealthPassport.Services;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace HealthPassport.Pages
{
    /// <summary>
    /// Логика взаимодействия для MoreEmployeeInfoPage.xaml
    /// </summary>
    public partial class MoreEmployeeInfoPage : Window
    {
        public Employee_ViewModel selectedEmployee;
        public List<JobType> jobTypes = new List<JobType>();
        bool isWindowLoaded = false;

        private readonly ByteArrayToImageSourceConverter_Service _imageSourceConverter;
        private readonly IDiseaseProcessing _diseaseProcessingService;
        private readonly IVaccinationProcessing _vaccinationProcessingService;
        private readonly IFamilyStatusProcessing _familyProcessingService;
        private readonly IAntropologicalResearchProcessing _antropologicalResearchProcessingService;
        private readonly IEducationProcessing _educationProcessingService;
        private readonly IJobProcessing _jobProcessingService;
        private readonly IEmployeeProcessing _employeeProcessingService;

        string _cellOldValue = string.Empty; //Старое значение ячейки до изменения

        public MoreEmployeeInfoPage(ByteArrayToImageSourceConverter_Service imageSourceConverter,
            IDiseaseProcessing diseaseProcessingService,
            IVaccinationProcessing vaccinationProcessingService,
            IFamilyStatusProcessing familyProcessingService,
            IAntropologicalResearchProcessing antropologicalResearchProcessingService,
            IEducationProcessing educationProcessingService,
            IJobProcessing jobProcessingService,
            IEmployeeProcessing employeeProcessingService)
        {
            InitializeComponent();

            _imageSourceConverter = imageSourceConverter;
            _diseaseProcessingService = diseaseProcessingService;
            _vaccinationProcessingService = vaccinationProcessingService;
            _familyProcessingService = familyProcessingService;
            _antropologicalResearchProcessingService = antropologicalResearchProcessingService;
            _educationProcessingService = educationProcessingService;
            _jobProcessingService = jobProcessingService;
            _employeeProcessingService = employeeProcessingService;
        }

        private void closeWindow_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            medicalCardInfo_label.Content = $"Медецинская карта для: {selectedEmployee.FIO}";

            var fileImageBytes = _imageSourceConverter.ConvertFromFileImageToByteArray("without_image_database.png");
            if (BitConverter.ToString(fileImageBytes) == BitConverter.ToString(selectedEmployee.Photo)) //Если нет фотографии
            {
                employeeImage.Source = new BitmapImage(new Uri("/resources/images/without_picture.png", UriKind.Relative));
            }
            else
            {
                employeeImage.Source = (BitmapImage)_imageSourceConverter.Convert(selectedEmployee.Photo, typeof(BitmapImage), null, CultureInfo.CurrentCulture);
            }

            //Заполнение основных данных сотрудника
            FIO_textBox.Text = selectedEmployee.FIO;
            CurrentJob_textBox.Text = selectedEmployee.Job;
            if(selectedEmployee.Diseases.Count > 0) 
            {
                LastDisease_textBox.Text = selectedEmployee.Diseases[selectedEmployee.Diseases.Count - 1].Name;
            }
            else
            {
                LastDisease_textBox.Text = "Отсутствует";
            }
            Birthday_datePicker.Text = selectedEmployee.Birthday;
            Mail_textBox.Text = selectedEmployee.MailAdress;

            //Заполнение таблиц данных из бд
            diseases_dataGrid.ItemsSource = selectedEmployee.Diseases;
            vaccinations_dataGrid.ItemsSource = selectedEmployee.Vaccinations;
            familyStatuses_dataGrid.ItemsSource = selectedEmployee.FamilyStatuses;
            antropologicalResearches_dataGrid.ItemsSource = selectedEmployee.AntropologicalResearches;
            educations_dataGrid.ItemsSource = selectedEmployee.Educations;
            jobs_dataGrid.ItemsSource = selectedEmployee.Jobs;

            jobTypes = _jobProcessingService.GetAllJobTypes();
            Job_comboBox.ItemsSource = jobTypes;

            isWindowLoaded = true;
        }

        private void addDisease_button_Click(object sender, RoutedEventArgs e)
        {
            if(DiseaseName_textBox.Text == string.Empty)
            {
                MessageBox.Show("Укажите название болезни.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Disease newDisease = new Disease { EmployeeId = selectedEmployee.EmployeeId, StardDiseaseDate = DateTime.Now, EndDiseaseDate = DateTime.Now, Name = DiseaseName_textBox.Text };
                _diseaseProcessingService.Add_Disease(newDisease);
                selectedEmployee.Diseases.Add((Disease_ViewModel)newDisease);

                diseases_dataGrid.Items.Refresh();
                DiseaseName_textBox.Text = string.Empty;

                LastDisease_textBox.Text = newDisease.Name;
            }
        }

        private void removeDisease_button_Click(object sender, RoutedEventArgs e)
        {
            Disease_ViewModel selectedDisease = (Disease_ViewModel)diseases_dataGrid.SelectedItem;
            if(selectedDisease != null)
            {
                selectedEmployee.Diseases.Remove(selectedDisease);
                _diseaseProcessingService.Delete_Disease(selectedDisease.DiseaseId);

                diseases_dataGrid.Items.Refresh();
                if(selectedEmployee.Diseases.Count > 0)
                {
                    LastDisease_textBox.Text = selectedEmployee.Diseases.LastOrDefault().Name;
                }
                else { LastDisease_textBox.Text = "Отсутствует"; }
            }
        }

        private void diseases_dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            bool isNotValidDate = false;
            var selectedItem = (Disease_ViewModel)diseases_dataGrid.SelectedItem;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    //Настройки для конвертации даты из строки
                    string format = "dd.MM.yyyy";
                    CultureInfo culture = CultureInfo.InvariantCulture;
                    if (!DateTime.TryParseExact(selectedItem.StardDiseaseDate, format, culture, DateTimeStyles.None, out DateTime stardDiseaseDate) ||
                        !DateTime.TryParseExact(selectedItem.EndDiseaseDate, format, culture, DateTimeStyles.None, out DateTime endDiseaseDate))
                    {
                        isNotValidDate = true;
                        throw new Exception();
                    }
                    else
                    {
                        if (stardDiseaseDate > endDiseaseDate)
                        {
                            isNotValidDate = false;
                            throw new Exception();
                        }

                        //Создание объекта для изменения в БД
                        Disease disease = new Disease
                        {
                            DiseaseId = selectedItem.DiseaseId,
                            Name = selectedItem.Name,
                            StardDiseaseDate = DateTime.ParseExact(selectedItem.StardDiseaseDate, format, culture),
                            EndDiseaseDate = DateTime.ParseExact(selectedItem.EndDiseaseDate, format, culture)
                        };

                        _diseaseProcessingService.Update_Disease(disease);
                    }
                }
                catch
                {
                    if (isNotValidDate)
                    {
                        MessageBox.Show("Введён неверный формат даты. Необходимый формат: dd.MM.yyyy", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    {
                        MessageBox.Show("Дата окончания болезни не может быть меньше её начала.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    if (e.Column.Header.ToString() == "Дата начала")
                        selectedItem.StardDiseaseDate = _cellOldValue;

                    if (e.Column.Header.ToString() == "Дата окончания")
                        selectedItem.EndDiseaseDate = _cellOldValue;

                    diseases_dataGrid.Items.Refresh();
                }
            }), DispatcherPriority.Background);
        }

        private void diseases_dataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Row.Item is Disease_ViewModel selectedItem)
            {
                // Сохраняем старое значение при начале редактирования
                if (e.Column.Header.ToString() == "Дата начала")
                    _cellOldValue = selectedItem.StardDiseaseDate;

                if (e.Column.Header.ToString() == "Дата окончания")
                    _cellOldValue = selectedItem.EndDiseaseDate;
            }
        }

        private void vaccinations_dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var selectedItem = (Vaccination_ViewModel)vaccinations_dataGrid.SelectedItem;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    //Настройки для конвертации даты из строки
                    string format = "dd.MM.yyyy";
                    CultureInfo culture = CultureInfo.InvariantCulture;
                    if (!DateTime.TryParseExact(selectedItem.Date, format, culture, DateTimeStyles.None, out DateTime stardDiseaseDate))
                    {
                        throw new Exception();
                    }
                    else
                    {
                        //Создание объекта для изменения в БД
                        Vaccination vaccination = new Vaccination
                        {
                            VaccinationId = selectedItem.VaccinationId,
                            Name = selectedItem.Name,
                            Date = DateTime.ParseExact(selectedItem.Date, format, culture),
                        };

                        _vaccinationProcessingService.Update_Vaccination(vaccination);
                    }
                }
                catch
                {
                    MessageBox.Show("Введён неверный формат даты. Необходимый формат: dd.MM.yyyy", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    if (e.Column.Header.ToString() == "Дата")
                        selectedItem.Date = _cellOldValue;

                    vaccinations_dataGrid.Items.Refresh();
                }
            }), DispatcherPriority.Background);
        }

        private void vaccinations_dataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Row.Item is Vaccination_ViewModel selectedItem)
            {
                // Сохраняем старое значение при начале редактирования
                if (e.Column.Header.ToString() == "Дата")
                    _cellOldValue = selectedItem.Date;
            }
        }

        private void addVaccination_button_Click(object sender, RoutedEventArgs e)
        {
            if (VaccinationName_textBox.Text == string.Empty)
            {
                MessageBox.Show("Укажите название прививки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Vaccination newVaccination = new Vaccination { EmployeeId = selectedEmployee.EmployeeId, Date = DateTime.Now, Name = VaccinationName_textBox.Text };
                _vaccinationProcessingService.Add_Vaccination(newVaccination);
                selectedEmployee.Vaccinations.Add((Vaccination_ViewModel)newVaccination);

                vaccinations_dataGrid.Items.Refresh();
                VaccinationName_textBox.Text = string.Empty;
            }
        }

        private void removeVaccination_button_Click(object sender, RoutedEventArgs e)
        {
            Vaccination_ViewModel selectedVaccination = (Vaccination_ViewModel)vaccinations_dataGrid.SelectedItem;
            if (selectedVaccination != null)
            {
                selectedEmployee.Vaccinations.Remove(selectedVaccination);
                _vaccinationProcessingService.Delete_Vaccination(selectedVaccination.VaccinationId);

                vaccinations_dataGrid.Items.Refresh();
            }
        }

        private void addFamilyStatus_button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEmployee.FamilyStatuses.Count > 0)
            {
                FamilyStatus_ViewModel lastFamilyStatus = selectedEmployee.FamilyStatuses[selectedEmployee.FamilyStatuses.Count - 1];
                lastFamilyStatus.Status = "Разведён";
                lastFamilyStatus.EndFamilyDate = DateTime.Now.ToString("dd.MM.yyyy");

                FamilyStatus updatedFamilyStatus = new FamilyStatus
                {
                    FamilyStatusId = lastFamilyStatus.FamilyStatusId,
                    Status = lastFamilyStatus.Status,
                    StartFamilyDate = DateTime.ParseExact(lastFamilyStatus.StartFamilyDate, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                    EndFamilyDate = DateTime.Now
                };

                _familyProcessingService.Update_FamilyStatus(updatedFamilyStatus);
            }

            FamilyStatus newFamilyStatus = new FamilyStatus { EmployeeId = selectedEmployee.EmployeeId, Status = "Женат", StartFamilyDate = startFamilyDate_datePicker.SelectedDate.Value, EndFamilyDate = DateTime.MinValue };
            _familyProcessingService.Add_FamilyStatus(newFamilyStatus);
            selectedEmployee.FamilyStatuses.Add((FamilyStatus_ViewModel)newFamilyStatus);
            
            familyStatuses_dataGrid.Items.Refresh();
        }

        private void removeFamilyStatus_button_Click(object sender, RoutedEventArgs e)
        {
            FamilyStatus_ViewModel selectedFamilyStatus = (FamilyStatus_ViewModel)familyStatuses_dataGrid.SelectedItem;
            if (selectedFamilyStatus != null)
            {
                selectedEmployee.FamilyStatuses.Remove(selectedFamilyStatus);
                _familyProcessingService.Delete_FamilyStatus(selectedFamilyStatus.FamilyStatusId);

                familyStatuses_dataGrid.Items.Refresh();
            }
        }

        private void familyStatuses_dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //Настройки для конвертации даты из строки
            string format = "dd.MM.yyyy";
            CultureInfo culture = CultureInfo.InvariantCulture;

            bool isNotValidDate = false;
            var selectedItem = (FamilyStatus_ViewModel)familyStatuses_dataGrid.SelectedItem;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    if (selectedItem.EndFamilyDate == "-")
                    {
                        selectedItem.Status = "Женат";
                        //Создание объекта для изменения в БД
                        FamilyStatus familyStatus = new FamilyStatus
                        {
                            FamilyStatusId = selectedItem.FamilyStatusId,
                            Status = selectedItem.Status,
                            StartFamilyDate = DateTime.ParseExact(selectedItem.StartFamilyDate, format, culture),
                            EndFamilyDate = DateTime.MinValue
                        };

                        _familyProcessingService.Update_FamilyStatus(familyStatus);

                        return;
                    }
                    
                    if (!DateTime.TryParseExact(selectedItem.StartFamilyDate, format, culture, DateTimeStyles.None, out DateTime startFamilyDate)||
                        !DateTime.TryParseExact(selectedItem.EndFamilyDate, format, culture, DateTimeStyles.None, out DateTime endFamilyDate))
                    {
                        isNotValidDate = true;
                        throw new Exception();
                    }
                    else
                    {
                        if(startFamilyDate > endFamilyDate)
                        {
                            isNotValidDate = false;
                            throw new Exception();
                        }
                        else
                        {
                            selectedItem.Status = "Разведён";
                            //Создание объекта для изменения в БД
                            FamilyStatus familyStatus = new FamilyStatus
                            {
                                FamilyStatusId = selectedItem.FamilyStatusId,
                                Status = selectedItem.Status,
                                StartFamilyDate = DateTime.ParseExact(selectedItem.StartFamilyDate, format, culture),
                                EndFamilyDate = DateTime.ParseExact(selectedItem.EndFamilyDate, format, culture)
                            };

                            _familyProcessingService.Update_FamilyStatus(familyStatus);
                        }
                    }
                }
                catch
                {
                    if (isNotValidDate)
                    {
                        MessageBox.Show("Введён неверный формат даты. Необходимый формат: dd.MM.yyyy", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Дата расторжения брака не может быть раньше его начала.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    if (e.Column.Header.ToString() == "Дата начала семейной жизни")
                        selectedItem.StartFamilyDate = _cellOldValue;

                    if (e.Column.Header.ToString() == "Дата окончания")
                        selectedItem.EndFamilyDate = _cellOldValue;

                }
            }), DispatcherPriority.Background);
        }

        private void familyStatuses_dataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Row.Item is FamilyStatus_ViewModel selectedItem)
            {
                // Сохраняем старое значение при начале редактирования
                if (e.Column.Header.ToString() == "Дата начала семейной жизни")
                    _cellOldValue = selectedItem.StartFamilyDate;

                if (e.Column.Header.ToString() == "Дата окончания")
                    _cellOldValue = selectedItem.EndFamilyDate;
            }
        }

        private void addAntropologicalResearch_button_Click(object sender, RoutedEventArgs e)
        {
            if(height_textBox.Text == string.Empty || weight_textBox.Text == string.Empty)
            {
                MessageBox.Show("Укажите антропологические параметры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var culture = new CultureInfo("en-US"); // en-US использует точку как разделитель
                culture.NumberFormat.NumberDecimalSeparator = "."; // Указываем точку как разделитель
                culture.NumberFormat.CurrencyDecimalSeparator = ",";

                AntropologicalResearch newAntropologicalResearch = new AntropologicalResearch 
                { 
                    EmployeeId = selectedEmployee.EmployeeId, 
                    Weight = double.Parse(weight_textBox.Text, NumberStyles.Any, culture), 
                    Height = double.Parse(height_textBox.Text, NumberStyles.Any, culture), 
                    Date = DateTime.Now 
                };
                _antropologicalResearchProcessingService.Add_AntropologicalResearch(newAntropologicalResearch);
                selectedEmployee.AntropologicalResearches.Add((AntropologicalResearch_ViewModel)newAntropologicalResearch);

                antropologicalResearches_dataGrid.Items.Refresh();
            }
        }

        private void removeAntropologicalResearch_button_Click(object sender, RoutedEventArgs e)
        {
            AntropologicalResearch_ViewModel selectedAntropologicalResearch = (AntropologicalResearch_ViewModel)antropologicalResearches_dataGrid.SelectedItem;
            if (selectedAntropologicalResearch != null)
            {
                selectedEmployee.AntropologicalResearches.Remove(selectedAntropologicalResearch);
                _antropologicalResearchProcessingService.Delete_AntropologicalResearch(selectedAntropologicalResearch.AntropologicalResearchId);

                antropologicalResearches_dataGrid.Items.Refresh();
            }
        }

        private void onlyNumbersTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex onlyNumbers = new Regex("[^0-9.,-]+");
            e.Handled = onlyNumbers.IsMatch(e.Text);
        }

        private void antropologicalResearches_dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var selectedItem = (AntropologicalResearch_ViewModel)antropologicalResearches_dataGrid.SelectedItem;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    string format = "dd.MM.yyyy";
                    CultureInfo culture = CultureInfo.InvariantCulture;
                    if (!DateTime.TryParseExact(selectedItem.Date, format, culture, DateTimeStyles.None, out DateTime stardDiseaseDate))
                    {
                        throw new Exception();
                    }
                    else
                    {
                        //Создание объекта для изменения в БД
                        AntropologicalResearch antropologicalResearch = new AntropologicalResearch
                        {
                            AntropologicalResearchId = selectedItem.AntropologicalResearchId,
                            Weight = selectedItem.Weight,
                            Height = selectedItem.Height,
                            Date = DateTime.ParseExact(selectedItem.Date, format, culture),
                        };

                        _antropologicalResearchProcessingService.Update_AntropologicalResearch(antropologicalResearch);
                    }
                }
                catch
                {
                    MessageBox.Show("Введён неверный формат даты. Необходимый формат: dd.MM.yyyy", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    if (e.Column.Header.ToString() == "Дата")
                        selectedItem.Date = _cellOldValue;

                    vaccinations_dataGrid.Items.Refresh();
                }
            }), DispatcherPriority.Background);
        }

        private void antropologicalResearches_dataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Row.Item is AntropologicalResearch_ViewModel selectedItem)
            {
                // Сохраняем старое значение при начале редактирования
                if (e.Column.Header.ToString() == "Дата")
                    _cellOldValue = selectedItem.Date;
            }
        }

        private void addJob_button_Click(object sender, RoutedEventArgs e)
        {
            if (Job_comboBox.Text == string.Empty || subunit_comboBox.Text == string.Empty)
            {
                MessageBox.Show("Укажите все необходимые параметры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (selectedEmployee.Jobs.Count > 0)
                {
                    Job_ViewModel lastJob = selectedEmployee.Jobs[selectedEmployee.Jobs.Count - 1];
                    lastJob.EndWorkingDate = DateTime.Now.ToString("dd.MM.yyyy");

                    Job updatedJob = new Job
                    {
                        JobId = lastJob.JobId,
                        Subunit = lastJob.Subunit,
                        WorkingRate = lastJob.WorkingRate,
                        StartWorkingDate = DateTime.ParseExact(lastJob.StartWorkingDate, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        EndWorkingDate = DateTime.Now,
                        EmployeeId = selectedEmployee.EmployeeId,
                        JobTypeId = _jobProcessingService.Get_JobTypeIdByName(lastJob.JobName)
                    };

                    _jobProcessingService.Update_Job(updatedJob);
                }

                Job newJob = new Job
                {
                    Subunit = subunit_comboBox.Text,
                    WorkingRate = 805,
                    StartWorkingDate = DateTime.Now,
                    EndWorkingDate = DateTime.MinValue,
                    EmployeeId = selectedEmployee.EmployeeId,
                    JobTypeId = _jobProcessingService.Get_JobTypeIdByName(Job_comboBox.Text),
                };
                _jobProcessingService.Add_Job(newJob);
                selectedEmployee.Jobs.Add((Job_ViewModel)newJob);

                jobs_dataGrid.Items.Refresh();
            }
        }

        private void removeJob_button_Click(object sender, RoutedEventArgs e)
        {
            Job_ViewModel selectedJob = (Job_ViewModel)jobs_dataGrid.SelectedItem;
            if (selectedJob != null)
            {
                selectedEmployee.Jobs.Remove(selectedJob);
                _jobProcessingService.Delete_Job(selectedJob.JobId);

                jobs_dataGrid.Items.Refresh();
            }
        }

        private void jobs_dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //Настройки для конвертации даты из строки
            string format = "dd.MM.yyyy";
            CultureInfo culture = CultureInfo.InvariantCulture;

            bool isNotValidDate = false;
            var selectedItem = (Job_ViewModel)jobs_dataGrid.SelectedItem;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    if (selectedItem.EndWorkingDate == "-")
                    {
                        //Создание объекта для изменения в БД
                        Job updatedJob = new Job
                        {
                            JobId = selectedItem.JobId,
                            Subunit = selectedItem.Subunit,
                            WorkingRate = selectedItem.WorkingRate,
                            StartWorkingDate = DateTime.ParseExact(selectedItem.StartWorkingDate, format, culture),
                            EndWorkingDate = DateTime.MinValue,
                            EmployeeId = selectedEmployee.EmployeeId,
                            JobTypeId = _jobProcessingService.Get_JobTypeIdByName(selectedItem.JobName)
                        };

                        _jobProcessingService.Update_Job(updatedJob);

                        return;
                    }

                    if (!DateTime.TryParseExact(selectedItem.StartWorkingDate, format, culture, DateTimeStyles.None, out DateTime startFamilyDate) ||
                        !DateTime.TryParseExact(selectedItem.EndWorkingDate, format, culture, DateTimeStyles.None, out DateTime endFamilyDate))
                    {
                        isNotValidDate = true;
                        throw new Exception();
                    }
                    else
                    {
                        if (startFamilyDate > endFamilyDate)
                        {
                            isNotValidDate = false;
                            throw new Exception();
                        }
                        else
                        {
                            //Создание объекта для изменения в БД
                            Job updatedJob = new Job
                            {
                                JobId = selectedItem.JobId,
                                Subunit = selectedItem.Subunit,
                                WorkingRate = selectedItem.WorkingRate,
                                StartWorkingDate = DateTime.ParseExact(selectedItem.StartWorkingDate, format, culture),
                                EndWorkingDate = DateTime.ParseExact(selectedItem.EndWorkingDate, format, culture),
                                EmployeeId = selectedEmployee.EmployeeId,
                                JobTypeId = _jobProcessingService.Get_JobTypeIdByName(selectedItem.JobName)
                            };

                            _jobProcessingService.Update_Job(updatedJob);
                        }
                    }
                }
                catch
                {
                    if (isNotValidDate)
                    {
                        MessageBox.Show("Введён неверный формат даты. Необходимый формат: dd.MM.yyyy", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Дата окончания работы не может быть раньше её начала.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    if (e.Column.Header.ToString() == "Дата вступления в должность")
                        selectedItem.StartWorkingDate = _cellOldValue;

                    if (e.Column.Header.ToString() == "Дата окончания")
                        selectedItem.EndWorkingDate = _cellOldValue;

                }
            }), DispatcherPriority.Background);
        }

        private void jobs_dataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Row.Item is Job_ViewModel selectedItem)
            {
                // Сохраняем старое значение при начале редактирования
                if (e.Column.Header.ToString() == "Дата вступления в должность")
                    _cellOldValue = selectedItem.StartWorkingDate;

                if (e.Column.Header.ToString() == "Дата окончания")
                    _cellOldValue = selectedItem.EndWorkingDate;
            }
        }












        private void removeEducation_button_Click(object sender, RoutedEventArgs e)
        {
            Education_ViewModel selectedEducation = (Education_ViewModel)educations_dataGrid.SelectedItem;
            if (selectedEducation != null)
            {
                selectedEmployee.Educations.Remove(selectedEducation);
                _educationProcessingService.Delete_Education(selectedEducation.EducationId);

                educations_dataGrid.Items.Refresh();
            }
        }

        private void addEducation_button_Click(object sender, RoutedEventArgs e)
        {
            if (EducationInstitutionName_textBox.Text == string.Empty || EducationType_comboBox.Text == string.Empty)
            {
                MessageBox.Show("Укажите название прививки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Education newEducation = new Education 
                {
                    EmployeeId = selectedEmployee.EmployeeId,
                    EducationType = EducationType_comboBox.Text,
                    EducationInstitution = EducationInstitutionName_textBox.Text,
                    Date = DateTime.Now
                };
                _educationProcessingService.Add_Education(newEducation);
                selectedEmployee.Educations.Add((Education_ViewModel)newEducation);

                educations_dataGrid.Items.Refresh();

                EducationInstitutionName_textBox.Text = string.Empty;
                EducationType_comboBox.Text = string.Empty;
            }
        }

        private void educations_dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var selectedItem = (Education_ViewModel)educations_dataGrid.SelectedItem;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    //Настройки для конвертации даты из строки
                    string format = "dd.MM.yyyy";
                    CultureInfo culture = CultureInfo.InvariantCulture;
                    if (!DateTime.TryParseExact(selectedItem.Date, format, culture, DateTimeStyles.None, out DateTime Date))
                    {
                        throw new Exception();
                    }
                    else
                    {
                        //Создание объекта для изменения в БД
                        Education education = new Education
                        {
                            EducationId = selectedItem.EducationId,
                            EducationType = selectedItem.EducationType,
                            EducationInstitution = selectedItem.EducationInstitution,
                            Date = DateTime.ParseExact(selectedItem.Date, format, culture),
                        };

                        _educationProcessingService.Update_Education(education);
                    }
                }
                catch
                {
                    MessageBox.Show("Введён неверный формат даты. Необходимый формат: dd.MM.yyyy", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    if (e.Column.Header.ToString() == "Дата окончания")
                        selectedItem.Date = _cellOldValue;

                    educations_dataGrid.Items.Refresh();
                }
            }), DispatcherPriority.Background);
        }

        private void educations_dataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Row.Item is Education_ViewModel selectedItem)
            {
                // Сохраняем старое значение при начале редактирования
                if (e.Column.Header.ToString() == "Дата окончания")
                    _cellOldValue = selectedItem.Date;
            }
        }

        private void Birthday_datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isWindowLoaded)
            {
                Employee updatedEmployee = new Employee
                {
                    EmployeeId = selectedEmployee.EmployeeId,
                    FIO = selectedEmployee.FIO,
                    Birthday = Birthday_datePicker.SelectedDate.Value,
                    Photo = selectedEmployee.Photo,
                    MailAdress = selectedEmployee.MailAdress,
                };

                selectedEmployee.Birthday = updatedEmployee.Birthday.ToString("dd.MM.yyyy");

                _employeeProcessingService.Update_employee(updatedEmployee);
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isWindowLoaded)
            {
                Employee updatedEmployee = new Employee
                {
                    EmployeeId = selectedEmployee.EmployeeId,
                    FIO = FIO_textBox.Text,
                    Birthday = Birthday_datePicker.SelectedDate.Value,
                    Photo = selectedEmployee.Photo,
                    MailAdress = Mail_textBox.Text,
                };

                selectedEmployee.FIO = updatedEmployee.FIO;
                selectedEmployee.MailAdress = updatedEmployee.MailAdress;

                _employeeProcessingService.Update_employee(updatedEmployee);
            }
        }

        private void uploadFromFile_button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
