using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using zoco.Application.DTOs.Auth;
using zoco.Application.Interfaces.Repositories;
using zoco.Domain.Entities;
using zoco.Domain.Enums;

namespace zoco.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserRepository userRepository, IConfiguration config, ISessionLogRepository sessionLogRepository) : ControllerBase
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ISessionLogRepository _sessionLogRepository = sessionLogRepository;
    private readonly IConfiguration _config = config;
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

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

    [HttpGet("token-test")]
    public IActionResult TokenTest()
    {
        var fakeUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User",
            Role = UserRole.User,
            PasswordHash = "x"
        };

        var token = GenerateJwtToken(fakeUser);
        return Ok(token);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);

        if (existingUser != null)
        {
            return BadRequest("El email ya está registrado");
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Role = UserRole.User
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _userRepository.AddAsync(user);

        return Ok("Usuario creado correctamente");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
            return Unauthorized("Credenciales inválidas");

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password
        );

        if (result == PasswordVerificationResult.Failed)
            return Unauthorized("Credenciales inválidas");

        var token = GenerateJwtToken(user);

        await _sessionLogRepository.CloseActiveAsync(user.Id, DateTime.UtcNow);

        await _sessionLogRepository.AddAsync(new SessionLog
        {
            UserId = user.Id,
            StartUtc = DateTime.UtcNow,
            EndUtc = null
        });

        return Ok(new AuthResponse
        {
            Token = token,
            UserId = user.Id,
            Role = user.Role.ToString(),
            Email = user.Email
        });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            return Unauthorized();

        await _sessionLogRepository.CloseActiveAsync(userId, DateTime.UtcNow);

        return Ok("Sesión cerrada");
    }
}
