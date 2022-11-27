using Companies.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Companies.VMs
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<EmployeeListDTO> EmployeeListDTOs { get; set; } = new ObservableCollection<EmployeeListDTO>();

        public AutoCommand EmloyeeListCommand =>
            new AutoCommand(obj => { EmloyeeListExecute(); });
        private void EmloyeeListExecute()
        {
            int companyId = ComboListCompany == null ? 0 : ComboListCompany.Id;
            int experience = SelectedExperience;
            List<EmployeeListDTO> reportResult = new List<EmployeeListDTO>();

            if (companyId != 0)
            {
                if (AgeSelection == "Age")
                    reportResult = Context.EmployeeReportAge(companyId, experience, SelectedAge);
                else
                    reportResult = Context.EmployeeReportBirthYear(companyId, experience, SelectedAge);

                EmployeeListDTOs.Clear();

                foreach (var item in reportResult)
                    EmployeeListDTOs.Add(item);
            }
            else
            {
                MessageBox.Show("Select a company!");
            }
        }
        private bool EmloyeeListExecuteCanExecute()
        {
            return Context.Companies.Count() != 0 &&
                   Context.Departments.Count() != 0 &&
                   Context.Employees.Count() != 0;
        }


    }
}
