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
        public ObservableCollection<SalaryReportDTO> SalaryReportDTOs = new ObservableCollection<SalaryReportDTO>();

        public AutoCommand SalaryReportCommand =>
            new AutoCommand(obj => { SalaryReportExecute(); }, obj => SalaryReportCanExecute());

        private bool SalaryReportCanExecute()
        {
            return Context.Companies.Count() != 0 &&
                   Context.Departments.Count() != 0 &&
                   Context.Employees.Count() != 0;
        }

        private void SalaryReportExecute()
        {
            var reportResult = Context.Report();
            SalaryReportDTOs.Clear();

            foreach (var item in reportResult)
                SalaryReportDTOs.Add(item);
        }


    }
}
