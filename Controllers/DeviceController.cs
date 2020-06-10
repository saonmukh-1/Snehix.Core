using System;
using System.Collections.Generic;
using System.Data;
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
    public class DeviceController : Controller
    {
        public string connString { get; set; }
        public DeviceController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("Default");
        }

        // Post api/User
        [HttpPost]
        public async Task<IActionResult> Create(DeviceModel model)
        {
            
                var service = new DeviceRepositoryService(connString);
                await service.CreateDevice(model.ModelName, model.Version, model.SerialNumber,
                    model.Description, "User1", model.UserId, model.Stratdate,model.InstituteId);
                var response = new GenericResponse<string>()
                {
                    IsSuccess = true,
                    Message = "Device created successfully.",
                    ResponseCode = 200,
                    Result = "Success"
                };
                return Ok(response);
            

        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, DeviceUpdateModel model)
        {
            try
            {
                var service = new DeviceRepositoryService(connString);
                await service.UpdateDevice(id,model.Model,model.Version,model.SerialNumber,model.Description,
                    "user1");
                return new ObjectResult("Success");
            }
            catch (Exception ex)
            {
                return new ObjectResult("Faliure: " + ex.Message);
            }
        }
        // PUT api/values/5
        [HttpPut("Association/{id}")]
        public async Task<IActionResult> Put(int id, DeviceUserAssociationUpdateModel model)
        {
            try
            {
                var service = new DeviceRepositoryService(connString);
                await service.UpdateDeviceUserAssociation(id, model.UserId, "user1",model.Stratdate);
                return new ObjectResult("Success");
            }
            catch (Exception ex)
            {
                return new ObjectResult("Faliure: " + ex.Message);
            }
        }

        // GET api/Entity
        [HttpGet("Institute/{id}")]
        public async Task<IActionResult> GetByInstitute(int id)
        {
            var service = new DeviceRepositoryService(connString);
            var result = await service.GetAllDeviceByInstitute(id);
            var response = new GenericResponse<List<DeviceExtended>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }
        [HttpGet("Assigned/{id}")]
        public async Task<IActionResult> GetAssignedByInstitute(int id)
        {
            var service = new DeviceRepositoryService(connString);
            var result = await service.GetAllAssignedDevice(id);
            var response = new GenericResponse<List<DeviceExtended>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        [HttpGet("UnAssigned/{id}")]
        public async Task<IActionResult> GetUnAssignedByInstitute(int id)
        {
            var service = new DeviceRepositoryService(connString);
            var result = await service.GetAllUnAssignedDevice(id);
            var response = new GenericResponse<List<DeviceExtended>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var service = new DeviceRepositoryService(connString);
            var result = await service.GetAllDeviceById(id);
            var response = new GenericResponse<List<DeviceExtended>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }
    }
}