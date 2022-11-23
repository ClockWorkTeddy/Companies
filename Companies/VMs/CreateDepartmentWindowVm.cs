using Companies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.VMs
{
    public class CreateDepartmentWindowVm : ViewModelBase
    {
        public CompaniesContext Context { get; set; }
        public Department CreatedDepartment { get; set; }
        public Action CloseAction { get; set; }
        public AutoCommand AddDepartmentCommand =>
            new AutoCommand(obj => { AddDepartmentExecute(); }, obj => AddDepartmentCanExecute());

        public AutoCommand CancelCommand =>
            new AutoCommand(obj => { CancelCommandExecute(); });

        public CreateDepartmentWindowVm()
		{
            Context = CompaniesContext.GetInstance();
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private bool AddDepartmentCanExecute()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        private void AddDepartmentExecute()
        {
            CreatedDepartment = new Department()
            {
                Name = this.Name,
            };
            Context.PlaceDepartment(CreatedDepartment);
            CloseAction();
        }
        private void CancelCommandExecute()
        {
            CloseAction();
        }

    }
}
