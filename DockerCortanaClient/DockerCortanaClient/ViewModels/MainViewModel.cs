using Docker.Cortana;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DockerCortanaClient.ViewModels
{
    public class MainViewModel: BindableBase
    {
        DockerVoiceClient mClient = new DockerVoiceClient("tcp://dockerconhack.cloudapp.net:2376");

        public ObservableCollection<IListItem> List { get; set; }
        public IHeaderItem Header { get; set; }
        public ObservableCollection<LogEntryViewModel> Logs { get; set; }
        public DelegateCommand<object> ShowLogCommand { get; private set; }

        public MainViewModel()
        {
            ShowLogCommand = new DelegateCommand<object>(ShowLog, CanShowLog);
        }
        private bool CanShowLog(object parm)
        {
            return parm != null && List != null && List.Count > 0;
        }
        public async void ShowLog(object parm)
        {
            if (parm == null)
                return;
            string id = parm.ToString();
            CancellationToken token = new CancellationToken(false);

            Stream s = await mClient.ShowLogs(id, token);
            this.Logs.Clear();
            using (StreamReader sr = new StreamReader(s))
            {
                string log = "";
                while (!sr.EndOfStream)
                {
                    log = sr.ReadLine();
                    this.Logs.Add(new LogEntryViewModel { Text = log });
                }
            }
        }
    }
}
