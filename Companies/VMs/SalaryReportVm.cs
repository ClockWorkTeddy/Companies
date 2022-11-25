using Companies.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.VMs
{
    public partial class MainWindowViewModel: ViewModelBase
    {
        public ObservableCollection<SalaryReportDTO> SalaryReportDTOs { get; set; } = new ObservableCollection<SalaryReportDTO>();

        public AutoCommand SalaryReportCommand =>
            new AutoCommand(obj => { SalaryReportExecute(); });

        private bool SalaryReportCanExecute()
        {
            return Context.Companies.Count() != 0 &&
                   Context.Departments.Count() != 0 &&
                   Context.Employees.Count() != 0;
        }

        private void SalaryReportExecute()
        {
            int companyId = ComboCompany == null ? 0 : ComboCompany.Id;
            int departmentId = ComboRepDepartment == null ? 0 : ComboRepDepartment.Id;
            List<SalaryReportDTO> reportResult = new List<SalaryReportDTO>();

            if (companyId != 0)
                if (departmentId != 0)
                    reportResult = Context.Report(companyId, departmentId);
                else
                    reportResult = Context.Report(companyId);
            else
                reportResult = Context.Report();

            SalaryReportDTOs.Clear();

            foreach (var item in reportResult)
                SalaryReportDTOs.Add(item);
        }
    }
}
