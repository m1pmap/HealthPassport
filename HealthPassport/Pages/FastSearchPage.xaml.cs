using HealthPassport.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace HealthPassport.Pages
{
    /// <summary>
    /// Логика взаимодействия для FastSearchPage.xaml
    /// </summary>
    public partial class FastSearchPage : Window
    {
        public bool isDbSearch = false;
        public MainWindow window;

        private DataGrid selectedDataGrid;
        private ObservableCollection<Employee_ViewModel> employees;
        public FastSearchPage()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (isDbSearch)
            {
                selectedDataGrid = window.employees_dataGrid;
                employees = window.employees;
            }
            else
            {
                selectedDataGrid = window.calcEmployees_dataGrid;
                employees = window.calcEmployees;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search_button_Click(sender, e);
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        int lastIndex = 0;

        bool isFIOLast = true;
        bool isJobLast = false;
        bool isEducationLast = false;
        bool isBirthdayLast = false;
        bool isFamilyStatusLast = false;

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox.IsChecked == true)
            {
                int columnIndex = 0;
                lastIndex = selectedDataGrid.SelectedIndex + 1;
                var currentCell = selectedDataGrid.CurrentCell;
                if (currentCell.Column != null)
                {
                    columnIndex = currentCell.Column.DisplayIndex;
                }
                if (columnIndex == 1)
                {
                    isFIOLast = false;
                    isJobLast = true;
                    isEducationLast = false;
                    isBirthdayLast = false;
                    isFamilyStatusLast = false;
                }
                else if (columnIndex == 2 && isDbSearch)
                {
                    isFIOLast = false;
                    isJobLast = false;
                    isEducationLast = true;
                    isBirthdayLast = false;
                    isFamilyStatusLast = false;
                }
                else if (columnIndex == 3 && isDbSearch)
                {
                    isFIOLast = false;
                    isJobLast = false;
                    isEducationLast = false;
                    isBirthdayLast = true;
                    isFamilyStatusLast = false;
                }
                else if (columnIndex == 4 && isDbSearch)
                {
                    isFIOLast = false;
                    isJobLast = false;
                    isEducationLast = false;
                    isBirthdayLast = false;
                    isFamilyStatusLast = true;
                }
                else 
                {
                    isFIOLast = true;
                    isJobLast = false;
                    isEducationLast = false;
                    isBirthdayLast = false;
                    isFamilyStatusLast = false;
                }
            }

            if (checkBox.IsChecked == false)
            {
                isFIOLast = true;
                isJobLast = false;
                isEducationLast = false;
                isBirthdayLast = false;
                isFamilyStatusLast = false;

                lastIndex = 0;
            }
        }

        private void searchingText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (checkBox.IsChecked == true)
            {
                int columnIndex = 0;
                lastIndex = selectedDataGrid.SelectedIndex + 1;
                var currentCell = selectedDataGrid.CurrentCell;
                if (currentCell.Column != null)
                {
                    columnIndex = currentCell.Column.DisplayIndex;
                }
                if (columnIndex == 1)
                {
                    isFIOLast = false;
                    isJobLast = true;
                    isEducationLast = false;
                    isBirthdayLast = false;
                    isFamilyStatusLast = false;
                }
                else if (columnIndex == 2 && isDbSearch)
                {
                    isFIOLast = false;
                    isJobLast = false;
                    isEducationLast = true;
                    isBirthdayLast = false;
                    isFamilyStatusLast = false;
                }
                else if (columnIndex == 3 && isDbSearch)
                {
                    isFIOLast = false;
                    isJobLast = false;
                    isEducationLast = false;
                    isBirthdayLast = true;
                    isFamilyStatusLast = false;
                }
                else if (columnIndex == 4 && isDbSearch)
                {
                    isFIOLast = false;
                    isJobLast = false;
                    isEducationLast = false;
                    isBirthdayLast = false;
                    isFamilyStatusLast = true;
                }
                else
                {
                    isFIOLast = true;
                    isJobLast = false;
                    isEducationLast = false;
                    isBirthdayLast = false;
                    isFamilyStatusLast = false;
                }
            }

            if (checkBox.IsChecked == false)
            {
                isFIOLast = true;
                isJobLast = false;
                isEducationLast = false;
                isBirthdayLast = false;
                isFamilyStatusLast = false;

                lastIndex = 0;
            }

            Search_button.Content = "Поиск";
        }

        private void Search_button_Click(object sender, RoutedEventArgs e)
        {
            Search_button.Content = "Перейти к следующему";
            string pattern = Regex.Escape(searchingText.Text.ToString());

            if (string.IsNullOrWhiteSpace(pattern))
            {
                return;
            }

            if (isFIOLast)
            {
                if (SearchInColumn(item => item.FIO, pattern, 0))
                {
                    checkBox.IsChecked = true;
                    return;
                }
                

                isFIOLast = false;
                lastIndex = 0;
                isJobLast = true;
            }

            if (isJobLast)
            {
                if (SearchInColumn(item => item.Job, pattern, 1))
                {
                    checkBox.IsChecked = true;
                    return;
                }

                isJobLast = false;
                lastIndex = 0;

                if (!isDbSearch)
                {
                    isFIOLast = true;
                    Search_button.Content = "Начать поиск сначала";
                    MessageBox.Show("Поиск окончен. Совпадений больше не обнаружено");
                }
                else
                    isEducationLast = true;

            }

            if (isEducationLast && isDbSearch)
            {
                if (SearchInColumn(item => item.Education, pattern, 2))
                {
                    checkBox.IsChecked = true;
                    return;
                }

                isEducationLast = false;
                lastIndex = 0;
                isBirthdayLast = true;
            }

            if (isBirthdayLast && isDbSearch)
            {
                if (SearchInColumn(item => item.Birthday, pattern, 3))
                {
                    checkBox.IsChecked = true;
                    return;
                }

                isBirthdayLast = false;
                lastIndex = 0;
                isFamilyStatusLast = true;

                
            }

            if (isFamilyStatusLast && isDbSearch)
            {
                if (SearchInColumn(item => item.FamilyStatus, pattern, 4))
                {
                    checkBox.IsChecked = true;
                    return;
                }

                isFamilyStatusLast = false;
                lastIndex = 0;
                isFIOLast = true;

                Search_button.Content = "Начать поиск сначала";
                MessageBox.Show("Поиск окончен. Совпадений больше не обнаружено");
            }
        }



        private bool SearchInColumn(Func<Employee_ViewModel, string> selector, string pattern, int columnIndex)
        {
            for (int i = lastIndex; i < employees.Count; i++)
            {
                Employee_ViewModel item = employees[i];
                FocusManager.SetFocusedElement(window, null);

                if (Regex.IsMatch(selector(item), pattern, RegexOptions.IgnoreCase))
                {
                    selectedDataGrid.SelectedItem = item;
                    selectedDataGrid.ScrollIntoView(item);
                    lastIndex = i + 1;

                    selectedDataGrid.UpdateLayout();

                    var row = selectedDataGrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;

                    if (row != null)
                    {
                        var cell = selectedDataGrid.Columns[columnIndex].GetCellContent(row).Parent as DataGridCell;
                        if (cell != null)
                        {
                            FocusManager.SetFocusedElement(selectedDataGrid, cell);
                            cell.Focus();
                        }
                    }

                    window.userFIO_label.Focus();
                    return true;
                }
            }

            return false;
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (checkBox.IsChecked == false)
            {
                isFIOLast = true;
                isJobLast = false;
                isEducationLast = false;
                isBirthdayLast = false;
                isFamilyStatusLast = false;

                lastIndex = 0;
            }

            Search_button.Content = "Поиск";
        }
    }
}
