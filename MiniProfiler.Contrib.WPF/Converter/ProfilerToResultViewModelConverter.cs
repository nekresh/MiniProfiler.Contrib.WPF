﻿using MiniProfiler.Contrib.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Profiler = StackExchange.Profiling.MiniProfiler;

namespace MiniProfiler.Contrib.WPF.Converter
{
    public class ProfilerToResultViewModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new ProfilerResultViewModel((Profiler)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
