using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.DTO
{
    public class State
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public int CoutryId { get; set; }
    }

    public class StateCountry
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string CountryShortName { get; set; }
        public int CountryId  { get; set; }
        
    }
    public class InternalState
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class NestedCountry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public List<InternalState> States { get; set; }
    }
}
