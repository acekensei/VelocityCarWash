using CarWashAPI.Authentication;
using CarWashAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarWashAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PaymentMethodController(AppDbContext context) => _context = context;

        [HttpGet]
        public IActionResult GetAllMethods()
        {
            var item = _context.PaymentMethods.ToList();
            if (item == null)
            {
                return BadRequest();
            }
            return Ok(item);
        }

        [HttpGet("{Id}")]
        public IActionResult GetMethodById(int Id)
        {
            var item = _context.PaymentMethods.FirstOrDefault(x => x.Id == Id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateMethod([FromBody] PaymentMethod values)
        {
            _context.PaymentMethods.Add(values);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetMethodById), new { Id = values.Id }, values);
        }

        [HttpPut("{Id}")]
        public IActionResult UpdateMethod(int Id, [FromBody] PaymentMethod values)
        {
            if (Id != values.Id)
            {
                return NotFound();
            }

            _context.PaymentMethods.Update(values);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetMethodById), new { Id = values.Id }, values);
        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteMethod(int Id)
        {
            var item = _context.PaymentMethods.FirstOrDefault(x => x.Id == Id);
            if (item == null)
            {
                return BadRequest();
            }

            _context.PaymentMethods.Remove(item);
            _context.SaveChanges();

            return Ok(item);
        }
    }
}
