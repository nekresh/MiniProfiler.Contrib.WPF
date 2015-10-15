using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using Profiler = StackExchange.Profiling.MiniProfiler;

namespace MiniProfiler.Contrib.WPF
{
    public class ProfilerManager
    {
        public ProfilerManager()
        {
            Sessions = new ObservableCollection<Profiler>();
        }

        public ObservableCollection<Profiler> Sessions { get; set; }

        public void AddProfilingSession(Uri baseAddress, string id)
        {
            if (Sessions.Select(s => s.Id).Any(g => g.ToString() == id))
                return;

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
            Sessions.Add(profiler);
        }
    }
}
