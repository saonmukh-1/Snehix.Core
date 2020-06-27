using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.DTO
{
    /// <summary>
    /// 
    /// </summary>
    public class StudentClassification
    {
        public int Id { get; set; }
        public int InstituteId { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OptionalGroup
    {
        public int Id { get; set; }
        public int InstituteId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
    }
}
