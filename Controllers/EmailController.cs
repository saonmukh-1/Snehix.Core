using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using Snehix.Core.API.DTO;
using Snehix.Core.API.Filters;
using Snehix.Core.API.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

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

        [HttpPost("SendSms")]        
        public async Task<IActionResult> SendSMS([FromBody] EmailModel model)
        {

            String message = HttpUtility.UrlEncode("Hi, Saon here this is a sms from our twilio sms service, how is it?");
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                {"apikey" , "5/esm/vq14I-BAkdonTsO7mBUIYpWxzQadKqYMKAM0"},
                {"numbers" , "918600411621"},
                {"message" , message},
                {"sender" , "TXTLCL"}
                });
                string result = System.Text.Encoding.UTF8.GetString(response);
                //return result;
            }

            //const string accountSid = "AC2dc6d27432a8d71ec567c0ddf99926fd";
            //const string authToken = "21216052d8108c6b88db99be246a7fee";

            //TwilioClient.Init(accountSid, authToken);

            //var message = MessageResource.Create(
            //    body: "Hi, Saon here this is a sms from our twilio sms service, how is it?",
            //    from: new Twilio.Types.PhoneNumber("+12566662902"),
            //    to: new Twilio.Types.PhoneNumber("+919886677508")
            //);
            var response1 = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "SMS sent successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response1);
        }



    }
}