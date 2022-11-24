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
        public CreateCompanyWindow(object dataContext)
        {
            InitializeComponent();
            var vm = (MainWindowViewModel)dataContext;
            DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => this.Close());
        }
    }
}
