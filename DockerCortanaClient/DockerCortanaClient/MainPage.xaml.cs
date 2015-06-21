using Docker.Cortana;
using Docker.DotNet;
using DockerCortanaClient.ViewModels;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DockerCortanaClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer mTimer = new DispatcherTimer();
        MainViewModel viewModel = new MainViewModel();
        DockerVoiceClient mClient = new DockerVoiceClient("tcp://dockerconhack.cloudapp.net:2376");
        int index = 0;
        const int MAX_LOG_ENTRIES = 10;
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void mTimer_Tick(object sender, object e)
        {
            viewModel.ActiveContainer.Logs.Add(new LogEntryViewModel { Text = "line " + index.ToString() });
            index++;
            if (viewModel.ActiveContainer.Logs.Count > MAX_LOG_ENTRIES)
                viewModel.ActiveContainer.Logs.RemoveAt(0);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            viewModel.List = new ObservableCollection<IListItem>(new ObservableCollection<ContainerViewModel>());
            var containers = await mClient.DockerGetContainers();
            foreach (var container in containers)
                viewModel.List.Add(new ContainerViewModel
                {
                    Name = container.Names.First(),
                    ImageName = container.Image,
                    Description = container.Status
                });

            //Since I don't know how depenency would work with pages, here's temp code:

            viewModel.Header = new HostViewModel
            {
                Name = "dockerconhack.cloudapp.net:2367",
                OS = "Ubuntu 14.10",
                Description = "3.16.0-38-generic - CPUs:2 RAM:6.807G"
            };

            viewModel.ActiveContainer = viewModel.List.First() as ContainerViewModel;
            viewModel.ActiveContainer.Logs = new ObservableCollection<LogEntryViewModel>();
            this.DataContext = viewModel;
            mTimer.Tick += mTimer_Tick;
            mTimer.Interval = TimeSpan.FromMilliseconds(100);
            mTimer.Start();

            if (e == null)
                return;
            string[] parts = e.Parameter.ToString().Split('|');

            //viewModel.ActiveContainer = viewModel.List.First((i) => i.Name == parts[0]) as ContainerViewModel;

        }
    }
}
