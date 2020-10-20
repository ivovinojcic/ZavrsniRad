using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VeterinarskaStanica.Model.Model.User
{
    public class LoginForm
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
