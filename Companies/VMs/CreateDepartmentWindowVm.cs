using Companies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.VMs
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public Department CreatedDepartment { get; set; }
        public AutoCommand AddDepartmentCommand =>
            new AutoCommand(obj => { AddDepartmentExecute(); }, obj => AddDepartmentCanExecute());

        private string departmentName;
        public string DepartmentName
        {
            get
            {
                return departmentName;
            }
            set
            {
                departmentName = value;
                OnPropertyChanged("DepartmentName");
            }
        }

        private bool AddDepartmentCanExecute()
        {
            return !string.IsNullOrWhiteSpace(DepartmentName);
        }

        private void AddDepartmentExecute()
        {
            CreatedDepartment = new Department()
            {
                Name = this.DepartmentName,
            };
            Context.PlaceDepartment(CreatedDepartment);
            CloseAction();
        }
    }
}
