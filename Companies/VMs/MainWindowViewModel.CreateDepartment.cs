using Companies.Models;
using Simplified;
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
        public RelayCommand AddDepartmentCommand => GetCommand(AddDepartmentExecute,AddDepartmentCanExecute);

        public RelayCommand CancelDepartmentCommand =>GetCommand(CancelDepartmentCommandExecute);

        private void CancelDepartmentCommandExecute()
        {
            CloseDepartmentAction();
        }
        public Action CloseDepartmentAction { get; set; }


        //private string departmentName;
        public string? DepartmentName { get => Get<string>(); set => Set(value); }
        //{
        //    get
        //    {
        //        return departmentName;
        //    }
        //    set
        //    {
        //        departmentName = value;
        //        OnPropertyChanged("DepartmentName");
        //    }
        //}

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
            var company = context.Companies.FirstOrDefault(c => c.Id == SelectedCompany.Id);
            company?.Departments?.Add(CreatedDepartment);
            context.SaveChanges();
            CloseDepartmentAction();
            ResetDepartmentData();
        }
    }
}
