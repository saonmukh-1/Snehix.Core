using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.Models
{
    public class DeviceModel
    {
        public string ModelName { get; set; }
        public string Version { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public int? UserId { get; set; }
        public DateTime Stratdate { get; set; }
        public int InstituteId { get; set; }
    }

    public class DeviceUpdateModel
    {

        public int DeviceId { get; set; }
        public string Model { get; set; }
        public string Version { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }
        public string ModifiedBy { get; set; }
        
    }

    public class DeviceUserAssociationUpdateModel
    {

        public int DeviceId { get; set; }
        public string CreatedBy { get; set; }
        public int UserId { get; set; }
        public DateTime Stratdate { get; set; }

    }
}
