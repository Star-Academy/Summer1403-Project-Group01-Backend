using System.Collections.Generic;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
    : IdentityDbContext<AppUser>(dbContextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        List<IdentityRole> roles =
        [
            new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Name = "User", NormalizedName = "USER" }
        ];
        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }
}