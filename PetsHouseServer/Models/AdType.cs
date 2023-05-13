using System;
using System.Collections.Generic;

namespace PetsHouseServer.Models
{
    public partial class AdType
    {
        public AdType()
        {
            Advertisments = new HashSet<Advertisment>();
        }

        public int Id { get; set; }
        public string AdTypeName { get; set; } = null!;

        public virtual ICollection<Advertisment> Advertisments { get; set; }
    }
}
