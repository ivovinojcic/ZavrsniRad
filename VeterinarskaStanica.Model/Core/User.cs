using System;
using System.Collections.Generic;

#nullable disable

namespace VeterinarskaStanica.Model.Core
{
    public partial class User
    {
        public User()
        {
            Pets = new HashSet<Pet>();
            VisitRecords = new HashSet<VisitRecord>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? Deleted { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Pet> Pets { get; set; }
        public virtual ICollection<VisitRecord> VisitRecords { get; set; }
    }
}
