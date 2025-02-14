using Autofac;
using HealthPassport;
using HealthPassport.BLL.Interfaces;
using HealthPassport.BLL.Models;
using HealthPassport.BLL.Services;
using HealthPassport.DAL.Models;
using HealthPassport.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
using static System.Formats.Asn1.AsnWriter;

namespace Dahmira.Pages
{
    /// <summary>
    /// Логика взаимодействия для WriteMailCodePage.xaml
    /// </summary>
    public partial class WriteMailCodePage : Window
    {
        private List<TextBox> allTextBoxes;
        private int textBoxIndex = 0;
        public string code;
        public Employee employee;
        public bool isRegistration;

        public LoginPage loginPage;

        private readonly IMailSender _mailSender;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEmployeeProcessing _employeeProcessingService;

        public WriteMailCodePage(IMailSender mailSender, IServiceProvider serviceProvider, IEmployeeProcessing employeeProcessingService)
        {
            InitializeComponent();

            _mailSender = mailSender;
            _serviceProvider = serviceProvider;
            _employeeProcessingService = employeeProcessingService;

            allTextBoxes = new List<TextBox> { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6 };
            allTextBoxes[0].Focus();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[A-Z0-9]$");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox currentTextBox = sender as TextBox;
            if (currentTextBox != null && currentTextBox.Text.Length == 1)
            {
                if(textBoxIndex != 5)
                {
                    allTextBoxes[textBoxIndex + 1].Focus();
                    textBoxIndex++;
                }
                else
                {
                    bool res = checkCorrectCode(code);
                    if (res)
                    {
                        if (isRegistration)
                        {
                            _employeeProcessingService.Add_Employee(employee);
                        }
                        else
                        {
                            //
                        }
                        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                        mainWindow.settings = new SettingsParameters { employeeMailAdress = employee.MailAdress, employeeFIO = employee.FIO };
                        mainWindow.Show();
                        loginPage.Close();
                        Close();
                    }
                    else
                    {
                        foreach (TextBox textBox in allTextBoxes)
                        {
                            textBox.BorderBrush = new SolidColorBrush(Colors.OrangeRed);
                            textBox.Text = string.Empty;
                        }
                        takeCodeAgain.Focus();
                    }
                    textBoxIndex = 0;
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox currentTextBox = sender as TextBox;
            currentTextBox.BorderBrush = new SolidColorBrush(Colors.Gray);
            currentTextBox.Text = string.Empty;
        }

        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBox currentTextBox = sender as TextBox;
            textBoxIndex = allTextBoxes.IndexOf(currentTextBox);
        }

        private bool checkCorrectCode(string code)
        {
            string textBoxesCode = string.Join("", allTextBoxes.Select(tb => tb.Text.Trim()));

            return code.Equals(textBoxesCode, StringComparison.OrdinalIgnoreCase);
        }

        private void takeCodeAgain_Click(object sender, RoutedEventArgs e)
        {
            string mailFrom = "dok_koks@mail.ru";
            string pass = "TKZB28r34gSTNVmY7DeW";
            string subject = "Подтверждение почты";
            code = _mailSender.GenerateAlphaNumericCode(6);
            string text = $"Ваш повторный код для подтверждения: {code}";


            if (_mailSender.SendEmailCode(employee.MailAdress, subject, text))
            {
                MessageBox.Show("Код успешно отправлен на почту.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if(e.Key == Key.V)
                {
                    //MessageBox.Show("1");
                }
            }
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
