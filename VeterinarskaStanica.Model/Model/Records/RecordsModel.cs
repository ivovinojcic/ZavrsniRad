using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VeterinarskaStanica.Model.Model.Records
{
    public class RecordsModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Datum je obavezan")]
        public string Date { get; set; }
        public string ClientDescription { get; set; }
        public string EmployeeDescription { get; set; }
        [Required(ErrorMessage = "Status je obavezan")]
        public string RecordStatusId { get; set; }
        public string EmployeeId { get; set; }
        public string PetId { get; set; }
    }
}
