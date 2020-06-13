using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.Models
{
    public class InstitutionModel: InstitutionModelUpdate
    {       
        public Address MailingAddress { get; set; }
        public Address BillingAddress { get; set; }
        public Contact ContactDetail { get; set; }        
    }

    public class InstitutionModelUpdate
    {
        public string Name { get; set; }
        public string BranchName { get; set; }
        public string Description { get; set; }
        public int BoardId { get; set; }
        public int TypeId { get; set; }        
        public string Actor { get; set; }
    }

    public class Address
    {
        public int AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public int? State { get; set; }
        public int Country { get; set; }
        public string Zipcode { get; set; }
    }

    public class Contact
    {
        public string LandLineNumber { get; set; }
        public string AltLandLineNumber { get; set; }
        public string MobileNumber { get; set; }
        public string AltMobileNumber { get; set; }
        public string EmailId{ get; set; }
        public string AltEmailId { get; set; }
    }
}
