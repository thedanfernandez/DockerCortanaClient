using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerCortanaClient.ViewModels
{
    public class HostViewModel: BindableBase, IListItem, IHeaderItem
    {
        public string Name { get; set; }     
        public string OS { get; set; }
        public string SecondaryName
        {
            get { return OS; }
            set { OS = value; }
        }
        public string Description { get; set; }
    }
}
