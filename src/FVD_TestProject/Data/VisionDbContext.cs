using FVD.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FVD.Data
{
    public class VisionDbContext : DbContext
    {
        public VisionDbContext(DbContextOptions<VisionDbContext> options)
           : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
    }
}

// dotnet ef  migrations add Initial --context FVD.Data.VisionDbContext -o Data/Migrations/Vision
// dotnet ef database update --context FVD.Data.VisionDbContext