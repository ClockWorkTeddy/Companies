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
