using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Companies.Models;

namespace Companies.VMs
{
    public class Root
    {
        public string Name { get; set; } = "Companies";
        public ObservableCollection<Company> Companies { get; set; } = new ObservableCollection<Company>();
    }
}
