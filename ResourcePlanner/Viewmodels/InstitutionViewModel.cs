using ResourcePlanner.Models;
using ResourcePlanner.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ResourcePlanner.Viewmodels
{
    internal class InstitutionViewModel
    {
        public InstitutionModel institution { get; set; }
        public ICommand saveButton { get; }
        public InstitutionViewModel()
        {
            institution = new InstitutionModel("1","1000","1200","30"); //Implement load institution from database
            saveButton = new CommandRelay(Save_Button_Click, CanButtonClick);
        }

        private void Save_Button_Click()
        {
            System.Diagnostics.Debug.WriteLine(institution.startTime); //Implement update institution function
        }
        private bool CanButtonClick() => institution != null ? true : false;
    }
}
