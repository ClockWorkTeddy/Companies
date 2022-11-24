using Companies.Models;
using Simplified;
using System;
using System.Linq;

namespace Companies.VMs
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public Employee CreatedEmployee { get; set; }
        public RelayCommand AddEmployeeCommand => GetCommand(AddEmployeeExecute, AddEmployeeCanExecute);

        public RelayCommand CancelEmployeeCommand => GetCommand(CancelEmployeeCommandExecute);

        private void CancelEmployeeCommandExecute()
        {
            CloseEmployeeAction();
        }

        public Action CloseEmployeeAction { get; set; }

        //private string employeeName;
        public string? EmployeeName { get => Get<string>(); set => Set(value); }
        //{
        //    get
        //    {
        //        return employeeName;
        //    }
        //    set
        //    {
        //        employeeName = value;
        //        OnPropertyChanged("EmployeeName");
        //    }
        //}
        //private string lastName;
        public string? LastName { get => Get<string>(); set => Set(value); }
        //{
        //    get
        //    {
        //        return lastName;
        //    }
        //    set
        //    {
        //        lastName = value;
        //        OnPropertyChanged("LastName");
        //    }
        //}

        //private string middleName;
        public string? MiddleName { get => Get<string>(); set => Set(value); }
        //{
        //    get
        //    {
        //        return middleName;
        //    }
        //    set
        //    {
        //        middleName = value;
        //        OnPropertyChanged("MiddleName");
        //    }
        //}

        //private string dateRecruitment;
        public string? DateRecruitment { get => Get<string>(); set => Set(value); }
        //{
        //    get
        //    {
        //        return dateRecruitment;
        //    }
        //    set
        //    {
        //        dateRecruitment = value;
        //        OnPropertyChanged("DateRecruitment");
        //    }
        //}

        //private string birthday;
        public string? Birthday { get => Get<string>(); set => Set(value); }
        //{
        //    get
        //    {
        //        return birthday;
        //    }
        //    set
        //    {
        //        birthday = value;
        //        OnPropertyChanged("Birthday");
        //    }
        //}

        //private string position;
        public string? Position { get => Get<string>(); set => Set(value); }
        //{
        //    get
        //    {
        //        return position;
        //    }
        //    set
        //    {
        //        position = value;
        //        OnPropertyChanged("Position");
        //    }
        //}

        //private double salary;
        public double Salary { get => Get<double>(); set => Set(value); }
        //{
        //    get
        //    {
        //        return salary;
        //    }
        //    set
        //    {
        //        salary = value;
        //        OnPropertyChanged("Salary");
        //    }
        //}

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
                BirthDay = this.Birthday,
                Salary = this.Salary,
                Middlename = this.MiddleName,
                Date = this.DateRecruitment,
                Position = this.Position
            };
            SelectedDepartment.Employees.Add(CreatedEmployee);
            context.SaveChanges();
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
            var department = context.Departments.FirstOrDefault(d => d.Id == departmentId);
            var company = context.Companies.FirstOrDefault(c => c.Id == department.CompanyId);
            department = company.Departments.FirstOrDefault(d => d.Id == departmentId);
            department.Employees.Add(newEployee);
        }
    }
}
