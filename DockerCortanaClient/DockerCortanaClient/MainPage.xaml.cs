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
        int index = 0;
        const int MAX_LOG_ENTRIES = 10;
        public MainPage()
        {
            this.InitializeComponent();
            //Since I don't know how depenency would work with pages, here's temp code:
            
            viewModel.Header = new HostViewModel
            {
                Name = "dockercon-web.westus.cloudapp.azure.com",
                OS = "Ubuntu 14.10",
                Description = "3.16.0-38-generic - CPUs:2 RAM:6.807G"
            };
            viewModel.List = new ObservableCollection<IListItem>(new ObservableCollection<ContainerViewModel>());
            viewModel.List.Add(new ContainerViewModel { Name = "focused_davinci", ImageName = "partsunlimited:latest"});
            viewModel.ActiveContainer = viewModel.List.First() as ContainerViewModel;
            viewModel.ActiveContainer.Logs = new ObservableCollection<LogEntryViewModel>();
            this.DataContext = viewModel;
            mTimer.Tick += mTimer_Tick;
            mTimer.Interval = TimeSpan.FromMilliseconds(100);
            mTimer.Start();
        }

        private void mTimer_Tick(object sender, object e)
        {
            viewModel.ActiveContainer.Logs.Add(new LogEntryViewModel { Text = "line " + index.ToString() });
            index++;
            if (viewModel.ActiveContainer.Logs.Count > MAX_LOG_ENTRIES)
                viewModel.ActiveContainer.Logs.RemoveAt(0);
        }
    }
}
