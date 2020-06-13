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
    public class InstituteController : ControllerBase
    {

        public string connString { get; set; }
        public InstituteController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("Default");
        }

        // GET api/Entity
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var service = new InstituteRepositoryService(connString);
            var result = await service.GetAllInstitutes();
            var response = new GenericResponse<List<InstituteDTO>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);

        }

        // GET api/values/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var service = new InstituteRepositoryService(connString);
            var result = await service.GetInstituteById(id);
            var response = new GenericResponse<List<InstituteDTO>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(InstitutionModel model)
        {

            var service = new InstituteRepositoryService(connString);
            model.Actor = "User1";
            await service.CreateInstitute(model);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Institute created successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);

        }

        // PUT api/values/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, InstitutionModelUpdate model)
        {

            var service = new InstituteRepositoryService(connString);
            await service.UpdateInstitute(model, id);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Institute updated successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);

        }
    }
}