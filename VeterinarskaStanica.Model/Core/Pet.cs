using System;
using System.Collections.Generic;

#nullable disable

namespace VeterinarskaStanica.Model.Core
{
    public partial class Pet
    {
        public Pet()
        {
            VisitRecords = new HashSet<VisitRecord>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? UserId { get; set; }
        public int? PetTypeId { get; set; }

        public virtual PetType PetType { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<VisitRecord> VisitRecords { get; set; }
    }
}
