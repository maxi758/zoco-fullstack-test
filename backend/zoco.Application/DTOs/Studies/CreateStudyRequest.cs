namespace zoco.Application.DTOs.Studies;

public class CreateStudyRequest
{
    public string Title { get; set; } = null!;
    public string? Institution { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}