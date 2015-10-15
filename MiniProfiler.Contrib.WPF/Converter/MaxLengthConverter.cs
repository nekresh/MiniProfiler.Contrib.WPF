using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MiniProfiler.Contrib.WPF.Converter
{
    public class MaxLengthConverter : IValueConverter
    {
        public int MaxLength { get; set; }

        public MaxLengthConverter()
        {
            MaxLength = 100;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value== null ? string.Empty : value.ToString();

            return str.Length > MaxLength ? str.Substring(0, MaxLength) + "..." : str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
