using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.Models
{
    public class AccountModel
    {
    }
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string DeviceIP { get; set; }

    }

    public class PasswordResetModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string token { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }

    public class RegisterDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
