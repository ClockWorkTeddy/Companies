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
            new AutoCommand(obj => { SalaryReportExecute(); },  obj => SalaryReportCanExecute());

        private bool SalaryReportCanExecute()
        {
            return Context.Companies.Count() != 0 &&
                   Context.Departments.Count() != 0 &&
                   Context.Employees.Count() != 0;
        }

        private void SalaryReportExecute()
        {
            int companyId = SalaryReportSelectedCompany == null ? 0 : SalaryReportSelectedCompany.Id;
            int departmentId = SalaryReportSelectedDepartment == null ? 0 : SalaryReportSelectedDepartment.Id;
            List<SalaryReportDTO> reportResult = new List<SalaryReportDTO>();

            if (companyId != 0)
                if (departmentId != 0)
                    reportResult = Context.SalaryReport(companyId, departmentId);
                else
                    reportResult = Context.SalaryReport(companyId);
            else
                reportResult = Context.SalaryReport();

            SalaryReportDTOs.Clear();

            foreach (var item in reportResult)
                SalaryReportDTOs.Add(item);
        }
    }
}
