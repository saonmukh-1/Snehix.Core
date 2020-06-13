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
    public class InstituteRepositoryService
    {
        MySqlConnection _connection = null;
        public InstituteRepositoryService(string conString)
        {
            _connection = new MySqlConnection(conString);
        }

        public async Task CreateInstitute(InstitutionModel model)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Create_Institute", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("InstituteName", model.Name);
                cmd.Parameters.AddWithValue("InstituteBranch", model.BranchName);
                cmd.Parameters.AddWithValue("InstituteDescription", model.Description);
                cmd.Parameters.AddWithValue("BoardId", model.BoardId);
                cmd.Parameters.AddWithValue("TypeId", model.TypeId);
                cmd.Parameters.AddWithValue("CreatedBy", model.Actor);

                cmd.Parameters.AddWithValue("MailingAddLine1", model.MailingAddress.AddressLine1);
                cmd.Parameters.AddWithValue("MailingAddLine2", model.MailingAddress.AddressLine2);
                cmd.Parameters.AddWithValue("MailingAddLine3", model.MailingAddress.AddressLine3);
                cmd.Parameters.AddWithValue("MailingCity", model.MailingAddress.City);
                cmd.Parameters.AddWithValue("MailingState", model.MailingAddress.State);
                cmd.Parameters.AddWithValue("MailingCountry", model.MailingAddress.Country);
                cmd.Parameters.AddWithValue("MailingZip", model.MailingAddress.Zipcode);

                cmd.Parameters.AddWithValue("BillingAddLine1", model.BillingAddress.AddressLine1);
                cmd.Parameters.AddWithValue("BillingAddLine2", model.BillingAddress.AddressLine2);
                cmd.Parameters.AddWithValue("BillingAddLine3", model.BillingAddress.AddressLine3);
                cmd.Parameters.AddWithValue("BillingCity", model.BillingAddress.City);
                cmd.Parameters.AddWithValue("BillingState", model.BillingAddress.State);
                cmd.Parameters.AddWithValue("BillingCountry", model.BillingAddress.Country);
                cmd.Parameters.AddWithValue("BillingZip", model.BillingAddress.Zipcode);

                cmd.Parameters.AddWithValue("LandNumber", model.ContactDetail.LandLineNumber);
                cmd.Parameters.AddWithValue("AltLandline", model.ContactDetail.AltLandLineNumber);
                cmd.Parameters.AddWithValue("MobNumber", model.ContactDetail.MobileNumber);
                cmd.Parameters.AddWithValue("AltMobNumber", model.ContactDetail.AltMobileNumber);
                cmd.Parameters.AddWithValue("Email", model.ContactDetail.EmailId);
                cmd.Parameters.AddWithValue("AltEmail", model.ContactDetail.AltEmailId);
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

        public async Task UpdateInstitute(InstitutionModelUpdate model, int ID)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Update_Institute", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("instituteId", ID);
                cmd.Parameters.AddWithValue("InstituteName", model.Name);
                cmd.Parameters.AddWithValue("InstituteBranch", model.BranchName);
                cmd.Parameters.AddWithValue("InstituteDescription", model.Description);
                cmd.Parameters.AddWithValue("BoardId", model.BoardId);
                cmd.Parameters.AddWithValue("TypeId", model.TypeId);
                cmd.Parameters.AddWithValue("ModifiedBy", model.Actor);               
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


        public async Task<List<InstituteDTO>> GetAllInstitutes()
        {
            var dt = new List<InstituteDTO>();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_Institutes", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var row = new InstituteDTO();
                    row.Id = Convert.ToInt32(dr["Id"]);
                    row.Name = dr["Name"].ToString();
                    row.Description = dr["Description"].ToString();
                    row.EducationalBoard = dr["EducationalBoard"].ToString();
                    row.InstitutionType = dr["InstitutionType"].ToString();
                   
                    dt.Add(row);
                }
            }

            return dt;
        }

        public async Task<List<InstituteDTO>> GetInstituteById(int Id)
        {
            var dt = new List<InstituteDTO>();
            await _connection.OpenAsync();
            using (MySqlCommand cmd = new MySqlCommand("Get_InstitutesById", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("InstituteId", Id);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var row = new InstituteDTO();
                    row.Id = Convert.ToInt32(dr["Id"]);
                    row.Name = dr["Name"].ToString();
                    row.Description = dr["Description"].ToString();
                    row.EducationalBoard = dr["EducationalBoard"].ToString();
                    row.InstitutionType = dr["InstitutionType"].ToString();

                    dt.Add(row);
                }
            }

            return dt;
        }
       
    }
}
