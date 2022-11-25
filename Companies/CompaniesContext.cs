using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Companies.Models;
using System.Windows;

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

        public List<SalaryReportDTO> Report( int companyId, int departmentId)
        {
            List<SalaryReportDTO> result = new List<SalaryReportDTO>();
            var query =  (from emp in Employees
                          join dep in Departments on emp.DepartmentId equals dep.Id
                          join com in Companies on dep.CompanyId equals com.Id
                          where com.Id == companyId && dep.Id == departmentId
                          select new
                          {
                              CompanyName = com.Name, 
                              DepartmentName = dep.Name, 
                              EmployeeName = emp.Name, 
                              Salary = emp.Salary
                          }).ToList();

            foreach (var item in query)
                result.Add(new SalaryReportDTO(item.CompanyName, item.DepartmentName, item.EmployeeName, item.Salary));

            return result;
        }

        public List<SalaryReportDTO> Report(int companyId)
        {
            List<SalaryReportDTO> result = new List<SalaryReportDTO>();
            var query = (from emp in Employees
                         join dep in Departments on emp.DepartmentId equals dep.Id
                         join com in Companies on dep.CompanyId equals com.Id
                         where com.Id == companyId
                         select new
                         {
                             CompanyName = com.Name,
                             DepartmentName = dep.Name,
                             EmployeeName = emp.Name,
                             Salary = emp.Salary
                         }).ToList();

            foreach (var item in query)
                result.Add(new SalaryReportDTO(item.CompanyName, item.DepartmentName, item.EmployeeName, item.Salary));

            return result;
        }

        public List<SalaryReportDTO> Report()
        {
            List<SalaryReportDTO> result = new List<SalaryReportDTO>();
            var query = (from emp in Employees
                         join dep in Departments on emp.DepartmentId equals dep.Id
                         join com in Companies on dep.CompanyId equals com.Id
                         select new
                         {
                             CompanyName = com.Name,
                             DepartmentName = dep.Name,
                             EmployeeName = emp.Name,
                             Salary = emp.Salary
                         }).ToList();

            foreach (var item in query)
                result.Add(new SalaryReportDTO(item.CompanyName, item.DepartmentName, item.EmployeeName, item.Salary));

            return result;
        }
    }
}
