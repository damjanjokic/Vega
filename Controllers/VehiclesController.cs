using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Core;
using Newtonsoft.Json;

namespace vega.Controllers
{
    [Route("/api/vehicles")]
    public class VehiclesController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IVehicleRepository _repo;
        private readonly IUnitOfWork _uow;

        public VehiclesController(IMapper mapper, IVehicleRepository repo, IUnitOfWork uow)
        {
            _uow = uow;
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<QueryResultResource<VehicleResource>> GetVehicles(VehicleQueryResource filterResource)
        {
            var filter = _mapper.Map<VehicleQueryResource, VehicleQuery>(filterResource);
            var queryResult = await _repo.GetVehicles(filter); 
            return _mapper.Map<QueryResult<Vehicle>,QueryResultResource<VehicleResource>>(queryResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id){
            var vehicle = await _repo.GetVehicle(id);
            
            if(vehicle == null)
                return NotFound();
            
            var vehicleResource = _mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(vehicleResource);
        }

        [HttpPost]
        public async  Task<IActionResult> CreateVehicle([FromBody] SaveVehicleResource vehicleResource){
           // throw new Exception();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehicle = _mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);
            vehicle.LastUpdate = DateTime.Now;

            //Saving Domain Model (Vehicle)
            _repo.Add(vehicle);
            await _uow.CompleteAsync();

            vehicle = await _repo.GetVehicle(vehicle.Id);


            var result = _mapper.Map<Vehicle, VehicleResource>(vehicle);

            //Returning Data Transfer Object (VehicleResource)
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] SaveVehicleResource vehicleResource){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehicleDb = await _repo.GetVehicle(id);
            
            if(vehicleDb == null)
                return NotFound();

            _mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicleDb);
            vehicleDb.LastUpdate = DateTime.Now;

            await _uow.CompleteAsync();

            vehicleDb = await _repo.GetVehicle(vehicleDb.Id);

            var result = _mapper.Map<Vehicle, VehicleResource>(vehicleDb);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id){
            var vehicle = await _repo.GetVehicle(id, includeRelated : false);

            if(vehicle == null)
                return NotFound();

            _repo.Remove(vehicle);  
            await _uow.CompleteAsync();

            return Ok(id);
        }
        
    }
}