using zoco.Application.DTOs.Studies;

namespace zoco.Application.DTOs.Users;

public class UserResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public List<StudyResponse> Studies { get; set; } = new();
}