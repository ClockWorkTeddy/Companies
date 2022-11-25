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
        public DbSet<Company> Companies {get; set;}
        public DbSet<Department> Departments { get; set;}
        public DbSet<Employee> Employees { get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = companies.db");
        }

        public void Report()
        {
            var report = from emp in Employees
                         join dep in Departments on emp.DepartmentId equals dep.Id
                         join com in Companies on dep.CompanyId equals com.Id
                         select new
                         {
                             comName = com.Name, 
                             depname = dep.Name, 
                             empName = emp.Name, 
                             salary = emp.Salary
                         };
        }
    }
}
