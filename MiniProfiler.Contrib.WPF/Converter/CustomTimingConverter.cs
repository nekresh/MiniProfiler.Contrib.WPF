using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MiniProfiler.Contrib.WPF.Converter
{
    class CustomTimingConverter : IValueConverter
    {
        public static readonly CustomTimingConverter Instance = new CustomTimingConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timing = value as Timing;
            var key = parameter as string;

            if (timing.HasCustomTimings)
            {
                List<CustomTiming> timings;
                if (timing.CustomTimings.TryGetValue(key, out timings))
                    return string.Format("{0} ({1}) {2}", timings.Sum(t => t.DurationMilliseconds), timings.Count(), timings.GroupBy(c => c.CommandString).Where(g => g.Count() > 1).Any() ? " !" : "");
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
