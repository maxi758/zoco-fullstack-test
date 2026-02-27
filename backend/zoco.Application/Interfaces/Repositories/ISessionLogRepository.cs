using zoco.Domain.Entities;

namespace zoco.Application.Interfaces.Repositories;

public interface ISessionLogRepository
{
    Task<SessionLog?> GetActiveByUserIdAsync(Guid userId);
    Task AddAsync(SessionLog log);
    Task CloseActiveAsync(Guid userId, DateTime endUtc);
}