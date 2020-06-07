using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get { return FirstName + " " + MiddleName + " " + LastName; } }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        //public int? GuardianId { get; set; }
        public int UserTypeId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int UserStatusId { get; set; }
    }
}
