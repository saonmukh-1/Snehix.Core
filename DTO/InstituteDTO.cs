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
}
