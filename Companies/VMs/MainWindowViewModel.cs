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
        public List<string> AgeSelector { get; set; } = new List<string>() { "Age", "Birth Year" };
        public ObservableCollection<Root> Roots { get; set; } = new();
        public ObservableCollection<Company> Companies { get; set; } 
            = new ObservableCollection<Company>();
        public ObservableCollection<Company?> ReportsCompanies { get; set; } 
            = new ObservableCollection<Company?>();
        public ObservableCollection<Department?> SalaryReportDepartments { get; set; } 
            = new ObservableCollection<Department?>();
        public ObservableCollection<ComboExperienceDTO> ComboExperience { get; set; } 
            = new ObservableCollection<ComboExperienceDTO>();
        public ObservableCollection<int> AgeValues { get; set; } = new ObservableCollection<int>();
        public ObservableCollection<Department> EmployeeInfoDepartments { get; set; } 
            = new ObservableCollection<Department>();

        public AutoCommand DeleteCommand =>
            new AutoCommand(obj => { DeleteCommandExecute(); }, obj =>  DeleteCommandCanExecute());

        public AutoCommand EditCommand =>
            new AutoCommand(obj => { EditCommandExecute(); });

        private object? selectedItem;

        public object? SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));

                if (value is Company company)
                    SelectedCompany = company;
                else if (value is Department department)
                    SelectedDepartment = department;
                else if (value is Employee employee)
                    SelectedEmployee = employee;
            }
        }

        private Company? selectedCompany;
        public Company? SelectedCompany
        {
            get
            {
                return selectedCompany;
            }
            set
            {
                selectedCompany = value;
                OnPropertyChanged(nameof(SelectedCompany));
            }
        }

        private Department? selectedDepartment;
        public Department? SelectedDepartment
        {
            get
            {
                return selectedDepartment;
            }
            set
            {
                selectedDepartment = value;
                OnPropertyChanged(nameof(SelectedDepartment));
            }
        }

        private Employee? selectedEmployee;
        public Employee? SelectedEmployee
        {
            get
            {
                return selectedEmployee;
            }
            set
            {
                selectedEmployee = value;
                if (selectedEmployee != null)
                {
                    RenewEmployeeInfoDepartments(selectedEmployee);
                    FormatEmployeeInfoDepartments(selectedEmployee);
                }

                if (selectedEmployee?.Department != null)
                    EmployeeInfoSelectedDepartment = selectedEmployee.Department;

                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }

        private Department? employeeInfoSelectedDepartment;
        public Department? EmployeeInfoSelectedDepartment
        {
            get
            {
                return employeeInfoSelectedDepartment;
            }
            set
            {
                employeeInfoSelectedDepartment = value;
                OnPropertyChanged(nameof(EmployeeInfoSelectedDepartment));
                if (employeeInfoSelectedDepartment != null && 
                    employeeInfoSelectedDepartment.Id != SelectedEmployee.DepartmentId)
                    ChangeDepartment(SelectedEmployee, employeeInfoSelectedDepartment);
            }
        }

        private int? selectedExperience;
        public int? SelectedExperience 
        { 
            get
            {
                return selectedExperience;
            } 
            set
            {
                selectedExperience = value;
                OnPropertyChanged(nameof(SelectedExperience));
            }
        }

        private Company? salaryReportSelectedCompany;
        public Company? SalaryReportSelectedCompany
        {
            get
            {
                return salaryReportSelectedCompany;
            }
            set
            {
                salaryReportSelectedCompany = value;
                SetComboDepartments();
                OnPropertyChanged(nameof(SalaryReportSelectedCompany));
            }
        }

       
        private Company? comboListCompany;
        public Company? ComboListCompany
        {
            get
            {
                return comboListCompany;
            }
            set
            {
                comboListCompany = value;
                OnPropertyChanged(nameof(ComboListCompany));
            }
        }

        private Department? salaryReportSelectedDepartment;
        public Department? SalaryReportSelectedDepartment
        {
            get
            {
                return salaryReportSelectedDepartment;
            }
            set
            {
                salaryReportSelectedDepartment = value;
                OnPropertyChanged(nameof(SalaryReportSelectedDepartment));
            }
        }

        private int? selectedAge;
        public int? SelectedAge
        {
            get
            {
                return selectedAge;
            }
            set
            {
                selectedAge = value;
                OnPropertyChanged(nameof(SelectedAge));
            }
        }

        private string? ageSelection;
        public string? AgeSelection
        {
            get
            {
                return ageSelection;
            }
            set
            {
                ageSelection = value;
                AgeValuesUpdate(ageSelection);
                OnPropertyChanged(nameof(AgeSelection));
            }
        }

        public void SetCombpoExperience()
        {
            for (int i = -1; i < 48; i++)
                ComboExperience.Add(new ComboExperienceDTO(i));
        }

        private void AgeValuesUpdate(string ageSelection)
        {
            AgeValues.Clear();

            int start = ageSelection == "Age" ? 18 : 1957;
            int limit = ageSelection == "Age" ? 65 : 2004;

            for (int i = start; i < limit; i++)
                AgeValues.Add(i);
        }

        private void SetComboDepartments()
        {
            if (SalaryReportSelectedCompany != null)
            {
                SalaryReportDepartments.Clear();
                SalaryReportDepartments.Add(new Department() { Name = " " });

                foreach (var item in SalaryReportSelectedCompany?.Departments)
                    SalaryReportDepartments.Add(item);
            }
        }

        private void RenewEmployeeInfoDepartments(Employee selectedEmployee)
        {
            var companyDepartments = GetCompanysDepartmentsList(selectedEmployee);
            var departmentsToAdd = new List<Department>();

            foreach (Department department in Context.Departments)
                if (EmployeeInfoDepartments.FirstOrDefault(d => d.Id == department.Id) == null)
                    departmentsToAdd.Add(department);
            foreach (Department dep in departmentsToAdd)
                EmployeeInfoDepartments.Add(dep);
        }

        private void FormatEmployeeInfoDepartments(Employee selectedEmployee)
        {
            var companyDepartments = GetCompanysDepartmentsList(selectedEmployee);
            var departmentsToDelete = new List<Department>();

            foreach (Department department in EmployeeInfoDepartments)
                if (companyDepartments.FirstOrDefault(d => d.Id == department.Id) == null)
                    departmentsToDelete.Add(department);
            foreach (Department dep in departmentsToDelete)
                EmployeeInfoDepartments.Remove(dep);
        }

        private List<Department> GetCompanysDepartmentsList(Employee employee)
        {
            var departmentId = employee.DepartmentId;
            var companyId = Context?.Departments?.FirstOrDefault(d => d.Id == departmentId).CompanyId;

            return Context.Companies.FirstOrDefault(c => c.Id == companyId).Departments.ToList();
        }

        private void ChangeDepartment(Employee selectedEmployee, Department comboDepartment)
        {
            var departmentId = Context?.Employees?.FirstOrDefault(e => e.Id == selectedEmployee.Id)
                                ?.DepartmentId;
            var department = Context?.Departments?.FirstOrDefault(d => d.Id == departmentId);

            department?.Employees?.Remove(selectedEmployee);

            var newDepartment = Context?.Departments.FirstOrDefault(d => d.Id == comboDepartment.Id);
            newDepartment?.Employees?.Add(selectedEmployee);
            Context?.SaveChanges();
        }

        public MainWindowViewModel()
        {
            Context = new CompaniesContext();
            Context.Database.EnsureCreated();
            Companies = new ObservableCollection<Company>(Context.Companies
                                                         .Include(c=>c.Departments)
                                                         .ThenInclude(d => d.Employees).ToList());
            SetReportsCompanies();
            SetCombpoExperience();
            SelectedExperience = -1;
            EmployeeInfoDepartments = new ObservableCollection<Department>(Context.Departments.ToList());
            Root root = new();
            root.Companies = Companies;
            Roots.Add(root);
        }

        private void SetReportsCompanies()
        {
            ReportsCompanies.Clear();
            ReportsCompanies.Add(new Company() { Name = " "});

            foreach (var item in Companies)
                ReportsCompanies.Add(item);
        }

        public void DeleteCommandExecute()
        {
            if (SelectedItem is Company company)
            {
                RemoveCompanyContext(company);
                Companies.Remove(company);
                SetReportsCompanies();
            }
            else if (SelectedItem is Department department)
            {
                RemoveDepartmentContext(department);
                RemoveDepartmentView(department);
                SetComboDepartments();
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
            return SelectedItem != null && (SelectedItem is Company || 
                                            SelectedItem is Department|| 
                                            SelectedItem is Employee);
        }

        public void EditCommandExecute()
        {
            Context.SaveChanges();
        }
    }

    public class ComboExperienceDTO
    {
        public int Value { get; set; }

        public ComboExperienceDTO(int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value >= 0 ? Value.ToString() : " ";
        }
    }
}
