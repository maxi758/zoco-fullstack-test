using Microsoft.EntityFrameworkCore;
using zoco.Application.Interfaces.Repositories;
using zoco.Domain.Entities;
using zoco.Infrastructure.Persistence;

namespace zoco.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}