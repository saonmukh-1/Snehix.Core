using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Snehix.Core.API.DTO;
using Snehix.Core.API.Filters;
using Snehix.Core.API.Services;
using MailKit.Net.Smtp;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using Snehix.Core.API.Models;

namespace Snehix.Core.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("[controller]/[action]")]
    [CustomException]
    [ModelValidationAction]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        string ConnString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="configuration"></param>
        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration =     configuration;
            ConnString = configuration.GetConnectionString("Default");
        }
        /// <summary>
        /// Login to the system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> Login([FromBody] LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.UserName == model.Username);
                var claims = await _userManager.GetClaimsAsync(appUser);
                var service = new UserRepositoryService(ConnString);
                var  userEntry = await service.GetUseryByUserName(model.Username);

                var response = new GenericResponse<LoginResponse>()
                {
                    IsSuccess = true,
                    Message = "Signed in successfully.",
                    ResponseCode = 200                    
                };
                var res = new LoginResponse()
                {
                    Jwt = await GenerateJwtToken(model.Username, appUser, claims)
                };
                res.PopulateCode(userEntry[0].IPAddress, userEntry[0].IsNewAccount, model.DeviceIP);
                response.Result = res;
                return Ok(response);
            }            
            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }
       
        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> Register([FromBody] RegisterDto model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email, 
                Email = model.Email
            };
           // _userManager.AddClaimAsync(user,new Claim("rId",""))
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                var response = new GenericResponse<string>()
                {
                    IsSuccess = true,
                    Message = "User created successfully.",
                    ResponseCode = 200,
                    Result = "User created successfully."
                };
                return Ok(response);
            }
            
            throw new ApplicationException("UNKNOWN_ERROR");
        }
        
        
        
        private async Task<string> GenerateJwtToken(string email, IdentityUser user,IList<Claim> userClaims)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            claims.AddRange(userClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Send password reset link
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("requestresetpassword/{username}")]
        public async Task<object> SendPasswordResetLink(string username)
        {
            var appUser = _userManager.Users.SingleOrDefault(r => r.UserName == username);
            var token = _userManager.GeneratePasswordResetTokenAsync(appUser).Result;
            var resetLink = $"{_configuration.GetValue<string>("JwtExpireDays")}/Account/ResetPassword?token={token}";
            var SendgridKey = _configuration.GetValue<string>("SendgridKey");
            StringBuilder sr = new StringBuilder();
            sr.Append($"<h1>Hi {appUser.UserName}, </h1>");
            sr.Append("<br/>");
            sr.Append($"<h1>Click {resetLink} </h1>");

            var client = new SendGridClient(SendgridKey);
            var from = new EmailAddress("no-reply@snehix.com", "Snehix-Admin");
            var subject = "Password reset request";
            var to = new EmailAddress(appUser.Email, appUser.UserName);
            var plainTextContent = sr.ToString();
            var htmlContent = sr.ToString();
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var resp = await client.SendEmailAsync(msg);

            if (!string.IsNullOrEmpty(token))
            {
                var response = new GenericResponse<string>()
                {
                    IsSuccess = true,
                    Message = "Signed in successfully.",
                    ResponseCode = 200,
                    Result= token
                };                
                return Ok(response);
            }
            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }

        [HttpPost("resetpassword")]
        public async Task<object> ResetPassword(PasswordResetModel model)
        {
            var appUser = _userManager.Users.SingleOrDefault(r => r.UserName == model.Username);
            var token = model.token;
            var res = await _userManager.ResetPasswordAsync(appUser, token, model.NewPassword);          

            if (res.Succeeded)
            {
                var response = new GenericResponse<string>()
                {
                    IsSuccess = true,
                    Message = "Password reset successfully.",
                    ResponseCode = 200,
                    Result = token
                };
                return Ok(response);
            }
            throw new ApplicationException("Sorry. Please try again..");
        }

       
    }
}