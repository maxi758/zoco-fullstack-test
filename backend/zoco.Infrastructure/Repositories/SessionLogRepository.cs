using Microsoft.EntityFrameworkCore;
using zoco.Application.Interfaces.Repositories;
using zoco.Domain.Entities;
using zoco.Infrastructure.Persistence;

namespace zoco.Infrastructure.Repositories;

public class SessionLogRepository : ISessionLogRepository
{
    private readonly AppDbContext _context;

    public SessionLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SessionLog?> GetActiveByUserIdAsync(Guid userId)
    {
        return await _context.SessionLogs
            .Where(x => x.UserId == userId && x.EndUtc == null)
            .OrderByDescending(x => x.StartUtc)
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(SessionLog log)
    {
        _context.SessionLogs.Add(log);
        await _context.SaveChangesAsync();
    }

    public async Task CloseActiveAsync(Guid userId, DateTime endUtc)
    {
        var active = await GetActiveByUserIdAsync(userId);
        if (active == null) return;

        active.EndUtc = endUtc;
        await _context.SaveChangesAsync();
    }
}