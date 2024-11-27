using ResourcePlanner.Viewmodels;
using System.Windows.Controls;

namespace ResourcePlanner.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LogOnScreen : UserControl
    {
        public LogOnScreen()
        {
            InitializeComponent();
            this.DataContext = new LogOnScreenViewModel();
        }
    }
}