using CarWashAPI.Authentication;
using CarWashAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarWashAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WasherController : ControllerBase
    {
        private readonly AppDbContext _context;
        public WasherController(AppDbContext context) => _context = context;

        [HttpGet]
        public IActionResult GetAllWasher()
        {
            var item = _context.Washers.ToList();
            if (item == null)
            {
                return BadRequest();
            }
            return Ok(item);
        }

        [HttpGet("{Id}")]
        public IActionResult GetWasherById(int Id)
        {
            var item = _context.Washers.FirstOrDefault(x => x.Id == Id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateWasher([FromBody] Washer values)
        {
            _context.Washers.Add(values);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetWasherById), new { Id = values.Id }, values);
        }

        [HttpPut("{Id}")]
        public IActionResult UpdateWasher(int Id, [FromBody] Washer values)
        {
            if (Id != values.Id)
            {
                return NotFound();
            }

            _context.Washers.Update(values);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetWasherById), new { Id = values.Id }, values);
        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteWasher(int Id)
        {
            var item = _context.Washers.FirstOrDefault(x => x.Id == Id);
            if (item == null)
            {
                return BadRequest();
            }

            _context.Washers.Remove(item);
            _context.SaveChanges();

            return Ok(item);
        }
    }
}
