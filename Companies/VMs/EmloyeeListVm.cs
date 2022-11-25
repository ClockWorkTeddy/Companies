using Companies.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.VMs
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<SalaryReportDTO> EmployeeListDTOs { get; set; } = new ObservableCollection<SalaryReportDTO>();

        public AutoCommand EmloyeeListCommand =>
            new AutoCommand(obj => { EmloyeeListExecute(); });
        private void EmloyeeListExecute()
        {
            int companyId = ComboListCompany == null ? 0 : ComboListCompany.Id;
            int experience = selectedExperience;
            List<SalaryReportDTO> reportResult = new List<SalaryReportDTO>();
        }

    }
}
