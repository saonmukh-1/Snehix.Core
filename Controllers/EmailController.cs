using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using Snehix.Core.API.DTO;
using Snehix.Core.API.Filters;
using Snehix.Core.API.Models;

namespace Snehix.Core.API.Controllers
{
    [Route("[controller]/[action]")]
    [CustomException]
    [ModelValidationAction]
    public class EmailController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        public string SendgridKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public EmailController(IConfiguration configuration)
        {
            SendgridKey = configuration.GetValue<string>("SendgridKey");            
        }
        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendEmail([FromBody] EmailModel model)
        {            
            var client = new SendGridClient(SendgridKey);
            var from = new EmailAddress(model.FromEmail, model.FromUser);
            var subject = model.Subject;
            var to = new EmailAddress(model.ToEmail, model.ToUser);
            var plainTextContent = model.Body;
            var htmlContent = model.Body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var resp = await client.SendEmailAsync(msg);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Email sent successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }
        
    }
}