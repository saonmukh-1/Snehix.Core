using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.DTO
{
    public class Country
    {
        public int Id { get; set; }
        public string SortName { get; set; }
        public string Name { get; set; }
        public string PhoneCode { get; set; }
    }
}
