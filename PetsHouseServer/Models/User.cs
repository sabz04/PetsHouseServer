using System;
using System.Collections.Generic;

namespace PetsHouseServer.Models
{
    public partial class User
    {
        public User()
        {
            Advertisments = new HashSet<Advertisment>();
        }

        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Advertisment> Advertisments { get; set; }
    }
}
