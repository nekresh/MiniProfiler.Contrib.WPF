using MiniProfiler.Contrib.WPF.Command;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Profiler = StackExchange.Profiling.MiniProfiler;

namespace MiniProfiler.Contrib.WPF.ViewModel
{
    public class ProfilerResultViewModel : ViewModelBase
    {
        public ProfilerResultViewModel(Profiler profiler)
        {
            Name = profiler.Name;
            MachineName = profiler.MachineName;
            Started = profiler.Started;

            Timings = profiler.GetTimingHierarchy().ToList();

            CustomTimingCommand = new SimpleCommand(_ =>
            {
                CustomTimingsOpen = true;
            });
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                raisePropertyChanged("Name");
            }
        }

        private string machineName;
        public string MachineName
        {
            get { return machineName; }
            set
            {
                machineName = value;
                raisePropertyChanged("MachineName");
            }
        }

        private DateTime started;
        public DateTime Started
        {
            get { return started; }
            set
            {
                started = value;
                raisePropertyChanged("Started");
            }
        }

        private IEnumerable<Timing> timings;
        public IEnumerable<Timing> Timings
        {
            get { return timings; }
            set
            {
                timings = value;
                raisePropertyChanged("Timings");
            }
        }

        private SimpleCommand customTimingCommand;
        public SimpleCommand CustomTimingCommand
        {
            get { return customTimingCommand; }
            set
            {
                customTimingCommand = value;
                raisePropertyChanged("CustomTimingCommand");
            }
        }

        private bool customTimingsOpen;
        public bool CustomTimingsOpen
        {
            get { return customTimingsOpen; }
            set
            {
                customTimingsOpen = value;
                raisePropertyChanged("CustomTimingsOpen");
            }
        }
    }
}
