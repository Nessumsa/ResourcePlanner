using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlanner.Models
{
    class Users : INotifyPropertyChanged
    {
        public String? userID;
        public String? name;
        public String? email;
        public String? phoneNr;
        public String? role;
        public String? userName;
        public String? password;
        public String institutionID = "";
        // This should maybe not be a string. Needs implementation. public String allowedResources;
        public ObservableCollection<Users>? allUsers;

        public String Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public Users(string name)
        {
            this.name = name;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

    }
}
