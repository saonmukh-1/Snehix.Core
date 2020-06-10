using MySql.Data.MySqlClient;
using Snehix.Core.API.DTO;
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
            ,string description, string createdBy,int? UserId,DateTime stratdate,int instituteId)
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
                if(UserId!=null)
                    cmd.Parameters.AddWithValue("UserIdValue", UserId);
                else
                    cmd.Parameters.AddWithValue("UserIdValue", DBNull.Value);
                cmd.Parameters.AddWithValue("StartDateVal", stratdate);
                cmd.Parameters.AddWithValue("InstituteIdVal", instituteId);
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
                cmd.CommandType = CommandType.StoredProcedure;
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
                cmd.CommandType = CommandType.StoredProcedure;
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
       
        public async Task<List<Device>> GetAllActiveDevice()
        {
            List<Device> dt = new List<Device>();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_AllActiveDevice", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var row = new Device();
                    row.Id = Convert.ToInt32(dr["Id"]);
                    row.Model = dr["Model"].ToString();
                    row.Description = dr["description"].ToString();
                    row.SerialNumber = dr["SerialNumber"].ToString();
                    row.Version = dr["Version"].ToString();
                    dt.Add(row);
                }
            }
            return dt;
        }

        public async Task<List<DeviceExtended>> GetAllDeviceByInstitute(int instituteId)
        {
            List<DeviceExtended> dt = new List<DeviceExtended>();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("GetAll_DeviceByInstitute", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("InstituteIdVal", instituteId);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var row = new DeviceExtended();
                    row.Id = Convert.ToInt32(dr["ID"]);
                    row.Model = dr["Model"].ToString();
                    row.Description = dr["description"].ToString();
                    row.SerialNumber = dr["SerialNumber"].ToString();
                    row.Version = dr["Version"].ToString();
                    if(dr["userId"]!=null && dr["userId"]!=DBNull.Value)
                        row.UserId = Convert.ToInt32(dr["userId"]);
                    row.UserName= dr["Username"].ToString();
                    dt.Add(row);
                }
            }
            return dt;
        }

        public async Task<List<DeviceExtended>> GetAllDeviceById(int deviceId)
        {
            List<DeviceExtended> dt = new List<DeviceExtended>();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_DeviceById", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("DeviceIdVal", deviceId);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var row = new DeviceExtended();
                    row.Id = Convert.ToInt32(dr["Id"]);
                    row.Model = dr["Model"].ToString();
                    row.Description = dr["description"].ToString();
                    row.SerialNumber = dr["SerialNumber"].ToString();
                    row.Version = dr["Version"].ToString();
                    if (dr["userId"] != null)
                        row.UserId = Convert.ToInt32(dr["userId"]);
                    row.UserName = dr["Username"].ToString();
                    dt.Add(row);
                }
            }
            return dt;
        }

        public async Task<List<DeviceExtended>> GetAllAssignedDevice(int instituteId)
        {
            var dt = new List<DeviceExtended>();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_AllAssignedDevice", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("instituteIdVal", instituteId);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var row = new DeviceExtended();
                    row.Id = Convert.ToInt32(dr["Id"]);
                    row.Model = dr["Model"].ToString();
                    row.Description = dr["description"].ToString();
                    row.SerialNumber = dr["SerialNumber"].ToString();
                    row.Version = dr["Version"].ToString();
                    if (dr["userId"] != null)
                        row.UserId = Convert.ToInt32(dr["userId"]);
                    row.UserName = dr["Username"].ToString();
                    dt.Add(row);
                }
            }

            return dt;
        }
        

        public async Task<List<DeviceExtended>> GetAllUnAssignedDevice(int instituteId)
        {
            var dt = new List<DeviceExtended>();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_AllUnAssignedDevice", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("instituteIdVal", instituteId);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var row = new DeviceExtended();
                    row.Id = Convert.ToInt32(dr["Id"]);
                    row.Model = dr["Model"].ToString();
                    row.Description = dr["description"].ToString();
                    row.SerialNumber = dr["SerialNumber"].ToString();
                    row.Version = dr["Version"].ToString();                    
                    dt.Add(row);
                }
            }
            return dt;
        }

        //public async Task<DataTable> GetDeviceByDeviceId(int deviceId)
        //{
        //    DataTable dt = new DataTable();
        //    await _connection.OpenAsync();
        //    using (MySqlCommand cmd = new MySqlCommand("Get_DeviceByDeviceId", _connection))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("DeviceIdval", deviceId);
        //        var dataReader = cmd.ExecuteReader();
        //        dt.Load(dataReader);
        //    }

        //    return dt;
        //}
    }
}
