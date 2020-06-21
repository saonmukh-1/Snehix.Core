using System;
using System.Collections.Generic;
using System.Data;
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

namespace Snehix.Core.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [CustomException]
    [ModelValidationAction]    
    public class UserRegistartionController : ControllerBase
    {
        string connString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public UserRegistartionController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("Default");
        }

        /// <summary>
        /// Create user registration
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(UserRegistrationModel model)
        {
           
            var service = new UserRepositoryService(connString);
            await service.CreateUserRegistration(model.UserId, model.InstituteId, "User1", model.StartDate);
                
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "User Registrtion Created successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);

        }
        /// <summary>
        /// update user registration
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UserRegistrationUpdateModel model)
        {            
            var service = new UserRepositoryService(connString);
            await service.TerminateUserRegistration(id,"user1",model.EndDate);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "User Registrtion Updated successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }

        /// <summary>
        /// Get user registrion by institute id, if institute id not sent will get all user registration
        /// </summary>
        /// <param name="instituteId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get(int? instituteId)
        {
            var result = new List<UserRegistrationDTO>();
            var service = new UserRepositoryService(connString);
            if (instituteId.HasValue)
                result = await service.GetAllUserRegistrationByInstituteId(instituteId.Value);
            else
                result = await service.GetAllUserRegistration();
            var response = new GenericResponse<List<UserRegistrationDTO>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        /// <summary>
        /// Get user registration by user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var service = new UserRepositoryService(connString);
            var result = await service.GetUserRegistrationByUserId(id);
            var response = new GenericResponse<List<UserRegistrationDTO>>()
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