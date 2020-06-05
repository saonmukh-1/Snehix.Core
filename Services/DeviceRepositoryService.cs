using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.Services
{
    public class DeviceRepositoryService
    {
        MySqlConnection _connection = null;
        public DeviceRepositoryService  (string conString)
        {
            _connection = new MySqlConnection(conString);
        }

        public async Task CreateDevice(string model, string version, string serialNumber
            ,string description, string createdBy,int? UserId,DateTime stratdate)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Create_Device", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("ModelValue", model);
                cmd.Parameters.AddWithValue("VersionValue", version);
                cmd.Parameters.AddWithValue("SerialNumberValue", serialNumber);
                cmd.Parameters.AddWithValue("DescriptionValue", description);
                cmd.Parameters.AddWithValue("CreatedByValue", createdBy);
                cmd.Parameters.AddWithValue("UserIdValue", UserId);
                cmd.Parameters.AddWithValue("StartDateVal", stratdate);
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

        public async Task UpdateDevice(int id, string model, string version, string serialNumber
            , string description, string modifiedBy)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Update_Device", _connection); 
                cmd.Parameters.AddWithValue("DeviceId", id);
                cmd.Parameters.AddWithValue("ModelValue", model);
                cmd.Parameters.AddWithValue("VersionValue", version);
                cmd.Parameters.AddWithValue("SerialNumberValue", serialNumber);
                cmd.Parameters.AddWithValue("DescriptionValue", description);
                cmd.Parameters.AddWithValue("ModifiedByValue", modifiedBy);                
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

        public async Task UpdateDeviceUserAssociation(int id, int userId, string createdBy, DateTime stratdate)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Update_DeviceUserAssociation", _connection);
                cmd.Parameters.AddWithValue("deviceIdval", id);
                cmd.Parameters.AddWithValue("UserIdValue", userId);
                cmd.Parameters.AddWithValue("CreatedByValue", createdBy);
                cmd.Parameters.AddWithValue("StartDateVal", stratdate);
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

       
        public async Task<DataTable> GetAllActiveDevice()
        {
            DataTable dt = new DataTable();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_AllActiveDevice", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                var dataReader = cmd.ExecuteReader();
                dt.Load(dataReader);
            }

            return dt;
        }

        public async Task<DataTable> GetAllAssignedDevice()
        {
            DataTable dt = new DataTable();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_AllAssignedDevice", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                var dataReader = cmd.ExecuteReader();
                dt.Load(dataReader);
            }

            return dt;
        }
        

        public async Task<DataTable> GetAllUnAssignedDevice()
        {
            DataTable dt = new DataTable();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_AllUnAssignedDevice", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var dataReader = cmd.ExecuteReader();
                dt.Load(dataReader);
            }

            return dt;
        }

        public async Task<DataTable> GetDeviceByDeviceId(int deviceId)
        {
            DataTable dt = new DataTable();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_DeviceByDeviceId", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("DeviceIdval", deviceId);
                var dataReader = cmd.ExecuteReader();
                dt.Load(dataReader);
            }

            return dt;
        }
    }
}
