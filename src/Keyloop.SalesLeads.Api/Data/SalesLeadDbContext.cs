using Keyloop.SalesLeads.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Keyloop.SalesLeads.Api.Data;

public class SalesLeadDbContext : DbContext
{
    public SalesLeadDbContext(DbContextOptions<SalesLeadDbContext> options) : base(options) { }

    public DbSet<SalesLead> SalesLeads => Set<SalesLead>();
    public DbSet<LeadActivity> LeadActivities => Set<LeadActivity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SalesLead>(entity =>
        {
            entity.Property(x => x.CustomerName).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(254).IsRequired();
            entity.Property(x => x.Phone).HasMaxLength(30);
            entity.Property(x => x.VehicleInterest).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(30).IsRequired();
            entity.HasMany(x => x.Activities)
                .WithOne(x => x.SalesLead)
                .HasForeignKey(x => x.SalesLeadId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<LeadActivity>(entity =>
        {
            entity.Property(x => x.ActivityType).HasMaxLength(60).IsRequired();
            entity.Property(x => x.Notes).HasMaxLength(1000).IsRequired();
            entity.HasIndex(x => new { x.SalesLeadId, x.CreatedAtUtc });
        });
    }
}
