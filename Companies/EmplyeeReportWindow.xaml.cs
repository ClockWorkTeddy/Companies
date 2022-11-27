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
    /// Interaction logic for EmplyeeReportWindow.xaml
    /// </summary>
    public partial class EmplyeeReportWindow : Window
    {
        public EmplyeeReportWindow(object dataContext)
        {
            InitializeComponent();
            var vm = (MainWindowViewModel)dataContext;
            DataContext = vm;
        }
    }
}
