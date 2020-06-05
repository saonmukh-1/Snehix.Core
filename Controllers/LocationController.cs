using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Snehix.Core.API.Services;

namespace Snehix.Core.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Location")]
    public class LocationController : Controller
    {
        public string connString { get; set; }
        public LocationController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("Default");
        }
        // GET api/Entity
        [HttpGet]
        [Route("GetCountry")]
        public async Task<IActionResult> GetCountry()
        {
            var service = new EntityRepositoryService(connString);
            var result = await service.GetAllCountries();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetStates/{id}")]
        public async Task<IActionResult> GetStates(int id)
        {
            var service = new EntityRepositoryService(connString);
            var result = await service.GetAllStatesByCountry(id);
            return Ok(result);
        }
    }
}