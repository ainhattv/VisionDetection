using Microsoft.EntityFrameworkCore;
using VDS.BlobService.Data.Entities;

namespace VDS.BlobService.Data
{
    public class BlobContext : DbContext
    {
        public BlobContext(DbContextOptions<BlobContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<BlobContainer> BlobContainers { get; set; }
        public DbSet<BlobFolder> BlobFolders { get; set; }
    }
}