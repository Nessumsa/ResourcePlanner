using ResourcePlanner.Models;
using ResourcePlanner.Utilities;
using ResourcePlanner.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ResourcePlanner.Viewmodels
{
    class ResourcesViewModel
    {
        public ResourcesModel resources;
        public ICommand saveButton { get; }

        public ResourcesViewModel()
        {
            saveButton = new CommandRelay(Save_Button_Click, CanButtonClick);
        }

        private void Save_Button_Click()
        {
            System.Diagnostics.Debug.WriteLine(resources.name); //Implement update institution function
        }
        private bool CanButtonClick() => resources != null ? true : false;
    }
}
