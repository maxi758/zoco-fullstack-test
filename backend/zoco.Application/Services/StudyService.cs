using zoco.Application.DTOs.Studies;
using zoco.Application.Interfaces.Repositories;
using zoco.Application.Services.Interfaces;
using zoco.Domain.Entities;

namespace zoco.Application.Services;

public class StudyService : IStudyService
{
    private readonly IStudyRepository _studyRepository;

    public StudyService(IStudyRepository studyRepository)
    {
        _studyRepository = studyRepository;
    }

    public async Task<List<StudyResponse>> GetMineAsync(Guid currentUserId)
    {
        var studies = await _studyRepository.GetByUserIdAsync(currentUserId);
        return studies.Select(ToResponse).ToList();
    }

    public async Task<StudyResponse> GetByIdAsync(Guid currentUserId, bool isAdmin, Guid id)
    {
        var study = await _studyRepository.GetByIdAsync(id);
        if (study == null) throw new InvalidOperationException("Estudio no encontrado");

        if (!isAdmin && study.UserId != currentUserId)
            throw new UnauthorizedAccessException("No autorizado");

        return ToResponse(study);
    }

    public async Task<StudyResponse> CreateAsync(Guid currentUserId, bool isAdmin,  CreateStudyRequest request)
    {
        var targetUserId = (isAdmin && request.UserId != Guid.Empty)
                       ? request.UserId
                       : currentUserId;
        var study = new Study
        {
            UserId = targetUserId,
            Title = request.Title,
            Institution = request.Institution,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        await _studyRepository.AddAsync(study);
        return ToResponse(study);
    }

    public async Task<StudyResponse> UpdateAsync(Guid currentUserId, bool isAdmin, Guid id, UpdateStudyRequest request)
    {
        var study = await _studyRepository.GetByIdAsync(id);
        if (study == null) throw new InvalidOperationException("Estudio no encontrado");

        if (!isAdmin && study.UserId != currentUserId)
            throw new UnauthorizedAccessException("No autorizado");

        study.Title = request.Title;
        study.Institution = request.Institution;
        study.StartDate = request.StartDate;
        study.EndDate = request.EndDate;

        await _studyRepository.UpdateAsync(study);
        return ToResponse(study);
    }

    public async Task DeleteAsync(Guid currentUserId, bool isAdmin, Guid id)
    {
        var study = await _studyRepository.GetByIdAsync(id);
        if (study == null) throw new InvalidOperationException("Estudio no encontrado");

        if (!isAdmin && study.UserId != currentUserId)
            throw new UnauthorizedAccessException("No autorizado");

        await _studyRepository.DeleteAsync(study);
    }

    private static StudyResponse ToResponse(Study s)
    {
        return new StudyResponse
        {
            Id = s.Id,
            UserId = s.UserId,
            Title = s.Title,
            Institution = s.Institution,
            StartDate = s.StartDate,
            EndDate = s.EndDate
        };
    }
}