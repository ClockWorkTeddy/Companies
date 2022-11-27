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
        public Employee CreatedEmployee { get; set; }
        public AutoCommand AddEmployeeCommand =>
            new AutoCommand(obj => { AddEmployeeExecute(); }, obj => AddEmployeeCanExecute());

        public AutoCommand CancelEmployeeCommand =>
        new AutoCommand(obj => { CancelEmployeeCommandExecute(); });

        private void CancelEmployeeCommandExecute()
        {
            CloseEmployeeAction();
        }

        public Action CloseEmployeeAction { get; set; }



        private string employeeName;
        public string EmployeeName
        {
            get
            {
                return employeeName;
            }
            set
            {
                employeeName = value;
                OnPropertyChanged("EmployeeName");
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
            return !string.IsNullOrWhiteSpace(EmployeeName) &&
                   !string.IsNullOrWhiteSpace(Position) &&
                   Salary > 0;
        }

        private void AddEmployeeExecute()
        {
            CreatedEmployee = new Employee()
            {
                Name = this.EmployeeName,
                Lastname = this.LastName,
                BirthYear = Convert.ToInt32(this.Birthday),
                Salary = this.Salary,
                Middlename = this.MiddleName,
                ReqruitmentDate = Convert.ToInt32(this.DateRecruitment),
                Position = this.Position,
            };
            SelectedDepartment.Employees.Add(CreatedEmployee);
            Context.SaveChanges();
            CloseEmployeeAction();
            ResetEmployeeData();
        }

        private void ResetEmployeeData()
        {
            this.EmployeeName = "";
            this.LastName = "";
            this.Birthday = "";
            this.Salary = 0;
            this.MiddleName = "";
            this.DateRecruitment = "";
            this.Position = "";
        }

        public void RefreshEmployees(int departmentId, Employee newEployee)
        {
            var department = Context.Departments.FirstOrDefault(d => d.Id == departmentId);
            var company = Context.Companies.FirstOrDefault(c => c.Id == department.CompanyId);
            department = company.Departments.FirstOrDefault(d => d.Id == departmentId);
            department.Employees.Add(newEployee);
        }
    }
}
