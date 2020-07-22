using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.DTO
{
    public class InstituteDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string BranchName { get { return FirstName + " " + MiddleName + " " + LastName; } }
        public string Description { get; set; }
        public string EducationalBoard { get; set; }
        public string InstitutionType { get; set; }
        
    }

    public class PublisherDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string BranchName { get { return FirstName + " " + MiddleName + " " + LastName; } }
        public string Description { get; set; }
        public string ContactName { get; set; }
        public int? InstituteId { get; set; }
        public string InstituteName { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }

        public int? AmazonAccountId { get; set; }
        public string BucketName { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string IamUserName { get; set; }

    }
}
