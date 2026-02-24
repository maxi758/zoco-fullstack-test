namespace zoco.Domain.Entities;

public class SessionLog
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public DateTime StartUtc { get; set; } = DateTime.UtcNow;
    public DateTime? EndUtc { get; set; }
}
