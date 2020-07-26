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

    public class EBookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        //public string BranchName { get { return FirstName + " " + MiddleName + " " + LastName; } }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Breadth { get; set; }
        public string DotInInch { get; set; }
      
        public string Author { get; set; }
        public string Description { get; set; }

        public int? ClassId { get; set; }
        public string ISDN { get; set; }
        public string Class { get; set; }
        public int? SubjectId { get; set; }
        public string Subject { get; set; }
        public int? Year { get; set; }
        public string Edition { get; set; }

    }
    public class EBookDTODetail: EBookDTO
    {
       
        public int? PublisherId { get; set; }
        public string PublisherName { get; set; }

    }
}
