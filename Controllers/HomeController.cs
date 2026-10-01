using CarDealerApp.Data;
using CarDealerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealerApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Showcase all cars
        public async Task<IActionResult> Index()
        {
            var cars = await _context.Cars.ToListAsync();
            return View(cars);
        }

        // Car details page
        public async Task<IActionResult> Details(int id)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);
            if (car == null) return NotFound();

            return View(car);
        }

        // Submit Inquiry (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitInquiry(Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                inquiry.SubmittedAt = DateTime.UtcNow;
                _context.Inquiries.Add(inquiry);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thank you for your inquiry! We will contact you soon.";
                return RedirectToAction(nameof(Details), new { id = inquiry.CarId });
            }

            var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == inquiry.CarId);
            return View("Details", car);
        }
    }
}