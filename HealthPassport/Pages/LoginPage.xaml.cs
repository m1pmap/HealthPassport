using Dahmira.Pages;
using HealthPassport.BLL.Interfaces;
using HealthPassport.BLL.Models;
using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using HealthPassport.Interfaces;
using HealthPassport.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Mail;
using System.Windows;


namespace HealthPassport.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        private readonly ByteArrayToImageSourceConverter_Service _imageSourceConverter;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEmployeeProcessing _employeeProcessingService;
        private readonly IFileWorker _fileWorker;
        private readonly IMailSender _mailSender;
        private readonly IShaderEffects _shaderEffectsService;
        private readonly IJobProcessing _jobProcessingService;


        bool isFormClosingNow = false;

        public LoginPage(ByteArrayToImageSourceConverter_Service imageSourceConverter,
            IMailSender mailSender, 
            IServiceProvider serviceProvider,
            IEmployeeProcessing employeeProcessingService,
            IFileWorker fileWorker,
            IShaderEffects shaderEffectsService,
            IJobProcessing jobProcessingService)
        {
            InitializeComponent();

            //Инициализация сервисов
            _mailSender = mailSender;
            _serviceProvider = serviceProvider;
            _employeeProcessingService = employeeProcessingService;
            _imageSourceConverter = imageSourceConverter;
            _fileWorker = fileWorker;
            _shaderEffectsService = shaderEffectsService;
            _jobProcessingService = jobProcessingService;

            //Функционал

            Job_comboBox.ItemsSource = _jobProcessingService.GetAllJobTypes();
            Subunit_comboBox.ItemsSource = _jobProcessingService.GetAllSubunits();

            if (File.Exists("settings.json"))
            {
                //MessageBox.Show("!");
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                SettingsParameters settings = _fileWorker.ReadJson<SettingsParameters>("settings.json");
                mainWindow.settings = settings;
                mainWindow.Show();
                isFormClosingNow = true;
            }
        }

        private void Close_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SignUp_button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SignUpMail_textBox.Text))
            {
                Employee employee = _employeeProcessingService.CheckEmployeeInDb(SignUpMail_textBox.Text);
                if(employee != null)
                {
                    MessageBox.Show("Пользователь с указанной почтой уже зарегестрирован. Выполните вход, используя её или измените регистрируемую почту.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(Name_textBox.Text) ||
                        string.IsNullOrWhiteSpace(Surname_textBox.Text) ||
                        string.IsNullOrWhiteSpace(SecondName_textBox.Text) ||
                        string.IsNullOrWhiteSpace(Job_comboBox.Text))
                    {
                        MessageBox.Show("Пользователь заполнил не все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        string mailTo = SignUpMail_textBox.Text;
                        if (!_mailSender.IsValidEmail(mailTo))
                        {
                            MessageBox.Show("Введён неверный формат почты.");
                            return;
                        }
                        else
                        {
                            string subject = "Регистрация";
                            string code = _mailSender.GenerateAlphaNumericCode(6);
                            string text = $"Ваш код подтверждения: {code}";

                            try
                            {
                                if (_mailSender.SendEmailCode(mailTo, subject, text))
                                {
                                    var writeMailCodePage = _serviceProvider.GetRequiredService<WriteMailCodePage>();
                                    writeMailCodePage.Owner = this;
                                    writeMailCodePage.code = code;
                                    writeMailCodePage.loginPage = this;

                                    Employee registerEmployee = new Employee
                                    {
                                        FIO = $"{Surname_textBox.Text} {Name_textBox.Text} {SecondName_textBox.Text}",
                                        Birthday = (DateTime)Birthday_datePicker.SelectedDate,
                                        MailAdress = SignUpMail_textBox.Text,
                                        Photo = _imageSourceConverter.ConvertFromFileImageToByteArray("without_image_database.png"),
                                    };

                                    Job employeeJob = new Job
                                    {
                                         SubunitId = _jobProcessingService.Get_SubunitIdByName(Subunit_comboBox.Text),
                                         JobTypeId = _jobProcessingService.Get_JobTypeIdByName(Job_comboBox.Text),
                                         StartWorkingDate = DateTime.Now,
                                         EndWorkingDate = DateTime.MinValue,
                                         WorkingRate = 805
                                    };

                                    writeMailCodePage.employee = registerEmployee;
                                    writeMailCodePage.employeeJob = employeeJob;
                                    writeMailCodePage.isRegistration = true;
                                    _shaderEffectsService.ApplyBlurEffect(this, 10);

                                    writeMailCodePage.ShowDialog();

                                    _shaderEffectsService.ClearEffect(this);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
            }
        }

        private void SignIn_button_Click(object sender, RoutedEventArgs e)
        {
            string mailTo = SignInMail_textBox.Text;
            
            if(mailTo != "") 
            {
                if (string.IsNullOrWhiteSpace(mailTo))
                {
                    MessageBox.Show("Пользователь не указал почту для входа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (!_mailSender.IsValidEmail(mailTo))
                    {
                        MessageBox.Show("Введён неверный формат почты.");
                        return;
                    }
                    else
                    {
                        Employee employee = _employeeProcessingService.CheckEmployeeInDb(mailTo);
                        if (employee != null)
                        {
                            string subject = "Вход";
                            string code = _mailSender.GenerateAlphaNumericCode(6);
                            string text = $"Ваш код подтверждения: {code}";

                            try
                            {
                                if (_mailSender.SendEmailCode(mailTo, subject, text))
                                {
                                    var writeMailCodePage = _serviceProvider.GetRequiredService<WriteMailCodePage>();
                                    writeMailCodePage.Owner = this;
                                    writeMailCodePage.code = code;
                                    writeMailCodePage.isRegistration = false;
                                    writeMailCodePage.employee = employee;
                                    writeMailCodePage.loginPage = this;
                                    _shaderEffectsService.ApplyBlurEffect(this, 10);

                                    writeMailCodePage.ShowDialog();
                                    _shaderEffectsService.ClearEffect(this);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Пользователь с указанной почтой не зарегестрирован. Выполните регистрацию в системе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else 
            {
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();

                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (isFormClosingNow)
            {
                Close();
            }
        }
    }
}
