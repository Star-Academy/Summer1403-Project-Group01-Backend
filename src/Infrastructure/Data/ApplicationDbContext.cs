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
    public DbSet<Edge> Edges { get; set; }
    public DbSet<EdgeAttribute> EdgeAttributes { get; set; }
    public DbSet<EdgeAttributeValue> EdgeAttributeValues { get; set; }
    public DbSet<EdgeType> EdgeTypes { get; set; }
    public DbSet<Node> Nodes { get; set; }
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
        
        modelBuilder.Entity<Edge>()
                .HasKey(e => e.Id);

        modelBuilder.Entity<Edge>()
            .HasOne(e => e.SourceNode)
            .WithMany() // Assuming there's no reverse navigation property in Node
            .HasForeignKey(e => e.SourceValue)
            .IsRequired(false);

        modelBuilder.Entity<Edge>()
            .HasOne(e => e.DestinationNode)
            .WithMany() // Assuming there's no reverse navigation property in Node
            .HasForeignKey(e => e.DestinationValue)
            .IsRequired(false);

        modelBuilder.Entity<Edge>()
            .HasOne(e => e.Type)
            .WithMany() // Assuming there's no reverse navigation property in EdgeType
            .HasForeignKey(e => e.TypeId);

        modelBuilder.Entity<Edge>()
            .HasMany(e => e.AttributeValues)
            .WithOne(av => av.Edge)
            .HasForeignKey(av => av.EdgeId);

        // EdgeType relationships
        modelBuilder.Entity<EdgeType>()
            .HasKey(et => et.Id);

        modelBuilder.Entity<EdgeType>()
            .HasOne(et => et.SourceNodeAttribute)
            .WithMany() // Assuming there's no reverse navigation property in NodeAttribute
            .HasForeignKey(et => et.SourceNodeAttributeId);

        modelBuilder.Entity<EdgeType>()
            .HasOne(et => et.DestinationNodeAttribute)
            .WithMany() // Assuming there's no reverse navigation property in NodeAttribute
            .HasForeignKey(et => et.DestinationNodeAttributeId);

        // EdgeAttribute relationships
        modelBuilder.Entity<EdgeAttribute>()
            .HasKey(ea => ea.Id);

        modelBuilder.Entity<EdgeAttribute>()
            .HasOne(ea => ea.EdgeType)
            .WithMany() // Assuming there's no reverse navigation property in EdgeType
            .HasForeignKey(ea => ea.EdgeTypeId);

        // EdgeAttributeValue relationships
        modelBuilder.Entity<EdgeAttributeValue>()
            .HasKey(eav => new { eav.EdgeId, eav.EdgeAttributeId });

        modelBuilder.Entity<EdgeAttributeValue>()
            .HasOne(eav => eav.Edge)
            .WithMany(e => e.AttributeValues)
            .HasForeignKey(eav => eav.EdgeId);

        modelBuilder.Entity<EdgeAttributeValue>()
            .HasOne(eav => eav.EdgeAttribute)
            .WithMany() // Assuming there's no reverse navigation property in EdgeAttribute
            .HasForeignKey(eav => eav.EdgeAttributeId);

        // EdgeAttribute to EdgeAttributeValue one-to-many relationship
        modelBuilder.Entity<EdgeAttribute>()
            .HasMany(a => a.EdgeAttributeValues)
            .WithOne(av => av.EdgeAttribute)
            .HasForeignKey(av => av.EdgeAttributeId);
    }
}