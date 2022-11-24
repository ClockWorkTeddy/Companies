using Companies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.VMs
{
    internal class CreateEmployeeWindowVm : ViewModelBase
    {
        public CompaniesContext Context { get; set; }
        public Employee CreatedEmployee { get; set; }
        public Action CloseAction { get; set; }
        public AutoCommand AddEmployeeCommand =>
            new AutoCommand(obj => { AddEmployeeExecute(); }, obj => AddEmployeeCanExecute());

        public AutoCommand CancelCommand =>
            new AutoCommand(obj => { CancelCommandExecute(); });

        public CreateEmployeeWindowVm()
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
        private string lastName;
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        private string middleName;
        public string MiddleName
        {
            get
            {
                return middleName;
            }
            set
            {
                middleName = value;
                OnPropertyChanged("MiddleName");
            }
        }

        private string dateRecruitment;
        public string DateRecruitment
        {
            get
            {
                return dateRecruitment;
            }
            set
            {
                dateRecruitment = value;
                OnPropertyChanged("DateRecruitment");
            }
        }

        private string birthday;
        public string Birthday
        {
            get
            {
                return birthday;
            }
            set
            {
                birthday = value;
                OnPropertyChanged("Birthday");
            }
        }

        private string position;
        public string Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        private double salary;
        public double Salary
        {
            get
            {
                return salary;
            }
            set
            {
                salary = value;
                OnPropertyChanged("Salary");
            }
        }


        private bool AddEmployeeCanExecute()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        private void AddEmployeeExecute()
        {
            CreatedEmployee = new Employee()
            {
                Name = this.Name,
            };
            Context.PlaceEmployee(CreatedEmployee);
            CloseAction();
        }
        private void CancelCommandExecute()
        {
            CloseAction();
        }

    }
}
