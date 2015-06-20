using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerCortanaClient.ViewModels
{
    public class ContainerViewModel: BindableBase, IListItem
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string SecondaryName
        {
            get { return ImageName; }
            set { ImageName = value; }
        }
        public string Description { get; set; }
        public ObservableCollection<LogEntryViewModel> Logs { get; set; }
    }
}
