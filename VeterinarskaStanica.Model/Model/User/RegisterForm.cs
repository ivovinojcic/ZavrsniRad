using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VeterinarskaStanica.Model.Model.User
{
    public class RegisterForm
    {
        [Required(ErrorMessage = "Ime je obavezno")]
        [RegularExpression("^[a-žA-Ž ]*$", ErrorMessage = "Molimo unesite ispravno ime")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Prezime je obavezno")]
        [RegularExpression("^[a-žA-Ž ]*$", ErrorMessage = "Molimo unesite ispravno prezime")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Godina rođenja je obavezna")]
        public string BirthDate { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Broj telefona je obavezan")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Molimo unesite ispravan broj telefona")]
        public string PhoneNumber { get; set; }
        [MinLength(6, ErrorMessage = "Korisničko ime mora sadržavati najmanje 6 znakova")]
        [Required(ErrorMessage = "Korisničko ime je obavezno")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Lozinka je obavezna")]
        [MinLength(6, ErrorMessage = "Lozinka mora sadržavati najmanje 6 znakova")]
        [MaxLength(15, ErrorMessage = "Lozinka ne smije sadržavati više od 15 znakova")]
        public string Password { get; set; }
    }
}
