using System;
using System.Collections.Generic;
using System.Text;
using FVD.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FVD.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}

// dotnet ef  migrations add Initial --context FVD.Data.ApplicationDbContext -o Data/Migrations/Identity
// dotnet ef database update --context FVD.Data.ApplicationDbContext