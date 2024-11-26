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
        public ObservableCollection<Users> allUsers { get; set; }

        public UsersViewModel()
        {
            allUsers = new ObservableCollection<Users>
            {
                new Users("Ole"),
                new Users("Ole2"),
                new Users("Ole3"),
                new Users("Ole4"),
                new Users("Ole5"),
                new Users("Ole6"),
                new Users("Ole7"),
                new Users("Ole8"),
            };
        }
    }
}
