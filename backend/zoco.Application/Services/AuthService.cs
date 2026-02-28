using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using zoco.Application.DTOs.Auth;
using zoco.Application.Interfaces.Repositories;
using zoco.Application.Services.Interfaces;
using zoco.Domain.Entities;
using zoco.Domain.Enums;

namespace zoco.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ISessionLogRepository _sessionLogRepository;
    private readonly IConfiguration _config;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public AuthService(
        IUserRepository userRepository,
        ISessionLogRepository sessionLogRepository,
        IConfiguration config)
    {
        _userRepository = userRepository;
        _sessionLogRepository = sessionLogRepository;
        _config = config;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
            throw new InvalidOperationException("El email ya está registrado");

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Role = UserRole.User
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _userRepository.AddAsync(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            throw new UnauthorizedAccessException("Credenciales inválidas");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Credenciales inválidas");

        // (opcional pro) cerrar sesión activa previa
        await _sessionLogRepository.CloseActiveAsync(user.Id, DateTime.UtcNow);

        await _sessionLogRepository.AddAsync(new SessionLog
        {
            UserId = user.Id,
            StartUtc = DateTime.UtcNow,
            EndUtc = null
        });

        var token = GenerateJwtToken(user);

        return new AuthResponse
        {
            Token = token,
            UserId = user.Id,
            Role = user.Role.ToString(),
            Email = user.Email
        };
    }

    public async Task LogoutAsync(Guid userId)
    {
        await _sessionLogRepository.CloseActiveAsync(userId, DateTime.UtcNow);
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSection = _config.GetSection("Jwt");

        var key = jwtSection["Key"]!;
        var issuer = jwtSection["Issuer"]!;
        var audience = jwtSection["Audience"]!;
        var expiresMinutesStr = jwtSection["ExpiresMinutes"];
        var expiresMinutes = int.TryParse(expiresMinutesStr, out var mins) ? mins : 60;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}