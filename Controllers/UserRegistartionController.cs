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
    [Route("api/[controller]")]
    [CustomException]
    [ModelValidationAction]    
    public class UserRegistartionController : ControllerBase
    {
        public string connString { get; set; }
        public UserRegistartionController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("Default");
        }

        // Post api/User
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
        // PUT api/values/5
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

        // GET api/Entity
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

        // GET api/values/5
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