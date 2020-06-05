using MySql.Data.MySqlClient;
using Snehix.Core.API.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Snehix.Core.API.Services
{
    public class UserRepositoryService
    {
        MySqlConnection _connection = null;
        public UserRepositoryService(string conString)
        {
            _connection = new MySqlConnection(conString);
        }

        public async Task CreateUser(UserModel model)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Create_User", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("usrName", model.Username);
                cmd.Parameters.AddWithValue("pssWord", model.Password);
                cmd.Parameters.AddWithValue("frstName", model.FirstName);
                cmd.Parameters.AddWithValue("mdlName", model.MiddleName);
                cmd.Parameters.AddWithValue("lstName", model.LastName);
                cmd.Parameters.AddWithValue("ftrName", model.FatherName);
                cmd.Parameters.AddWithValue("mtrName", model.LastName);
               
                if (model.GuardianId.HasValue)
                    cmd.Parameters.AddWithValue("grdId", model.GuardianId);
                else
                    cmd.Parameters.AddWithValue("grdId",DBNull.Value);

                if (model.InstituteId.HasValue)
                    cmd.Parameters.AddWithValue("InstituteIdVal", model.InstituteId);
                else
                    cmd.Parameters.AddWithValue("InstituteIdVal", DBNull.Value);
                cmd.Parameters.AddWithValue("usrtypeId", model.UserTypeId);
                cmd.Parameters.AddWithValue("dob", model.DateOfBirth); 
                cmd.Parameters.AddWithValue("usrStatusId", model.UserStatusId);
                
                cmd.Parameters.AddWithValue("crtBy", model.Actor);
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                //await _connection.col
            }
        }

        public async Task UpdateUser(UserModel model, int ID)
        {
            try
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
                cmd.Parameters.AddWithValue("grdId", model.GuardianId);
                cmd.Parameters.AddWithValue("usrtypeId", model.UserTypeId);
                cmd.Parameters.AddWithValue("dob", model.DateOfBirth);
                cmd.Parameters.AddWithValue("usrStatusId", model.UserStatusId);
                cmd.Parameters.AddWithValue("modBy", model.Actor);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                //await _connection.col
            }
        }

        public async Task<DataTable> GetAllUser()
        {
            DataTable dt = new DataTable();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_AllUser", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                var dataReader = cmd.ExecuteReader();
                dt.Load(dataReader);
            }

            return dt;
        }

        public async Task<DataTable> GetUseryById(int userId)
        {
            DataTable dt = new DataTable();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_UserById", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("usrId", userId);
                var dataReader = cmd.ExecuteReader();
                dt.Load(dataReader);
            }

            return dt;
        }

        public async Task<DataTable> GetUseryByUserName(string username)
        {
            DataTable dt = new DataTable();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_UserByUserName", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("usrName", username);
                var dataReader = cmd.ExecuteReader();
                dt.Load(dataReader);
            }

            return dt;
        }

        public async Task CreateUserRegistration(int userId,int instituteId,string actor
            ,DateTime startDate)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Create_Registration", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("UserIdVal", userId);
                cmd.Parameters.AddWithValue("InstituteIdVal", instituteId);
                cmd.Parameters.AddWithValue("CreatedByValue", actor);
                cmd.Parameters.AddWithValue("StartDateVal", startDate);
               
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //await _connection.col
            }
        }

        public async Task TerminateUserRegistration(int registrationId,string actor
            , DateTime finishDate)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Update_Registration", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("RegistrationIdVal", registrationId);                
                cmd.Parameters.AddWithValue("actor", actor);
                cmd.Parameters.AddWithValue("EndDateVal", finishDate);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //await _connection.col
            }
        }

        public async Task<DataTable> GetUserRegistrationByUserId(int userId)
        {
            DataTable dt = new DataTable();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_UserRegistrationByUserId", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("UserIdVal", userId);
                var dataReader = cmd.ExecuteReader();
                dt.Load(dataReader);
            }

            return dt;
        }

        public async Task<DataTable> GetAllUserRegistrationByInstituteId(int instituteId)
        {
            DataTable dt = new DataTable();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_AllUserRegistrationByInstitute", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("InstituteIdval", instituteId);
                var dataReader = cmd.ExecuteReader();
                dt.Load(dataReader);
            }

            return dt;
        }

        public async Task<DataTable> GetAllUserRegistration()
        {
            DataTable dt = new DataTable();
            await _connection.OpenAsync();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Get_AllUserRegistration", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dataReader = cmd.ExecuteReader();
                    dt.Load(dataReader);
                }
            }
            catch(Exception ex)
            {

            }

            return dt;
        }
    }
}
