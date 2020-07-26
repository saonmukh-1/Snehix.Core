using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Snehix.Core.API.DTO;
using Snehix.Core.API.Filters;
using Snehix.Core.API.Models;
using Snehix.Core.API.Services;
using Snehix.Core.API.Utility;

namespace Snehix.Core.API.Controllers
{
    [Route("api/[controller]")]
    [CustomException]
    [ModelValidationAction]
    public class PublisherController : Controller
    {
        string connString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public PublisherController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("Default");
            AmazonIAMService.AWSAccessKey = configuration.GetValue<string>("AWSAccessKey");
            AmazonIAMService.AWSSecurityKey = configuration.GetValue<string>("AWSSecurityKey");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(PublisherModel model)
        {
            var username = ApplicationUtility.GetTokenAttribute(Request.Headers["Authorization"], "sub");
            var service = new PublisherRepositoryService(connString);
            var pubId = await service.CreatePublisher(model,username);
            PublisherAmazonAccount amazonAccountModel;
            if (model.CloudAccountRequired)
            {
                var bucketSevice = new AmazonS3Service();
                var bucketName = GetName(model.PublisherName);
                await bucketSevice.CreateBucketToS3(bucketName);
                var iamService = new AmazonIAMService();
                var iamUserName = GetName(model.PublisherName);
                var accesKeyInfo = await iamService.CreateIAMUser("/", iamUserName);
                amazonAccountModel = new PublisherAmazonAccount()
                {
                    AccessKey = accesKeyInfo.AccessKey,
                    Actor = username,
                    BucketName = bucketName,
                    IamUsername = iamUserName,
                    PublisherId = pubId,
                    SecretKey = accesKeyInfo.SecurityKey
                };
                await service.CreatePublisherAmazonAccount(amazonAccountModel);
            }
            var response = new GenericResponse<int>()
            {
                IsSuccess = true,
                Message = "Publisher created successfully.",
                ResponseCode = 200,
                Result = pubId
            };
            return Ok(response);
        }

        private string GetName(string name)
        {
            return name.ToLower().Substring(0, 3).TrimEnd().TrimStart() + Guid.NewGuid().ToString().ToLowerInvariant();
        }

        [Authorize]
        [HttpPost("publisherinstituteasscoiation")]
        public async Task<IActionResult> CreateInstituteAssociation(PublisherAssociationModel model)
        {
            var username = ApplicationUtility.GetTokenAttribute(Request.Headers["Authorization"], "sub");
            var service = new PublisherRepositoryService(connString);            
            await service.CreatePublisherInstituteAssociation(model.InstituteId, model.PublisherId, username);            
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Publisher Institute Association created successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }

        [Authorize]
        [HttpPost("publisheruserasscoiation")]
        public async Task<IActionResult> CreateUserAssociation(PublisherUserAssociationModel model)
        {
            var username = ApplicationUtility.GetTokenAttribute(Request.Headers["Authorization"], "sub");
            var service = new PublisherRepositoryService(connString);
            await service.CreatePublisherUserAssociation(model.UserId, model.PublisherId, username);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Publisher User Association created successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var service = new PublisherRepositoryService(connString);
            var result = await service.GetAllPublisher();
            var response = new GenericResponse<List<PublisherDTO>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        [Authorize]
        [HttpGet("id")]
        public async Task<IActionResult> Get(int id)
        {
            var service = new PublisherRepositoryService(connString);
            var result = await service.GetPublisherById(id);
            var response = new GenericResponse<PublisherDTO>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);

        }

        [Authorize]
        [HttpPost("ebook")]
        public async Task<IActionResult> CreateEbook(EBookModel model)
        {
            var username = ApplicationUtility.GetTokenAttribute(Request.Headers["Authorization"], "sub");
            var service = new PublisherRepositoryService(connString);
            var pubId = await service.CreateEBook(model, username);
           
            var response = new GenericResponse<int>()
            {
                IsSuccess = true,
                Message = "Ebook created successfully.",
                ResponseCode = 200,
                Result = pubId
            };
            return Ok(response);
        }
        [Authorize]
        [HttpPut("ebook")]
        public async Task<IActionResult> UpdateEbook(EbookUpdateModel model)
        {
            var username = ApplicationUtility.GetTokenAttribute(Request.Headers["Authorization"], "sub");
            var service = new PublisherRepositoryService(connString);
            await service.UpdateEbook(model, username);

            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Ebook updated successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }
        [Authorize]
        [HttpGet("ebook/{Id}")]
        public async Task<IActionResult> GetEbookById(int Id)
        {
            var username = ApplicationUtility.GetTokenAttribute(Request.Headers["Authorization"], "sub");
            var service = new PublisherRepositoryService(connString);
            var result = await service.GetEBookById(Id);

            var response = new GenericResponse<EBookDTODetail>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }
        [Authorize]
        [HttpGet("ebook/{Id}/publisher")]
        public async Task<IActionResult> GetEbookBypublisher(int Id)
        {
            var username = ApplicationUtility.GetTokenAttribute(Request.Headers["Authorization"], "sub");
            var service = new PublisherRepositoryService(connString);
            var result = await service.GetEBookByPublisherId(Id);

            var response = new GenericResponse<List<EBookDTO>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }
    }
}