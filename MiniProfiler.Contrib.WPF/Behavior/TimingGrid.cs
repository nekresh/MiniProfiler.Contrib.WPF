using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MiniProfiler.Contrib.WPF.Converter;

namespace MiniProfiler.Contrib.WPF.Behavior
{
    public class TimingGrid
    {
        public static readonly DependencyProperty TimingsSourceProperty =
            DependencyProperty.RegisterAttached("TimingsSource", typeof(IEnumerable<Timing>), typeof(TimingGrid),
                new FrameworkPropertyMetadata(null, OnTimingsSourceChanged));

        public static IEnumerable<Timing> GetTimingsSource(DependencyObject d)
        {
            return (IEnumerable<Timing>)d.GetValue(TimingsSourceProperty);
        }

        public static void SetTimingsSource(DependencyObject d, IEnumerable<Timing> timings)
        {
            d.SetValue(TimingsSourceProperty, timings);
        }

        private static void OnTimingsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listView = d as ListView;
            var data = e.NewValue as IEnumerable<Timing>;

            var gridView = listView.View as GridView;

            // Remove generated columns
            var toRemove = gridView.Columns.Where(c => GetGeneratedColumn(c)).ToList();
            foreach (var column in toRemove)
                gridView.Columns.Remove(column);

            listView.ItemsSource = data;

            // Generate new columns
            var customColumns = extractCustomTimings(data);
            foreach (var columnName in customColumns)
            {
                var content = new FrameworkElementFactory(typeof(TextBlock));
                content.SetBinding(TextBlock.TextProperty, new Binding
                {
                    Converter = CustomTimingConverter.Instance,
                    ConverterParameter = columnName,
                    Mode = BindingMode.OneTime
                });
                content.AddHandler(UIElement.MouseDownEvent, buildMouseHandler(listView));

                var gridViewColumn = new GridViewColumn
                {
                    Header = columnName,
                    CellTemplate = new DataTemplate
                    {
                        DataType = typeof(Timing),
                        VisualTree = content
                    }
                };

                SetGeneratedColumn(gridViewColumn, true);

                gridView.Columns.Add(gridViewColumn);
            }
        }

        private static MouseButtonEventHandler buildMouseHandler(ListView listView)
        {
            return (s, e) =>
            {
                var command = GetCustomTimingCommand(listView);

                if (command != null)
                {
                    if (command.CanExecute(null))
                    {
                        command.Execute(null);
                    }
                }
            };
        }

        private static IEnumerable<string> extractCustomTimings(IEnumerable<Timing> timings)
        {
            var set = new HashSet<string>();

            extractCustomTimings(set, timings);
            return set;
        }

        private static void extractCustomTimings(HashSet<string> set, IEnumerable<Timing> timings)
        {
            foreach (var timing in timings)
            {
                if (timing.HasCustomTimings)
                    foreach (var customTiming in timing.CustomTimings)
                        if (customTiming.Value != null && customTiming.Value.Any())
                            set.Add(customTiming.Key);

                if (timing.HasChildren)
                    extractCustomTimings(set, timing.Children);
            }
        }

        public static readonly DependencyProperty GeneratedColumnProperty =
            DependencyProperty.RegisterAttached("GeneratedColumn", typeof(bool), typeof(TimingGrid),
                new FrameworkPropertyMetadata(false));

        public static bool GetGeneratedColumn(DependencyObject d)
        {
            return (bool)d.GetValue(GeneratedColumnProperty);
        }

        public static void SetGeneratedColumn(DependencyObject d, bool value)
        {
            d.SetValue(GeneratedColumnProperty, value);
        }

        public static readonly DependencyProperty CustomTimingCommandProperty =
            DependencyProperty.RegisterAttached("CustomTimingCommand", typeof(ICommand), typeof(TimingGrid));

        public static ICommand GetCustomTimingCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CustomTimingCommandProperty);
        }

        public static void SetCustomTimingCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CustomTimingCommandProperty, value);
        }
    }
}
