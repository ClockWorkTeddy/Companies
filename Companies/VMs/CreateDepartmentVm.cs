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
                OnPropertyChanged(nameof(DepartmentName));
            }
        }

        private void CancelDepartmentCommandExecute() =>
            CloseDepartmentAction();

        private bool AddDepartmentCanExecute()
        {
            return !string.IsNullOrWhiteSpace(DepartmentName);
        }

        private void AddDepartmentExecute()
        {
            CreatedDepartment = new Department()
            {
                Name = DepartmentName,
            };
            var company = Context.Companies.FirstOrDefault(c => c.Id == SelectedCompany.Id);
            company?.Departments?.Add(CreatedDepartment);
            Context.SaveChanges();
            SetComboDepartments();
            CloseDepartmentAction();
            ResetDepartmentData();
        }

        private void ResetDepartmentData() =>
            DepartmentName = "";
    }
}
