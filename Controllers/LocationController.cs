using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Snehix.Core.API.DTO;
using Snehix.Core.API.Filters;
using Snehix.Core.API.Services;

namespace Snehix.Core.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Location")]
    [CustomException]
    [ModelValidationAction]
    public class LocationController : Controller
    {
        string connString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public LocationController(IConfiguration configuration)
        {
            connString =  configuration.GetConnectionString("Default");
        }
        /// <summary>
        /// Get country list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCountry")]
        public async Task<IActionResult> GetCountry()
        {
            var service = new EntityRepositoryService(connString);
            var result = await service.GetAllCountries();
            var response = new GenericResponse<List<Country>>()
            {
                IsSuccess = true,
                Message = "Data Fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        /// <summary>
        /// Get states by country id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetStates/{id}")]
        public async Task<IActionResult> GetStates(int id)
        {
            var service = new EntityRepositoryService(connString);
            var result = await service.GetAllStatesByCountry(id);
            var response = new GenericResponse<List<State>>()
            {
                IsSuccess = true,
                Message = "Data Fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }
    }
}