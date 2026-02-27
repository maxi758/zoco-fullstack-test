using zoco.Application.DTOs.Users;

namespace zoco.Application.Services.Interfaces;

public interface IUserService
{
    Task<UserResponse> GetMeAsync(Guid currentUserId);
    Task<List<UserResponse>> GetAllAsync();
    Task<UserResponse> GetByIdAsync(Guid id);

    Task<UserResponse> UpdateAsync(Guid currentUserId, bool isAdmin, Guid targetUserId, UpdateUserRequest request);

    Task DeleteAsync(Guid id);
}