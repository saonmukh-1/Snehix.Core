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
        public int? SectionId { get; set; }
        public int? ClassId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserUpdateModel
    {
        /// <summary>
        /// FirstName
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// MiddleName
        /// </summary>
        public string MiddleName { get; set; }
        /// <summary>
        /// LastName
        /// </summary>
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

    /// <summary>
    /// User Registration Model
    /// </summary>
    public class UserRegistrationModel
    {   
        /// <summary>
        /// User id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Start date
        /// </summary>
        public DateTime StartDate { get; set; }  
        /// <summary>
        /// Institute id
        /// </summary>
        public int InstituteId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ClassId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SectionId { get; set; }
    }
    public class UserRegistrationUpdateModel
    {   
        /// <summary>
        /// 
        /// </summary>
        public DateTime EndDate { get; set; }        
    }

    public class UpdateUserLoginModel
    {        
        public bool IsNewAccount { get; set; }
        public string IPAddress { get; set; }
    }

    public class IAMModel
    {
        public string BucketName { get; set; }
        public string UserName { get; set; }
        
    }

    public class UserSearch
    {
        public int InstituteId { get; set; } = 0;
        public int ClassId { get; set; } = 0;
        public int SectionId { get; set; } = 0;
    }

    public class BucketModel
    {
        public string Name { get; set; }
        
    }
}
