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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.treeView1.SelectedItem is Root)
            {
                CreateCompanyWindow createCompanyWindow = new CreateCompanyWindow();
                createCompanyWindow.Owner = this;
                createCompanyWindow.Show();
            }
            else if (this.treeView1.SelectedItem is Company)
            {
                CreateDepartmentWindow createDepartmentWindow = new();
                createDepartmentWindow.Owner = this;
                createDepartmentWindow.Show();
            }
        }
    }
}
