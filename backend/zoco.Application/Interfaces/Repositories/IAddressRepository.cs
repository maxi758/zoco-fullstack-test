using zoco.Domain.Entities;

namespace zoco.Application.Interfaces.Repositories;

public interface IAddressRepository
{
    Task<List<Address>> GetByUserIdAsync(Guid userId);
    Task<Address?> GetByIdAsync(Guid id);
    Task AddAsync(Address address);
    Task UpdateAsync(Address address);
    Task DeleteAsync(Address address);
}