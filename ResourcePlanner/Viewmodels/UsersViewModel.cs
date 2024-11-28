using ResourcePlanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlanner.Viewmodels
{
    class UsersViewModel
    {
        public ObservableCollection<UsersModel> allUsers { get; set; }

        public UsersViewModel()
        {
            allUsers = new ObservableCollection<UsersModel>
            {
                new UsersModel("Ole"),
                new UsersModel("Ole2"),
                new UsersModel("Ole3"),
                new UsersModel("Ole4"),
                new UsersModel("Ole5"),
                new UsersModel("Ole6"),
                new UsersModel("Ole7"),
                new UsersModel("Ole8"),
            };
        }
    }
}
