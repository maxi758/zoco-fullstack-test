using zoco.Domain.Entities;

namespace zoco.Application.Interfaces.Repositories;

public interface IStudyRepository
{
    Task<List<Study>> GetByUserIdAsync(Guid userId);
    Task AddAsync(Study study);
    Task<Study?> GetByIdAsync(Guid id);
    Task UpdateAsync(Study study);
    Task DeleteAsync(Study study);
}