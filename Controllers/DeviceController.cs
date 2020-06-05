using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Snehix.Core.API.Models;
using Snehix.Core.API.Services;

namespace Snehix.Core.API.Controllers
{
    [Route("api/[controller]")]
    
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
            try
            {
                var service = new DeviceRepositoryService(connString);
                await service.CreateDevice(model.model, model.version, model.serialNumber,
                    model.description, "User1", model.UserId, model.stratdate);
                return new ObjectResult("Success");
            }
            catch (Exception ex)
            {
                return new ObjectResult("Faliure: " + ex.Message);
            }

        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, DeviceUpdateModel model)
        {
            try
            {
                var service = new DeviceRepositoryService(connString);
                await service.UpdateDevice(id,model.model,model.version,model.serialNumber,model.description,
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
                await service.UpdateDeviceUserAssociation(id, model.UserId, "user1",model.stratdate);
                return new ObjectResult("Success");
            }
            catch (Exception ex)
            {
                return new ObjectResult("Faliure: " + ex.Message);
            }
        }

        // GET api/Entity
        [HttpGet]
        public async Task<IActionResult> Get(bool? assigned)
        {
            var result = new DataTable();
            var service = new DeviceRepositoryService(connString);
            if(!assigned.HasValue)
                result = await service.GetAllActiveDevice();
            else if(assigned.Value)
                result = await service.GetAllAssignedDevice();
            else
                result = await service.GetAllUnAssignedDevice();
            return new ObjectResult(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var service = new DeviceRepositoryService(connString);
            var result = await service.GetDeviceByDeviceId(id);
            return new ObjectResult(result);
        }
    }
}