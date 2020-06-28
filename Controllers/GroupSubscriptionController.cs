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
    [Produces("application/json")]
    [Route("api/GroupSubscription")]
    [CustomException]
    [ModelValidationAction]
    public class GroupSubscriptionController : Controller
    {
        string connString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public GroupSubscriptionController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("Default");
        }

        /// <summary>
        /// Create student classification
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(GroupSubscriptionModel model)
        {
            var username = ApplicationUtility.GetTokenAttribute(Request.Headers["Authorization"], "sub");
            var service = new StudentClassificationRepository(connString);
            await service.CreateGroupSubscription(model, username);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Group subscription created successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Deactive")]
        public async Task<IActionResult> Deactive(DeactiveGroupSubscriptionModel model)
        {
            var username = ApplicationUtility.GetTokenAttribute(Request.Headers["Authorization"], "sub");
            var service = new StudentClassificationRepository(connString);
            await service.DeactiveGroupSubscription(model, username);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Subscription deactivated successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }

        /// <summary>
        /// Get student classification by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var service = new StudentClassificationRepository(connString);
            var result = await service.GetGroupSubscriptionById(id);
            var response = new GenericResponse<List<OptionalGroupSubscription>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        /// <summary>
        /// Get student classic=fication by institute
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("User/{id}")]
        public async Task<IActionResult> GetSubscriptionByUser(int id)
        {
            var service = new StudentClassificationRepository(connString);
            var result = await service.GetSubscriptionByUser(id);
            var response = new GenericResponse<List<OptionalGroupSubscription>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        /// <summary>
        /// Get student classic=fication by institute
        /// </summary>
        /// <param name="id">GroupSubscriptionId</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GroupSubscription/{id}")]
        public async Task<IActionResult> GetSubscription(int id)
        {
            var service = new StudentClassificationRepository(connString);
            var result = await service.GetUserLisByGroupSubscription(id);
            var response = new GenericResponse<List<OptionalGroupSubscriptionUser>>()
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