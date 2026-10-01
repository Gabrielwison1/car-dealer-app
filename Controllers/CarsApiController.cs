using CarDealerApp.Data;
using CarDealerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CarsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CarsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            return await _context.Cars.ToListAsync();
        }

        // GET: api/CarsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();

            return car;
        }

        // POST: api/CarsApi/inquire
        [HttpPost("inquire")]
        public async Task<IActionResult> PostInquiry([FromBody] Inquiry inquiry)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            inquiry.SubmittedAt = DateTime.UtcNow;
            _context.Inquiries.Add(inquiry);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Inquiry submitted successfully.", inquiryId = inquiry.Id });
        }
    }
}