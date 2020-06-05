using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Password  { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
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
}
