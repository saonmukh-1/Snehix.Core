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
    [CustomException]
    [ModelValidationAction]
    [Route("api/[controller]")]
    public class InstituteController : ControllerBase
    {

        string connString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public InstituteController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("Default");
            AmazonIAMService.AWSAccessKey = configuration.GetValue<string>("AWSAccessKey");
            AmazonIAMService.AWSSecurityKey = configuration.GetValue<string>("AWSSecurityKey");
        }

        /// <summary>
        /// Get all institutes
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var service = new InstituteRepositoryService(connString);
            var result = await service.GetAllInstitutes();
            var response = new GenericResponse<List<InstituteDTO>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);

        }

        /// <summary>
        /// Get all institutes
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("institutes/{name}")]
        public async Task<IActionResult> GetInstitutesByName(string name)
        {
            var service = new InstituteRepositoryService(connString);
            var result = await service.GetAllInstitutesByName(name);
            var response = new GenericResponse<List<InstituteDTO>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        /// <summary>
        /// Get institute by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var service = new InstituteRepositoryService(connString);
            var result = await service.GetInstituteById(id);
            var response = new GenericResponse<List<InstituteDTO>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        [Authorize]
        [HttpPost("checkduplicate")]
        public async Task<IActionResult> CheckDuplicate(InstitutionDuplicateModel model)
        {
            var service = new InstituteRepositoryService(connString);
            var res = await service.GetInstitutesByNameBranchName(model.Name,model.BranchName);
            bool result = res.Count > 0;
            var response = new GenericResponse<bool>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        /// <summary>
        /// Create institution
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(InstitutionModel model)
        {
            var username = ApplicationUtility.GetTokenAttribute(Request.Headers["Authorization"], "sub");
            var service = new InstituteRepositoryService(connString);
            InstituteAmazonAccount amazonAccountModel = null;
            model.Actor = username;
            var id = await service.CreateInstitute(model);
            if (model.CloudAccountRequired)
            {
                var bucketSevice = new AmazonS3Service();
                var bucketName = GetName(model.Name);
                await bucketSevice.CreateBucketToS3(bucketName);
                var iamService = new AmazonIAMService();
                var iamUserName = GetName(model.Name);
                var accesKeyInfo = await iamService.CreateIAMUser("/", iamUserName);
                amazonAccountModel = new InstituteAmazonAccount()
                {
                    AccessKey = accesKeyInfo.AccessKey,
                    Actor = username,
                    BucketName = bucketName,
                    IamUsername = iamUserName,
                    InstituteId = id,
                    SecretKey = accesKeyInfo.SecurityKey
                };
                await service.CreateInstituteAmazonAccount(amazonAccountModel);
            }

            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Institute created successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);

        }

        private string GetName(string name)
        {
            return name.ToLower().Substring(0, 3).TrimEnd().TrimStart() + Guid.NewGuid().ToString().ToLowerInvariant();
        }

        /// <summary>
        /// Update institution
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, InstitutionModelUpdate model)
        {
            var username = ApplicationUtility.GetTokenAttribute(Request.Headers["Authorization"], "sub");
            model.Actor = username;
            var service = new InstituteRepositoryService(connString);
            await service.UpdateInstitute(model, id);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Institute updated successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);

        }
        /// <summary>
        /// create cloud account 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("createcloudaccount/{id}")]
        public async Task<IActionResult> CreateAmazonAccount(int id)
        {
            var username = ApplicationUtility.GetTokenAttribute(Request.Headers["Authorization"], "sub");
            var service = new InstituteRepositoryService(connString);
            var institute = (await service.GetInstituteById(id)).FirstOrDefault().Name;
            var bucketSevice = new AmazonS3Service();
            var bucketName = GetName(institute);
            await bucketSevice.CreateBucketToS3(bucketName);
            var iamService = new AmazonIAMService();
            var iamUserName = GetName(institute);
            var accesKeyInfo = await iamService.CreateIAMUser("/", iamUserName);
            var amazonAccountModel = new InstituteAmazonAccount()
            {
                AccessKey = accesKeyInfo.AccessKey,
                Actor = username,
                BucketName = bucketName,
                IamUsername = iamUserName,
                InstituteId = id,
                SecretKey = accesKeyInfo.SecurityKey
            };
            await service.CreateInstituteAmazonAccount(amazonAccountModel);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Cloud account created successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);

        }
    }
}