using System;
using System.Collections.Generic;

namespace PetsHouseServer.Models
{
    public partial class Advertisment
    {
        public int Id { get; set; }
        public int PetTypeId { get; set; }
        public int AdTypeId { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; } = null!;
        public string Photo { get; set; } = null!;
        public string? Phone { get; set; }
        public string? City { get; set; }

        public virtual AdType AdType { get; set; } = null!;
        public virtual PetType PetType { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
