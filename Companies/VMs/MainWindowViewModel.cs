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
        public ObservableCollection<Root> Roots { get; set; } = new();
        public ObservableCollection<Company> Companies { get; set; } = new ObservableCollection<Company>();
        public CompaniesContext Context { get; set; }

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
                {
                    SelectedCompany = selectedItem as Company;
                }
                else if (value is Department department)
                {
                    SelectedDepartment.Clear();
                    SelectedDepartment.Add(department);
                }
                Context.SelectedItem = selectedItem;
            }
        }

        public Company SelectedCompany { get; set; } = new Company();
        public ObservableCollection<Department> SelectedDepartment { get; set; } = new ObservableCollection<Department>();
        public MainWindowViewModel()
        {
            Context = CompaniesContext.GetInstance();
            Context.Database.EnsureCreated();
            Context.Refresh += RefreshExecute;
            Companies = new ObservableCollection<Company>(Context.Companies.Include(c=>c.Departments).ToList());
            Root root = new();
            root.Companies = Companies;
            Roots.Add(root);
            RefreshCollection();
        }

        public void RefreshCollection()
        {
            foreach (var company in Context.Companies.ToList())
                if (Companies.FirstOrDefault(c => c.Name == company.Name) == null)
                    Companies.Add(company);
        }

        public void RefreshExecute(object sender, EventArgs e)
        {
            RefreshCollection();
        }

        public void DeleteCommandExecute()
        {
            if (SelectedItem is Company company)
            {
                Context.Companies.Remove(company);
                Context.SaveChanges();
                Companies.Remove(company);
            }
            else if (SelectedItem is Department department)
            {
                var contextCompany = Context.Companies.FirstOrDefault(c => c.Id == department.CompanyId);
                contextCompany.Departments.Remove(department);
                Context.SaveChanges();
                var viewCompany = Companies.FirstOrDefault(c => c.Id == department.CompanyId);
                viewCompany.Departments.Remove(department);
            }
        }

        public bool DeleteCommandCanExecute()
        {
            return SelectedItem != null;
        }

        public void EditCommandExecute()
        {
            Context.SaveChanges();
        }
    }

}
