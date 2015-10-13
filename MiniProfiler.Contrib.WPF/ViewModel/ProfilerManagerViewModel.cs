using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MiniProfiler.Contrib.WPF.Command;
using Profiler = StackExchange.Profiling.MiniProfiler;

namespace MiniProfiler.Contrib.WPF.ViewModel
{
    class ProfilerManagerViewModel : ViewModelBase
    {
        public ProfilerManagerViewModel()
        {
            Sessions = new ObservableCollection<ProfilerResultViewModel>();
            OpenPopupCommand = new SimpleCommand(_ => IsOpen = true);
        }

        public void AddProfilingSession(Uri baseAddress, string id)
        {
            var client = new HttpClient
            {
                BaseAddress = baseAddress
            };
            var task = client.PostAsync(
                "mini-profiler-resources/results",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "id", id },
                    { "popup", "1" }
                })
            );

            var result = task.Result;

            var profiler = Profiler.FromJson(result.Content.ReadAsStringAsync().Result);
            Sessions.Add(new ProfilerResultViewModel(profiler));
        }

        public ObservableCollection<ProfilerResultViewModel> Sessions { get; set; }

        private bool isOpen;
        public bool IsOpen
        {
            get { return isOpen; }
            set
            {
                isOpen = value;
                raisePropertyChanged("IsOpen");
            }
        }

        public ICommand OpenPopupCommand { get; private set; }
    }
}
