using Companies.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Simplified;
using System.Collections.ObjectModel;
using System.Linq;

namespace Companies.VMs
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly CompaniesContext context;

        //public ObservableCollection<Root> Roots { get; set; } = new();
        public ObservableCollection<Company> Companies { get; set; } = new ObservableCollection<Company>();

        public RelayCommand DeleteCommand => GetCommand(DeleteCommandExecute, DeleteCommandCanExecute);

        public RelayCommand EditCommand => GetCommand(EditCommandExecute);


        //private object selectedItem;

        public object? SelectedItem
        {
            get
            {
                return Get<object>();
            }
            set
            {
                //selectedItem = value;
                //OnPropertyChanged("SelectedItem");
                if (Set(value))
                {
                    Company? company = value as Company;
                    if (company is null)
                    {
                        Department? department = value as Department;
                        if (department is null)
                        {
                            Employee? employee = value as Employee;

                            SelectedEmployee = employee;

                            department = employee?.Department;
                        }
                        SelectedDepartment = department;
                        company = department?.Company;
                    }
                    SelectedCompany = company;
                }
            }
        }

        public Company? SelectedCompany { get => Get<Company>(); private set => Set(value); }
        public Department? SelectedDepartment { get => Get<Department>(); private set => Set(value); }
        public Employee? SelectedEmployee { get => Get<Employee>(); private set => Set(value); }
        public MainWindowViewModel()
        {
            context = new CompaniesContext();
            context.Database.EnsureCreated();
            Companies = new ObservableCollection<Company>(context.Companies.Include(c => c.Departments).ThenInclude(d => d.Employees).ToList());
            //Root root = new();
            //root.Companies = Companies;
            //Roots.Add(root);
        }

        public void DeleteCommandExecute()
        {
            if (SelectedItem is Company company)
            {
                RemoveCompanyContext(company);
                Companies.Remove(company);
            }
            else if (SelectedItem is Department department)
            {
                RemoveDepartmentContext(department);
                RemoveDepartmentView(department);
            }
            else if (SelectedItem is Employee employee)
            {
                int departmentId = context.Departments.FirstOrDefault(d => d.Id == employee.DepartmentId).Id;
                int companyId = context.Departments.FirstOrDefault(d => d.Id == employee.DepartmentId).CompanyId;
                RemoveEmployeeContext(companyId, departmentId, employee);
                RemoveEmployeeView(companyId, departmentId, employee);
            }
        }

        private void RemoveCompanyContext(Company company)
        {
            context.Companies.Remove(company);
            context.SaveChanges();
        }

        private void RemoveDepartmentContext(Department department)
        {
            var contextCompany = context.Companies.FirstOrDefault(c => c.Id == department.CompanyId);
            contextCompany.Departments.Remove(department);
            context.SaveChanges();
        }

        private void RemoveDepartmentView(Department department)
        {
            var viewCompany = Companies.FirstOrDefault(c => c.Id == department.CompanyId);
            viewCompany.Departments.Remove(department);
        }

        private void RemoveEmployeeContext(int companyId, int departmentId, Employee employee)
        {
            var company = context.Companies.FirstOrDefault(c => c.Id == companyId);
            var department = company.Departments.FirstOrDefault(d => d.Id == departmentId);
            department.Employees.Remove(employee);
            context.SaveChanges();
        }

        private void RemoveEmployeeView(int companyId, int departmentId, Employee employee)
        {
            var company = Companies.FirstOrDefault(c => c.Id == companyId);
            var department = company.Departments.FirstOrDefault(d => d.Id == departmentId);
            department.Employees.Remove(employee);
        }

        public bool DeleteCommandCanExecute()
        {
            return SelectedItem != null;
        }

        public void EditCommandExecute()
        {
            context.SaveChanges();
        }
    }

}
