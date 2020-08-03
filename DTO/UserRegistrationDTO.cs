using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.DTO
{
    public class UserRegistrationDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }
        public int InstituteId { get; set; }
        public string InstituteName { get; set; }
        public string BranchName { get; set; }
        public DateTime StartDate { get; set; }
    }

    public class InstituteAmazonRegistrationDetail
    {
        public int UserId { get; set; }
        public int InstituteId { get; set; }
        public string BucketName { get; set; }
        public string AccessKey { get; set; }       
        public string SecretKey { get; set; }
        public string IamUserName { get; set; }        
        public string UserName { get; set; }
    }

    public class UserDetails
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? JoiningDate { get; set; }
        public int? InstituteId { get; set; }
        public string InstituteName { get; set; }
        public int? UsertypeId { get; set; }
        public string Usertype { get; set; }
        public int? ClassId { get; set; }
        public string ClassName { get; set; }
        public int? SectionId { get; set; }
        public string SectionName { get; set; }
        public string RollNumber { get; set; }
        public string PictureUrl { get; set; }

    }
}
