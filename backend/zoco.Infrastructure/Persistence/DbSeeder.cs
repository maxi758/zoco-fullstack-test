using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using zoco.Domain.Entities;
using zoco.Domain.Enums;

namespace zoco.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();

        // Admin fijo para la prueba
        const string adminEmail = "admin@zoco.local";
        const string adminPassword = "Admin123!";

        var exists = await db.Users.AnyAsync(u => u.Email == adminEmail);
        if (exists) return;

        var admin = new User
        {
            FirstName = "Admin",
            LastName = "Zoco",
            Email = adminEmail,
            Role = UserRole.Admin,
            CreatedAtUtc = DateTime.UtcNow
        };

        var hasher = new PasswordHasher<User>();
        admin.PasswordHash = hasher.HashPassword(admin, adminPassword);

        db.Users.Add(admin);
        await db.SaveChangesAsync();
    }
}