using System;
using System.Collections.Generic;

#nullable disable

namespace VeterinarskaStanica.Model.Core
{
    public partial class PageSetting
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
    }
}
