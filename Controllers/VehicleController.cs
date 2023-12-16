using CarWashAPI.Authentication;
using CarWashAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarWashAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly AppDbContext _context;
        public VehicleController(AppDbContext context) => _context = context;

        [HttpGet]
        public IActionResult GetAllVehicles()
        {
            var item = _context.Vehicles.ToList();
            if (item == null)
            {
                return BadRequest();
            }
            return Ok(item);
        }

        [HttpGet("{Id}")]
        public IActionResult GetVehicleById(int Id)
        {
            var item = _context.Vehicles.FirstOrDefault(x => x.Id == Id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateVehicle([FromBody] Vehicle values)
        {
            _context.Vehicles.Add(values);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetVehicleById), new { Id = values.Id }, values);
        }

        [HttpPut("{Id}")]
        public IActionResult UpdateVehicle(int Id, [FromBody] Vehicle values)
        {
            if (Id != values.Id)
            {
                return NotFound();
            }

            _context.Vehicles.Update(values);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetVehicleById), new { Id = values.Id }, values);
        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteVehicle(int Id)
        {
            var item = _context.Vehicles.FirstOrDefault(x => x.Id == Id);
            if (item == null)
            {
                return BadRequest();
            }

            _context.Vehicles.Remove(item);
            _context.SaveChanges();

            return Ok(item);
        }
    }
}
