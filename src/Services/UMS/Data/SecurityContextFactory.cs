using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VDS.UMS.Data
{
    public class UMSContextFactory : IDesignTimeDbContextFactory<UMSContext>
    {
        public UMSContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var connectionString = configuration.GetConnectionString("UMSDbConnection");

            var optionsBuilder = new DbContextOptionsBuilder<UMSContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new UMSContext(optionsBuilder.Options);
        }
    }
}
