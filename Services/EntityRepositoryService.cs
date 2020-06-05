using MySql.Data.MySqlClient;
using Snehix.Core.API.DTO;
using Snehix.Core.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.Services
{
    public class EntityRepositoryService
    {

        MySqlConnection _connection = null;
        public EntityRepositoryService(string conString)
        {
            _connection = new MySqlConnection(conString); 
        }
        public async Task<List<string>> myquery()
        {
            try
            {
                await _connection.OpenAsync();
                var command = new MySqlCommand("SELECT name FROM Entity;", _connection);
                var reader = await command.ExecuteReaderAsync();
                List<string> a = new List<string>();
                while (await reader.ReadAsync())
                {
                    var value = reader.GetValue(0).ToString();
                    a.Add(value);
                    // do something with 'value'
                }
                return a;
            }
            catch
            {
                throw ;
            }
            finally
            {
                //await _connection.col
            }
        }


        public async Task CreateEntity(string name, string description,int entityTypeId)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Add_Entity", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("EntityName", name);
                cmd.Parameters.AddWithValue("Description", description);
                cmd.Parameters.AddWithValue("TypeId", entityTypeId);
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

        public async Task UpdateEntity(int id,string name, string description, int entityTypeId)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Update_Entity", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("EntityId", id);
                cmd.Parameters.AddWithValue("EntityName", name);
                cmd.Parameters.AddWithValue("Description", description);
                cmd.Parameters.AddWithValue("TypeId", entityTypeId);
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

        public async Task CreateEntityType(string name, string description)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Add_EntityType", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("EntityTypeName", name);
                cmd.Parameters.AddWithValue("Description", description);
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

        public async Task<List<EntityTypeResponse>> GetAllEntityType()
        {
            try
            {
                List<EntityTypeResponse> dt = new List<EntityTypeResponse>();
                
                await _connection.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand("Get_AllEntityType", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var row = new EntityTypeResponse();
                        row.Id = dr["ID"].ToString();
                        row.Name = dr["Name"].ToString();
                        row.Description = dr["Description"].ToString();
                        dt.Add(row);
                    }
                }
                return dt;                
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<DataTable> GetAllEntityByType(int typeId)
        {
            DataTable dt = new DataTable();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_AllEntityByTypeId", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("TypeId", typeId);
                var dataReader = cmd.ExecuteReader();
                dt.Load(dataReader);
            }

            return dt;
        }

        public async Task<DataTable> GetAllEntityById(int id)
        {
            DataTable dt = new DataTable();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_EntityById", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("entityId", id);
                var dataReader = cmd.ExecuteReader();
                dt.Load(dataReader);
            }

            return dt;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            try
            {
                List<Country> dt = new List<Country>();

                await _connection.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand("GetAll_Countries", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var row = new Country();
                        row.Id = Convert.ToInt32( dr["id"]);
                        row.Name = dr["name"].ToString();
                        row.SortName = dr["sortname"].ToString();
                        row.PhoneCode = dr["phonecode"].ToString();
                        dt.Add(row);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<State>> GetAllStatesByCountry(int countryId=0)
        {
            try
            {
                List<State> dt = new List<State>();
                await _connection.OpenAsync();
                if (countryId == 0)
                {
                    using (MySqlCommand cmd = new MySqlCommand("Get_AllState", _connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            var row = new State();
                            row.Id = Convert.ToInt32(dr["id"]);
                            row.Name = dr["name"].ToString();
                            row.CoutryId = Convert.ToInt32(dr["country_id"]);                            
                            dt.Add(row);
                        }
                    }
                    return dt;
                }
                else
                {
                    using (MySqlCommand cmd = new MySqlCommand("GetAll_StatesByCountry", _connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("countryId", countryId);
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            var row = new State();
                            row.Id = Convert.ToInt32(dr["id"]);
                            row.Name = dr["name"].ToString();
                            row.CoutryId = Convert.ToInt32(dr["country_id"]);
                            dt.Add(row);
                        }
                    }
                    return dt;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
