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

        private void CancelEmployeeCommandExecute() =>
            CloseEmployeeAction();

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
                OnPropertyChanged(nameof(EmployeeName));
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
                OnPropertyChanged(nameof(LastName));
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
                OnPropertyChanged(nameof(MiddleName));
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
                OnPropertyChanged(nameof(DateRecruitment));
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
                OnPropertyChanged(nameof(Birthday));
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
                OnPropertyChanged(nameof(Position));
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
                OnPropertyChanged(nameof(Salary));
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
                Name            = EmployeeName,
                Lastname        = LastName,
                BirthYear       = Convert.ToInt32(Birthday),
                Salary          = Salary,
                Middlename      = MiddleName,
                ReqruitmentDate = Convert.ToInt32(DateRecruitment),
                Position        = Position,
            };
            SelectedDepartment?.Employees?.Add(CreatedEmployee);
            Context.SaveChanges();
            CloseEmployeeAction();
            ResetEmployeeData();
        }

        private void ResetEmployeeData()
        {
            EmployeeName = "";
            LastName = "";
            Birthday = "";
            Salary = 0;
            MiddleName = "";
            DateRecruitment = "";
            Position = "";
        }
    }
}
