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
    public class PeriodRepository
    {
        MySqlConnection _connection = null;
        public PeriodRepository(string conString)
        {
            _connection = new MySqlConnection(conString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CreatePeriod(PeriodModel model)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Create_Period", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("TeacherIdVal", model.TeacherId);
                cmd.Parameters.AddWithValue("SubjectIdVal", model.SubjectId);
                cmd.Parameters.AddWithValue("DescriptionVal", model.Description);
                cmd.Parameters.AddWithValue("StartDateVal", model.StartDate);
                cmd.Parameters.AddWithValue("EndDateVal", model.EndDate);
                if (model.StudentClasificationId.HasValue)
                    cmd.Parameters.AddWithValue("StudentClasificationIdVal", model.StudentClasificationId.Value);
                else
                    cmd.Parameters.AddWithValue("StudentClasificationIdVal", DBNull.Value);
                if (model.OptionalGroupId.HasValue)
                    cmd.Parameters.AddWithValue("OptionalGroupIdVal", model.OptionalGroupId);
                else
                    cmd.Parameters.AddWithValue("OptionalGroupIdVal", DBNull.Value);
                
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
        /// <returns></returns>
        public async Task DeletePeriod(int id)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Delete_Period", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("PeriodIdVal", id);                
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
      /// <param name="studentId"></param>
      /// <param name="startDate"></param>
      /// <param name="endTime"></param>
      /// <returns></returns>
        public async Task<List<Period>> GetAllPeriodByStudent(int studentId,DateTime startDate, DateTime endTime)
        {
            List<Period> dt = new List<Period>();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_RoutineForStudent", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("StudentIdVal", studentId);
                cmd.Parameters.AddWithValue("StartDateVal", startDate);
                cmd.Parameters.AddWithValue("EndDateVal", endTime);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var row = new Period();
                    row.PeriodId = Convert.ToInt32(dr["PeriodId"]);
                    row.SubjectId = Convert.ToInt32(dr["SubjectId"]);
                    row.SectionId = Convert.ToInt32(dr["SectionId"]);
                    row.ClassId = Convert.ToInt32(dr["ClassId"]);
                    row.StudentId = Convert.ToInt32(dr["StudentId"]);
                    row.TeacherId = Convert.ToInt32(dr["TeacherId"]);
                    row.TeacherUserName = dr["TeacherUserName"].ToString();
                    row.TeacherFirstName = dr["TeacherFirstName"].ToString();
                    row.TeacherLastName = dr["TeacherLastName"].ToString();
                    row.SubjectName = dr["SubjectName"].ToString();

                    row.Description = dr["Description"].ToString();
                    row.ClassName = dr["ClassName"].ToString();
                    row.SectionName = dr["SectionName"].ToString();
                    row.StudentUserName = dr["StudentUserName"].ToString();
                    row.StudentLastName = dr["StudentLastName"].ToString();
                    row.StudentFirstName = dr["StudentFirstName"].ToString();
                    row.OptionalGroupName = dr["OptionalGroupName"].ToString();

                    row.StartDateTime = Convert.ToDateTime(dr["StartTime"]);
                    row.EndDateTime = Convert.ToDateTime(dr["EndTime"]);

                    dt.Add(row);
                }
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="startDate"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<List<Period>> GetAllPeriodByTeacher(int teacherId, DateTime startDate, DateTime endTime)
        {
            List<Period> dt = new List<Period>();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_RoutineForTeacher", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("TeacherIdVal", teacherId);
                cmd.Parameters.AddWithValue("StartDateVal", startDate);
                cmd.Parameters.AddWithValue("EndDateVal", endTime);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var row = new Period();
                    row.PeriodId = Convert.ToInt32(dr["PeriodId"]);
                    row.SubjectId = Convert.ToInt32(dr["SubjectId"]);
                    row.SectionId = Convert.ToInt32(dr["SectionId"]);
                    row.ClassId = Convert.ToInt32(dr["ClassId"]);
                    row.StudentId = Convert.ToInt32(dr["StudentId"]);
                    row.TeacherId = Convert.ToInt32(dr["TeacherId"]);
                    row.TeacherUserName = dr["TeacherUserName"].ToString();
                    row.TeacherFirstName = dr["TeacherFirstName"].ToString();
                    row.TeacherLastName = dr["TeacherLastName"].ToString();
                    row.SubjectName = dr["SubjectName"].ToString();

                    row.Description = dr["Description"].ToString();
                    row.ClassName = dr["ClassName"].ToString();
                    row.SectionName = dr["SectionName"].ToString();
                    row.StudentUserName = dr["StudentUserName"].ToString();
                    row.StudentLastName = dr["StudentLastName"].ToString();
                    row.StudentFirstName = dr["StudentFirstName"].ToString();
                    row.OptionalGroupName = dr["OptionalGroupName"].ToString();

                    row.StartDateTime = Convert.ToDateTime(dr["StartTime"]);
                    row.EndDateTime = Convert.ToDateTime(dr["EndTime"]);

                    dt.Add(row);
                }
            }
            return dt;
        }
    }
}
