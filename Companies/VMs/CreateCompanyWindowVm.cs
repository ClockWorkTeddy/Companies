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
        private string name;
        private string date;
        private string adress;

        public AutoCommand AddCompanyCommand =>
            new AutoCommand(obj => { AddCompanyExecute(); }, obj => AddCompanyCanExecute());

        public AutoCommand CancelCommand =>
            new AutoCommand(obj => { CancelCommandExecute(); });

        private void CancelCommandExecute()
        {
            CloseAction();
        }

        public Action CloseAction { get; set; }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        public string Adress
        {
            get
            {
                return adress;
            }
            set
            {
                adress = value;
                OnPropertyChanged("Adress");
            }
        }

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

            Context.Companies.Add(newCompany);
            Context.SaveChanges();
            Context.OnRefresh();
            CloseAction();
        }
    }
}
