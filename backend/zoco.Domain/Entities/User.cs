using System.Net;
using zoco.Domain.Enums;

namespace zoco.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public UserRole Role { get; set; } = UserRole.User;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<Study> Studies { get; set; } = new List<Study>();
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<SessionLog> SessionLogs { get; set; } = new List<SessionLog>();
}
