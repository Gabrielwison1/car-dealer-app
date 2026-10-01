using CarDealerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealerApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial cars
            modelBuilder.Entity<Car>().HasData(
                new Car
                {
                    Id = 1,
                    Make = "Toyota",
                    Model = "Camry",
                    Year = 2022,
                    Price = 28000,
                    ImageUrl = "https://images.unsplash.com/photo-1621007947382-bb3c3994e3fb?w=600",
                    Description = "Reliable, fuel-efficient sedan in excellent condition."
                },
                new Car
                {
                    Id = 2,
                    Make = "Tesla",
                    Model = "Model 3",
                    Year = 2023,
                    Price = 42000,
                    ImageUrl = "https://images.unsplash.com/photo-1560958089-b8a1929cea89?w=600",
                    Description = "All-electric performance sedan with autopilot capability."
                }
            );
        }
    }
}