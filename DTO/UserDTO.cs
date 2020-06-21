using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get { return FirstName + " " + MiddleName + " " + LastName; } }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        //public int? GuardianId { get; set; }
        public int UserTypeId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int UserStatusId { get; set; }
        public bool IsNewAccount { get; set; }
        public string IPAddress { get; set; }
    }

    public class LoginResponse
    {
        public string Jwt { get; set; }        
        public int ActionCode { get; set; }

        public void PopulateCode(string ipAddress,bool newAccount,string requestIdAddress)
        {
            if(newAccount)
            {
                this.ActionCode = (int)ActionResponseCode.NewAccount;
            }
            else if(requestIdAddress.Equals(ipAddress))
            {
                this.ActionCode = (int)ActionResponseCode.NewDevice;
            }
            else
            {
                this.ActionCode = (int)ActionResponseCode.Ok;
            }
        }
    }

    public enum ActionResponseCode
    {
        Ok=2000,
        NewAccount =2001,
        NewDevice=2002
    }
}
