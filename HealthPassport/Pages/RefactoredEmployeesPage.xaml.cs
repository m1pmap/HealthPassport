using DocumentFormat.OpenXml.Spreadsheet;
using HealthPassport.BLL.Interfaces;
using HealthPassport.DAL.Models;
using HealthPassport.Interfaces;
using HealthPassport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HealthPassport.Pages
{
    /// <summary>
    /// Логика взаимодействия для RefactoredEmployeesPage.xaml
    /// </summary>
    public partial class RefactoredEmployeesPage : Window
    {
        public List<Employee_ViewModel> employees;
        public string vaccinationName;
        public DateTime vaccinationDate;

        private readonly ICudProcessing<Vaccination> _vaccinationProcessingService;

        public RefactoredEmployeesPage(ICudProcessing<Vaccination> vaccinationProcessingService)
        {
            InitializeComponent();
            _vaccinationProcessingService = vaccinationProcessingService;
        }

        private void closeWindow_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        bool isSearchAgain = false;
        private void search_button_Click(object sender, RoutedEventArgs e)
        {
            string searchText = search_textBox.Text;

            int index = employees_dataGrid.SelectedIndex + 1;
            if (isSearchAgain)
            {
                index = 0;
                isSearchAgain = false;
            }

            for (int i = index; i < employees.Count; i++)
            {
                if (employees[i].FIO.Contains(searchText))
                {
                    employees_dataGrid.SelectedIndex = i;
                    return;
                }
            }

            isSearchAgain = true;
            MessageBox.Show("Совпадения не найдены.");
        }

        private void addVaccinations_button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(vaccinationName_textBox.Text))
            {
                MessageBox.Show("Введите наименование прививки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (var employee in employees)
            {
                Vaccination newVaccination = new Vaccination
                {
                    EmployeeId = employee.EmployeeId,
                    Name = vaccinationName_textBox.Text,
                    Date = vaccinationDate
                };

                _vaccinationProcessingService.Add_Item(newVaccination);
                employee.Vaccinations.Insert(0, (Vaccination_ViewModel)newVaccination);
            }

            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            employees_dataGrid.ItemsSource = employees;
            vaccinationName_textBox.Text = vaccinationName;
        }

        private void deleteEmployee_button_Click(object sender, RoutedEventArgs e)
        {
            Employee_ViewModel selectedEmployee = (Employee_ViewModel)employees_dataGrid.SelectedItem;

            employees.Remove(selectedEmployee);
            employees_dataGrid.Items.Refresh();
        }

        private void search_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = search_textBox.Text;

            int index = employees.IndexOf(employees.FirstOrDefault(e => e.FIO.Contains(searchText)));
            employees_dataGrid.SelectedIndex = index;
        }
    }
}
