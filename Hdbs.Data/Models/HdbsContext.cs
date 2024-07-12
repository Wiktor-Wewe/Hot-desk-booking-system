using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Hdbs.Data.Models
{

    public class HdbsContext : IdentityDbContext<Employee>
    {
        public HdbsContext(DbContextOptions<HdbsContext> options)
        : base(options) { }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Desk> Desks { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>().HasMany(l => l.Desks).WithOne(d => d.Location).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Desk>().HasMany(d => d.Reservations).WithOne(r => r.Desk).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Employee>().HasMany(e => e.Reservations).WithOne(r => r.Employee).OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
        public async Task SaveOrHandleExceptionAsync()
        {
            try
            {
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                Log.Logger.Error(ex.StackTrace);
                throw;
            }
        }
    }
}
