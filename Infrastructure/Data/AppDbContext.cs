using ims.Domain.Entities;
using ims.Domain.Entities.Base;
using ims.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ims.Infrastructure.Data;

public class AppDbContext
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.ProfileImage)
                .HasMaxLength(500);

            entity.HasMany(x => x.RefreshTokens)
                .WithOne()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ApplicationRole>(entity =>
        {
            entity.Property(x => x.Description)
                .HasMaxLength(250);
        });

        builder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.TokenHash)
                .HasMaxLength(256)
                .IsRequired();

            entity.Property(x => x.JwtId)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.CreatedByIp)
                .HasMaxLength(45)
                .IsRequired();

            entity.Property(x => x.RevokedByIp)
                .HasMaxLength(45);

            entity.Property(x => x.ReplacedByTokenHash)
                .HasMaxLength(256);

            entity.HasIndex(x => x.TokenHash).IsUnique();
            entity.HasIndex(x => x.UserId);
        });

        builder.Entity<AuditLog>(entity =>
        {
            entity.Property(x => x.Action).HasMaxLength(100);
            entity.Property(x => x.EntityName).HasMaxLength(150);
            entity.Property(x => x.IpAddress).HasMaxLength(45);
            entity.Property(x => x.UserAgent).HasMaxLength(500);
            entity.Property(x => x.Path).HasMaxLength(500);
            entity.Property(x => x.HttpMethod).HasMaxLength(20);
        });

        builder.Entity<Notification>(entity =>
        {
            entity.Property(x => x.Title)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Message)
                .HasMaxLength(2000)
                .IsRequired();
        });

        builder.Entity<Product>(entity =>
        {
            entity.Property(x => x.CostPrice).HasPrecision(18, 2);
            entity.Property(x => x.SellingPrice).HasPrecision(18, 2);
            entity.Property(x => x.TaxRate).HasPrecision(5, 2);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditAndSoftDelete();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditAndSoftDelete()
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity baseEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        baseEntity.CreatedAt = utcNow;
                        baseEntity.UpdatedAt = utcNow;
                        break;

                    case EntityState.Modified:
                        baseEntity.UpdatedAt = utcNow;
                        break;

                    case EntityState.Deleted:
                        baseEntity.IsDeleted = true;
                        baseEntity.DeletedAt = utcNow;
                        baseEntity.UpdatedAt = utcNow;
                        entry.State = EntityState.Modified;
                        break;
                }
            }

            if (entry.Entity is ApplicationUser user)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        user.CreatedAt = utcNow;
                        user.UpdatedAt = utcNow;
                        break;

                    case EntityState.Modified:
                        user.UpdatedAt = utcNow;
                        break;

                    case EntityState.Deleted:
                        user.IsDeleted = true;
                        user.DeletedAt = utcNow;
                        user.UpdatedAt = utcNow;
                        entry.State = EntityState.Modified;
                        break;
                }
            }
        }
    }
}