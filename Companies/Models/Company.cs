using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Date { get; set; }
        public string? Adress { get; set; }
        public virtual ICollection<Department>? Departments { get; private set; } =
            new ObservableCollection<Department>();

        public override string ToString()
        {
            return Name;
        }
    }
}
