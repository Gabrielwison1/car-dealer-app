using CarDealerApp.Data;
using CarDealerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealerApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Admin dashboard listing cars
        public async Task<IActionResult> Index()
        {
            var cars = await _context.Cars.ToListAsync();
            return View(cars);
        }

        // Add new car (GET)
        public IActionResult Create() => View();

        // Add new car (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // View customer inquiries
        public async Task<IActionResult> Inquiries()
        {
            var inquiries = await _context.Inquiries.Include(i => i.Car).OrderByDescending(i => i.SubmittedAt).ToListAsync();
            return View(inquiries);
        }
    }
}