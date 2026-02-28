using Microsoft.AspNetCore.Identity;
using zoco.Application.DTOs.Studies;
using zoco.Application.DTOs.Users;
using zoco.Application.Interfaces.Repositories;
using zoco.Application.Services.Interfaces;
using zoco.Domain.Entities;

namespace zoco.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse> GetMeAsync(Guid currentUserId)
    {
        var user = await _userRepository.GetByIdAsync(currentUserId);
        if (user == null) throw new InvalidOperationException("Usuario no encontrado");

        return ToResponse(user);
    }

    public async Task<List<UserResponse>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(ToResponse).ToList();
    }

    public async Task<UserResponse> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) throw new InvalidOperationException("Usuario no encontrado");

        return new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role.ToString(),
            Studies = user.Studies.Select(s => new StudyResponse
            {
                Id = s.Id,
                UserId = s.UserId,
                Title = s.Title,
                Institution = s.Institution,
                StartDate = s.StartDate
            }).ToList()
        };

        //return ToResponse(user);
    }

    public async Task<UserResponse> UpdateAsync(Guid currentUserId, bool isAdmin, Guid targetUserId, UpdateUserRequest request)
    {
        // ownership: si no es admin, solo se puede editar a sí mismo
        if (!isAdmin && targetUserId != currentUserId)
            throw new UnauthorizedAccessException("No autorizado");

        var user = await _userRepository.GetByIdAsync(targetUserId);
        if (user == null) throw new InvalidOperationException("Usuario no encontrado");

        if (!string.IsNullOrWhiteSpace(request.FirstName))
            user.FirstName = request.FirstName;

        if (!string.IsNullOrWhiteSpace(request.LastName))
            user.LastName = request.LastName;

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existing = await _userRepository.GetByEmailAsync(request.Email);
                if (existing != null) throw new InvalidOperationException("El email ya está registrado");

                user.Email = request.Email;
            }
        }

        if (!string.IsNullOrWhiteSpace(request.Password)
            && !string.IsNullOrWhiteSpace(request.ConfirmPassword))
        {
            if (request.Password != request.ConfirmPassword)
                throw new InvalidOperationException("Las contraseñas no coinciden");
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, request.Password);
        }

        await _userRepository.UpdateAsync(user);

        return ToResponse(user);
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) throw new InvalidOperationException("Usuario no encontrado");

        await _userRepository.DeleteAsync(user);
    }

    private static UserResponse ToResponse(zoco.Domain.Entities.User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
}