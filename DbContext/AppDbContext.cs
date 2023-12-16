using CarWashAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarWashAPI.Authentication
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Washer> Washers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Bill> Bills { get; set; }
    }
}
