using Companies.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Companies.VMs;

namespace Companies
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void CreateButtonClick(object sender, RoutedEventArgs e)
        {
            if (treeView1.SelectedItem is Root)
            {
                CreateCompanyWindow createCompanyWindow = new (DataContext);
                createCompanyWindow.Owner = this;
                createCompanyWindow.Show();
            }
            else if (treeView1.SelectedItem is Company)
            {
                CreateDepartmentWindow createDepartmentWindow = new(DataContext);
                createDepartmentWindow.Owner = this;
                createDepartmentWindow.Show();
            }
            else if (treeView1.SelectedItem is Department)
            {
                CreateEmployeeWindow createEmployeeWindow = new (DataContext);
                createEmployeeWindow.Owner = this;
                createEmployeeWindow.Show();
            }
        }

        private void SalaryReportButtomClick(object sender, RoutedEventArgs e)
        {
            SalaryReportWindow salaryReportWindow = new(DataContext);
            salaryReportWindow.Show();
        }

        private void EmployeeReportButtonClick(object sender, RoutedEventArgs e)
        {
            EmplyeeReportWindow emplyeeReportWindow = new(DataContext);

            if (((MainWindowViewModel)DataContext).ComboListCompany != null &&
                ((MainWindowViewModel)DataContext)?.ComboListCompany?.Name != " ")
                emplyeeReportWindow.Show();
        }
    }
}
