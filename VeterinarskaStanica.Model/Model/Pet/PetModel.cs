using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VeterinarskaStanica.Model.Model.Pet
{
    public class PetModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ime je obavezno")]
        [RegularExpression("^[a-žA-Ž ]*$", ErrorMessage = "Molimo unesite ispravno ime")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Godina rođenja je obavezna")]
        public string BirthDate { get; set; }
        public int? UserId { get; set; }
        [Required(ErrorMessage = "Vrsta životinje je obavezna")]
        public string PetTypeId { get; set; }
    }
}
