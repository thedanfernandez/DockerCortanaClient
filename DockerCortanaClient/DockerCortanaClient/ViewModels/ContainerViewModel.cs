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
    public class ContainerViewModel: BindableBase, IListItem
    {
        private MainViewModel mParent;
        public ContainerViewModel(MainViewModel parent)
        {
            mParent = parent;
        }
        public string Name { get; set; }
        public string Id { get; set; }
        public string ImageName { get; set; }
        public string SecondaryName
        {
            get { return ImageName; }
            set { ImageName = value; }
        }
        public string Description { get; set; }
        public DelegateCommand<object> ShowLogCommand { get; private set; }
        public bool CanShowLog(object parm)
        {
            return mParent != null;
        }
        public async void ShowLog(object parm)
        {
            if (parm == null)
                return;
            string id = parm.ToString();

            DockerVoiceClient mClient = new DockerVoiceClient("tcp://dockerconhack.cloudapp.net:2376");


            CancellationToken token = new CancellationToken(false);

            Stream s = await mClient.ShowLogs(id, token);
            mParent.Logs.Clear();
            using (StreamReader sr = new StreamReader(s))
            {
                string log = "";
                while (!sr.EndOfStream)
                {
                    log = sr.ReadLine();
                    mParent.Logs.Add(new LogEntryViewModel { Text = log });
                }
            }
        }
    }
}
