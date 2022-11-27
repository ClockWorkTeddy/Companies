using Companies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


namespace Companies.VMs
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public Action CloseCompanyAction { get; set; }
        public AutoCommand AddCompanyCommand =>
            new AutoCommand(obj => { AddCompanyExecute(); }, obj => AddCompanyCanExecute());

        public AutoCommand CancelCompanyCommand =>
            new AutoCommand(obj => { CancelCompanyCommandExecute(); });

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string date; 
        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private string adress;
        public string Adress
        {
            get
            {
                return adress;
            }
            set
            {
                adress = value;
                OnPropertyChanged(nameof(Adress));
            }
        }


        private void CancelCompanyCommandExecute() =>
            CloseCompanyAction();

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
                Name = Name,
                Date = Date,
                Adress = Adress
            };

            Context.Companies.Add(newCompany);
            Context.SaveChanges();
            Companies.Add(newCompany);
            SetReportsCompanies();
            CloseCompanyAction();
            ResetCompanyData();
        }

        private void ResetCompanyData()
        {
            Name = "";
            Date = "";
            Adress = "";
        }
    }
}
