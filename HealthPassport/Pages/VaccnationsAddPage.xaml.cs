using DocumentFormat.OpenXml.Drawing;
using HealthPassport.BLL.Interfaces;
using HealthPassport.DAL.Models;
using HealthPassport.Interfaces;
using HealthPassport.Models;
using HealthPassport.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для VaccnationsAddPage.xaml
    /// </summary>
    public partial class VaccnationsAddPage : Window
    {
        public ObservableCollection<Employee_ViewModel> employees;

        private readonly ISubunitProcessing _subunitProcessingService;
        private readonly ICudProcessing<Vaccination> _vaccinationProcessingService;
        private readonly IServiceProvider _serviceProvider;

        public VaccnationsAddPage(ISubunitProcessing subunitProcessingService,
            ICudProcessing<Vaccination> vaccinationProcessingService,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _subunitProcessingService = subunitProcessingService;
            _vaccinationProcessingService = vaccinationProcessingService;
            _serviceProvider = serviceProvider;
        }

        private void closeWindow_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            employees_comboBox.ItemsSource = employees;

            subunits_comboBox.ItemsSource = _subunitProcessingService.Get_AllItems();
            vaccinationDate_datePicker.SelectedDate = DateTime.Now;
        }

        private void addVaccination_button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(vaccinatioName_textBox.Text))
            {
                TabItem selectedTabItem = (TabItem)Type_TabControl.SelectedItem;
                switch (selectedTabItem.Header.ToString())
                {
                    case "Определённый сотрудник":
                        {
                            Employee_ViewModel selectedEmployee = (Employee_ViewModel)employees_comboBox.SelectedItem;
                            if(selectedEmployee != null)
                            {
                                Vaccination newVaccination = new Vaccination
                                {
                                    EmployeeId = selectedEmployee.EmployeeId,
                                    Name = vaccinatioName_textBox.Text,
                                    Date = (DateTime)vaccinationDate_datePicker.SelectedDate
                                };

                                _vaccinationProcessingService.Add_Item(newVaccination);
                                selectedEmployee.Vaccinations.Add((Vaccination_ViewModel)newVaccination);

                                MessageBox.Show("Прививка выбранному сотруднику успешно добавлена.");
                            }
                            else
                            {
                                MessageBox.Show("Сотрудник с указанным ФИО не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            break;
                        }
                    case "По алфавиту":
                        {
                            var filteredEmployees = employees.Where(emp =>
                            {
                                string lastName = emp.FIO.Split(' ')[0].ToUpper();
                                string start = start_textBox.Text.ToUpper();
                                string end = end_textBox.Text.ToUpper() + char.MaxValue;

                                return string.Compare(lastName, start, StringComparison.OrdinalIgnoreCase) >= 0 &&
                                       string.Compare(lastName, end, StringComparison.OrdinalIgnoreCase) <= 0;
                            }).OrderBy(e => e.FIO).ToList();

                            if(filteredEmployees.Count > 0)
                            {
                                RefactoredEmployeesPage refactoredEmployeesPage = _serviceProvider.GetRequiredService<RefactoredEmployeesPage>();
                                refactoredEmployeesPage.Owner = this;

                                refactoredEmployeesPage.employees = filteredEmployees;
                                refactoredEmployeesPage.vaccinationName = vaccinatioName_textBox.Text;
                                refactoredEmployeesPage.vaccinationDate = (DateTime)vaccinationDate_datePicker.SelectedDate;

                                refactoredEmployeesPage.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("Сотрудники не найдены.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            break;
                        }
                    case "Подразделение":
                        {
                            var filteredEmployees = employees.Where(e => e.Jobs.Any() && e.Jobs.Last().Subunit == subunits_comboBox.Text).OrderBy(e => e.FIO).ToList();

                            if(filteredEmployees.Count > 0)
                            {
                                RefactoredEmployeesPage refactoredEmployeesPage = _serviceProvider.GetRequiredService<RefactoredEmployeesPage>();
                                refactoredEmployeesPage.Owner = this;

                                refactoredEmployeesPage.employees = filteredEmployees;
                                refactoredEmployeesPage.vaccinationName = vaccinatioName_textBox.Text;
                                refactoredEmployeesPage.vaccinationDate = (DateTime)vaccinationDate_datePicker.SelectedDate;

                                refactoredEmployeesPage.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("Сотрудники не найдены.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                            }

                            break;
                        }
                }
            }
            else
            {
                MessageBox.Show("Не заполнено наименование прививки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
