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
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/OptionalGroup")]
    [CustomException]
    [ModelValidationAction]
    public class OptionalGroupController : Controller
    {
        string connString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public OptionalGroupController(IConfiguration configuration)
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
        public async Task<IActionResult> Create(OptionalGroupModel model)
        {
            var username = ApplicationUtility.GetTokenAttribute(Request.Headers["Authorization"], "sub");
            var service = new StudentClassificationRepository(connString);
            await service.CreateGroup(model, username);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Optional group created successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }
        /// <summary>
        /// update student classification
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, OptionalGroupUpdateModel model)
        {
            var username = ApplicationUtility.GetTokenAttribute(Request.Headers["Authorization"], "sub");
            var service = new StudentClassificationRepository(connString);
            await service.UpdateGroup(id, model, username);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Optional group updated successfully.",
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
            var result = await service.GetGroupById(id);
            var response = new GenericResponse<List<OptionalGroup>>()
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
        [HttpGet("Institute/{id}")]
        public async Task<IActionResult> GetAssignedByInstitute(int id)
        {
            var service = new StudentClassificationRepository(connString);
            var result = await service.GetGroupByInstitute(id);
            var response = new GenericResponse<List<OptionalGroup>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        /// <summary>
        /// Delete student classification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var service = new StudentClassificationRepository(connString);
            await service.DeleteGroup(id);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Group deleted successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }
    }
}