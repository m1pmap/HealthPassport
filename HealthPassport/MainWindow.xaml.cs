using HealthPassport.BLL.Interfaces;
using HealthPassport.BLL.Models;
using HealthPassport.DAL.Models;
using HealthPassport.Interfaces;
using HealthPassport.Models;
using HealthPassport.Pages;
using HealthPassport.Services;
using LiveCharts.Wpf;
using LiveCharts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HealthPassport.DAL;
using LiveCharts.Maps;
using HealthPassport.DAL.Interfaces;
using System.Windows.Media.Animation;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace HealthPassport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Зависимости
        private readonly ByteArrayToImageSourceConverter_Service _imageSourceConverter;
        private readonly IEmployeeProcessing _employeeProcessingService;
        private readonly IImageUpdater _imageUpdater;
        private readonly IFileWorker _fileWorker;
        private readonly IServiceProvider _serviceProvider;
        private readonly IShaderEffects _shaderEffectsService;
        private readonly IMailSender _mailSenderService;

        private readonly ICudProcessing<Job> _jobProcessingService;
        private readonly IJobTypeProcessing _jobTypeProcessingService;
        private readonly ISubunitProcessing _subunitProcessingService;
        private readonly IDiseaseProcessing _diseaseProcessingService;

        private readonly IExportWorker _excelWorkerService;

        //Коллекция для хранения всех сотрудников
        public ObservableCollection<Employee_ViewModel> employees = new ObservableCollection<Employee_ViewModel>();
        private Employee_ViewModel currentUser = new();

        public ObservableCollection<Employee_ViewModel> calcEmployees = new ObservableCollection<Employee_ViewModel>();

        //Настройки и ифнормация текущего пользователяп 
        public SettingsParameters settings = new SettingsParameters();

        private bool isNowExit = false;
        int oldCurrentProductIndex = 1; //Прошлый выбранный элемент в базе данных

        public MainWindow(ByteArrayToImageSourceConverter_Service imageSourceConverter, 
            IEmployeeProcessing employeeProcessingService,
            IImageUpdater imageUpdater,
            IFileWorker fileWorker,
            IServiceProvider serviceProvider,
            IShaderEffects shaderEffectsService,
            ICudProcessing<Job> jobProcessingService,
            IJobTypeProcessing jobTypeProcessingService,
            ISubunitProcessing subunitProcessingService,
            IMailSender mailSenderService,
            IDiseaseProcessing diseaseProcessingService)
        {
            InitializeComponent();

            //Инициализация сервисов
            _imageSourceConverter = imageSourceConverter;
            _employeeProcessingService = employeeProcessingService;
            _imageUpdater = imageUpdater;
            _fileWorker = fileWorker;
            _serviceProvider = serviceProvider;
            _mailSenderService = mailSenderService;
            _jobProcessingService = jobProcessingService;
            _jobTypeProcessingService = jobTypeProcessingService;
            _subunitProcessingService = subunitProcessingService;
            _diseaseProcessingService = diseaseProcessingService;

            _excelWorkerService = serviceProvider.GetRequiredKeyedService<IExportWorker>("Excel");

            //Начальное заполнение данными
            List<Employee> dbEmployees = _employeeProcessingService.Get_AllEmployees();
            foreach (Employee employee in dbEmployees)
            {
                Employee_ViewModel emploeyeVM = (Employee_ViewModel)_employeeProcessingService.Connect_EmployeeInformation(employee);
                employees.Add(emploeyeVM);
            }

            employees_dataGrid.ItemsSource = employees;
            calcEmployees_dataGrid.ItemsSource = calcEmployees;
            employeesCount_label.Content = $"из {employees.Count}";
            _serviceProvider = serviceProvider;
            _shaderEffectsService = shaderEffectsService;

            NewJob_comboBox.ItemsSource = _jobTypeProcessingService.Get_AllItems();
            NewSubunit_comboBox.ItemsSource = _subunitProcessingService.Get_AllItems();
        }

        private void addEmployee_button_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(NewFIO_textBox.Text) ||
               !string.IsNullOrWhiteSpace(NewJob_comboBox.Text) ||
               !string.IsNullOrWhiteSpace(NewSubunit_comboBox.Text) ||
               !string.IsNullOrWhiteSpace(NewMail_textBox.Text) ||
               !string.IsNullOrWhiteSpace(NewDayBirth_textBox.Text) ||
               !string.IsNullOrWhiteSpace(NewMonthBirth_textBox.Text) ||
               !string.IsNullOrWhiteSpace(NewYearBirth_textBox.Text)) 
            {
                if (!_mailSenderService.IsValidEmail(NewMail_textBox.Text))
                {
                    MessageBox.Show("Указан невалидный адрес почты. Подкорректируйте и повторите попытку снова.");
                    return;
                }
                int year = Convert.ToInt32(NewYearBirth_textBox.Text);
                int month = Convert.ToInt32(NewMonthBirth_textBox.Text);
                int day = Convert.ToInt32(NewDayBirth_textBox.Text);

                Employee newEmployee = new Employee
                {
                    FIO = NewFIO_textBox.Text,
                    MailAdress = NewMail_textBox.Text,
                    Birthday = new DateTime(year, month, day),
                };                

                if (addedEmployeeImage.Source.ToString() != "pack://application:,,,/Resources/images/without_picture.png")
                {
                    newEmployee.Photo = _imageSourceConverter.ConvertFromComponentImageToByteArray(addedEmployeeImage);
                }
                if (newEmployee.Photo == null)
                {
                    newEmployee.Photo = _imageSourceConverter.ConvertFromFileImageToByteArray("without_image_database.png");
                }

                if (_employeeProcessingService.Add_Item(newEmployee))
                {
                    Job newJob = new Job
                    {
                        WorkingRate = 0,
                        StartWorkingDate = DateTime.Now,
                        EndWorkingDate = DateTime.MinValue,
                        EmployeeId = newEmployee.EmployeeId,
                        JobTypeId = _jobTypeProcessingService.Get_IdByMainParam(NewJob_comboBox.Text),
                        SubunitId = _subunitProcessingService.Get_IdByMainParam(NewSubunit_comboBox.Text),
                    };

                    _jobProcessingService.Add_Item(newJob);

                    employees.Add((Employee_ViewModel)newEmployee);
                    employeesCount_label.Content = $"из {employees.Count}";
                }
                else
                {
                    MessageBox.Show("Пользователь не добавлен.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("ЗАполните все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void deleteSelectedEmployee_button_Click(object sender, RoutedEventArgs e)
        {
            Employee_ViewModel selectedEmployee = (Employee_ViewModel)employees_dataGrid.SelectedItem;
            if (selectedEmployee != null) 
            {
                _employeeProcessingService.Delete_Item(selectedEmployee.EmployeeId);
                employees.Remove(selectedEmployee);
                employeesCount_label.Content = $"из {employees.Count}";
            }
        }

        private void uploadFromFileNewImage_button_button_Click(object sender, RoutedEventArgs e)
        {
            _imageUpdater.UploadImageFromFile(addedEmployeeImage);
        }

        private void uploadFromClipboardAdd_button_Click(object sender, RoutedEventArgs e)
        {
            _imageUpdater.UploadImageFromClipboard(addedEmployeeImage);
        }

        private void deleteAdd_button_Click(object sender, RoutedEventArgs e)
        {
            _imageUpdater.DeleteImage(addedEmployeeImage);
        }

        private void employees_dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            oldCurrentProductIndex = employees_dataGrid.SelectedIndex;
            Employee_ViewModel selectedEmployee = (Employee_ViewModel)employees_dataGrid.SelectedItem;
            if(selectedEmployee != null) 
            {
                FIO_textBox.Text = selectedEmployee.FIO;
                Birthday_textBox.Text = selectedEmployee.Birthday;
                if (selectedEmployee.Job != string.Empty)
                    Job_textBox.Text = selectedEmployee.Job;
                else
                    Job_textBox.Text = "Не указано";


                if (selectedEmployee.Education != string.Empty) 
                    Education_textBox.Text = selectedEmployee.Education;
                else
                    Education_textBox.Text = "Не указано";

                if(selectedEmployee.FamilyStatus != string.Empty)
                    FamilyStatus_textBox.Text = selectedEmployee.FamilyStatus;
                else
                    FamilyStatus_textBox.Text = "Не указано";

                //Отображение картинки выбранного пользователя
                var fileImageBytes = _imageSourceConverter.ConvertFromFileImageToByteArray("without_image_database.png");
                if (BitConverter.ToString(fileImageBytes) == BitConverter.ToString(selectedEmployee.Photo)) //Если нет фотографии
                {
                    EmployeeImage.Source = new BitmapImage(new Uri("resources/images/without_picture.png", UriKind.Relative));
                }
                else
                {
                    EmployeeImage.Source = (BitmapImage)_imageSourceConverter.Convert(selectedEmployee.Photo, typeof(BitmapImage), null, CultureInfo.CurrentCulture);
                }

                employeeNum_textBox.Text = (employees.IndexOf(selectedEmployee) + 1).ToString();
            }
        }

        public bool isNowNextEmployee = false;
        public bool isNowPrevEmployee = false;

        private void employees_dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Employee_ViewModel selectedEmployee = (Employee_ViewModel)employees_dataGrid.SelectedItem;
            if (selectedEmployee != null)
            {
                MoreEmployeeInfoPage infoPage = _serviceProvider.GetRequiredService<MoreEmployeeInfoPage>();
                infoPage.selectedEmployee = selectedEmployee;
                infoPage.currentUser = currentUser;
                infoPage.Owner = this;
                _shaderEffectsService.ApplyBlurEffect(this, 20);

                infoPage.ShowDialog();

                if(isNowNextEmployee || isNowPrevEmployee)
                {
                    int index = employees_dataGrid.SelectedIndex;

                    if (isNowNextEmployee)
                    {
                        isNowNextEmployee = false;
                        index++;
                    }
                    else if (isNowPrevEmployee)
                    {
                        isNowPrevEmployee = false;
                        index--;
                    }

                    if(index != -1)
                    {
                        employees_dataGrid.SelectedIndex = index;
                        employees_dataGrid.ScrollIntoView((Employee_ViewModel)employees_dataGrid.SelectedItem);
                    }
                    else
                    {
                        employees_dataGrid.SelectedIndex = 0;
                    }

                    employees_dataGrid_MouseDoubleClick(sender, e);
                }


                _shaderEffectsService.ClearEffect(this);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!isNowExit) 
            {
                settings.employeeFIO = currentUser.FIO;
                settings.employeeMailAdress = currentUser.MailAdress;
                _fileWorker.WriteJson(settings, "settings.json"); 
            }
        }

        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            isNowExit = true; //Установка флага для того, чтобы при закрытии приложения не обноились настройки
            File.Delete("settings.json"); //Удаление настроек

            //Закрытие текущего приложения и открытие заново
            string exePath = AppDomain.CurrentDomain.BaseDirectory + "HealthPassport.exe";
            Process.Start(exePath);
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            userFIO_label.Content = settings.employeeFIO;
            currentUser = employees.FirstOrDefault(e => e.FIO == settings.employeeFIO);
            currentUser.IsCurrentUser = true;

            JobType currentJobType = _employeeProcessingService.Get_EmployeeJobType(currentUser.EmployeeId);
            if (!currentJobType.isCanAddRows)
            {
                add_tabItem.Visibility = Visibility.Collapsed;
            }

            if (!currentJobType.isCanEditItems)
            {
                data_menuItem.Visibility = Visibility.Collapsed;
            }

            deleteDiseaseDinamicDate_button_Click(sender, e);
            deleteEmployeeVaccinationDate_button_Click(sender, e);
            deleteDate_button_Click(sender, e);

            _employeeProcessingService.SetCurrentEmployeeId(currentUser.EmployeeId);
        }

        private void employeeNum_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter) //Если нажат Enter
                {
                    int index = Convert.ToInt32(employeeNum_textBox.Text) - 1;

                    if (index > employees_dataGrid.Items.Count) //Если индекс выше допустимого
                    {
                        throw new Exception("Выход за предел количества элементов");
                    }

                    employees_dataGrid.SelectedIndex = index;
                    oldCurrentProductIndex = index;

                    employees_dataGrid.Focus();
                }
            }
            catch
            {
                employees_dataGrid.SelectedIndex = oldCurrentProductIndex;
                employeeNum_textBox.Text = (oldCurrentProductIndex + 1).ToString();
                employees_dataGrid.Focus();
            }
        }

        private void EmployeeMoreInfo_Click(object sender, RoutedEventArgs e)
        {
            employees_dataGrid_MouseDoubleClick(sender, new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left));
        }


        int selectDbTabItem = 0;
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                TabControl tabControl = (TabControl)sender;
                TabItem selectedTabItem = (TabItem)tabControl.SelectedItem;
                if(selectedTabItem.Tag.ToString() == "changeDataGrid")
                {
                    DataBase_border.Visibility = Visibility.Hidden;
                    CalcDataGrid_grid.Visibility = Visibility.Visible;

                    List<string> diseases = new List<string> {"Все"};
                    employees.ToList().ForEach(e => e.Diseases.ForEach(d => diseases.Add(d.Name)));
                    diseases = diseases.Select(s => s.Trim()).Distinct().ToList();
                    diseases_comboBox.ItemsSource = diseases;

                    List<string> vaccinations = new();
                    employees.ToList().ForEach(e => e.Vaccinations.ForEach(v => vaccinations.Add(v.Name)));
                    vaccinations = vaccinations.Select(s => s.Trim()).Distinct().ToList();
                    vaccinations_comboBox.ItemsSource = vaccinations;

                    selectDbTabItem = 2;
                }
                else
                {
                    if (selectedTabItem.Tag.ToString() == "noneChangeDataGrid")
                    {
                        selectDbTabItem = 0;
                    }
                    else
                    {
                        selectDbTabItem = 1;
                    }

                    DataBase_border.Visibility = Visibility.Visible;
                    CalcDataGrid_grid.Visibility = Visibility.Hidden;
                }
            }
        }

        private void addEmplyeeVaccination_menuItem_Click(object sender, RoutedEventArgs e)
        {
            VaccnationsAddPage vaccnationsAddPage = _serviceProvider.GetRequiredService<VaccnationsAddPage>();
            vaccnationsAddPage.Owner = this;
            vaccnationsAddPage.employees = employees;
            _shaderEffectsService.ApplyBlurEffect(this, 20);
            vaccnationsAddPage.ShowDialog();
            _shaderEffectsService.ClearEffect(this);
        }

        private void showHide_button_image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double from = chart_column.Width.Value;
            double to = chart_column.Width.Value > 0 ? 0 : 180;

            //var animation = new GridLengthAnimation
            //{
            //    From = new GridLength(from, GridUnitType.Star),
            //    To = new GridLength(to, GridUnitType.Star),
            //    Duration = new Duration(TimeSpan.FromMilliseconds(650))
            //};

            //var storyboard = new Storyboard();
            //storyboard.Children.Add(animation);

            //Storyboard.SetTarget(animation, chart_column);
            //Storyboard.SetTargetProperty(animation, new PropertyPath("Width"));
            //storyboard.Begin();

            chart_column.Width = new GridLength(to, GridUnitType.Star);

            // Обновляем визуал и подсказку
            showHide_button_image.RenderTransform = new RotateTransform(to == 0 ? 180 : 360);
            showHide_button_image.ToolTip = to == 0 ? "Отображать график" : "Скрыть график";
        }

        private List<LineSeries> _chartSeries = new List<LineSeries>();

        private void DiseaseDinamic_button_Click(object sender, RoutedEventArgs e)
        {
            currentChartIndex = 0;
            if (diseases_comboBox.Text == string.Empty)
            {
                MessageBox.Show("Болезнь не выбрана! Выберите болезнь и повторите попытку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string selectedDisease = diseases_comboBox.Text;

            DateTime minDate = startDiseaseDinamicDate_datePicker.SelectedDate ?? DateTime.Parse("01.01.2024");
            DateTime maxDate = endDiseaseDinamicDate_datePicker.SelectedDate ?? DateTime.Now;

            if (minDate > maxDate)
            {
                MessageBox.Show("Дата начала не может быть больше даты окончания временного промежутка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<DateTime> intervals = new List<DateTime>();
            DateTime current = new DateTime(minDate.Year, minDate.Month, 1);
            while (current <= maxDate)
            {
                intervals.Add(current);
                current = current.AddMonths(1);
            }
            intervals.Add(current); // конец последнего месяца

            int periodCount = intervals.Count - 1;
            List<int> casesCountHigh = Enumerable.Repeat(0, periodCount).ToList();
            List<int> casesCountMiddle = Enumerable.Repeat(0, periodCount).ToList();
            List<int> casesCountNone = Enumerable.Repeat(0, periodCount).ToList();

            foreach (var employee in employees)
            {
                foreach (var disease in employee.Diseases)
                {
                    if (selectedDisease != "Все" && disease.Name != selectedDisease)
                        continue;

                    for (int i = 0; i < periodCount; i++)
                    {
                        DateTime intervalStart = intervals[i];
                        DateTime intervalEnd = intervals[i + 1].AddDays(-1);

                        if (DateTime.Parse(disease.StardDiseaseDate) <= intervalEnd &&
                            DateTime.Parse(disease.EndDiseaseDate) >= intervalStart)
                        {
                            switch (employee.Education)
                            {
                                case "Высшее":
                                    casesCountHigh[i]++;
                                    break;
                                case "Среднее профессиональное":
                                case "Среднее общее: 10-11 классы":
                                case "Основное общее: 5-9 классы":
                                    casesCountMiddle[i]++;
                                    break;
                                case "Начальное общее: 1-4 классы":
                                case "Без образования":
                                case "Дошкольное":
                                    casesCountNone[i]++;
                                    break;
                            }
                        }
                    }
                }
            }

            // Список серий с цветами
            var seriesList = new List<HighlightableSeries>
            {
                new HighlightableSeries
                {
                    Title = "Высшее образование",
                    Values = new ChartValues<int>(casesCountHigh),
                    StrokeBrush = new SolidColorBrush(Color.FromRgb(136, 194, 216)),
                    FillBrush = new SolidColorBrush(Color.FromArgb(50, 136, 194, 216))
                },
                new HighlightableSeries
                {
                    Title = "Среднее образование",
                    Values = new ChartValues<int>(casesCountMiddle),
                    StrokeBrush = new SolidColorBrush(Color.FromRgb(136, 188, 143)),
                    FillBrush = new SolidColorBrush(Color.FromArgb(30, 136, 188, 143))

                },
                new HighlightableSeries
                {
                    Title = "Без образования",
                    Values = new ChartValues<int>(casesCountNone),
                    StrokeBrush = new SolidColorBrush(Color.FromRgb(233, 78, 100)),
                    FillBrush = new SolidColorBrush(Color.FromArgb(30, 233, 78, 100))
                }
            };

            var labels = intervals.Take(periodCount)
                                  .Select(d => d.ToString("MM.yyyy"))
                                  .ToList();

            GraphChart.Series = new SeriesCollection();
            _chartSeries.Clear();

            foreach (var series in seriesList)
            {
                var line = series.ToLineSeries();
                GraphChart.Series.Add(line);
                _chartSeries.Add(line);
            }

            GraphChart.AxisX[0].Labels = labels;

            changeOpacity_button_Click(sender, e);
        }


        int currentChartIndex = 0;
        private void changeOpacity_button_Click(object sender, RoutedEventArgs e)
        {
            _chartSeries[currentChartIndex].Stroke.Opacity = 1.0;
            _chartSeries[currentChartIndex].Fill.Opacity = 0.5;
            changeOpacity_button.Background = _chartSeries[currentChartIndex].Stroke;

            for (int i = 0; i < _chartSeries.Count; i++)
            {
                if(i != currentChartIndex)
                {
                    _chartSeries[i].Stroke.Opacity = 0.2;
                    _chartSeries[i].Fill.Opacity = 0.1;
                }
            }

            if(currentChartIndex == 2)
                currentChartIndex = 0;
            else
                currentChartIndex++;
        }

        private void employeeVaccinationDate_datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            employeeVaccinationDate_datePicker.Width = 253;
        }

        private void deleteEmployeeVaccinationDate_button_Click(object sender, RoutedEventArgs e)
        {
            employeeVaccinationDate_datePicker.SelectedDate = DateTime.Now;
            employeeVaccinationDate_datePicker.Width = 274;
        }

        private void deleteDiseaseDinamicDate_button_Click(object sender, RoutedEventArgs e)
        {
            startDiseaseDinamicDate_datePicker.SelectedDate = null;
            endDiseaseDinamicDate_datePicker.SelectedDate = DateTime.Now;

            diseaseDinamicLabel_label.Visibility = Visibility.Visible;
            deleteDiseaseDinamicDate_button.Visibility = Visibility.Collapsed;
        }

        private void diseaseDinamicDate_datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            diseaseDinamicLabel_label.Visibility = Visibility.Collapsed;
            deleteDiseaseDinamicDate_button.Visibility = Visibility.Visible;
        }

        private void employeesWithoutVacсination_button_Click(object sender, RoutedEventArgs e)
        {
            string selectedVaccination = vaccinations_comboBox.Text;
            DateTime selectedDate = (DateTime)employeeVaccinationDate_datePicker.SelectedDate;

            calcEmployees.Clear();

            foreach(var employee in employees)
            {
                if(!employee.Vaccinations.Any(v => v.Name == selectedVaccination && DateTime.Parse(v.Date) <= selectedDate))
                {
                    calcEmployees.Add(employee);
                }
            }

            calcDataGridLabel_label.Content = $"Сотрудники без прививки \"{selectedVaccination}\"";

            if (calcEmployees.Count == 0)
            {
                MessageBox.Show($"Сотрудники без прививки \"{selectedVaccination}\" не обнаружены.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void calcEmployees_dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Employee_ViewModel selectedEmployee = (Employee_ViewModel)calcEmployees_dataGrid.SelectedItem;
            if (selectedEmployee != null)
            {
                MoreEmployeeInfoPage infoPage = _serviceProvider.GetRequiredService<MoreEmployeeInfoPage>();
                infoPage.selectedEmployee = selectedEmployee;
                infoPage.currentUser = currentUser;
                infoPage.Owner = this;
                _shaderEffectsService.ApplyBlurEffect(this, 20);

                infoPage.ShowDialog();

                if (isNowNextEmployee || isNowPrevEmployee)
                {
                    int index = calcEmployees_dataGrid.SelectedIndex;

                    if (isNowNextEmployee)
                    {
                        isNowNextEmployee = false;
                        index++;
                    }
                    else if (isNowPrevEmployee)
                    {
                        isNowPrevEmployee = false;
                        index--;
                    }

                    if (index != -1)
                    {
                        calcEmployees_dataGrid.SelectedIndex = index;
                        calcEmployees_dataGrid.ScrollIntoView((Employee_ViewModel)employees_dataGrid.SelectedItem);
                    }
                    else
                    {
                        calcEmployees_dataGrid.SelectedIndex = 0;
                    }

                    calcEmployees_dataGrid_MouseDoubleClick(sender, e);
                }


                _shaderEffectsService.ClearEffect(this);
            }
        }

        private void addVaccination_button_Click(object sender, RoutedEventArgs e)
        {
            if(calcEmployees.Count > 0)
            {
                RefactoredEmployeesPage refactoredEmployeesPage = _serviceProvider.GetRequiredService<RefactoredEmployeesPage>();
                refactoredEmployeesPage.employees = calcEmployees.ToList();
                refactoredEmployeesPage.vaccinationName = "";
                refactoredEmployeesPage.vaccinationDate = DateTime.Now;

                _shaderEffectsService.ApplyBlurEffect(this, 20);

                refactoredEmployeesPage.ShowDialog();

                _shaderEffectsService.ClearEffect(this);
            }
            else
            {
                MessageBox.Show("Нет сотрудников для добавления прививок.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void deleteSelectedCalcEmployee_button_Click(object sender, RoutedEventArgs e)
        {
            Employee_ViewModel selectedEmployee = (Employee_ViewModel)calcEmployees_dataGrid.SelectedItem;
            if(selectedEmployee != null)
            {
                calcEmployees.Remove(selectedEmployee);
            }
        }

        private void deleteDate_button_Click(object sender, RoutedEventArgs e)
        {
            startDate_datePicker.SelectedDate = null;
            endDate_datePicker.SelectedDate = DateTime.Now;

            dateLabel_label.Visibility = Visibility.Visible;
            deleteDate_button.Visibility = Visibility.Collapsed;
        }

        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dateLabel_label.Visibility = Visibility.Collapsed;
            deleteDate_button.Visibility = Visibility.Visible;
        }

        private void findMaxCountEmployeesDisease_button_Click(object sender, RoutedEventArgs e)
        {
            DateTime startDate;
            if (startDate_datePicker.SelectedDate != null)
                startDate = (DateTime)startDate_datePicker.SelectedDate;
            else
                startDate = DateTime.Parse("01.01.2024");

            DateTime endDate;
            if (endDate_datePicker.SelectedDate != null)
                endDate = (DateTime)endDate_datePicker.SelectedDate;
            else
                endDate = DateTime.Now;


            List<Disease> diseases = _diseaseProcessingService.Get_AllItems();

            var mostCommonDisease = diseases
                .Where(d => d.StardDiseaseDate <= endDate && d.EndDiseaseDate >= startDate)
                .GroupBy(d => d.Name)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .FirstOrDefault();

            MessageBox.Show($"Заболевание с максимальным количеством\nзарегестрированных случаев ({mostCommonDisease.Count}) - {mostCommonDisease.Name}");

            calcEmployees.Clear();

            calcDataGridLabel_label.Content = $"Сотрудники с заболеванием \"{mostCommonDisease.Name}\"";

            foreach (var employee in employees)
            {
                if(employee.Diseases.Any(d => d.Name == mostCommonDisease.Name))
                {
                    calcEmployees.Add(employee);
                }
            }
        }

        private void fastSearch_button_Click(object sender, RoutedEventArgs e)
        {
            FastSearchPage fastSearchPage = _serviceProvider.GetRequiredService<FastSearchPage>();
            fastSearchPage.isDbSearch = true;
            fastSearchPage.Owner = this;
            fastSearchPage.window = this;
            Keyboard.ClearFocus();
            fastSearchPage.ShowDialog();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.F)
                {
                    if (selectDbTabItem < 2)
                        fastSearch_button_Click(sender, e);
                    else
                        fastCalcSearch_button_Click(sender, e);
                }
                else if (e.Key == Key.Tab)
                {
                    e.Handled = true;
                    NextTabItem();
                }
            }
        }

        private void NextTabItem()
        {
            if(selectDbTabItem < 2)
                selectDbTabItem++;
            else
                selectDbTabItem = 0;

            MyTabControl.SelectedIndex = selectDbTabItem;
        }

        private void settings_menuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsPage settingsPage = _serviceProvider.GetRequiredService<SettingsPage>();

            _shaderEffectsService.ApplyBlurEffect(this, 20);

            settingsPage.Owner = this;
            settingsPage.ShowDialog();

            _shaderEffectsService.ClearEffect(this);
        }

        private void fastCalcSearch_button_Click(object sender, RoutedEventArgs e)
        {
            FastSearchPage fastSearchPage = _serviceProvider.GetRequiredService<FastSearchPage>();
            fastSearchPage.isDbSearch = false;
            fastSearchPage.Owner = this;
            fastSearchPage.window = this;
            Keyboard.ClearFocus();
            fastSearchPage.ShowDialog();
        }

        private void exportEmployeesToExcel_button_Click(object sender, RoutedEventArgs e)
        {
            if (calcEmployees.Count > 0)
                _excelWorkerService.exportEmployeesListInfo(calcEmployees, calcDataGridLabel_label.Content.ToString());
            else
                MessageBox.Show("Невозможно выполнить экспорт из-за отсутствия сотрудников.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        
    }
}