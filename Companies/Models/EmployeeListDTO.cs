using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Models
{
    public class EmployeeListDTO
    {
        public string? CompanyName { get; set; }
        public string? DepartmentName { get; set; }
        public string? EmployeeName { get; set; }
        public int? Experience { get; set; }
        public int? Age { get; set; }
        public EmployeeListDTO(string? companyName, string? departmentName, string? employeeName, int? reqruitmentYear, int? birthYear)
        {
            CompanyName = companyName;
            DepartmentName = departmentName;
            EmployeeName = employeeName;

            int currentYear = DateTime.Now.Year;
            Experience = currentYear - reqruitmentYear;
            Age = currentYear - birthYear;
        }

    }
}
