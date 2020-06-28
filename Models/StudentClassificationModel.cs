using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class StudentClassificationModel
    {
        public int InstituteId { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OptionalGroupModel: OptionalGroupUpdateModel
    {        
        public int InstituteId { get; set; }
    }

    public class OptionalGroupUpdateModel
    {
        public string Name { get; set; }
        public string Description { get; set; }        
    }

    public class GroupSubscriptionModel
    {
        public int Userid { get; set; }
        public int GroupId { get; set; }
        public DateTime StartDate { get; set; }
    }

    public class DeactiveGroupSubscriptionModel
    {
        public int SubscriptionId { get; set; }        
        public DateTime EndDate { get; set; }
    }
}
