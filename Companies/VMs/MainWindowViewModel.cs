using Companies.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Windows;
using Microsoft.Identity.Client;

namespace Companies.VMs
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private CompaniesContext Context;

        public ObservableCollection<Root> Roots { get; set; } = new();
        public ObservableCollection<Company> Companies { get; set; } = new ObservableCollection<Company>();
        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();
        public AutoCommand DeleteCommand =>
            new AutoCommand(obj => { DeleteCommandExecute(); },
                            obj =>  DeleteCommandCanExecute());

        public AutoCommand EditCommand =>
            new AutoCommand(obj => { EditCommandExecute(); });


        private object selectedItem;

        public object SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");

                if (value is Company company)
                    SelectedCompany = company;
                else if (value is Department department)
                    SelectedDepartment = department;
                else if (value is Employee employee)
                {
                    SelectedEmployee = employee;
                }
            }
        }
        private Company selectedCompany;
        public Company SelectedCompany
        {
            get
            {
                return selectedCompany;
            }
            set
            {
                selectedCompany = value;
                OnPropertyChanged("SelectedCompany");
            }
        }
        private Department selectedDepartment;
        public Department SelectedDepartment
        {
            get
            {
                return selectedDepartment;
            }
            set
            {
                selectedDepartment = value;
                OnPropertyChanged("SelectedDepartment");
            }
        }

        private Employee selectedEmployee;
        public Employee SelectedEmployee
        {
            get
            {
                return selectedEmployee;
            }
            set
            {
                selectedEmployee = value;
                RenewDepartments(selectedEmployee);
                FormatDepartments(selectedEmployee);
                ComboDepartment = selectedEmployee.Department;
                OnPropertyChanged("SelectedEmployee");
            }
        }

        private void RenewDepartments(Employee selectedEmployee)
        {
            var companyDepartments = GetCompDeps(selectedEmployee);
            var departmentsToAdd = new List<Department>();

            foreach (Department department in Context.Departments)
                if (Departments.FirstOrDefault(d => d.Id == department.Id) == null)
                    departmentsToAdd.Add(department);
            foreach (Department dep in departmentsToAdd)
                Departments.Add(dep);
        }

        private void FormatDepartments(Employee selectedEmployee)
        {
            var companyDepartments = GetCompDeps(selectedEmployee);
            var departmentsToDelete = new List<Department>();

            foreach (Department department in Departments)
                if (companyDepartments.FirstOrDefault(d => d.Id == department.Id) == null)
                    departmentsToDelete.Add(department);
            foreach (Department dep in departmentsToDelete)
                Departments.Remove(dep);
        }

        private List<Department> GetCompDeps(Employee employee)
        {
            var departmentId = employee.DepartmentId;
            var companyId = Context.Departments.FirstOrDefault(d => d.Id == departmentId).CompanyId;

            return Context.Companies.FirstOrDefault(c => c.Id == companyId).Departments.ToList();
        }

        private Department comboDepartment;
        public Department ComboDepartment
        {
            get
            {
                return comboDepartment;
            }
            set
            {
                comboDepartment = value;
                OnPropertyChanged("ComboDepartment");
                if (comboDepartment != null && comboDepartment.Id != SelectedEmployee.DepartmentId)
                    ChangeDepartment(SelectedEmployee, comboDepartment);
            }
        }

        private void ChangeDepartment(Employee selectedEmployee, Department comboDepartment)
        {
            var departmentId = Context.Employees.FirstOrDefault(e => e.Id == selectedEmployee.Id).DepartmentId;
            var department = Context.Departments.FirstOrDefault(d => d.Id == departmentId);
            department.Employees.Remove(selectedEmployee);
            var newDepartment = Context.Departments.FirstOrDefault(d => d.Id == comboDepartment.Id);
            newDepartment.Employees.Add(selectedEmployee);
            Context.SaveChanges();
        }

        public MainWindowViewModel()
        {
            Context = new CompaniesContext();
            Context.Database.EnsureCreated();
            Companies = new ObservableCollection<Company>(Context.Companies.Include(c=>c.Departments).ThenInclude(d => d.Employees).ToList());
            Departments = new ObservableCollection<Department>(Context.Departments.ToList());
            Root root = new();
            root.Companies = Companies;
            Roots.Add(root);
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
                int departmentId = Context.Departments.FirstOrDefault(d => d.Id == employee.DepartmentId).Id;
                int companyId = Context.Departments.FirstOrDefault(d => d.Id == employee.DepartmentId).CompanyId;
                RemoveEmployeeContext(companyId, departmentId, employee);
                RemoveEmployeeView(companyId, departmentId, employee);
            }
        }

        private void RemoveCompanyContext(Company company)
        {
            Context.Companies.Remove(company);
            Context.SaveChanges();
        }

        private void RemoveDepartmentContext(Department department)
        {
            var contextCompany = Context.Companies.FirstOrDefault(c => c.Id == department.CompanyId);
            contextCompany.Departments.Remove(department);
            Context.SaveChanges();
        }

        private void RemoveDepartmentView(Department department)
        {
            var viewCompany = Companies.FirstOrDefault(c => c.Id == department.CompanyId);
            viewCompany.Departments.Remove(department);
        }

        private void RemoveEmployeeContext(int companyId, int departmentId, Employee employee)
        {
            var company = Context.Companies.FirstOrDefault(c => c.Id == companyId);
            var department = company.Departments.FirstOrDefault(d => d.Id == departmentId);
            department.Employees.Remove(employee);
            Context.SaveChanges();
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
            Context.SaveChanges();
        }

        public void Rep()
        {
            Context.Report();
        }
    }

}
