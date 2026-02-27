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
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(Study study)
    {
        _context.Studies.Add(study);
        await _context.SaveChangesAsync();
    }

    public async Task<Study?> GetByIdAsync(Guid id)
    {
        return await _context.Studies.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateAsync(Study study)
    {
        _context.Studies.Update(study);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Study study)
    {
        _context.Studies.Remove(study);
        await _context.SaveChangesAsync();
    }
}