﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Snehix.Core.API.DTO;
using Snehix.Core.API.Filters;
using Snehix.Core.API.Models;
using Snehix.Core.API.Services;

namespace Snehix.Core.API.Controllers
{
   
    [Route("api/[controller]")]
    [CustomException]
    [ModelValidationAction]
    public class EntityController : Controller
    {

        public string connString { get; set; }
        public EntityController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("Default");
        }

        // GET api/Entity
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

        // GET api/values/5
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

        // PUT api/values/5
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