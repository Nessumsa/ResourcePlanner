using ResourcePlanner.Utilities.Regexes;
using ResourcePlanner.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
