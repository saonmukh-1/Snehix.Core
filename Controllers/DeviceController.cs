using System;
using System.Collections.Generic;
using System.Data;
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
    public class DeviceController : Controller
    {
        string connString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public DeviceController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("Default");
        }

        /// <summary>
        /// Create device into the system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(DeviceModel model)
        {
            var service = new DeviceRepositoryService(connString);
            await service.CreateDevice(model.ModelName, model.Version, model.SerialNumber,
                model.Description, "User1", model.UserId, model.Stratdate, model.InstituteId);
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Device created successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }
        /// <summary>
        /// Update device
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="model">model</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, DeviceUpdateModel model)
        {
            var service = new DeviceRepositoryService(connString);
            await service.UpdateDevice(id, model.Model, model.Version, model.SerialNumber, model.Description,
                "user1");
            var response = new GenericResponse<string>()
            {
                IsSuccess = true,
                Message = "Device updated successfully.",
                ResponseCode = 200,
                Result = "Success"
            };
            return Ok(response);
        }
        
        /// <summary>
        /// Update device user association
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("Association/{id}")]
        public async Task<IActionResult> Put(int id, DeviceUserAssociationUpdateModel model)
        {
            
                var service = new DeviceRepositoryService(connString);
                await service.UpdateDeviceUserAssociation(id, model.UserId, "user1", model.Stratdate);
                var response = new GenericResponse<string>()
                {
                    IsSuccess = true,
                    Message = "DeviceAssociation updated successfully.",
                    ResponseCode = 200,
                    Result = "Success"
                };
                return Ok(response);
            }

        /// <summary>
        /// Get Device by Institute
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
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

        /// <summary>
        /// Get only assigned device by institute Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
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

        /// <summary>
        /// Get only unassigned device by Institution Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
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

        /// <summary>
        /// Get device by device id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
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

        /// <summary>
        /// Get all devices detail list
        /// </summary>
        /// <param name="id">institute id</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("getalldevice/{id}")]
        public async Task<IActionResult> GetDeviceByInstitute(int id)
        {
            var service = new DeviceRepositoryService(connString);
            var result = await service.GetAllDetailDeviceByInstitute(id);
            var response = new GenericResponse<List<DeviceExtended>>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slnumber"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("getdevicebyserialnumber/{slnumber}")]
        public async Task<IActionResult> GetDeviceDetailBySerialNumber(string slnumber)
        {
            var service = new DeviceRepositoryService(connString);
            var result = await service.GetAllDetailDeviceBySerialNumber(slnumber);
            var response = new GenericResponse<DeviceDetails>()
            {
                IsSuccess = true,
                Message = "Data fetched successfully.",
                ResponseCode = 200,
                Result = result
            };
            return Ok(response);
        }

        [Authorize]
        [HttpGet("exist/{slnumber}")]
        public async Task<IActionResult> DeviceExist(string slnumber)
        {
            var service = new DeviceRepositoryService(connString);
            var res = await service.GetAllDetailDeviceBySerialNumber(slnumber);
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
    }
}