using MySql.Data.MySqlClient;
using Snehix.Core.API.DTO;
using Snehix.Core.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class StudentClassificationRepository
    {
        MySqlConnection _connection = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conString"></param>
        public StudentClassificationRepository(string conString)
        {
            _connection = new MySqlConnection(conString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        public async Task CreateStudentClassification(StudentClassificationModel model, string actor)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Create_StudentClasification", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("InstititueIdVal", model.InstituteId);
                cmd.Parameters.AddWithValue("ClassIdVal", model.ClassId);
                cmd.Parameters.AddWithValue("SectionIdVal", model.SectionId);
                cmd.Parameters.AddWithValue("CreatedByValue", actor);               
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        public async Task UpdateStudentClassification(int id, StudentClassificationModel model, string actor)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Update_StudentClassification", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("StudentClassificationIdVal", id);
                cmd.Parameters.AddWithValue("ClassIdVal", model.ClassId);
                cmd.Parameters.AddWithValue("SectionIdVal", model.SectionId);
                cmd.Parameters.AddWithValue("UpdatedByVal", actor);              
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

       /// <summary>
       /// 
       /// </summary>
       /// <param name="instituteId"></param>
       /// <returns></returns>
        public async Task<List<StudentClassification>> GetStudentClassificationByInstitute(int instituteId)
        {
            List<StudentClassification> dt = new List<StudentClassification>();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_StudentClarificationByInstituteId", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("InstituteIdVal", instituteId);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var row = new StudentClassification();
                    row.Id = Convert.ToInt32(dr["ID"]);
                    row.InstituteId = Convert.ToInt32(dr["InstituteId"]);
                    row.ClassId = Convert.ToInt32(dr["ClassId"]);
                    row.SectionId = Convert.ToInt32(dr["SectionId"]);
                    row.ClassName = dr["ClassName"].ToString();
                    row.SectionName = dr["SectionName"].ToString();
                    dt.Add(row);
                }
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<StudentClassification>> GetStudentClassificationById(int id)
        {
            List<StudentClassification> dt = new List<StudentClassification>();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_StudentClarificationById", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("StudentAssociationId", id);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var row = new StudentClassification();
                    row.Id = Convert.ToInt32(dr["ID"]);
                    row.InstituteId = Convert.ToInt32(dr["InstituteId"]);
                    row.ClassId = Convert.ToInt32(dr["ClassId"]);
                    row.SectionId = Convert.ToInt32(dr["SectionId"]);
                    row.ClassName = dr["ClassName"].ToString();
                    row.SectionName = dr["SectionName"].ToString();
                    dt.Add(row);
                }
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteStudentClassification(int id)
        {
            await _connection.OpenAsync();
            var cmd = new MySqlCommand("Delete_StudentClassification", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("StudentClassificationIdVal", id);            
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteGroup(int id)
        {
            await _connection.OpenAsync();
            var cmd = new MySqlCommand("Delete_OptionalGroup", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("GroupId", id);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<OptionalGroup>> GetGroupById(int id)
        {
            List<OptionalGroup> dt = new List<OptionalGroup>();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_GroupById", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("GroupIdVal", id);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var row = new OptionalGroup();
                    row.Id = Convert.ToInt32(dr["ID"]);
                    row.InstituteId = Convert.ToInt32(dr["InstituteId"]);
                    row.Name = dr["Name"].ToString();
                    row.Description = dr["Description"].ToString();                    
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
        public async Task<List<OptionalGroup>> GetGroupByInstitute(int instituteId)
        {
            List<OptionalGroup> dt = new List<OptionalGroup>();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("GetAll_GroupByInstitute", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("InstituteIdVal", instituteId);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var row = new OptionalGroup();
                    row.Id = Convert.ToInt32(dr["ID"]);
                    row.InstituteId = Convert.ToInt32(dr["InstituteId"]);
                    row.Name = dr["Name"].ToString();
                    row.Description = dr["Description"].ToString();
                    dt.Add(row);
                }
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        public async Task CreateGroup(OptionalGroupModel model, string actor)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Create_OptionalGroup", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("InstititueIdVal", model.InstituteId);
                cmd.Parameters.AddWithValue("NameVal", model.Name);
                cmd.Parameters.AddWithValue("DescriptionVal", model.Description);
                cmd.Parameters.AddWithValue("CreatedByValue", actor);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        public async Task UpdateGroup(int id, OptionalGroupUpdateModel model, string actor)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Update_OptionalGroup", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("GroupId", id);
                cmd.Parameters.AddWithValue("NameVal", model.Name);
                cmd.Parameters.AddWithValue("DescriptionVal", model.Description);
                cmd.Parameters.AddWithValue("ModifiedByValue", actor);
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
    }
}
