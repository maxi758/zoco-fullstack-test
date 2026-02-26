namespace zoco.Application.DTOs.Users;

public class UpdateUserRequest
{
    public string? FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public string? Email { get; set; } = null!;
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }

}