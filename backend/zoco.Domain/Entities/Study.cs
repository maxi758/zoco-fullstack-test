namespace zoco.Domain.Entities;

public class Study
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string? Institution { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

