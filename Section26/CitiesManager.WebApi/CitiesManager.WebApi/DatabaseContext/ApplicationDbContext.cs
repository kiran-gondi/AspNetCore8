using CitiesManager.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebApi.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {
            
        }

        public ApplicationDbContext() { }

        public virtual DbSet<City> Cities { get;set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<City>().HasData(new City()
            {
                CityID = Guid.Parse("E6B9AFB0-1249-45C2-8598-074D3A04FC34"),
                CityName = "Delhi"
            });
            modelBuilder.Entity<City>().HasData(new City()
            {
                CityID = Guid.Parse("60D2F70D-27BD-4B73-9D9C-42C6A5686F20"),
                CityName = "New York"
            });
        }
    }
}
