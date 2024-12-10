using ResourcePlanner.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
namespace ResourcePlanner.Views
{
    /// <summary>
    /// Interaction logic for ErrorReports.xaml
    /// </summary>
    public partial class ErrorReports : UserControl
    {
        public ErrorReports()
        {
            InitializeComponent();
            this.DataContext = new ErrorReportsViewModel();
        }
    }
}
