using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Snehix.Core.API.Models;
using Snehix.Core.API.Services;

namespace Snehix.Core.API.Controllers
{
    [Route("api/[controller]")]
    
    public class UserRegistartionController : ControllerBase
    {
        public string connString { get; set; }
        public UserRegistartionController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("Default");
        }

        // Post api/User
        [HttpPost]
        public async Task<IActionResult> Create(UserRegistrationModel model)
        {
            try
            {
                var service = new UserRepositoryService(connString);
                await service.CreateUserRegistration(model.UserId, model.InstituteId, "User1", model.StartDate);
                return new ObjectResult("Success");
            }
            catch (Exception ex)
            {
                return new ObjectResult("Faliure: " + ex.Message);
            }

        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UserRegistrationUpdateModel model)
        {
            try
            {
                var service = new UserRepositoryService(connString);
                await service.TerminateUserRegistration(id,"user1",model.EndDate);
                return new ObjectResult("Success");
            }
            catch (Exception ex)
            {
                return new ObjectResult("Faliure: " + ex.Message);
            }
        }

        // GET api/Entity
        [HttpGet]
        public async Task<IActionResult> Get(int? instituteId)
        {
            var result = new DataTable();
            var service = new UserRepositoryService(connString);
            if (instituteId.HasValue)
                result = await service.GetAllUserRegistrationByInstituteId(instituteId.Value);
            else
                result = await service.GetAllUserRegistration();
            return new ObjectResult(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var service = new UserRepositoryService(connString);
            var result = await service.GetUserRegistrationByUserId(id);
            return new ObjectResult(result);
        }
    }
}