using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerCortanaClient.ViewModels
{
    public interface IHeaderItem
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}
