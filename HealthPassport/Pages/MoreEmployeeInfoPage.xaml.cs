using HealthPassport.Models;
using HealthPassport.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для MoreEmployeeInfoPage.xaml
    /// </summary>
    public partial class MoreEmployeeInfoPage : Window
    {
        public Employee_ViewModel selectedEmployee;

        private readonly ByteArrayToImageSourceConverter_Service _imageSourceConverter;
        public MoreEmployeeInfoPage(ByteArrayToImageSourceConverter_Service imageSourceConverter)
        {
            InitializeComponent();

            _imageSourceConverter = imageSourceConverter;
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
        }
    }
}
