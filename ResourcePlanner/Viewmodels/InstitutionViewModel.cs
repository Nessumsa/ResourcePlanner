using ResourcePlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlanner.Viewmodels
{
    internal class InstitutionViewModel
    {
        public Institution institution { get; set; }
        public InstitutionViewModel()
        {
            institution = new Institution("1","1000","1200","30"); //Implement load institution from database
        }
    }
}
