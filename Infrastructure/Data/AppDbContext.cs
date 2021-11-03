using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RepairMarketPlace.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepairMarketPlace.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Component> Components { get; set; }
    }
}
