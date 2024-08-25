using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
    : IdentityDbContext<AppUser>(dbContextOptions)
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Node> Nodes { get; set; }
    public DbSet<NodeType> NodeTypes { get; set; }
    public DbSet<NodeAttribute> NodeAttributes { get; set; }
    public DbSet<NodeAttributeValue> NodeAttributeValues { get; set; }

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
        
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.SourceAccount)
            .WithMany(a => a.SourceTransactions)
            .HasForeignKey(t => t.SourceAccountId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.DestinationAccount)
            .WithMany(a => a.DestinationTransactions)
            .HasForeignKey(t => t.DestinationAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Node>()
            .HasOne(n => n.Type)
            .WithMany(t => t.Nodes)
            .HasForeignKey(n => n.TypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<NodeAttribute>()
            .HasOne(attr => attr.NodeType)
            .WithMany(t => t.Attributes)
            .HasForeignKey(attr => attr.NodeTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<NodeAttributeValue>()
            .HasOne(val => val.Node)
            .WithMany(n => n.AttributeValues)
            .HasForeignKey(val => val.NodeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<NodeAttributeValue>()
            .HasOne(val => val.NodeAttribute)
            .WithMany()
            .HasForeignKey(val => val.NodeAttributeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}