using CarWashAPI.Authentication;
using CarWashAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarWashAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BillController(AppDbContext context) => _context = context;

        [HttpGet]
        public IActionResult GetAllBill()
        {
            var item = _context.Bills
                .Include(b => b.Washer)
                .Include(b => b.Vehicle)
                .Include(b => b.PaymentMethod)
                .ToList();

            if (item == null)
            {
                return BadRequest();
            }
            return Ok(item);
        }

        [HttpGet("{Id}")]
        public IActionResult GetBillById(int Id)
        {
            //var item = _context.Bills.FirstOrDefault(x => x.Id == Id);


            // This had to be done so that I could use the data from
            // other models (dbsets) in my details and delete views
            // basically anywhere that uses the getbyid action

            var item = _context.Bills
                .Include(b => b.Washer)
                .Include(b => b.Vehicle)
                .Include(b => b.PaymentMethod)
                .FirstOrDefault(x => x.Id == Id);

            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateBill([FromBody] Bill values)
        {
            _context.Bills.Add(values);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetBillById), new { Id = values.Id }, values);
        }

        [HttpPut("{Id}")]
        public IActionResult UpdateBills(int Id, [FromBody] Bill values)
        {
            if (Id != values.Id)
            {
                return NotFound();
            }

            _context.Bills.Update(values);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetBillById), new { Id = values.Id }, values);
        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteWasher(int Id)
        {
            var item = _context.Bills.FirstOrDefault(x => x.Id == Id);
            if (item == null)
            {
                return BadRequest();
            }

            _context.Bills.Remove(item);
            _context.SaveChanges();

            return Ok(item);
        }
    }
}
