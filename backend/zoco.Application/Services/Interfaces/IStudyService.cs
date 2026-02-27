using zoco.Application.DTOs.Studies;

namespace zoco.Application.Services.Interfaces;

public interface IStudyService
{
    Task<List<StudyResponse>> GetMineAsync(Guid currentUserId);
    Task<StudyResponse> GetByIdAsync(Guid currentUserId, bool isAdmin, Guid id);
    Task<StudyResponse> CreateAsync(Guid currentUserId, bool isAdmin, CreateStudyRequest request);
    Task<StudyResponse> UpdateAsync(Guid currentUserId, bool isAdmin, Guid id, UpdateStudyRequest request);
    Task DeleteAsync(Guid currentUserId, bool isAdmin, Guid id);
}