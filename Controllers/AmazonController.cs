using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snehix.Core.API.DTO;
using Snehix.Core.API.Filters;
using Snehix.Core.API.Models;
using Snehix.Core.API.Services;

namespace Snehix.Core.API.Controllers
{
    [CustomException]
    [ModelValidationAction]
    [Route("api/Amazon")]
    public class AmazonController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("post")]
        public async Task<IActionResult> CreateIAM(IAMModel model)
        {
            var s3service = new AmazonS3Service();
            await s3service.CreateBucketToS3(model.BucketName);
            var iamservice = new AmazonIAMService();
            await iamservice.CreateIAMUser(@"/"+model.BucketName+@"/", model.UserName);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "User created in IIM successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("get")]
        public async Task<IActionResult> ListIIM()
        {
            //var username = ApplicationUtility.GetTokenAttribute(Request.Headers["Authorization"], "sub");
            //string value = Request.Headers["Authorization"];
            //value = value.Remove(0, 7);
            //var jwtHandler = new JwtSecurityTokenHandler();
            //var token = jwtHandler.ReadJwtToken(value);
            //var usernameClaim = token.Claims.Where(a=>a.Type=="sub").FirstOrDefault();
            var service = new AmazonIAMService();
            var res = await service.ListUsers();
            var response = new GenericResponse<List<string>>()
            {
                IsSuccess = true,
                Message = "User created in IAM successfully.",
                ResponseCode = 200,
                Result = res
            };
            return Ok(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("delete/{username}")]
        public async Task<IActionResult> DeleteIIM(string username)
        {

            var service = new AmazonIAMService();
            await service.DeleteUser(username);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "User deleted successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }
        [HttpPost("postbucket")]
        public async Task<IActionResult> CreateBucket(BucketModel model)
        {
            var service = new AmazonS3Service();
            await service.CreateBucketToS3(model.Name);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Bucket created successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }
    }
}