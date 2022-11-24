using Companies.VMs;
using System;
using System.Windows;

namespace Companies
{
    /// <summary>
    /// Interaction logic for CreateDepartmentWindow.xaml
    /// </summary>
    public partial class CreateDepartmentWindow : Window
    {
        public CreateDepartmentWindow(object dataContext)
        {
            InitializeComponent();
            var vm = (MainWindowViewModel)dataContext;
            DataContext = vm;
            vm.CloseDepartmentAction = new Action(() => this.Close());
        }
    }
}
