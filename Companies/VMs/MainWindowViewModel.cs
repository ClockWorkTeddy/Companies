﻿using Companies.Models;
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
        public ObservableCollection<Company> Companies { get; set; } 
            = new ObservableCollection<Company>();
        public ObservableCollection<Company?> ComboCompanies { get; set; } 
            = new ObservableCollection<Company?>();
        public ObservableCollection<Department?> ComboDepartments { get; set; } 
            = new ObservableCollection<Department?>();
        public List<string> AgeSelector { get; set; } = new List<string>() { "Age", "Birth Year"};
        public ObservableCollection<ComboExperienceDTO> ComboExperience { get; set; } 
            = new ObservableCollection<ComboExperienceDTO>();
        public ObservableCollection<int> AgeValues { get; set; } = new ObservableCollection<int>();
        public ObservableCollection<Department> Departments { get; set; } 
            = new ObservableCollection<Department>();
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

        private int selectedExperience;
        public int SelectedExperience 
        { 
            get
            {
                return selectedExperience;
            } 
            set
            {
                selectedExperience = value;
                OnPropertyChanged("SelectedExperience");
            }
        }


        private Company? comboCompany;
        public Company? ComboCompany
        {
            get
            {
                return comboCompany;
            }
            set
            {
                comboCompany = value;
                SetComboDepartments();
                OnPropertyChanged("ComboCompany");
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
                OnPropertyChanged("ComboListCompany");
            }
        }


        private Department? comboRepDepartment;
        public Department? ComboRepDepartment
        {
            get
            {
                return comboRepDepartment;
            }
            set
            {
                comboRepDepartment = value;
                OnPropertyChanged("ComboRepDepartment");
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
                OnPropertyChanged("SelectedAge");
            }
        }

        private string ageSelection;
        public string AgeSelection
        {
            get
            {
                return ageSelection;
            }
            set
            {
                ageSelection = value;
                AgeValuesUpdate(ageSelection);
                OnPropertyChanged("AgeSelection");
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
            if (ComboCompany != null)
            {
                ComboDepartments.Clear();
                ComboDepartments.Add(new Department() { Name = " " });

                foreach (var item in ComboCompany.Departments)
                    ComboDepartments.Add(item);
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
            SetComboCompanies();
            SetCombpoExperience();
            SelectedExperience = -1;
            Departments = new ObservableCollection<Department>(Context.Departments.ToList());
            Root root = new();
            root.Companies = Companies;
            Roots.Add(root);
        }

        private void SetComboCompanies()
        {
            ComboCompanies.Clear();
            ComboCompanies.Add(new Company() { Name = " "});

            foreach (var item in Companies)
                ComboCompanies.Add(item);
        }

        public void DeleteCommandExecute()
        {
            if (SelectedItem is Company company)
            {
                RemoveCompanyContext(company);
                Companies.Remove(company);
                SetComboCompanies();
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
