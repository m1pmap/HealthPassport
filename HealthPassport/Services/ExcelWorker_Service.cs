using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using HealthPassport.Interfaces;
using HealthPassport.Models;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace HealthPassport.Services
{
    public class ExcelWorker_Service : IExportWorker
    {
        private readonly ByteArrayToImageSourceConverter_Service _imageConverter;
        public ExcelWorker_Service(ByteArrayToImageSourceConverter_Service imageConverter)
        {
            _imageConverter = imageConverter;
        }
        public void exportEmployeeInfo(Employee_ViewModel employee)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                var package = new ExcelPackage(); //Создание нового документа
                var worksheet = package.Workbook.Worksheets.Add("Медкарта сотрудника");

                //Область с фамилией сотрудника
                ExcelRange EmployeeFIORange = worksheet.Cells[1, 1, 1, 6];
                SetRangeStyles(EmployeeFIORange, System.Drawing.Color.MediumSeaGreen, System.Drawing.Color.White, System.Drawing.Color.MediumSeaGreen, "Arial Black", true);
                worksheet.Cells[1, 1].Value = employee.FIO;

                //Область с фото сотрудника
                ExcelRange EmployeePhotoRange = worksheet.Cells[2, 1, 8, 2];
                SetRangeStyles(EmployeePhotoRange, System.Drawing.Color.White, System.Drawing.Color.Gray, System.Drawing.Color.Gray, isMerge: true);

                //Добавление изображения в Excel
                BitmapImage bitmapImage = (BitmapImage)_imageConverter.Convert(employee.Photo, typeof(BitmapImage), null, CultureInfo.CurrentCulture);
                using (var memoryStream = new MemoryStream())
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                    encoder.Save(memoryStream);

                    memoryStream.Position = 0;
                    var excelImage = worksheet.Drawings.AddPicture("employeePhoto", memoryStream);
                    excelImage.SetPosition(1, 5, 0, 5);
                    excelImage.SetSize(128, 132);
                }

                //Стили области основных данных сотрудника
                ExcelRange MainEmployeeData = worksheet.Cells[2, 3, 8, 6];
                SetRangeStyles(MainEmployeeData, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, hAligment: ExcelHorizontalAlignment.Left);

                for (int i = 2; i < 9; i++)
                {
                    ExcelRange MainEmployeeDataValueRange = worksheet.Cells[i, 4, i, 6];
                    MainEmployeeDataValueRange.Merge = true;

                    switch (i)
                    {
                        case 2:
                            {
                                worksheet.Cells[i, 3].Value = "Дата рождения:";
                                worksheet.Cells[i, 4].Value = employee.Birthday;
                                break;
                            }
                        case 3:
                            {
                                worksheet.Cells[i, 3].Value = "Должность:";
                                if (employee.Jobs.Count > 0)
                                {
                                    worksheet.Cells[i, 4].Value = employee.Job;
                                }
                                else
                                {
                                    worksheet.Cells[i, 4].Value = "Не указано";
                                }
                                break;
                            }
                        case 4:
                            {
                                worksheet.Cells[i, 3].Value = "Подразделение:";
                                if (employee.Jobs.Count > 0)
                                {
                                    worksheet.Cells[i, 4].Value = employee.Jobs[0].Subunit;
                                }
                                else
                                {
                                    worksheet.Cells[i, 4].Value = "Не указано";
                                }
                                break;
                            }
                        case 5:
                            {
                                worksheet.Cells[i, 3].Value = "Последняя болезнь:";
                                if (employee.Diseases.Count > 0)
                                {
                                    worksheet.Cells[i, 4].Value = employee.Diseases[0].Name;
                                }
                                else
                                {
                                    worksheet.Cells[i, 4].Value = "Отсутствует";
                                }
                                break;
                            }
                        case 6:
                            {
                                worksheet.Cells[i, 3].Value = "Семейное положение:";
                                worksheet.Cells[i, 4].Value = employee.FamilyStatus;
                                break;
                            }
                        case 7:
                            {
                                worksheet.Cells[i, 3].Value = "Образование:";
                                worksheet.Cells[i, 4].Value = employee.Education;
                                break;
                            }
                        case 8:
                            {
                                worksheet.Cells[i, 3].Value = "Почта:";
                                worksheet.Cells[i, 4].Value = employee.MailAdress;
                                break;
                            }
                    }
                }

                int currentExcelRow = 8;
                //Отображение болезней сотрудника
                if (employee.Diseases.Count > 0)
                {
                    ExcelRange diseasesStartRange = worksheet.Cells[currentExcelRow + 2, 1, currentExcelRow + 2, 6];
                    SetRangeStyles(diseasesStartRange, System.Drawing.Color.FromArgb(255, 198, 239, 206), System.Drawing.Color.FromArgb(255, 0, 97, 0), System.Drawing.Color.FromArgb(255, 0, 97, 0), isMerge: true);
                    worksheet.Cells[currentExcelRow + 2, 1].Value = "Болезни";

                    currentExcelRow = CreateDiseasesExcelTable(worksheet, currentExcelRow + 3, employee);
                }

                //Отображение прививок сотрудника
                if (employee.Vaccinations.Count > 0)
                {
                    ExcelRange vaccinationsStartRange = worksheet.Cells[currentExcelRow + 2, 1, currentExcelRow + 2, 6];
                    SetRangeStyles(vaccinationsStartRange, System.Drawing.Color.FromArgb(255, 198, 239, 206), System.Drawing.Color.FromArgb(255, 0, 97, 0), System.Drawing.Color.FromArgb(255, 0, 97, 0), isMerge: true);
                    worksheet.Cells[currentExcelRow + 2, 1].Value = "Прививки";

                    currentExcelRow = CreateVaccinationsExcelTable(worksheet, currentExcelRow + 3, employee);
                }


                //Отображение атропологических исследований сотрудника
                if (employee.AntropologicalResearches.Count > 0)
                {
                    ExcelRange antropologicalResearchesStartRange = worksheet.Cells[currentExcelRow + 2, 1, currentExcelRow + 2, 6];
                    SetRangeStyles(antropologicalResearchesStartRange, System.Drawing.Color.FromArgb(255, 198, 239, 206), System.Drawing.Color.FromArgb(255, 0, 97, 0), System.Drawing.Color.FromArgb(255, 0, 97, 0), isMerge: true);
                    worksheet.Cells[currentExcelRow + 2, 1].Value = "Антропологические исследования";

                    currentExcelRow = CreateAntropologicalResearchesExcelTable(worksheet, currentExcelRow + 3, employee);
                }

                //Отображение образований сотрудника
                if (employee.Educations.Count > 0)
                {
                    ExcelRange educationsResearchesStartRange = worksheet.Cells[currentExcelRow + 2, 1, currentExcelRow + 2, 6];
                    SetRangeStyles(educationsResearchesStartRange, System.Drawing.Color.FromArgb(255, 198, 239, 206), System.Drawing.Color.FromArgb(255, 0, 97, 0), System.Drawing.Color.FromArgb(255, 0, 97, 0), isMerge: true);
                    worksheet.Cells[currentExcelRow + 2, 1].Value = "Образования";

                    currentExcelRow = CreateEducationsExcelTable(worksheet, currentExcelRow + 3, employee);
                }

                //Отображение должностей сотрудника
                if (employee.Jobs.Count > 0)
                {
                    ExcelRange jobsStartRange = worksheet.Cells[currentExcelRow + 2, 1, currentExcelRow + 2, 6];
                    SetRangeStyles(jobsStartRange, System.Drawing.Color.FromArgb(255, 198, 239, 206), System.Drawing.Color.FromArgb(255, 0, 97, 0), System.Drawing.Color.FromArgb(255, 0, 97, 0), isMerge: true);
                    worksheet.Cells[currentExcelRow + 2, 1].Value = "Должности";

                    currentExcelRow = CreateJobsExcelTable(worksheet, currentExcelRow + 3, employee);
                }

                //Отображение семейных статусов сотрудника
                if (employee.FamilyStatuses.Count > 0)
                {
                    ExcelRange familyStatusesStartRange = worksheet.Cells[currentExcelRow + 2, 1, currentExcelRow + 2, 6];
                    SetRangeStyles(familyStatusesStartRange, System.Drawing.Color.FromArgb(255, 198, 239, 206), System.Drawing.Color.FromArgb(255, 0, 97, 0), System.Drawing.Color.FromArgb(255, 0, 97, 0), isMerge: true);
                    worksheet.Cells[currentExcelRow + 2, 1].Value = "Семейные статусы";

                    currentExcelRow = CreateFamilyStatusesExcelTable(worksheet, currentExcelRow + 3, employee);
                }

                ExcelRange creatingDateRange = worksheet.Cells[currentExcelRow + 1, 1, currentExcelRow + 1, 6];
                SetRangeStyles(creatingDateRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);
                worksheet.Cells[currentExcelRow + 1, 1].Value = $"Сформировано: {DateTime.Now.ToString("dd.MM.yyyy")}";



                //Установка ширины столбцов 
                worksheet.Column(1).Width = 4.29;
                worksheet.Column(2).Width = 15.29;
                worksheet.Column(3).Width = 25.71;
                worksheet.Column(4).Width = 25.71;
                worksheet.Column(5).Width = 24.71;
                worksheet.Column(6).Width = 24.71;

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    Title = "Сохранить Excel документ"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    worksheet.Protection.IsProtected = false;
                    worksheet.Protection.AllowSelectLockedCells = false;
                    package.SaveAs(new FileInfo(filePath));
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Файл открыт в другой программе.", "Открытие невозможно", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void exportEmployeesListInfo(ObservableCollection<Employee_ViewModel> employees, string label)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                var package = new ExcelPackage(); //Создание нового документа
                var worksheet = package.Workbook.Worksheets.Add(label);

                ExcelRange diseasesStartRange = worksheet.Cells[1, 1, 1, 7];
                SetRangeStyles(diseasesStartRange, System.Drawing.Color.MediumSeaGreen, System.Drawing.Color.White, System.Drawing.Color.MediumSeaGreen, fontFamily: "Arial Black", isMerge: true);
                worksheet.Cells[1, 1].Value = label;

                ExcelRange diseasesHeaderRange = worksheet.Cells[2, 1, 2, 7];
                SetRangeStyles(diseasesHeaderRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray);

                worksheet.Cells[2, 1].Value = "№";
                worksheet.Cells[2, 2].Value = "ФИО";
                worksheet.Cells[2, 3].Value = "Должность";
                worksheet.Cells[2, 4].Value = "Образование";
                worksheet.Cells[2, 5].Value = "Дата рождения";
                worksheet.Cells[2, 6].Value = "Семейное положение";
                worksheet.Cells[2, 7].Value = "Фото";


                ExcelRange employeesRange = worksheet.Cells[3, 1, 3 + employees.Count - 1, 7];
                SetRangeStyles(employeesRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, hAligment: ExcelHorizontalAlignment.Center);

                for (int i = 3; i < 3 + employees.Count; i++)
                {
                    worksheet.Cells[i, 1].Value = i - 2;
                    worksheet.Cells[i, 2].Value = employees[i - 3].FIO;
                    worksheet.Cells[i, 3].Value = employees[i - 3].Job;
                    worksheet.Cells[i, 4].Value = employees[i - 3].Education;
                    worksheet.Cells[i, 5].Value = employees[i - 3].Birthday;
                    worksheet.Cells[i, 6].Value = employees[i - 3].FamilyStatus;

                    //ExcelRange photoRange = worksheet.Cells[i, 7];
                    //SetRangeStyles(photoRange, System.Drawing.Color.Transparent, System.Drawing.Color.Black, System.Drawing.Color.Gray);


                    //Конвертация картинки в Excel с заданной шириной и высотой
                    int width = 50;
                    int height = width / 2;

                    BitmapImage bitmapImage = (BitmapImage)_imageConverter.Convert(employees[i - 3].Photo, typeof(BitmapImage), null, CultureInfo.CurrentCulture);
                    int originalWidth = bitmapImage.PixelWidth; //Оригинальная ширина картинки
                    int originalHeight = bitmapImage.PixelHeight; //Оригинальная высота картинки

                    using (var memoryStream = new MemoryStream())
                    {
                        int excelImageWidth = originalWidth;
                        int excelImageHeight = originalHeight;

                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                        encoder.Save(memoryStream);

                        memoryStream.Position = 0;
                        var excelImage = worksheet.Drawings.AddPicture(i.ToString(), memoryStream);

                        if (excelImageWidth > width) //Если оригинальный размер больше необходимого
                        {
                            excelImageWidth = width;
                            excelImageHeight = (int)((double)width / originalWidth * originalHeight);
                        }

                        if (excelImageHeight > height)
                        {
                            excelImageHeight = height;
                            excelImageWidth = (int)((double)excelImageHeight / originalHeight * originalWidth);
                        }

                        excelImage.SetPosition(i - 1, 5, 6, (width - excelImageWidth) / 2);

                        worksheet.Column(7).Width = width / 7;
                        worksheet.Rows[i].Height = (height + 10) / 1.33;

                        excelImage.SetSize(excelImageWidth, excelImageHeight);
                    }
                }

                //Дата формирования
                ExcelRange creatingDateRange = worksheet.Cells[employees.Count + 4, 1, employees.Count + 4, 7];
                SetRangeStyles(creatingDateRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);
                worksheet.Cells[employees.Count + 4, 1].Value = $"Сформировано: {DateTime.Now.ToString("dd.MM.yyyy")}";

                //Установка ширины столбцов 
                worksheet.Column(1).Width = 4.29;
                worksheet.Column(2).Width = 40.29;
                worksheet.Column(3).Width = 25.71;
                worksheet.Column(4).Width = 25.71;
                worksheet.Column(5).Width = 24.71;
                worksheet.Column(6).Width = 24.71;
                //worksheet.Column(7).Width = 24.71;

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    Title = "Сохранить Excel документ"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    worksheet.Protection.IsProtected = false;
                    worksheet.Protection.AllowSelectLockedCells = false;
                    package.SaveAs(new FileInfo(filePath));
                }
        }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Файл открыт в другой программе.", "Открытие невозможно", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void exportEmployeeDiseases(Employee_ViewModel employee)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                var package = new ExcelPackage(); //Создание нового документа
                var worksheet = package.Workbook.Worksheets.Add($"Болезни сотрудника");

                ExcelRange diseasesStartRange = worksheet.Cells[1, 1, 1, 6];
                SetRangeStyles(diseasesStartRange, System.Drawing.Color.MediumSeaGreen, System.Drawing.Color.White, System.Drawing.Color.MediumSeaGreen, fontFamily: "Arial Black", isMerge: true);
                worksheet.Cells[1, 1].Value = $"Болезни сотрудника: {employee.FIO}";

                int currentExcelRow = CreateDiseasesExcelTable(worksheet, 2, employee);

                //Дата формирования
                ExcelRange creatingDateRange = worksheet.Cells[currentExcelRow + 1, 1, currentExcelRow + 1, 6];
                SetRangeStyles(creatingDateRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);
                worksheet.Cells[currentExcelRow + 1, 1].Value = $"Сформировано: {DateTime.Now.ToString("dd.MM.yyyy")}";

                //Установка ширины столбцов 
                worksheet.Column(1).Width = 4.29;
                worksheet.Column(2).Width = 15.29;
                worksheet.Column(3).Width = 25.71;
                worksheet.Column(4).Width = 25.71;
                worksheet.Column(5).Width = 24.71;
                worksheet.Column(6).Width = 24.71;

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    Title = "Сохранить Excel документ"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    worksheet.Protection.IsProtected = false;
                    worksheet.Protection.AllowSelectLockedCells = false;
                    package.SaveAs(new FileInfo(filePath));
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Файл открыт в другой программе.", "Открытие невозможно", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void exportEmployeeVaccinations(Employee_ViewModel employee)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                var package = new ExcelPackage(); //Создание нового документа
                var worksheet = package.Workbook.Worksheets.Add($"Прививки сотрудника");

                ExcelRange VaccinationsStartRange = worksheet.Cells[1, 1, 1, 6];
                SetRangeStyles(VaccinationsStartRange, System.Drawing.Color.MediumSeaGreen, System.Drawing.Color.White, System.Drawing.Color.MediumSeaGreen, fontFamily: "Arial Black", isMerge: true);
                worksheet.Cells[1, 1].Value = $"Прививки сотрудника: {employee.FIO}";

                int currentExcelRow = CreateVaccinationsExcelTable(worksheet, 2, employee);

                //Дата формирования
                ExcelRange creatingDateRange = worksheet.Cells[currentExcelRow + 1, 1, currentExcelRow + 1, 6];
                SetRangeStyles(creatingDateRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);
                worksheet.Cells[currentExcelRow + 1, 1].Value = $"Сформировано: {DateTime.Now.ToString("dd.MM.yyyy")}";

                //Установка ширины столбцов 
                worksheet.Column(1).Width = 4.29;
                worksheet.Column(2).Width = 15.29;
                worksheet.Column(3).Width = 25.71;
                worksheet.Column(4).Width = 25.71;
                worksheet.Column(5).Width = 24.71;
                worksheet.Column(6).Width = 24.71;

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    Title = "Сохранить Excel документ"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    worksheet.Protection.IsProtected = false;
                    worksheet.Protection.AllowSelectLockedCells = false;
                    package.SaveAs(new FileInfo(filePath));
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Файл открыт в другой программе.", "Открытие невозможно", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void exportEmployeeAntropologicalResearches(Employee_ViewModel employee)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                var package = new ExcelPackage(); //Создание нового документа
                var worksheet = package.Workbook.Worksheets.Add($"Антропология сотрудника");

                ExcelRange AntropologicalResearchesStartRange = worksheet.Cells[1, 1, 1, 6];
                SetRangeStyles(AntropologicalResearchesStartRange, System.Drawing.Color.MediumSeaGreen, System.Drawing.Color.White, System.Drawing.Color.MediumSeaGreen, fontFamily: "Arial Black", isMerge: true);
                worksheet.Cells[1, 1].Value = $"Антропология сотрудника: {employee.FIO}";

                int currentExcelRow = CreateAntropologicalResearchesExcelTable(worksheet, 2, employee);

                //Дата формирования
                ExcelRange creatingDateRange = worksheet.Cells[currentExcelRow + 1, 1, currentExcelRow + 1, 6];
                SetRangeStyles(creatingDateRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);
                worksheet.Cells[currentExcelRow + 1, 1].Value = $"Сформировано: {DateTime.Now.ToString("dd.MM.yyyy")}";
                //Установка ширины столбцов 
                worksheet.Column(1).Width = 4.29;
                worksheet.Column(2).Width = 15.29;
                worksheet.Column(3).Width = 25.71;
                worksheet.Column(4).Width = 25.71;
                worksheet.Column(5).Width = 24.71;
                worksheet.Column(6).Width = 24.71;

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    Title = "Сохранить Excel документ"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    worksheet.Protection.IsProtected = false;
                    worksheet.Protection.AllowSelectLockedCells = false;
                    package.SaveAs(new FileInfo(filePath));
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Файл открыт в другой программе.", "Открытие невозможно", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void exportEmployeeEducations(Employee_ViewModel employee)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                var package = new ExcelPackage(); //Создание нового документа
                var worksheet = package.Workbook.Worksheets.Add($"Образования сотрудника");

                ExcelRange EducationsStartRange = worksheet.Cells[1, 1, 1, 6];
                SetRangeStyles(EducationsStartRange, System.Drawing.Color.MediumSeaGreen, System.Drawing.Color.White, System.Drawing.Color.MediumSeaGreen, fontFamily: "Arial Black", isMerge: true);
                worksheet.Cells[1, 1].Value = $"Образования сотрудника: {employee.FIO}";

                int currentExcelRow = CreateEducationsExcelTable(worksheet, 2, employee);

                //Дата формирования
                ExcelRange creatingDateRange = worksheet.Cells[currentExcelRow + 1, 1, currentExcelRow + 1, 6];
                SetRangeStyles(creatingDateRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);
                worksheet.Cells[currentExcelRow + 1, 1].Value = $"Сформировано: {DateTime.Now.ToString("dd.MM.yyyy")}";
                //Установка ширины столбцов 
                worksheet.Column(1).Width = 4.29;
                worksheet.Column(2).Width = 15.29;
                worksheet.Column(3).Width = 25.71;
                worksheet.Column(4).Width = 25.71;
                worksheet.Column(5).Width = 24.71;
                worksheet.Column(6).Width = 24.71;

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    Title = "Сохранить Excel документ"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    worksheet.Protection.IsProtected = false;
                    worksheet.Protection.AllowSelectLockedCells = false;
                    package.SaveAs(new FileInfo(filePath));
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Файл открыт в другой программе.", "Открытие невозможно", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void exportEmployeeJobs(Employee_ViewModel employee)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                var package = new ExcelPackage(); //Создание нового документа
                var worksheet = package.Workbook.Worksheets.Add($"Должности сотрудника");

                ExcelRange JobsStartRange = worksheet.Cells[1, 1, 1, 6];
                SetRangeStyles(JobsStartRange, System.Drawing.Color.MediumSeaGreen, System.Drawing.Color.White, System.Drawing.Color.MediumSeaGreen, fontFamily: "Arial Black", isMerge: true);
                worksheet.Cells[1, 1].Value = $"Должности сотрудника: {employee.FIO}";

                int currentExcelRow = CreateJobsExcelTable(worksheet, 2, employee);

                //Дата формирования
                ExcelRange creatingDateRange = worksheet.Cells[currentExcelRow + 1, 1, currentExcelRow + 1, 6];
                SetRangeStyles(creatingDateRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);
                worksheet.Cells[currentExcelRow + 1, 1].Value = $"Сформировано: {DateTime.Now.ToString("dd.MM.yyyy")}";
                //Установка ширины столбцов 
                worksheet.Column(1).Width = 4.29;
                worksheet.Column(2).Width = 15.29;
                worksheet.Column(3).Width = 25.71;
                worksheet.Column(4).Width = 25.71;
                worksheet.Column(5).Width = 24.71;
                worksheet.Column(6).Width = 24.71;

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    Title = "Сохранить Excel документ"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    worksheet.Protection.IsProtected = false;
                    worksheet.Protection.AllowSelectLockedCells = false;
                    package.SaveAs(new FileInfo(filePath));
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Файл открыт в другой программе.", "Открытие невозможно", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void exportEmployeeFamilyStatuses(Employee_ViewModel employee)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                var package = new ExcelPackage(); //Создание нового документа
                var worksheet = package.Workbook.Worksheets.Add($"Семейные статусы сотрудника");

                ExcelRange FamilyStatusesStartRange = worksheet.Cells[1, 1, 1, 6];
                SetRangeStyles(FamilyStatusesStartRange, System.Drawing.Color.MediumSeaGreen, System.Drawing.Color.White, System.Drawing.Color.MediumSeaGreen, fontFamily: "Arial Black", isMerge: true);
                worksheet.Cells[1, 1].Value = $"Семейные статусы сотрудника: {employee.FIO}";

                int currentExcelRow = CreateFamilyStatusesExcelTable(worksheet, 2, employee);

                //Дата формирования
                ExcelRange creatingDateRange = worksheet.Cells[currentExcelRow + 1, 1, currentExcelRow + 1, 6];
                SetRangeStyles(creatingDateRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);
                worksheet.Cells[currentExcelRow + 1, 1].Value = $"Сформировано: {DateTime.Now.ToString("dd.MM.yyyy")}";
                //Установка ширины столбцов 
                worksheet.Column(1).Width = 4.29;
                worksheet.Column(2).Width = 15.29;
                worksheet.Column(3).Width = 25.71;
                worksheet.Column(4).Width = 25.71;
                worksheet.Column(5).Width = 24.71;
                worksheet.Column(6).Width = 24.71;

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    Title = "Сохранить Excel документ"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    worksheet.Protection.IsProtected = false;
                    worksheet.Protection.AllowSelectLockedCells = false;
                    package.SaveAs(new FileInfo(filePath));
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Файл открыт в другой программе.", "Открытие невозможно", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }














        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // СОЗДАНИЕ ТАБЛИЦЫ БОЛЕЗНЕЙ В ЗАДАННОМ ЛИСТЕ EXCEL
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private int CreateDiseasesExcelTable(ExcelWorksheet worksheet, int currentExcelRow, Employee_ViewModel employee)
        {
            ExcelRange diseasesHeaderRange = worksheet.Cells[currentExcelRow, 1, currentExcelRow, 6];
            SetRangeStyles(diseasesHeaderRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray);

            ExcelRange diseasesHeaderColumnsRange = worksheet.Cells[currentExcelRow, 2, currentExcelRow, 4];
            SetRangeStyles(diseasesHeaderColumnsRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);

            worksheet.Cells[currentExcelRow, 1].Value = "№";
            worksheet.Cells[currentExcelRow, 2].Value = "Наименование заболевания";
            worksheet.Cells[currentExcelRow, 5].Value = "Дата начала";
            worksheet.Cells[currentExcelRow, 6].Value = "Дата окончания";

            currentExcelRow++;
            ExcelRange diseasesRange = worksheet.Cells[currentExcelRow, 1, currentExcelRow + employee.Diseases.Count - 1, 6];
            SetRangeStyles(diseasesRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray);

            for (int i = currentExcelRow; i < currentExcelRow + employee.Diseases.Count; i++)
            {
                ExcelRange diseasesColumnRange = worksheet.Cells[i, 2, i, 4];
                SetRangeStyles(diseasesColumnRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);

                worksheet.Cells[i, 1].Value = i - currentExcelRow + 1;
                worksheet.Cells[i, 2].Value = employee.Diseases[i - currentExcelRow].Name;
                worksheet.Cells[i, 5].Value = employee.Diseases[i - currentExcelRow].StardDiseaseDate;
                worksheet.Cells[i, 6].Value = employee.Diseases[i - currentExcelRow].EndDiseaseDate;
            }

            return currentExcelRow + employee.Diseases.Count;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // СОЗДАНИЕ ТАБЛИЦЫ ПРИВИВОК В ЗАДАННОМ ЛИСТЕ EXCEL
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private int CreateVaccinationsExcelTable(ExcelWorksheet worksheet, int currentExcelRow, Employee_ViewModel employee)
        {
            ExcelRange VaccinationHeaderRange = worksheet.Cells[currentExcelRow, 1, currentExcelRow, 6];
            SetRangeStyles(VaccinationHeaderRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray);

            ExcelRange VaccinationsHeaderColumnsRange = worksheet.Cells[currentExcelRow, 2, currentExcelRow, 5];
            SetRangeStyles(VaccinationsHeaderColumnsRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);

            worksheet.Cells[currentExcelRow, 1].Value = "№";
            worksheet.Cells[currentExcelRow, 2].Value = "Наименование прививки";
            worksheet.Cells[currentExcelRow, 6].Value = "Дата";

            currentExcelRow++;
            ExcelRange VaccinationsRange = worksheet.Cells[currentExcelRow, 1, currentExcelRow + employee.Vaccinations.Count - 1, 6];
            SetRangeStyles(VaccinationsRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray);

            for (int i = currentExcelRow; i < currentExcelRow + employee.Vaccinations.Count; i++)
            {
                ExcelRange vaccinationsColumnRange = worksheet.Cells[i, 2, i, 5];
                SetRangeStyles(vaccinationsColumnRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);

                worksheet.Cells[i, 1].Value = i - currentExcelRow + 1;
                worksheet.Cells[i, 2].Value = employee.Vaccinations[i - currentExcelRow].Name;
                worksheet.Cells[i, 6].Value = employee.Vaccinations[i - currentExcelRow].Date;
            }

            return currentExcelRow + employee.Vaccinations.Count;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // СОЗДАНИЕ ТАБЛИЦЫ АНТРОПОЛОГИЧЕСКИХ ИССЛЕДОВАНИЙ В ЗАДАННОМ ЛИСТЕ EXCEL
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private int CreateAntropologicalResearchesExcelTable(ExcelWorksheet worksheet, int currentExcelRow, Employee_ViewModel employee)
        {
            ExcelRange AntropologicalResearchesHeaderRange = worksheet.Cells[currentExcelRow, 1, currentExcelRow, 6];
            SetRangeStyles(AntropologicalResearchesHeaderRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray);

            ExcelRange AntropologicalResearchesHeaderWeightColumnsRange = worksheet.Cells[currentExcelRow, 2, currentExcelRow, 3];
            SetRangeStyles(AntropologicalResearchesHeaderWeightColumnsRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);

            ExcelRange AntropologicalResearchesHeaderHeightColumnsRange = worksheet.Cells[currentExcelRow, 4, currentExcelRow, 5];
            SetRangeStyles(AntropologicalResearchesHeaderHeightColumnsRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);

            worksheet.Cells[currentExcelRow, 1].Value = "№";
            worksheet.Cells[currentExcelRow, 2].Value = "Вес";
            worksheet.Cells[currentExcelRow, 4].Value = "Рост";
            worksheet.Cells[currentExcelRow, 6].Value = "Дата";

            currentExcelRow++;
            ExcelRange AntropologicalResearchesRange = worksheet.Cells[currentExcelRow, 1, currentExcelRow + employee.AntropologicalResearches.Count - 1, 6];
            SetRangeStyles(AntropologicalResearchesRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray);

            for (int i = currentExcelRow; i < currentExcelRow + employee.AntropologicalResearches.Count; i++)
            {
                ExcelRange AntropologicalResearchesWeightColumnRange = worksheet.Cells[i, 2, i, 3];
                SetRangeStyles(AntropologicalResearchesWeightColumnRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);

                ExcelRange AntropologicalResearchesHeightColumnRange = worksheet.Cells[i, 4, i, 5];
                SetRangeStyles(AntropologicalResearchesHeightColumnRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);

                worksheet.Cells[i, 1].Value = i - currentExcelRow + 1;
                worksheet.Cells[i, 2].Value = employee.AntropologicalResearches[i - currentExcelRow].Weight;
                worksheet.Cells[i, 4].Value = employee.AntropologicalResearches[i - currentExcelRow].Height;
                worksheet.Cells[i, 6].Value = employee.AntropologicalResearches[i - currentExcelRow].Date;
            }

            return currentExcelRow + employee.AntropologicalResearches.Count;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // СОЗДАНИЕ ТАБЛИЦЫ ОБРАЗОВАНИЙ В ЗАДАННОМ ЛИСТЕ EXCEL
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private int CreateEducationsExcelTable(ExcelWorksheet worksheet, int currentExcelRow, Employee_ViewModel employee)
        {
            ExcelRange EducationsHeaderRange = worksheet.Cells[currentExcelRow, 1, currentExcelRow, 6];
            SetRangeStyles(EducationsHeaderRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray);

            ExcelRange EducationsHeaderColumnsRange = worksheet.Cells[currentExcelRow, 2, currentExcelRow, 4];
            SetRangeStyles(EducationsHeaderColumnsRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);

            worksheet.Cells[currentExcelRow, 1].Value = "№";
            worksheet.Cells[currentExcelRow, 2].Value = "Учреждение образования";
            worksheet.Cells[currentExcelRow, 5].Value = "Уровень образования";
            worksheet.Cells[currentExcelRow, 6].Value = "Дата окончания";

            currentExcelRow++;
            ExcelRange EducationsRange = worksheet.Cells[currentExcelRow, 1, currentExcelRow + employee.Educations.Count - 1, 6];
            SetRangeStyles(EducationsRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray);

            for (int i = currentExcelRow; i < currentExcelRow + employee.Educations.Count; i++)
            {
                ExcelRange EducationsColumnRange = worksheet.Cells[i, 2, i, 4];
                SetRangeStyles(EducationsColumnRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);

                worksheet.Cells[i, 1].Value = i - currentExcelRow + 1;
                worksheet.Cells[i, 2].Value = employee.Educations[i - currentExcelRow].EducationInstitution;
                worksheet.Cells[i, 5].Value = employee.Educations[i - currentExcelRow].EducationType;
                worksheet.Cells[i, 6].Value = employee.Educations[i - currentExcelRow].Date;
            }

            return currentExcelRow + employee.Educations.Count;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // СОЗДАНИЕ ТАБЛИЦЫ ДОЛЖНОСТЕЙ В ЗАДАННОМ ЛИСТЕ EXCEL
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private int CreateJobsExcelTable(ExcelWorksheet worksheet, int currentExcelRow, Employee_ViewModel employee)
        {
            ExcelRange JobsHeaderRange = worksheet.Cells[currentExcelRow, 1, currentExcelRow, 6];
            SetRangeStyles(JobsHeaderRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray);

            worksheet.Cells[currentExcelRow, 1].Value = "№";
            worksheet.Cells[currentExcelRow, 2].Value = "Должность";
            worksheet.Cells[currentExcelRow, 3].Value = "Подразделение";
            worksheet.Cells[currentExcelRow, 4].Value = "Ставка";
            worksheet.Cells[currentExcelRow, 5].Value = "Дата начала";
            worksheet.Cells[currentExcelRow, 6].Value = "Дата окончания";

            currentExcelRow++;
            ExcelRange diseasesRange = worksheet.Cells[currentExcelRow, 1, currentExcelRow + employee.Jobs.Count - 1, 6];
            SetRangeStyles(diseasesRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray);

            for (int i = currentExcelRow; i < currentExcelRow + employee.Jobs.Count; i++)
            {
                worksheet.Cells[i, 1].Value = i - currentExcelRow + 1;
                worksheet.Cells[i, 2].Value = employee.Jobs[i - currentExcelRow].JobName;
                worksheet.Cells[i, 3].Value = employee.Jobs[i - currentExcelRow].Subunit;
                worksheet.Cells[i, 4].Value = employee.Jobs[i - currentExcelRow].WorkingRate;
                worksheet.Cells[i, 5].Value = employee.Jobs[i - currentExcelRow].StartWorkingDate;
                worksheet.Cells[i, 6].Value = employee.Jobs[i - currentExcelRow].EndWorkingDate;
            }

            return currentExcelRow + employee.Jobs.Count;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // СОЗДАНИЕ ТАБЛИЦЫ СЕСЕЙНЫХ СТАТУСОВ В ЗАДАННОМ ЛИСТЕ EXCEL
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private int CreateFamilyStatusesExcelTable(ExcelWorksheet worksheet, int currentExcelRow, Employee_ViewModel employee)
        {
            ExcelRange FamilyStatusesHeaderRange = worksheet.Cells[currentExcelRow, 1, currentExcelRow, 6];
            SetRangeStyles(FamilyStatusesHeaderRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray);

            ExcelRange FamilyStatusesHeaderColumnsRange = worksheet.Cells[currentExcelRow, 2, currentExcelRow, 4];
            SetRangeStyles(FamilyStatusesHeaderColumnsRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);

            worksheet.Cells[currentExcelRow, 1].Value = "№";
            worksheet.Cells[currentExcelRow, 2].Value = "Статус";
            worksheet.Cells[currentExcelRow, 5].Value = "Дата начала";
            worksheet.Cells[currentExcelRow, 6].Value = "Дата окончания";

            currentExcelRow++;
            ExcelRange FamilyStatusesRange = worksheet.Cells[currentExcelRow, 1, currentExcelRow + employee.FamilyStatuses.Count - 1, 6];
            SetRangeStyles(FamilyStatusesRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray);

            for (int i = currentExcelRow; i < currentExcelRow + employee.FamilyStatuses.Count; i++)
            {
                ExcelRange FamilyStatusesColumnRange = worksheet.Cells[i, 2, i, 4];
                SetRangeStyles(FamilyStatusesColumnRange, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Gray, isMerge: true);

                worksheet.Cells[i, 1].Value = i - currentExcelRow + 1;
                worksheet.Cells[i, 2].Value = employee.FamilyStatuses[i - currentExcelRow].Status;
                worksheet.Cells[i, 5].Value = employee.FamilyStatuses[i - currentExcelRow].StartFamilyDate;
                worksheet.Cells[i, 6].Value = employee.FamilyStatuses[i - currentExcelRow].EndFamilyDate;
            }

            return currentExcelRow + employee.FamilyStatuses.Count;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // УСТАНОВКА СТИЛЕЙ ДЛЯ ОБЛАСТИ В EXCEL
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void SetRangeStyles(ExcelRange range, System.Drawing.Color backgroundColor, System.Drawing.Color foreground, System.Drawing.Color borderColor, string fontFamily = null, bool isMerge = false, ExcelHorizontalAlignment hAligment = ExcelHorizontalAlignment.Center)
        {
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(backgroundColor);

            range.Style.Font.Color.SetColor(foreground);

            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            range.Style.Border.Top.Color.SetColor(borderColor);
            range.Style.Border.Bottom.Color.SetColor(borderColor);
            range.Style.Border.Left.Color.SetColor(borderColor);
            range.Style.Border.Right.Color.SetColor(borderColor);

            if (fontFamily != null)
            {
                range.Style.Font.Name = "Arial Black";
            }

            if (isMerge) { range.Merge = true; }

            if (hAligment != null)
            {
                range.Style.HorizontalAlignment = hAligment;
            }
            else { range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; }

            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }
    }
}
