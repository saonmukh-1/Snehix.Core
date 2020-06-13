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

namespace Snehix.Core.API.Controllers
{
    [CustomException]
    [ModelValidationAction]
    [Route("api/[controller]")]    
    public class EntityTypeController : ControllerBase
    {
        public string connString { get; set; }
        public EntityTypeController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("Default");
        }

        // GET api/Entity
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var service = new EntityRepositoryService(connString);
            var result = await service.GetAllEntityType();
            var response = new GenericResponse<List<EntityTypeResponse>>()
            {
                IsSuccess = true,
                Message = "Data Fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(EntityTypeModel model)
        {
           
            var service = new EntityRepositoryService(connString);
            await service.CreateEntityType(model.Name, model.Description);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "EntityType Created successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }
    }
}