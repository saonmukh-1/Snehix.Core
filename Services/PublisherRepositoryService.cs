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
    public class PublisherRepositoryService
    {
        MySqlConnection _connection = null;
        public PublisherRepositoryService(string conString)
        {
            _connection = new MySqlConnection(conString);
        }

        public async Task<int> CreateEbook(EbookModel model,string actor)
        {
            var result = 0;
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Create_EBook", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("PublisherIdVal", model.PublisherId);
                cmd.Parameters.AddWithValue("PublisherDescription", model.PublisherDescription);                
                cmd.Parameters.AddWithValue("Actor", actor);
                cmd.Parameters.AddWithValue("TitleVal", model.Title);
                cmd.Parameters.AddWithValue("HeightVal", model.Height);               
                cmd.Parameters.AddWithValue("WidthVal", model.Width);
                cmd.Parameters.AddWithValue("BreadthVal", model.Breadth);
                cmd.Parameters.AddWithValue("DotInInchVal", model.DotInInch);
                cmd.Parameters.AddWithValue("AuthorVal", model.Author);
                cmd.Parameters.AddWithValue("ISDNval", model.ISDN);

                cmd.Parameters.AddWithValue("EditionVal", model.Edition);
                cmd.Parameters.AddWithValue("YearVal", model.Year);
                cmd.Parameters.AddWithValue("FreeOfCost", model.FreeOfCost);
                cmd.Parameters.AddWithValue("SubjectIdVal", model.SubjectId);
                cmd.Parameters.AddWithValue("ClassIdVal", model.ClassId);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result = Convert.ToInt32(dr["EbookId"]);
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

        public async Task<int> CreatePublisher(PublisherModel model, string actor)
        {
            var result = 0;
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Create_Publisher", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("PublisherNameVal", model.PublisherName);
                cmd.Parameters.AddWithValue("ContactNameVal", model.ContactNameVal);
                cmd.Parameters.AddWithValue("Actor", actor);
                cmd.Parameters.AddWithValue("PublisherDescription", model.PublisherDescription);
                cmd.Parameters.AddWithValue("MailingAddLine1", model.MailingAddress.AddressLine1);
                cmd.Parameters.AddWithValue("MailingAddLine2", model.MailingAddress.AddressLine2);
                cmd.Parameters.AddWithValue("MailingAddLine3", model.MailingAddress.AddressLine3);
                cmd.Parameters.AddWithValue("MailingCity", model.MailingAddress.City);
                cmd.Parameters.AddWithValue("MailingState", model.MailingAddress.State);
                cmd.Parameters.AddWithValue("MailingCountry", model.MailingAddress.Country);

                cmd.Parameters.AddWithValue("MailingZip", model.MailingAddress.Zipcode);
                cmd.Parameters.AddWithValue("LandNumber", model.ContactDetail.LandLineNumber);
                cmd.Parameters.AddWithValue("AltLandline", model.ContactDetail.AltLandLineNumber);
                cmd.Parameters.AddWithValue("MobNumber", model.ContactDetail.MobileNumber);
                cmd.Parameters.AddWithValue("AltMobNumber", model.ContactDetail.AltMobileNumber);
                cmd.Parameters.AddWithValue("Email", model.ContactDetail.EmailId);
                cmd.Parameters.AddWithValue("AltEmail", model.ContactDetail.AltEmailId);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result = Convert.ToInt32(dr["PublisherId"]);
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

        public async Task CreatePublisherInstituteAssociation(int instituteId,int publisherId, string actor)
        {
           
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Create_PublisherInstituteAssociation", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("instituteidVal", instituteId);
                cmd.Parameters.AddWithValue("pubId", publisherId);
                cmd.Parameters.AddWithValue("Actor", actor);                
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

        public async Task CreatePublisherUserAssociation(int userId, int publisherId, string actor)
        {

            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Create_PublisherUserAssociation", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("useridVal", userId);
                cmd.Parameters.AddWithValue("pubId", publisherId);
                cmd.Parameters.AddWithValue("Actor", actor);
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

        public async Task<List<PublisherDTO>> GetAllPublisher()
        {
            var dt = new List<PublisherDTO>();
            await _connection.OpenAsync();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Get_PublisherList", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var row = new PublisherDTO();
                        row.Id = Convert.ToInt32(dr["ID"]);
                        row.Name = dr["PublisherName"].ToString();
                        row.Description = dr["Description"].ToString();
                        row.ContactName = dr["ContactName"].ToString();
                        if (int.TryParse(dr["InstituteId"].ToString(), out int instId))
                            row.InstituteId = instId;                        
                        row.InstituteName = dr["InstituteName"].ToString();                       
                        if (int.TryParse(dr["UserId"].ToString(), out int userId))
                            row.UserId = userId;
                        row.UserName = dr["UserName"].ToString();
                        row.UserFullName = dr["UserFullName"].ToString();

                        if (int.TryParse(dr["AmazonAccountId"].ToString(), out int amazonAccountId))
                            row.AmazonAccountId = amazonAccountId;
                        row.BucketName = dr["BucketName"].ToString();
                        row.AccessKey = dr["AccessKey"].ToString();
                        row.SecretKey = dr["SecretKey"].ToString();
                        row.IamUserName = dr["IamUserName"].ToString();

                        dt.Add(row);
                    }
                }

                return dt;
            }
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }

        public async Task<PublisherDTO> GetPublisherById(int publisherId)
        {
            var row = new PublisherDTO();
            await _connection.OpenAsync();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("Get_PublisherListById", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("publisherIdVal", publisherId);
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {                        
                        row.Id = Convert.ToInt32(dr["ID"]);
                        row.Name = dr["PublisherName"].ToString();
                        row.Description = dr["Description"].ToString();
                        row.ContactName = dr["ContactName"].ToString();
                        if (int.TryParse(dr["InstituteId"].ToString(), out int instId))
                            row.InstituteId = instId;
                        row.InstituteName = dr["InstituteName"].ToString();
                        if (int.TryParse(dr["UserId"].ToString(), out int userId))
                            row.UserId = userId;
                        row.UserName = dr["UserName"].ToString();
                        row.UserFullName = dr["UserFullName"].ToString();
                        if (int.TryParse(dr["AmazonAccountId"].ToString(), out int amazonAccountId))
                            row.AmazonAccountId = amazonAccountId;
                        row.BucketName = dr["BucketName"].ToString();
                        row.AccessKey = dr["AccessKey"].ToString();
                        row.SecretKey = dr["SecretKey"].ToString();
                        row.IamUserName = dr["IamUserName"].ToString();
                    }
                }

                return row;
            }
            catch { throw; }
            finally { await _connection.CloseAsync(); }
        }

        public async Task CreatePublisherAmazonAccount(PublisherAmazonAccount model)
        {
            try
            {
                await _connection.OpenAsync();
                var cmd = new MySqlCommand("Create_PublisherAmazonAccount", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("PublisherIdVal", model.PublisherId);
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
