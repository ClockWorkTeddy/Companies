using Companies.Models;
using Simplified;
using System;

namespace Companies.VMs
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        //private string name;
        //private string date;
        //private string adress;

        public RelayCommand AddCompanyCommand => GetCommand(AddCompanyExecute, AddCompanyCanExecute);

        public RelayCommand CancelCompanyCommand => GetCommand(CancelCompanyCommandExecute);

        private void CancelCompanyCommandExecute()
        {
            CloseCompanyAction();
        }

        public Action CloseCompanyAction { get; set; }

        public string? Name { get => Get<string>(); set => Set(value); }
        //{
        //    get
        //    {
        //        return name;
        //    }
        //    set
        //    {
        //        name = value;
        //        OnPropertyChanged("Name");
        //    }
        //}
        public string? Date { get => Get<string>(); set => Set(value); }
        //{
        //    get
        //    {
        //        return date;
        //    }
        //    set
        //    {
        //        date = value;
        //        OnPropertyChanged("Date");
        //    }
        //}

        public string? Adress { get => Get<string>(); set => Set(value); }
        //{
        //    get
        //    {
        //        return adress;
        //    }
        //    set
        //    {
        //        adress = value;
        //        OnPropertyChanged("Adress");
        //    }
        //}

        private bool AddCompanyCanExecute()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Date) &&
                   !string.IsNullOrWhiteSpace(Adress);
        }

        private void AddCompanyExecute()
        {
            Company newCompany = new Company()
            {
                Name = this.Name,
                Date = this.Date,
                Adress = this.Adress
            };

            context.Companies.Add(newCompany);
            context.SaveChanges();
            Companies.Add(newCompany);
            CloseCompanyAction();
            ResetCompanyData();
        }

        private void ResetCompanyData()
        {
            this.Name = "";
            this.Date = "";
            this.Adress = "";
        }

        private void ResetDepartmentData()
        {
            this.DepartmentName = "";
        }
    }
}
