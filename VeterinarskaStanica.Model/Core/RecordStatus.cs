using System;
using System.Collections.Generic;

#nullable disable

namespace VeterinarskaStanica.Model.Core
{
    public partial class RecordStatus
    {
        public RecordStatus()
        {
            VisitRecords = new HashSet<VisitRecord>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<VisitRecord> VisitRecords { get; set; }
    }
}
