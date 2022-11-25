using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Models
{
    public class SalaryReportDTO
    {
        public string? CompanyName { get; set; }
        public string? DepartmentName { get; set; }
        public string? EmployeeName { get; set; }
        public double? Salary { get; set; }

        public SalaryReportDTO(string? companyName, string? departmentName, string? employeeName, double? salary)
        {
            CompanyName = companyName;
            DepartmentName = departmentName;
            EmployeeName = employeeName;
            Salary = salary;
        }
    }
}
