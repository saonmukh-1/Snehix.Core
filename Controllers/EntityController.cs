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
   
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [CustomException]
    [ModelValidationAction]    
    public class EntityController : Controller
    {

        string connString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public EntityController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("Default");
        }

        /// <summary>
        /// This method will fetch all the Entities exist in the database
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllEntity()
        {
            var service = new EntityRepositoryService(connString);
            var result = await service.GetAllEntity();
            var ent = result.Select(a => a.EntityTypeId).Distinct();
            var finalresult = new List<EntityTypeDTO>();
            foreach(var item in ent)
            {
                var list = result.Where(a => a.EntityTypeId == item).ToList();
                var entTypeDTO = new EntityTypeDTO();
                entTypeDTO.EntityTypeId = list[0].EntityTypeId;
                entTypeDTO.EntityTypeName = list[0].EntityTypeName;
                entTypeDTO.Entities = new List<EntityDTO>();
                foreach (var elm in list)
                {
                    var entDTO = new EntityDTO();
                    entDTO.EntityId = elm.EntityId;
                    entDTO.EntityName = elm.EntityName;
                    entDTO.EntityDescription = elm.EntityDescription;
                    entTypeDTO.Entities.Add(entDTO);
                }
                finalresult.Add(entTypeDTO);
            }
            var response = new GenericResponse<List<EntityTypeDTO>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = finalresult
            };
            return Ok(response);
        }

        /// <summary>
        /// Get entity by entity type
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("Search")]
        public async Task<IActionResult> GetAllEntity(EntitySearch entityType)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            var service = new EntityRepositoryService(connString);
            var result = await service.GetAllEntity();
            var filteredRes = result.Where(e => entityType.EntityTypeId.Any(mg => mg == e.EntityTypeId));
            
            var ent = filteredRes.Select(a => a.EntityTypeId).Distinct();
            var finalresult = new List<EntityTypeDTO>();
            foreach (var item in ent)
            {
                var list = result.Where(a => a.EntityTypeId == item).ToList();
                var entTypeDTO = new EntityTypeDTO();
                entTypeDTO.EntityTypeId = list[0].EntityTypeId;
                entTypeDTO.EntityTypeName = list[0].EntityTypeName;
                entTypeDTO.Entities = new List<EntityDTO>();
                foreach (var elm in list)
                {
                    var entDTO = new EntityDTO();
                    entDTO.EntityId = elm.EntityId;
                    entDTO.EntityName = elm.EntityName;
                    entDTO.EntityDescription = elm.EntityDescription;
                    entTypeDTO.Entities.Add(entDTO);
                }
                finalresult.Add(entTypeDTO);
            }
            var response = new GenericResponse<List<EntityTypeDTO>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = finalresult
            };
            return Ok(response);
        }

        /// <summary>
        /// Get entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var service = new EntityRepositoryService(connString);
            var result = await service.GetAllEntityById(id);
            var response = new GenericResponse<List<EntityDTO>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        [Authorize]
        [HttpGet("search/{name}")]
        public async Task<IActionResult> Search(string name)
        {
            var service = new EntityRepositoryService(connString);
            var result = await service.SearchEntity(name);
            var response = new GenericResponse<List<EntityDTO>>()
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
            var res = await service.GetEntityByName(name);
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

        /// <summary>
        /// Create entity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(EntityModel model)
        {
            var service = new EntityRepositoryService(connString);
            await service.CreateEntity(model.Name, model.Description, model.EntityTypeId);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Entity Created successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EntityModel model)
        {           
            var service = new EntityRepositoryService(connString);
            await service.UpdateEntity(id,model.Name, model.Description, model.EntityTypeId);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Entity Updated successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }
    }
}