using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Dahmira.Services
{
    public class NullToEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) //Конвертер нулевого значения в пустую строку
        {
            //Проверка на пустое значение
            if (value == null ||
               (value is double && double.IsNaN((double)value)) ||
               (value is string && (string)value == string.Empty))
            {
                return "Не указано";
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue && string.IsNullOrEmpty(strValue))
            {
                if (targetType == typeof(int))
                {
                    return 0;
                }
                else if (targetType == typeof(double))
                {
                    return 0.0;
                }
                else if (targetType == typeof(string))
                {
                    return null;
                }
            }

            //Если значение не пустое, возвращается как есть
            return value;
        }
    }
}
