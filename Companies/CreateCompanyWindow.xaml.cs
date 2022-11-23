using System;
using System.Windows;
using Companies.VMs;
namespace Companies
{
    /// <summary>
    /// Interaction logic for CreateCompanyWindow.xaml
    /// </summary>
    public partial class CreateCompanyWindow : Window
    {
        public CreateCompanyWindow()
        {
            InitializeComponent();
            var vm = new CreateCompanyWindowVm();
            DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => this.Close());
        }
    }
}
