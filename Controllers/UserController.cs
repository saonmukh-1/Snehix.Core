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
using Snehix.Core.API.Utility;

namespace Snehix.Core.API.Controllers
{
    [CustomException]
    [ModelValidationAction]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        string connString { get; set; }
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="configuration"></param>
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
       
        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get user by user name
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get user by user id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
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

        /// <summary>
        /// Update user login with device IP and set flag to false for IsNewAccount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("UpdateUserLogin")]
        public async Task<IActionResult> UpdateUserLogin(UpdateUserLoginModel model)
        {
            var username = ApplicationUtility.GetTokenAttribute(Request.Headers["Authorization"], "sub");
            //string value = Request.Headers["Authorization"];
            //value = value.Remove(0, 7);
            //var jwtHandler = new JwtSecurityTokenHandler();
            //var token = jwtHandler.ReadJwtToken(value);
            //var usernameClaim = token.Claims.Where(a=>a.Type=="sub").FirstOrDefault();
            var service = new UserRepositoryService(connString);
            await service.UpdateUserLogin(username, model.IsNewAccount,model.IPAddress);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "User updated successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }

        [Authorize]
        [HttpPost("usersearch")]
        public async Task<IActionResult> UserSearch(UserSearch model)
        {
            var service = new UserRepositoryService(connString);
            
            var result = await service.GetUserByInstitute(model.InstituteId,model.ClassId,model.SectionId);
            var response = new GenericResponse<List<UserDetails>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }
        [Authorize]
        [HttpPost("usersearchbyname")]
        public async Task<IActionResult> UserSearch(UserSearchByName model)
        {
            var service = new UserRepositoryService(connString);

            var result = await service.GetUserSearchByName(model.InstituteId, model.Name);
            var response = new GenericResponse<List<UserDetails>>()
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