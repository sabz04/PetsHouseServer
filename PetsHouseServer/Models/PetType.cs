using System;
using System.Collections.Generic;

namespace PetsHouseServer.Models
{
    public partial class PetType
    {
        public PetType()
        {
            Advertisments = new HashSet<Advertisment>();
        }

        public int Id { get; set; }
        public string TypeName { get; set; } = null!;

        public virtual ICollection<Advertisment> Advertisments { get; set; }
    }
}
