using System;
using System.Collections.Generic;

#nullable disable

namespace VeterinarskaStanica.Model.Core
{
    public partial class PetType
    {
        public PetType()
        {
            Pets = new HashSet<Pet>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }
    }
}
