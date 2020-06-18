using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.Models
{
    public class UserModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password  { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FatherName { get; set; }
        [Required]
        public string MotherName { get; set; }
        [Required]
        public string EmailId { get; set; }
        public int? GuardianId { get; set; }
        [Required]
        public int UserTypeId { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
        [Required]
        public int UserStatusId { get; set; }
        public string Actor { get; set; }
        public int? InstituteId { get; set; }
    }

    public class UserUpdateModel
    {       
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string EmailId { get; set; }
        public int? GuardianId { get; set; }
        public int UserTypeId { get; set; }
        public string DateOfBirth { get; set; }
        public int UserStatusId { get; set; }
        public string Actor { get; set; }
        public int? InstituteId { get; set; }
    }

    public class UserRegistrationModel
    {       
        public int UserId { get; set; }        
        public DateTime StartDate { get; set; }        
        public int InstituteId { get; set; }
    }
    public class UserRegistrationUpdateModel
    {        
        public DateTime EndDate { get; set; }        
    }

    public class UpdateUserLoginModel
    {        
        public bool IsNewAccount { get; set; }
        public string IPAddress { get; set; }
    }
}
