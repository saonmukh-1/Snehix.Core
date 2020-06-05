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
    public class UserController : ControllerBase
    {
        public string connString { get; set; }
        public UserController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("Default");
        }

        // Post api/User
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserModel model)
        {
            try
            {
                var service = new UserRepositoryService(connString);
                await service.CreateUser(model);
                return new ObjectResult("Success");
            }
            catch (Exception ex)
            {
                return new ObjectResult("Faliure: " + ex.Message);
            }           
           
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UserModel model)
        {
            try
            {
                var service = new UserRepositoryService(connString);
                await service.UpdateUser(model,id);
                return new ObjectResult("Success");
            }
            catch (Exception ex)
            {
                return new ObjectResult("Faliure: " + ex.Message);
            }
        }

        // GET api/Entity
        [HttpGet]
        public async Task<IActionResult> Get(string username)
        {
            var result = new DataTable();
            var service = new UserRepositoryService(connString);
            if (string.IsNullOrEmpty(username))
                result = await service.GetAllUser();
            else
                result = await service.GetUseryByUserName(username);
            return new ObjectResult(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var service = new UserRepositoryService(connString);
            var result = await service.GetUseryById(id);
            return new ObjectResult(result);
        }

       
    }
}