using Companies.VMs;
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
using System.Windows.Shapes;

namespace Companies
{
    /// <summary>
    /// Interaction logic for CreateEmployeeWindow.xaml
    /// </summary>
    public partial class CreateEmployeeWindow : Window
    {
        public CreateEmployeeWindow(object dataContext)
        {
            InitializeComponent();
            var vm = (MainWindowViewModel)dataContext;
            DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => this.Close());
        }
    }
}
