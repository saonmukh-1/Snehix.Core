using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.Models
{
    public class StudentClassificationModel
    {
        public int InstituteId { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
    }
}
