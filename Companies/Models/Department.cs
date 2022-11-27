using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Companies.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? BossId { get; set; }
        public int CompanyId { get; set; }
        public virtual Company? Company { get; set; }

        public virtual ICollection<Employee>? Employees { get; private set; } = 
            new ObservableCollection<Employee>();

        public override string ToString()
        {
            if (Name != null)
                return Name;
            else
                return base.ToString();
        }
    }
}
