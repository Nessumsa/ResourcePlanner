using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlanner.Domain
{
    public class User
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? InstitutionId { get; set; }
        public User(string name, string email, string phone, string role, string username, string password, string institutionId)
        {
            this.Name = name;
            this.Email = email;
            this.Phone = phone;
            this.Role = role;
            this.Username = username;
            this.Password = password;
            this.InstitutionId = institutionId;
        }
    }
}
