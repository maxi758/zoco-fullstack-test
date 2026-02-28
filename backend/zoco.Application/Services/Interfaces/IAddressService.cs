using zoco.Application.DTOs.Addresses;

namespace zoco.Application.Services.Interfaces;

public interface IAddressService
{
    Task<List<AddressResponse>> GetMineAsync(Guid currentUserId);
    Task<AddressResponse> GetByIdAsync(Guid currentUserId, bool isAdmin, Guid id);
    Task<AddressResponse> CreateAsync(Guid currentUserId, CreateAddressRequest request);
    Task<AddressResponse> UpdateAsync(Guid currentUserId, bool isAdmin, Guid id, UpdateAddressRequest request);
    Task DeleteAsync(Guid currentUserId, bool isAdmin, Guid id);
}
