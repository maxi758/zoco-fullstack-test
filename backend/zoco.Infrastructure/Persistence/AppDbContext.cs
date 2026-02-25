using Microsoft.EntityFrameworkCore;
using zoco.Domain.Entities;

namespace zoco.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Study> Studies => Set<Study>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<SessionLog> SessionLogs => Set<SessionLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Email).HasMaxLength(256).IsRequired();
            e.Property(x => x.PasswordHash).IsRequired();
            e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            e.Property(x => x.LastName).HasMaxLength(100).IsRequired();
        });

        // Relationships
        modelBuilder.Entity<Study>()
            .HasOne(x => x.User)
            .WithMany(u => u.Studies)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Address>()
            .HasOne(x => x.User)
            .WithMany(u => u.Addresses)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SessionLog>()
            .HasOne(x => x.User)
            .WithMany(u => u.SessionLogs)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
