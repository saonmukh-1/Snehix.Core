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
        public int? InstituteId { get; set; }
        public string InstituteName { get; set; }
        public int? UserTypeId { get; set; }
        public string UserType { get; set; }
        public string UserFullName { get; set; }

    }

    public class DeviceDetails : DeviceExtended
    {
        public string BucketName { get; set; }
        public string InstituteAccessKey { get; set; }
        public string InstituteSecretKey { get; set; }
        public string InstituteIamUserName { get; set; }
        public string BucketPath { get; set; }
        public string UserAccessKey { get; set; }
        public string UserSecretKey { get; set; }
        public string UserIamUserName { get; set; }
    }



    public class Period
    {
        public int PeriodId { get; set; }
        public int TeacherId { get; set; }
        public string TeacherUserName { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }

        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }

        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int? SectionId { get; set; }
        public string SectionName { get; set; }
        public DateTime EndDateTime { get; set; }

        public int StudentId { get; set; }
        public string StudentUserName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentFirstName { get; set; }

        public string OptionalGroupName { get; set; }
        public int InstituteId { get; set; }

        public DateTime Date {
            get { return StartDateTime.Date; }
            private set { }
        }
        public string Time
        {
            get { return StartDateTime.ToShortTimeString(); }
            private set { }
        }
    }
}
