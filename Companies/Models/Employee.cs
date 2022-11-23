﻿namespace Companies.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Lastname { get; set; }
        public string? Name { get; set; }
        public string? Middlename { get; set; }
        public string? BirthDay { get; set; }
        public string? Date { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department? Department { get; set; }
        public string? Position { get; set; }
        public double? Salary { get; set; }
    }
}