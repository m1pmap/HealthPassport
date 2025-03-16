using HealthPassport.BLL.Interfaces;
using HealthPassport.BLL.Models;
using HealthPassport.DAL.Models;
using HealthPassport.Interfaces;
using HealthPassport.Models;
using HealthPassport.Pages;
using HealthPassport.Services;
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
        private readonly IJobProcessing _jobProcessingService;

        //Коллекция для хранения всех сотрудников
        private ObservableCollection<Employee_ViewModel> employees = new ObservableCollection<Employee_ViewModel>();

        //Настройки и ифнормация текущего пользователяп 
        public SettingsParameters settings = new SettingsParameters();

        private bool isNowEmployeeAdding = false;
        private bool isNowExit = false;
        int oldCurrentProductIndex = 1; //Прошлый выбранный элемент в базе данных



        public MainWindow(ByteArrayToImageSourceConverter_Service imageSourceConverter, 
            IEmployeeProcessing employeeProcessingService,
            IImageUpdater imageUpdater,
            IFileWorker fileWorker,
            IServiceProvider serviceProvider,
            IShaderEffects shaderEffectsService,
            IJobProcessing jobProcessingService)
        {
            InitializeComponent();

            //Инициализация сервисов
            _imageSourceConverter = imageSourceConverter;
            _employeeProcessingService = employeeProcessingService;
            _imageUpdater = imageUpdater;
            _fileWorker = fileWorker;
            _serviceProvider = serviceProvider;
            _jobProcessingService = jobProcessingService;

            //Начальное заполнение данными
            List<Employee> dbEmployees = _employeeProcessingService.Get_AllEmployees();
            foreach (Employee employee in dbEmployees)
            {
                Employee_ViewModel emploeyeVM = (Employee_ViewModel)_employeeProcessingService.Connect_EmployeeInformation(employee);
                employees.Add(emploeyeVM);
            }

            employees_dataGrid.ItemsSource = employees;
            employeesCount_label.Content = $"из {employees.Count}";
            _serviceProvider = serviceProvider;
            _shaderEffectsService = shaderEffectsService;

            NewJob_comboBox.ItemsSource = _jobProcessingService.GetAllJobTypes();
            NewSubunit_comboBox.ItemsSource = _jobProcessingService.GetAllSubunits();
        }

        private void addGrid_Button_Click(object sender, RoutedEventArgs e)
        {
            addGrid.Visibility = Visibility.Visible;
            searchGrid.Visibility = Visibility.Hidden;
            searchGrid_Button.Background = new SolidColorBrush(Colors.LightGray);
            addGrid_Button.Background = new SolidColorBrush(Colors.MediumSeaGreen);
            isNowEmployeeAdding = true;
        }

        private void searchGrid_Button_Click(object sender, RoutedEventArgs e)
        {
            searchGrid.Visibility = Visibility.Visible;
            addGrid.Visibility = Visibility.Hidden;
            searchGrid_Button.Background = new SolidColorBrush(Colors.Coral);
            addGrid_Button.Background = new SolidColorBrush(Colors.LightGray);
            isNowEmployeeAdding = false;
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

                if (_employeeProcessingService.Add_Employee(newEmployee))
                {
                    Job newJob = new Job
                    {
                        WorkingRate = 0,
                        StartWorkingDate = DateTime.Now,
                        EndWorkingDate = DateTime.MinValue,
                        EmployeeId = newEmployee.EmployeeId,
                        JobTypeId = _jobProcessingService.Get_JobTypeIdByName(NewJob_comboBox.Text),
                        SubunitId = _jobProcessingService.Get_SubunitIdByName(NewSubunit_comboBox.Text),
                    };

                    _jobProcessingService.Add_Job(newJob);

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
                _employeeProcessingService.Delete_Employee(selectedEmployee.EmployeeId);
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
                if (isNowEmployeeAdding)
                    searchGrid_Button_Click(sender, new RoutedEventArgs());
                else searchGrid_Button_Click(sender, new RoutedEventArgs());

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

        private void employees_dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Employee_ViewModel selectedEmployee = (Employee_ViewModel)employees_dataGrid.SelectedItem;
            if (selectedEmployee != null)
            {
                MoreEmployeeInfoPage infoPage = _serviceProvider.GetRequiredService<MoreEmployeeInfoPage>();
                infoPage.selectedEmployee = selectedEmployee;
                infoPage.Owner = this;
                _shaderEffectsService.ApplyBlurEffect(this, 20);

                infoPage.ShowDialog();

                int index = employees.IndexOf(selectedEmployee);
                employees_dataGrid.SelectedItem = null;
                employees_dataGrid.SelectedIndex = index;

                _shaderEffectsService.ClearEffect(this);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!isNowExit) { _fileWorker.WriteJson(settings, "settings.json"); }
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
    }
}