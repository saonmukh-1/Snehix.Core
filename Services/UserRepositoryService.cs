using MySql.Data.MySqlClient;
using Snehix.Core.API.DTO;
using Snehix.Core.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Snehix.Core.API.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRepositoryService
    {
        MySqlConnection _connection = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conString"></param>
        public UserRepositoryService(string conString)
        {
            _connection = new MySqlConnection(conString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CreateUser(UserModel model)
        {
            await _connection.OpenAsync();
            var cmd = new MySqlCommand("Create_User", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("usrName", model.Username);
            cmd.Parameters.AddWithValue("pssWord", "XXX-XXX-XXX");
            cmd.Parameters.AddWithValue("frstName", model.FirstName);
            cmd.Parameters.AddWithValue("mdlName", model.MiddleName);
            cmd.Parameters.AddWithValue("lstName", model.LastName);
            cmd.Parameters.AddWithValue("ftrName", model.FatherName);
            cmd.Parameters.AddWithValue("mtrName", model.LastName);

            if (model.GuardianId.HasValue)
                cmd.Parameters.AddWithValue("grdId", model.GuardianId);
            else
                cmd.Parameters.AddWithValue("grdId", DBNull.Value);

            if (model.InstituteId.HasValue)
                cmd.Parameters.AddWithValue("InstituteIdVal", model.InstituteId);
            else
                cmd.Parameters.AddWithValue("InstituteIdVal", DBNull.Value);
            cmd.Parameters.AddWithValue("StudentClassificationIdval", DBNull.Value);
            cmd.Parameters.AddWithValue("usrtypeId", model.UserTypeId);
            cmd.Parameters.AddWithValue("dob", model.DateOfBirth);
            cmd.Parameters.AddWithValue("usrStatusId", model.UserStatusId);

            cmd.Parameters.AddWithValue("crtBy", model.Actor);
            cmd.ExecuteNonQuery();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task UpdateUser(UserUpdateModel model, int ID)
        {
            await _connection.OpenAsync();
            var cmd = new MySqlCommand("Update_User", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("usrId", ID);
            cmd.Parameters.AddWithValue("frstName", model.FirstName);
            cmd.Parameters.AddWithValue("mdlName", model.MiddleName);
            cmd.Parameters.AddWithValue("lstName", model.LastName);
            cmd.Parameters.AddWithValue("ftrName", model.FatherName);
            cmd.Parameters.AddWithValue("mtrName", model.LastName);
            if(model.GuardianId.HasValue)
                cmd.Parameters.AddWithValue("grdId", model.GuardianId);
            else
                cmd.Parameters.AddWithValue("grdId", DBNull.Value);
            cmd.Parameters.AddWithValue("usrtypeId", model.UserTypeId);
            cmd.Parameters.AddWithValue("dob", model.DateOfBirth);
            cmd.Parameters.AddWithValue("usrStatusId", model.UserStatusId);
            cmd.Parameters.AddWithValue("modBy", model.Actor);
            cmd.ExecuteNonQuery();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserDTO>> GetAllUser()
        {
            var dt = new List<UserDTO>();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_AllUser", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var row = new UserDTO();
                    row.UserId = Convert.ToInt32(dr["ID"]);
                    row.UserName = dr["Name"].ToString();
                    row.FirstName = dr["FirstName"].ToString();
                    row.LastName = dr["LastName"].ToString();
                    row.MiddleName = dr["MiddleName"].ToString();
                    row.FatherName = dr["FatherName"].ToString();
                    row.UserTypeId = Convert.ToInt32(dr["UserTypeId"]);
                    row.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]);
                    row.UserStatusId = Convert.ToInt32(dr["UserStatusId"]);
                    dt.Add(row);
                }
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<UserDTO>> GetUseryById(int userId)
        {
            List<UserDTO> dt = new List<UserDTO>();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_UserById", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("usrId", userId);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var row = new UserDTO();
                    row.UserId = Convert.ToInt32(dr["ID"]);
                    row.UserName = dr["UserName"].ToString();
                    row.FirstName = dr["FirstName"].ToString();
                    row.LastName = dr["LastName"].ToString();
                    row.MiddleName = dr["MiddleName"].ToString();
                    row.FatherName = dr["FatherName"].ToString();
                    row.UserTypeId = Convert.ToInt32(dr["UserTypeId"]);
                    row.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]);
                    row.UserStatusId = Convert.ToInt32(dr["UserStatusId"]);
                    dt.Add(row);
                }
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<List<UserDTO>> GetUseryByUserName(string username)
        {
            List<UserDTO> dt = new List<UserDTO>();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_UserByUserName", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("usrName", username);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var row = new UserDTO();
                    row.UserId = Convert.ToInt32(dr["ID"]);
                    row.UserName = dr["UserName"].ToString();
                    row.FirstName = dr["FirstName"].ToString();
                    row.LastName = dr["LastName"].ToString();
                    row.MiddleName = dr["MiddleName"].ToString();
                    row.FatherName = dr["FatherName"].ToString();
                    row.UserTypeId = Convert.ToInt32(dr["UserTypeId"]);
                    row.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]);
                    row.UserStatusId = Convert.ToInt32(dr["UserStatusId"]);
                    row.IsNewAccount = Convert.ToBoolean(dr["NewAccount"]);
                    row.IPAddress = dr["IPAddress"].ToString();
                    dt.Add(row);
                }
            }

            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="instituteId"></param>
        /// <param name="actor"></param>
        /// <param name="startDate"></param>
        /// <param name="classId"></param>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public async Task CreateUserRegistration(int userId, int instituteId, string actor
            , DateTime startDate,int classId, int sectionId)
        {
            await _connection.OpenAsync();
            var cmd = new MySqlCommand("Create_Registration", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("UserIdVal", userId);
            cmd.Parameters.AddWithValue("InstituteIdVal", instituteId);
            cmd.Parameters.AddWithValue("CreatedByValue", actor);
            cmd.Parameters.AddWithValue("StartDateVal", startDate);
            cmd.Parameters.AddWithValue("ClassIdVal", classId);
            cmd.Parameters.AddWithValue("SectionIdVal", sectionId);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="actor"></param>
        /// <param name="finishDate"></param>
        /// <returns></returns>
        public async Task TerminateUserRegistration(int registrationId, string actor
            , DateTime finishDate)
        {
            await _connection.OpenAsync();
            var cmd = new MySqlCommand("Update_Registration", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("RegistrationIdVal", registrationId);
            cmd.Parameters.AddWithValue("actor", actor);
            cmd.Parameters.AddWithValue("EndDateVal", finishDate);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<UserRegistrationDTO>> GetUserRegistrationByUserId(int userId)
        {
            List<UserRegistrationDTO> dt = new List<UserRegistrationDTO>();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_UserRegistrationByUserId", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("UserIdVal", userId);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var row = new UserRegistrationDTO();
                    row.UserId = Convert.ToInt32(dr["UserId"]);
                    row.Username = dr["Username"].ToString();
                    row.FirstName = dr["FirstName"].ToString();
                    row.LastName = dr["LastName"].ToString();
                    row.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]);
                    row.StartDate = Convert.ToDateTime(dr["StartDate"]);
                    row.InstituteId = Convert.ToInt32(dr["InstituteId"]);
                    row.InstituteName = dr["InstituteName"].ToString();
                    row.BranchName = dr["BranchName"].ToString();
                    dt.Add(row);
                }
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instituteId"></param>
        /// <returns></returns>
        public async Task<List<UserRegistrationDTO>> GetAllUserRegistrationByInstituteId(int instituteId)
        {
            List<UserRegistrationDTO> dt = new List<UserRegistrationDTO>();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_AllUserRegistrationByInstitute", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("InstituteIdval", instituteId);

                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var row = new UserRegistrationDTO();
                    row.UserId = Convert.ToInt32(dr["UserId"]);
                    row.Username = dr["Username"].ToString();
                    row.FirstName = dr["FirstName"].ToString();
                    row.LastName = dr["LastName"].ToString();

                    row.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]);
                    row.StartDate = Convert.ToDateTime(dr["StartDate"]);
                    row.InstituteId = Convert.ToInt32(dr["InstituteId"]);
                    row.InstituteName = dr["InstituteName"].ToString();
                    row.BranchName = dr["BranchName"].ToString();
                    dt.Add(row);
                }
            }

            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserRegistrationDTO>> GetAllUserRegistration()
        {
            List<UserRegistrationDTO> dt = new List<UserRegistrationDTO>();
            await _connection.OpenAsync();

            using (MySqlCommand cmd = new MySqlCommand("Get_AllUserRegistration", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var row = new UserRegistrationDTO();
                    row.UserId = Convert.ToInt32(dr["UserId"]);
                    row.Username = dr["Username"].ToString();
                    row.FirstName = dr["FirstName"].ToString();
                    row.LastName = dr["LastName"].ToString();

                    row.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]);
                    row.StartDate = Convert.ToDateTime(dr["StartDate"]);
                    row.InstituteId = Convert.ToInt32(dr["InstituteId"]);
                    row.InstituteName = dr["InstituteName"].ToString();
                    row.BranchName = dr["BranchName"].ToString();
                    dt.Add(row);
                }
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="IsNewAccount"></param>
        /// <param name="IpAddress"></param>
        /// <returns></returns>
        public async Task UpdateUserLogin(string UserName, bool IsNewAccount, string IpAddress)
        {
            await _connection.OpenAsync();
            var cmd = new MySqlCommand("Update_UserLogin", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("usrName", UserName);
            cmd.Parameters.AddWithValue("ipAddressVal", IpAddress);
            cmd.Parameters.AddWithValue("isNewAccount", IsNewAccount);            
            cmd.ExecuteNonQuery();
        }
    }
}
