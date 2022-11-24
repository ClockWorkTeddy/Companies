using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Companies.Models;

namespace Companies
{
    public class CompaniesContext : DbContext
    {
        private static CompaniesContext instance = null;
        public DbSet<Company> Companies {get; set;}
        public DbSet<Department> Departments { get; set;}
        public DbSet<Employee> Employees { get; set;}

        public object SelectedItem { get; set; }
        public EventHandler Refresh { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = companies.db");
        }

        public static CompaniesContext GetInstance()
        {
            if (instance == null)
                instance = new CompaniesContext();
            
            return instance;
        }

        public void OnRefresh()
        {
            Refresh.Invoke(this, new EventArgs());
        }

        public void PlaceDepartment(Department newDepatrment)
        {
            if (SelectedItem is Company company)
            {
                var comp = Companies.FirstOrDefault(c => c.Id == company.Id);
                comp.Departments.Add(newDepatrment);
                SaveChanges();
            }
        }

        public void PlaceEmployee(Employee employee)
        {
            if (SelectedItem is Department department)
            {
                var company = Companies.FirstOrDefault(c => c.Id == department.Id);
                var dep = company.Departments.FirstOrDefault(d => d.Id == department.Id);
                dep.Employees.Add(employee);
                SaveChanges();
            }
        }
    }
}
