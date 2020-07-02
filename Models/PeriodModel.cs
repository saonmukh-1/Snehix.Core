using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.Models
{
    public class PeriodModel
    {
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? StudentClasificationId { get; set; }
        public int? OptionalGroupId { get; set; }
    }

    public class SearchPeriodByTeacherModel
    {
        public int TeacherId { get; set; }       
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }        
    }

    public class SearchPeriodByStudentModel
    {
        public int StudentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
