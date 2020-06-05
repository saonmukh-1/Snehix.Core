using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.Models
{
    public class DeviceModel
    {
        

        public string model { get; set; }
        public string version { get; set; }
        public string serialNumber { get; set; }
        public string description { get; set; }
        public string createdBy { get; set; }
        public int? UserId { get; set; }
        public DateTime stratdate { get; set; }
    }

    public class DeviceUpdateModel
    {

        public int DeviceId { get; set; }
        public string model { get; set; }
        public string version { get; set; }
        public string serialNumber { get; set; }
        public string description { get; set; }
        public string modifiedBy { get; set; }
        
    }

    public class DeviceUserAssociationUpdateModel
    {

        public int DeviceId { get; set; }
        public string createdBy { get; set; }
        public int UserId { get; set; }
        public DateTime stratdate { get; set; }

    }
}
