using System;
using System.Collections.Generic;

#nullable disable

namespace VeterinarskaStanica.Model.Core
{
    public partial class VisitRecord
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string ClientDescription { get; set; }
        public string EmployeeDescription { get; set; }
        public int? EmployeeId { get; set; }
        public int? PetId { get; set; }

        public virtual User Employee { get; set; }
        public virtual Pet Pet { get; set; }
    }
}
