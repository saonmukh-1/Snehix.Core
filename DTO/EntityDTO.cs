using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.DTO
{
    public class RawEntityDTO
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string EntityDescription { get; set; }
        public int EntityTypeId { get; set; }
        public string EntityTypeName { get; set; }
    }

    public class EntityDTO
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string EntityDescription { get; set; }
        public int EntityTypeId { get; set; }
    }

    public class EntityTypeDTO
    {       
        public List<EntityDTO> Entities { get; set; }
        public int EntityTypeId { get; set; }
        public string EntityTypeName { get; set; }
    }
    public class EntitySearch
    {
       
        public int[] EntityTypeId { get; set; }        
    }

}
