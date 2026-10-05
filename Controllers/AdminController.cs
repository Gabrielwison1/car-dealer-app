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
        private readonly IWebHostEnvironment _env;
        private const string UploadFolder = "uploads/cars";

        public AdminController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // Admin dashboard listing cars
        public async Task<IActionResult> Index()
        {
            var cars = await _context.Cars.ToListAsync();
            return View(cars);
        }

        // Add new car (GET)
        public IActionResult Create() => View();

        // Add new car (POST) — accepts a file upload instead of an image URL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car, IFormFile? imageFile)
        {
            // Remove ImagePath from model validation since it's set by the upload
            ModelState.Remove(nameof(Car.ImagePath));

            if (ModelState.IsValid)
            {
                car.ImagePath = await SaveImageAsync(imageFile);
                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // Edit car (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();
            return View(car);
        }

        // Edit car (POST) — optionally replaces image if a new file is uploaded
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Car car, IFormFile? imageFile)
        {
            if (id != car.Id) return BadRequest();

            ModelState.Remove(nameof(Car.ImagePath));

            if (ModelState.IsValid)
            {
                var existing = await _context.Cars.FindAsync(id);
                if (existing == null) return NotFound();

                existing.Make = car.Make;
                existing.Model = car.Model;
                existing.Year = car.Year;
                existing.Price = car.Price;
                existing.Description = car.Description;

                // Only replace the image if a new file was uploaded
                if (imageFile != null && imageFile.Length > 0)
                {
                    DeleteImageFile(existing.ImagePath);
                    existing.ImagePath = await SaveImageAsync(imageFile);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // Delete car (GET) — confirmation page
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();
            return View(car);
        }

        // Delete car (POST) — removes from DB and deletes image from disk
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                DeleteImageFile(car.ImagePath);
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // View customer inquiries
        public async Task<IActionResult> Inquiries()
        {
            var inquiries = await _context.Inquiries
                .Include(i => i.Car)
                .OrderByDescending(i => i.SubmittedAt)
                .ToListAsync();
            return View(inquiries);
        }

        // --- Helpers ---

        private async Task<string> SaveImageAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            var uploadsDir = Path.Combine(_env.WebRootPath, UploadFolder);
            Directory.CreateDirectory(uploadsDir);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsDir, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{UploadFolder}/{fileName}";
        }

        private void DeleteImageFile(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return;
            var fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
        }
    }
}