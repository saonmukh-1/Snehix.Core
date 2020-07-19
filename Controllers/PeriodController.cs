using System;
using System.Collections.Generic;
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
using Snehix.Core.API.Utility;

namespace Snehix.Core.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Period")]
    [CustomException]
    [ModelValidationAction]
    public class PeriodController : Controller
    {
        string connString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public PeriodController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("Default");
        }

        /// <summary>
        /// Create student classification
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(PeriodModel model)
        {
            var username = ApplicationUtility.GetTokenAttribute(Request.Headers["Authorization"], "sub");
            var service = new PeriodRepository(connString);
            await service.CreatePeriod(model);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Period created successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }
       

        /// <summary>
        /// routine for teacher
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("teacher")]
        public async Task<IActionResult> GetByTeacher(SearchPeriodByTeacherModel model)
        {
            var service = new PeriodRepository(connString);
            var result = await service.GetAllPeriodByTeacher(model.TeacherId,model.StartDate,model.EndDate);
            var response = new GenericResponse<List<Period>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }
        /// <summary>
        /// next period for teacher
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("teachernextperiod/{id}")]
        public async Task<IActionResult> GetByTeacherNext( int id)
        {
            var service = new PeriodRepository(connString);
            var result = await service.GetAllPeriodByTeacher(id, DateTime.Today, DateTime.Today.AddDays(1));
            var res = result.Where(a => a.StartDateTime > DateTime.Now).OrderBy(a => a.StartDateTime).FirstOrDefault();
            var response = new GenericResponse<Period>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = res
            };
            return Ok(response);
        }

        /// <summary>
        /// Routine for student
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("student")]
        public async Task<IActionResult> GetByStudent(SearchPeriodByStudentModel model)
        {
            var service = new PeriodRepository(connString);
            var result = await service.GetAllPeriodByStudent(model.StudentId, model.StartDate, model.EndDate);
            var response = new GenericResponse<List<Period>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }
        /// <summary>
        /// Next period for a student
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("studentnextperiod/{id}")]
        public async Task<IActionResult> GetByStudentNext(int id)
        {
            var service = new PeriodRepository(connString);
            var result = await service.GetAllPeriodByStudent(id, DateTime.Today, DateTime.Today.AddDays(1));
            var res = result.Where(a => a.StartDateTime > DateTime.Now).OrderBy(a => a.StartDateTime).FirstOrDefault();
            var response = new GenericResponse<Period>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = res
            };
            return Ok(response);
        }

        /// <summary>
        /// Delete student classification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var service = new PeriodRepository(connString);
            await service.DeletePeriod(id);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Period deleted successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }
    }
}