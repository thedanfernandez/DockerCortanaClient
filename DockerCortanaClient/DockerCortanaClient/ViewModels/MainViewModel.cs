using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerCortanaClient.ViewModels
{
    public class MainViewModel: BindableBase
    {
        public ObservableCollection<IListItem> List { get; set; }
        public IHeaderItem Header { get; set; }
        public ContainerViewModel ActiveContainer { get; set; }
    }
}
