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
            optionsBuilder.UseSqlite("Data Source = Newcompanies.db");
        }
    }
}
