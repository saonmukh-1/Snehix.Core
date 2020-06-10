using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.DTO
{
    public class Device
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Version { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }
    }
    public class DeviceExtended:Device
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
    }
}
