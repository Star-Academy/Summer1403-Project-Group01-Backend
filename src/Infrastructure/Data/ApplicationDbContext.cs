using System.Collections.Generic;
using Domain.Constants;
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
            new IdentityRole { Name = AppRoles.Admin, NormalizedName = AppRoles.Admin.ToUpper()},
            new IdentityRole { Name = AppRoles.DataAdmin, NormalizedName = AppRoles.DataAdmin.ToUpper()},
            new IdentityRole { Name = AppRoles.DataAnalyst, NormalizedName = AppRoles.DataAnalyst.ToUpper()}
        ];
        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }
}