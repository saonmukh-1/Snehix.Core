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
                await _connection.CloseAsync();
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
                await _connection.CloseAsync();
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
                await _connection.CloseAsync();
            }
        }
       
        public async Task<List<Device>> GetAllActiveDevice()
        {
            List<Device> dt = new List<Device>();
            await _connection.OpenAsync();
            try
            {
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
            catch
            {
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<List<DeviceExtended>> GetAllDeviceByInstitute(int instituteId)
        {
            List<DeviceExtended> dt = new List<DeviceExtended>();
            await _connection.OpenAsync();
            try
            {
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
                        if (int.TryParse(dr["userId"].ToString(), out int userId))
                            row.UserId = userId;                       
                        row.UserName = dr["Username"].ToString();
                        dt.Add(row);
                    }
                }
                return dt;
            }
            catch
            {
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<List<DeviceExtended>> GetAllDeviceById(int deviceId)
        {
            List<DeviceExtended> dt = new List<DeviceExtended>();
            await _connection.OpenAsync();
            try
            {
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
                        if (int.TryParse(dr["userId"].ToString(), out int userId))
                            row.UserId = userId;                        
                        row.UserName = dr["Username"].ToString();
                        dt.Add(row);
                    }
                }
                return dt;
            }
            catch
            {
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<List<DeviceExtended>> GetAllAssignedDevice(int instituteId)
        {
            var dt = new List<DeviceExtended>();
            await _connection.OpenAsync();
            try
            {
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
                        if (int.TryParse(dr["userId"].ToString(), out int userId))
                            row.UserId = userId;
                        row.UserName = dr["Username"].ToString();
                        dt.Add(row);
                    }
                }

                return dt;
            }
            catch { throw; }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        

        public async Task<List<DeviceExtended>> GetAllUnAssignedDevice(int instituteId)
        {
            var dt = new List<DeviceExtended>();
            await _connection.OpenAsync();
            try
            {
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
            catch
            {
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<List<DeviceExtended>> GetAllDetailDeviceByInstitute(int instituteId)
        {
            var dt = new List<DeviceExtended>();
            await _connection.OpenAsync();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Get_AllDeviceByInstitute", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("InstituteIdVal", instituteId);
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var row = new DeviceExtended();
                        row.Id = Convert.ToInt32(dr["deviceId"]);
                        row.Model = dr["Model"].ToString();
                        row.Description = dr["Description"].ToString();
                        row.SerialNumber = dr["SerialNumber"].ToString();
                        row.Version = dr["Version"].ToString();
                        if (int.TryParse(dr["userId"].ToString(), out int userId))
                            row.UserId = userId;
                        row.UserName = dr["Username"].ToString();
                        if (int.TryParse(dr["InstituteId"].ToString(), out int InstituteId))
                            row.InstituteId = InstituteId;
                        row.InstituteName = dr["InstituteName"].ToString();
                        row.UserFullName = dr["UserFullName"].ToString();
                        if (int.TryParse(dr["UserTypeId"].ToString(), out int UserTypeId))
                            row.UserTypeId = UserTypeId;
                        row.UserType = dr["UserType"].ToString();
                        dt.Add(row);
                    }
                }

                return dt;
            }
            catch
            {
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<DeviceDetails> GetAllDetailDeviceBySerialNumber(string serialNumber)
        {
            DeviceDetails row = null;
            await _connection.OpenAsync();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Get_DeviceDetailBySerialNumber", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("SerialNumberVal", serialNumber);
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        row = new DeviceDetails();
                        row.Id = Convert.ToInt32(dr["deviceId"]);
                        row.Model = dr["Model"].ToString();
                        row.Description = dr["Description"].ToString();
                        row.SerialNumber = dr["SerialNumber"].ToString();
                        row.Version = dr["Version"].ToString();
                        if (int.TryParse(dr["userId"].ToString(), out int userId))
                            row.UserId = userId;
                        row.UserName = dr["Username"].ToString();
                        if (int.TryParse(dr["InstituteId"].ToString(), out int InstituteId))
                            row.InstituteId = InstituteId;
                        row.InstituteName = dr["InstituteName"].ToString();
                        row.UserFullName = dr["UserFullName"].ToString();
                        if (int.TryParse(dr["UserTypeId"].ToString(), out int UserTypeId))
                            row.UserTypeId = UserTypeId;
                        row.UserType = dr["UserType"].ToString();
                        row.BucketName = dr["BucketName"].ToString();
                        row.InstituteAccessKey = dr["InstituteAccessKey"].ToString();
                        row.InstituteSecretKey = dr["InstituteSecretKey"].ToString();
                        row.InstituteIamUserName = dr["InstituteIamUserName"].ToString();
                        row.BucketPath = dr["BucketPath"].ToString();
                        row.UserAccessKey = dr["UserAccessKey"].ToString();
                        row.UserSecretKey = dr["UserSecretKey"].ToString();
                        row.UserIamUserName = dr["UserIamUserName"].ToString();
                        //dt.Add(row);
                    }
                }

                return row;
            }
            catch
            {
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task DeleteDevice(int id,string actor)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Delete_Device", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("DeviceId", id);
                cmd.Parameters.AddWithValue("Actor", actor);
                cmd.ExecuteNonQuery();
            }
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }
    }
}
