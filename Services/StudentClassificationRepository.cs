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
            catch { throw; }
            finally { await _connection.CloseAsync(); }
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
            catch { throw; }
            finally { await _connection.CloseAsync(); }
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
            try
            {
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
            catch { throw; }
            finally { await _connection.CloseAsync(); }
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
            try
            {
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
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteStudentClassification(int id)
        {
            await _connection.OpenAsync();
            try
            {
                var cmd = new MySqlCommand("Delete_StudentClassification", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("StudentClassificationIdVal", id);
                cmd.ExecuteNonQuery();
            }
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteGroup(int id)
        {
            await _connection.OpenAsync();
            try
            {
                var cmd = new MySqlCommand("Delete_OptionalGroup", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("GroupId", id);
                cmd.ExecuteNonQuery();
            }
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<OptionalGroup>> GetGroupById(int id)
        {
            List<OptionalGroup> dt = new List<OptionalGroup>();
            try
            {
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
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instituteId"></param>
        /// <returns></returns>
        public async Task<List<OptionalGroup>> GetGroupByInstitute(int instituteId)
        {
            List<OptionalGroup> dt = new List<OptionalGroup>();
            try
            {
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
            catch { throw; }
            finally { await _connection.CloseAsync(); }
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
            catch { throw; }
            finally { await _connection.CloseAsync(); }
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
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        public async Task CreateGroupSubscription(GroupSubscriptionModel model, string actor)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Create_GroupSubscription", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("UserIdVal", model.Userid);
                cmd.Parameters.AddWithValue("GroupIdVal", model.GroupId);
                cmd.Parameters.AddWithValue("StartDateVal", model.StartDate);
                cmd.Parameters.AddWithValue("CreatedByValue", actor);
                cmd.ExecuteNonQuery();
            }
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        public async Task DeactiveGroupSubscription(DeactiveGroupSubscriptionModel model, string actor)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Deactive_GroupSubscription", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("SubscriptionIdVal", model.SubscriptionId);                
                cmd.Parameters.AddWithValue("EndDateDateVal", model.EndDate);
                cmd.Parameters.AddWithValue("ModifiedByValue", actor);
                cmd.ExecuteNonQuery();
            }
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<OptionalGroupSubscription>> GetSubscriptionByUser(int userId)
        {
            List<OptionalGroupSubscription> dt = new List<OptionalGroupSubscription>();
            try
            {
                await _connection.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand("Get_GroupSubscriptionByUser", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("UserIdVal", userId);
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var row = new OptionalGroupSubscription();
                        row.Id = Convert.ToInt32(dr["ID"]);
                        row.InstituteId = Convert.ToInt32(dr["InstituteId"]);
                        row.OptionalGroupId = Convert.ToInt32(dr["OptionalGroupId"]);
                        row.UserId = Convert.ToInt32(dr["UserId"]);
                        row.Name = dr["Name"].ToString();
                        row.Description = dr["Description"].ToString();
                        row.StartDate = Convert.ToDateTime(dr["StartDate"]);
                        dt.Add(row);
                    }
                }
                return dt;
            }
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OptionalGroupId"></param>
        /// <returns></returns>
        public async Task<List<OptionalGroupSubscriptionUser>> GetUserLisByGroupSubscription(int OptionalGroupId)
        {
            List<OptionalGroupSubscriptionUser> dt = new List<OptionalGroupSubscriptionUser>();
            try
            {
                await _connection.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand("Get_UserLisByGroupSubscription", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("OptionalGroupId", OptionalGroupId);
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var row = new OptionalGroupSubscriptionUser();
                        row.Id = Convert.ToInt32(dr["ID"]);
                        row.UserName = dr["Username"].ToString();
                        row.UserId = Convert.ToInt32(dr["UserId"]);
                        row.OptionalGroupId = Convert.ToInt32(dr["OptionalGroupId"]);
                        row.FirstName = dr["FirstName"].ToString();
                        row.LastName = dr["LastName"].ToString();
                        row.StartDate = Convert.ToDateTime(dr["StartDate"]);
                        dt.Add(row);
                    }
                }
                return dt;
            }
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        public async Task<List<OptionalGroupSubscription>> GetGroupSubscriptionById(int subscriptionId)
        {
            List<OptionalGroupSubscription> dt = new List<OptionalGroupSubscription>();
            try
            {
                await _connection.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand("Get_GroupSubscriptionById", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("GroupSubscriptionId", subscriptionId);
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var row = new OptionalGroupSubscription();
                        row.Id = Convert.ToInt32(dr["ID"]);
                        row.InstituteId = Convert.ToInt32(dr["InstituteId"]);
                        row.OptionalGroupId = Convert.ToInt32(dr["OptionalGroupId"]);
                        row.UserId = Convert.ToInt32(dr["UserId"]);
                        row.Name = dr["Name"].ToString();
                        row.Description = dr["Description"].ToString();
                        row.StartDate = Convert.ToDateTime(dr["StartDate"]);
                        dt.Add(row);
                    }
                }
                return dt;
            }
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }
    }
}
