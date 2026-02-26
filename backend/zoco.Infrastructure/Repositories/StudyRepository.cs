using Microsoft.EntityFrameworkCore;
using zoco.Application.Interfaces.Repositories;
using zoco.Domain.Entities;
using zoco.Infrastructure.Persistence;

namespace zoco.Infrastructure.Repositories;

public class StudyRepository : IStudyRepository
{
    private readonly AppDbContext _context;

    public StudyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Study>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Studies
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(Study study)
    {
        _context.Studies.Add(study);
        await _context.SaveChangesAsync();
    }
}