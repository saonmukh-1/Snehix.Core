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
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

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
            var serviceEntity = new EntityRepositoryService(connString);
            var user = new IdentityUser
            {
                UserName = model.Username,
                Email = model.EmailId
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            var userType = await serviceEntity.GetAllEntityById(model.UserTypeId);

            
            var response = new GenericResponse<string>();
            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new Claim("rId", userType[0].EntityName));
                await service.CreateUser(model);

                response = new GenericResponse<string>()
                {
                    IsSuccess = true,
                    Message = "User created successfully.",
                    ResponseCode = 200,
                    Result = "Success"
                };
            }
            else
            {
                response = new GenericResponse<string>()
                {
                    IsSuccess = false,
                    Message = "Failure",
                    ErrorMessage = result.Errors.Select(a=>a.Description).ToList(),
                    ResponseCode = 500,
                    Result = "Failure"
                };              
                
            }
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


        [Authorize]
        [HttpPut("UpdateUserLogin")]
        public async Task<IActionResult> UpdateUserLogin(UpdateUserLoginModel model)
        {
            string value = Request.Headers["Authorization"];
            value = value.Remove(0, 7);
            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.ReadJwtToken(value);
            var usernameClaim = token.Claims.Where(a=>a.Type=="sub").FirstOrDefault();
            var service = new UserRepositoryService(connString);
            await service.UpdateUserLogin(usernameClaim.Value, model.IsNewAccount,model.IPAddress);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "User updated successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }
    }
}