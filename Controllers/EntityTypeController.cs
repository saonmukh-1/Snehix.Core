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
        string connString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public EntityTypeController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("Default");
        }

        /// <summary>
        /// Get all entity type
        /// </summary>
        /// <returns></returns>
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

       /// <summary>
       /// Get entity type by ID
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var service = new EntityRepositoryService(connString);
            var result = await service.GetEntityTypeByID(id);           
            var response = new GenericResponse<EntityTypeResponse>()
            {
                IsSuccess = true,
                Message = "Data Fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        /// <summary>
        /// Create entity type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,EntityTypeModel model)
        {

            var service = new EntityRepositoryService(connString);
            await service.UpdateEntityType(id,model.Name, model.Description);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "EntityType updated successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }

        [Authorize]
        [HttpGet("search/{name}")]
        public async Task<IActionResult> Search(string name)
        {
            var service = new EntityRepositoryService(connString);
            var result = await service.SearchEntityType(name);
            var response = new GenericResponse<List<EntityTypeResponse>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        [Authorize]
        [HttpGet("exist/{name}")]
        public async Task<IActionResult> Exist(string name)
        {
            var service = new EntityRepositoryService(connString);
            var res = await service.GetEntityTypeByName(name);
            bool result = false;
            if (res != null) result = true;
            var response = new GenericResponse<bool>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var service = new EntityRepositoryService(connString);
            await service.DeleteEntityType(id);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "EntityType deleted successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }
    }
}