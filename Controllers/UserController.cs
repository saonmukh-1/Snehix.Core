using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Snehix.Core.API.DTO;
using Snehix.Core.API.Models;
using Snehix.Core.API.Services;
using Snehix.Core.API.Filters;
using Microsoft.AspNetCore.Authorization;

namespace Snehix.Core.API.Controllers
{
    [CustomException]
    [ModelValidationAction]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public string connString { get; set; }
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public UserController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            connString = configuration.GetConnectionString("Default");
        }
        //public UserController(IConfiguration configuration)
        //{
        //    connString = configuration.GetConnectionString("Default");
        //}

        // Post api/User
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserModel model)
        {
            var service = new UserRepositoryService(connString);
            var user = new IdentityUser
            {
                UserName = model.Username,
                Email = model.EmailId
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await service.CreateUser(model);
            }
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "User created successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }
        // PUT api/values/5
        [Authorize]
        [HttpPut("{id}")]        
        public async Task<IActionResult> Put(int id, UserUpdateModel model)
        {
            var service = new UserRepositoryService(connString);
            await service.UpdateUser(model, id);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "User updated successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }

        // GET api/Entity
        [Authorize]
        [HttpGet]        
        public async Task<IActionResult> Get(string username)
        {
            var result = new List<UserDTO>();
            var service = new UserRepositoryService(connString);
            if (string.IsNullOrEmpty(username))
                result = await service.GetAllUser();
            else
                result = await service.GetUseryByUserName(username);
            var response = new GenericResponse<List<UserDTO>>()
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
            var result = await service.GetUseryById(id);
            var response = new GenericResponse<List<UserDTO>>()
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