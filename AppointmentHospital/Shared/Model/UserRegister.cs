using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentHospital.Shared.Model
{
    public class UserRegister
    {
        [Required(ErrorMessage ="Email alanı gereklidir"),EmailAddress]
        public string Email { get; set; }
        [Required,StringLength(100,MinimumLength =6)]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage = "pasword do not match")]
        public string ConfirmPassword { get; set; }
    }
}
