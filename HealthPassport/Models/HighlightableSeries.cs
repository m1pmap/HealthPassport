using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HealthPassport.Models
{
    public class HighlightableSeries
    {
        public string Title { get; set; }
        public ChartValues<int> Values { get; set; }
        public SolidColorBrush StrokeBrush { get; set; }
        public SolidColorBrush FillBrush { get; set; }

        public LineSeries ToLineSeries()
        {
            return new LineSeries
            {
                Title = Title,
                Values = Values,
                Stroke = StrokeBrush,
                Fill = FillBrush,
                PointGeometrySize = 10
            };
        }
    }

}
