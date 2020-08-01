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

        public async Task<int> CreateInstitute(InstitutionModel model)
        {
            int result = 0;
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
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result = Convert.ToInt32(dr["InstituteId"]);                   
                }
                return result;
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
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }


        public async Task<List<InstituteDTO>> GetAllInstitutes()
        {
            var dt = new List<InstituteDTO>();
            await _connection.OpenAsync();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Get_Institutes", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var row = new InstituteDTO();
                        row.Id = Convert.ToInt32(dr["InstituteId"]);
                        row.Name = dr["InstituteName"].ToString();
                        row.BranchName = dr["InstituteBranchName"].ToString();
                        row.Description = dr["InstituteDescription"].ToString();

                        row.InstitutionType = new EntityDetail();
                        if (int.TryParse(dr["InstituteTypeId"].ToString(), out int InstituteTypeId))
                            row.InstitutionType.Id = InstituteTypeId;
                        row.InstitutionType.Name = dr["InstituteTypeName"].ToString();

                        row.EducationalBoard = new EntityDetail();
                        if (int.TryParse(dr["EducationalBoardId"].ToString(), out int EducationalBoardId))
                            row.EducationalBoard.Id = EducationalBoardId;
                        row.EducationalBoard.Name = dr["EducationalBoardName"].ToString();

                        row.ContactInfo = new DTO.Contact();
                        if (int.TryParse(dr["ContactId"].ToString(), out int ContactId))
                            row.ContactInfo.ContactId = ContactId;
                        row.ContactInfo.LandLineNumber = dr["LandlineNumber"].ToString();
                        row.ContactInfo.AltLandLineNumber = dr["AlternateLandLineNumber"].ToString();
                        row.ContactInfo.MobileNumber = dr["MobileNumber"].ToString();
                        row.ContactInfo.AltMobileNumber = dr["AlternateMobileNumber"].ToString();
                        row.ContactInfo.EmailId = dr["EmailId"].ToString();
                        row.ContactInfo.AltEmailId = dr["AlternateEmailId"].ToString();

                        row.BillingAddress = new DTO.Address();
                        if (int.TryParse(dr["BillingAddressId"].ToString(), out int BillingAddressId))
                            row.BillingAddress.AddressId = BillingAddressId;
                        row.BillingAddress.AddressLine1 = dr["BillingAddressLine1"].ToString();
                        row.BillingAddress.AddressLine2 = dr["BillingAddressLine2"].ToString();
                        row.BillingAddress.AddressLine3 = dr["BillingAddressLine3"].ToString();
                        row.BillingAddress.City = dr["BillingAddressCity"].ToString();
                        row.BillingAddress.Zipcode = dr["BillingAddressZip"].ToString();
                        row.BillingAddress.State = new EntityDetail();
                        if (int.TryParse(dr["BillingStateId"].ToString(), out int BillingStateId))
                            row.BillingAddress.State.Id = BillingStateId;
                        row.BillingAddress.State.Name = dr["BillingState"].ToString();
                        row.BillingAddress.Country = new EntityDetail();
                        if (int.TryParse(dr["BillingCountryId"].ToString(), out int BillingCountryId))
                            row.BillingAddress.Country.Id = BillingCountryId;
                        row.BillingAddress.Country.Name = dr["BillingCountry"].ToString();

                        row.MailingAddress = new DTO.Address();
                        if (int.TryParse(dr["MailingAddressId"].ToString(), out int MailingAddressId))
                            row.MailingAddress.AddressId = MailingAddressId;
                        row.MailingAddress.AddressLine1 = dr["MailingAddressLine1"].ToString();
                        row.MailingAddress.AddressLine2 = dr["MailingAddressLine2"].ToString();
                        row.MailingAddress.AddressLine3 = dr["MailingAddressLine3"].ToString();
                        row.MailingAddress.City = dr["MailingAddressCity"].ToString();
                        row.MailingAddress.Zipcode = dr["MailingAddressZip"].ToString();
                        row.MailingAddress.State = new EntityDetail();
                        if (int.TryParse(dr["MailingStateId"].ToString(), out int MailingStateId))
                            row.MailingAddress.State.Id = MailingStateId;
                        row.MailingAddress.State.Name = dr["MailingState"].ToString();

                        row.MailingAddress.Country = new EntityDetail();
                        if (int.TryParse(dr["MailingCountryId"].ToString(), out int MailingCountryId))
                            row.MailingAddress.Country.Id = MailingCountryId;
                        row.MailingAddress.Country.Name = dr["MaillingCountry"].ToString();

                        dt.Add(row);
                    }
                }

                return dt;
            }
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }


        public async Task<List<InstituteShortDTO>> GetInstitutesByNameBranchName(string name, 
            string branchName)
        {
            var dt = new List<InstituteShortDTO>();
            await _connection.OpenAsync();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Get_InstituteByNameAndBranch", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("NameVal", name);
                    cmd.Parameters.AddWithValue("BranchNameVal", branchName);
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var row = new InstituteShortDTO();
                        row.Id = Convert.ToInt32(dr["ID"]);
                        row.Name = dr["Name"].ToString();
                        row.Branch = dr["BranchName"].ToString();
                        dt.Add(row);
                    }
                }
                return dt;
            }
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }

        public async Task<List<InstituteDTO>> GetAllInstitutesByName(string name)
        {
            var dt = new List<InstituteDTO>();
            await _connection.OpenAsync();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Get_InstitutesByName", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("InstituteNameVal", name);
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var row = new InstituteDTO();
                        row.Id = Convert.ToInt32(dr["InstituteId"]);
                        row.Name = dr["InstituteName"].ToString();
                        row.BranchName = dr["InstituteBranchName"].ToString();
                        row.Description = dr["InstituteDescription"].ToString();

                        row.InstitutionType = new EntityDetail();
                        if (int.TryParse(dr["InstituteTypeId"].ToString(), out int InstituteTypeId))
                            row.InstitutionType.Id = InstituteTypeId;
                        row.InstitutionType.Name = dr["InstituteTypeName"].ToString();

                        row.EducationalBoard = new EntityDetail();
                        if (int.TryParse(dr["EducationalBoardId"].ToString(), out int EducationalBoardId))
                            row.EducationalBoard.Id = EducationalBoardId;
                        row.EducationalBoard.Name = dr["EducationalBoardName"].ToString();

                        row.ContactInfo = new DTO.Contact();
                        if (int.TryParse(dr["ContactId"].ToString(), out int ContactId))
                            row.ContactInfo.ContactId = ContactId;                        
                        row.ContactInfo.LandLineNumber = dr["LandlineNumber"].ToString();
                        row.ContactInfo.AltLandLineNumber = dr["AlternateLandLineNumber"].ToString();
                        row.ContactInfo.MobileNumber = dr["MobileNumber"].ToString();
                        row.ContactInfo.AltMobileNumber = dr["AlternateMobileNumber"].ToString();
                        row.ContactInfo.EmailId = dr["EmailId"].ToString();
                        row.ContactInfo.AltEmailId = dr["AlternateEmailId"].ToString();

                        row.BillingAddress = new DTO.Address();                        
                        if (int.TryParse(dr["BillingAddressId"].ToString(), out int BillingAddressId))                                
                            row.BillingAddress.AddressId = BillingAddressId;
                        row.BillingAddress.AddressLine1 = dr["BillingAddressLine1"].ToString();
                        row.BillingAddress.AddressLine2 = dr["BillingAddressLine2"].ToString();
                        row.BillingAddress.AddressLine3 = dr["BillingAddressLine3"].ToString();
                        row.BillingAddress.City = dr["BillingAddressCity"].ToString();
                        row.BillingAddress.Zipcode = dr["BillingAddressZip"].ToString();
                        row.BillingAddress.State = new EntityDetail();                        
                        if (int.TryParse(dr["BillingStateId"].ToString(), out int BillingStateId))
                            row.BillingAddress.State.Id = BillingStateId;
                        row.BillingAddress.State.Name = dr["BillingState"].ToString();
                        row.BillingAddress.Country = new EntityDetail();
                        if (int.TryParse(dr["BillingCountryId"].ToString(), out int BillingCountryId))                                
                            row.BillingAddress.Country.Id = BillingCountryId;
                        row.BillingAddress.Country.Name = dr["BillingCountry"].ToString();

                        row.MailingAddress = new DTO.Address();
                        if (int.TryParse(dr["MailingAddressId"].ToString(), out int MailingAddressId))
                            row.MailingAddress.AddressId = MailingAddressId;                        
                        row.MailingAddress.AddressLine1 = dr["MailingAddressLine1"].ToString();
                        row.MailingAddress.AddressLine2 = dr["MailingAddressLine2"].ToString();
                        row.MailingAddress.AddressLine3 = dr["MailingAddressLine3"].ToString();
                        row.MailingAddress.City = dr["MailingAddressCity"].ToString();
                        row.MailingAddress.Zipcode = dr["MailingAddressZip"].ToString();
                        row.MailingAddress.State = new EntityDetail();
                        if (int.TryParse(dr["MailingStateId"].ToString(), out int MailingStateId))
                            row.MailingAddress.State.Id = MailingStateId;                        
                        row.MailingAddress.State.Name = dr["MailingState"].ToString();

                        row.MailingAddress.Country = new EntityDetail();
                        if (int.TryParse(dr["MailingCountryId"].ToString(), out int MailingCountryId))
                            row.MailingAddress.Country.Id = MailingCountryId;
                        row.MailingAddress.Country.Name = dr["MaillingCountry"].ToString(); 

                        dt.Add(row);
                    }
                }

                return dt.Where(p=>p.Name.Contains(name)).ToList();
            }
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }

        public async Task<InstituteDTO> GetInstituteById(int Id)
        {
            InstituteDTO row = null;
            await _connection.OpenAsync();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Get_InstitutesById", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("InstituteIdVal", Id);
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        row = new InstituteDTO();
                        row.Id = Convert.ToInt32(dr["InstituteId"]);
                        row.Name = dr["InstituteName"].ToString();
                        row.BranchName = dr["InstituteBranchName"].ToString();
                        row.Description = dr["InstituteDescription"].ToString();

                        row.InstitutionType = new EntityDetail();
                        if (int.TryParse(dr["InstituteTypeId"].ToString(), out int InstituteTypeId))
                            row.InstitutionType.Id = InstituteTypeId;
                        row.InstitutionType.Name = dr["InstituteTypeName"].ToString();

                        row.EducationalBoard = new EntityDetail();
                        if (int.TryParse(dr["EducationalBoardId"].ToString(), out int EducationalBoardId))
                            row.EducationalBoard.Id = EducationalBoardId;
                        row.EducationalBoard.Name = dr["EducationalBoardName"].ToString();

                        row.ContactInfo = new DTO.Contact();
                        if (int.TryParse(dr["ContactId"].ToString(), out int ContactId))
                            row.ContactInfo.ContactId = ContactId;
                        row.ContactInfo.LandLineNumber = dr["LandlineNumber"].ToString();
                        row.ContactInfo.AltLandLineNumber = dr["AlternateLandLineNumber"].ToString();
                        row.ContactInfo.MobileNumber = dr["MobileNumber"].ToString();
                        row.ContactInfo.AltMobileNumber = dr["AlternateMobileNumber"].ToString();
                        row.ContactInfo.EmailId = dr["EmailId"].ToString();
                        row.ContactInfo.AltEmailId = dr["AlternateEmailId"].ToString();

                        row.BillingAddress = new DTO.Address();
                        if (int.TryParse(dr["BillingAddressId"].ToString(), out int BillingAddressId))
                            row.BillingAddress.AddressId = BillingAddressId;
                        row.BillingAddress.AddressLine1 = dr["BillingAddressLine1"].ToString();
                        row.BillingAddress.AddressLine2 = dr["BillingAddressLine2"].ToString();
                        row.BillingAddress.AddressLine3 = dr["BillingAddressLine3"].ToString();
                        row.BillingAddress.City = dr["BillingAddressCity"].ToString();
                        row.BillingAddress.Zipcode = dr["BillingAddressZip"].ToString();
                        row.BillingAddress.State = new EntityDetail();
                        if (int.TryParse(dr["BillingStateId"].ToString(), out int BillingStateId))
                            row.BillingAddress.State.Id = BillingStateId;
                        row.BillingAddress.State.Name = dr["BillingState"].ToString();
                        row.BillingAddress.Country = new EntityDetail();
                        if (int.TryParse(dr["BillingCountryId"].ToString(), out int BillingCountryId))
                            row.BillingAddress.Country.Id = BillingCountryId;
                        row.BillingAddress.Country.Name = dr["BillingCountry"].ToString();

                        row.MailingAddress = new DTO.Address();
                        if (int.TryParse(dr["MailingAddressId"].ToString(), out int MailingAddressId))
                            row.MailingAddress.AddressId = MailingAddressId;
                        row.MailingAddress.AddressLine1 = dr["MailingAddressLine1"].ToString();
                        row.MailingAddress.AddressLine2 = dr["MailingAddressLine2"].ToString();
                        row.MailingAddress.AddressLine3 = dr["MailingAddressLine3"].ToString();
                        row.MailingAddress.City = dr["MailingAddressCity"].ToString();
                        row.MailingAddress.Zipcode = dr["MailingAddressZip"].ToString();
                        row.MailingAddress.State = new EntityDetail();
                        if (int.TryParse(dr["MailingStateId"].ToString(), out int MailingStateId))
                            row.MailingAddress.State.Id = MailingStateId;
                        row.MailingAddress.State.Name = dr["MailingState"].ToString();

                        row.MailingAddress.Country = new EntityDetail();
                        if (int.TryParse(dr["MailingCountryId"].ToString(), out int MailingCountryId))
                            row.MailingAddress.Country.Id = MailingCountryId;
                        row.MailingAddress.Country.Name = dr["MaillingCountry"].ToString();
                    }
                }

                return row;
            }
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }

        public async Task CreateInstituteAmazonAccount(InstituteAmazonAccount model)
        {            
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Create_InstituteAmazonAccount", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("InstituteIdVal", model.InstituteId);
                cmd.Parameters.AddWithValue("BucketNameVal", model.BucketName);
                cmd.Parameters.AddWithValue("AccessKeyVal", model.AccessKey);
                cmd.Parameters.AddWithValue("SecretKeyVal", model.SecretKey);
                cmd.Parameters.AddWithValue("IamUserNameVal", model.IamUsername);
                cmd.Parameters.AddWithValue("Actor", model.Actor);  
                var dr = cmd.ExecuteNonQuery();                
            }
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }

    }
}
