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

        [HttpPost]
        public async Task<object> SendPasswordResetLink(string username)
        {
            var appUser = _userManager.Users.SingleOrDefault(r => r.UserName == username);
            var token = _userManager.GeneratePasswordResetTokenAsync(appUser).Result;
            var resetLink = $"http://localhost:50232/Account/ResetPassword?token={token}";
            
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress("no-reply",
            "no-reply@Snehix.Com");
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress(appUser.UserName,
            appUser.Email);
            message.To.Add(to);

            message.Subject = "Password-Reset";

            BodyBuilder bodyBuilder = new BodyBuilder();
            StringBuilder sr = new StringBuilder();
            sr.Append($"<h1>Hi {appUser.UserName}, </h1>");
            sr.Append("<br/>");
            sr.Append($"<h1>Click {resetLink} </h1>");
            bodyBuilder.HtmlBody = sr.ToString();
            
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

        public class LoginDto
        {
            [Required]
            public string Username { get; set; }

            [Required]
            public string Password { get; set; }

            [Required]
            public string DeviceIP { get; set; }

        }
        
        public class RegisterDto
        {
            [Required]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
            public string Password { get; set; }
        }
    }
}