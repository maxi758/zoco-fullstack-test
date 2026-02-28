using zoco.Application.DTOs.Auth;

namespace zoco.Application.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task RegisterAsync(RegisterRequest request);
    Task LogoutAsync(Guid userId);
}