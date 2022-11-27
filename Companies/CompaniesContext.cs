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

        public List<SalaryReportDTO> SalaryReport( int companyId, int departmentId)
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

        public List<SalaryReportDTO> SalaryReport(int companyId)
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

        public List<SalaryReportDTO> SalaryReport()
        {
            var query = (from emp in Employees
                         join dep in Departments on emp.DepartmentId equals dep.Id
                         join com in Companies on dep.CompanyId equals com.Id
                         select new SalaryReportDTO(com.Name, dep.Name, emp.Name, emp.Salary)).ToList();

            return query;
        }

        public List<EmployeeListDTO> EmployeeReportAge(int companyId, int? experience, int? value)
        {
            if (experience != -1)
            {
                if (value != null)
                    return EmployeeReportAgeExpVal(companyId, value, experience);
                else
                    return EmployeeReportAgeExp(companyId, experience);
            }
            else
            {
                if (value != null)
                    return EmployeeReportAgeVal(companyId, value);
                else
                    return EmployeeReportDefault(companyId);
            }
        }

        public List<EmployeeListDTO> EmployeeReportBirthYear(int companyId, int? experience, 
                                                             int? value)
        {
            if (experience != -1)
            {
                if (value != null)
                    return EmployeeReportBirthYearExpVal(companyId, experience, value);
                else
                    return EmployeeReportBirthYearExp(companyId, experience);
            }
            else
            {
                if (value != null)
                    return EmployeeReportBirthYearVal(companyId, value);
                else
                    return EmployeeReportDefault(companyId);
            }
        }

        private List<EmployeeListDTO> EmployeeReportAgeExpVal(int companyId, int? value, 
                                                              int? experience)
        {
            var result = (from com in Companies
                          join dep in Departments on com.Id equals dep.CompanyId
                          join emp in Employees on dep.Id equals emp.DepartmentId
                          where com.Id == companyId && 
                                DateTime.Now.Year - emp.BirthDay == value &&
                                DateTime.Now.Year - emp.Date == experience
                          select new EmployeeListDTO(com.Name, dep.Name, emp.Name,
                                                     emp.Date, emp.BirthDay)).ToList();
            return result;
        }

        private List<EmployeeListDTO> EmployeeReportAgeExp(int companyId, int? experience)
        {
            var result = (from com in Companies
                          join dep in Departments on com.Id equals dep.CompanyId
                          join emp in Employees on dep.Id equals emp.DepartmentId
                          where com.Id == companyId && 
                                DateTime.Now.Year - emp.Date == experience
                          select new EmployeeListDTO(com.Name, dep.Name, emp.Name,
                                                     emp.Date, emp.BirthDay)).ToList();
            return result;
        }

        private List<EmployeeListDTO> EmployeeReportAgeVal(int companyId, int? value)
        {
            var result = (from com in Companies
                          join dep in Departments on com.Id equals dep.CompanyId
                          join emp in Employees on dep.Id equals emp.DepartmentId
                          where com.Id == companyId && 
                                DateTime.Now.Year - emp.BirthDay == value
                          select new EmployeeListDTO(com.Name, dep.Name, emp.Name, 
                                                     emp.Date, emp.BirthDay)).ToList();
            return result;
        }

        private List<EmployeeListDTO> EmployeeReportBirthYearExpVal(int companyId, int? experience, 
                                                                    int? value)
        {
            var result = (from com in Companies
                          join dep in Departments on com.Id equals dep.CompanyId
                          join emp in Employees on dep.Id equals emp.DepartmentId
                          where emp.BirthDay == value && com.Id == companyId &&
                                DateTime.Now.Year - emp.Date == experience
                          select new EmployeeListDTO(com.Name, dep.Name, emp.Name,
                                                     emp.Date, emp.BirthDay)).ToList();
            return result;
        }

        private List<EmployeeListDTO> EmployeeReportBirthYearExp(int companyId, int? experience)
        {
            var result = (from com in Companies
                          join dep in Departments on com.Id equals dep.CompanyId
                          join emp in Employees on dep.Id equals emp.DepartmentId
                          where com.Id == companyId && 
                                DateTime.Now.Year - emp.Date == experience
                          select new EmployeeListDTO(com.Name, dep.Name, emp.Name,
                                                     emp.Date, emp.BirthDay)).ToList();
            return result;
        }

        private List<EmployeeListDTO> EmployeeReportBirthYearVal(int companyId, int? value)
        {
            var result = (from com in Companies
                          join dep in Departments on com.Id equals dep.CompanyId
                          join emp in Employees on dep.Id equals emp.DepartmentId
                          where emp.BirthDay == value &&
                                com.Id == companyId
                          select new EmployeeListDTO(com.Name, dep.Name, emp.Name,
                                                     emp.Date, emp.BirthDay)).ToList();
            return result;
        }

        private List<EmployeeListDTO> EmployeeReportDefault(int companyId)
        {
            var result = (from com in Companies
                          join dep in Departments on com.Id equals dep.CompanyId
                          join emp in Employees on dep.Id equals emp.DepartmentId
                          where com.Id == companyId
                          select new EmployeeListDTO(com.Name, dep.Name, emp.Name,
                                                     emp.Date, emp.BirthDay)).ToList();
            return result;
        }
    }
}
