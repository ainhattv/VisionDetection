using Microsoft.EntityFrameworkCore;
using VDS.WPS.Data.Entities;

namespace VDS.WPS.Data
{
    public class WPSContext : DbContext
    {
        public WPSContext(DbContextOptions<WPSContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        // Entities
        public DbSet<WorkPlace> WorkPlaces { get; set; }
        public DbSet<WorkPlaceSetting> WorkPlaceSettings { get; set; }
    }
}