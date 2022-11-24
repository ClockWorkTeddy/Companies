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

        public AutoCommand CancelDepartmentCommand =>
            new AutoCommand(obj => { CancelDepartmentCommandExecute(); });

        private void CancelDepartmentCommandExecute()
        {
            CloseDepartmentAction();
        }
        public Action CloseDepartmentAction { get; set; }


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
            var company = Context.Companies.FirstOrDefault(c => c.Id == SelectedCompany[0].Id);
            company?.Departments?.Add(CreatedDepartment);
            Context.SaveChanges();
            CloseDepartmentAction();
        }
    }
}
