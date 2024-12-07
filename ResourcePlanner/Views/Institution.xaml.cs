using ResourcePlanner.Viewmodels;
using System.Windows.Controls;

namespace ResourcePlanner.Views
{
    /// <summary>
    /// Interaction logic for Institution.xaml
    /// </summary>
    public partial class Institution : UserControl
    {
        public Institution()
        {
            InitializeComponent();
            this.DataContext = new InstitutionViewModel();
        }
    }
}
