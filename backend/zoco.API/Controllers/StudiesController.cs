using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zoco.API.Common;
using zoco.Application.DTOs.Studies;
using zoco.Application.Interfaces.Repositories;
using zoco.Domain.Entities;

namespace zoco.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudiesController : ControllerBase
{
    private readonly IStudyRepository _studyRepository;

    public StudiesController(IStudyRepository studyRepository)
    {
        _studyRepository = studyRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetMine()
    {
        var userId = User.GetUserId();

        var studies = await _studyRepository.GetByUserIdAsync(userId);

        var result = studies.Select(s => new StudyResponse
        {
            Id = s.Id,
            UserId = s.UserId,
            Title = s.Title,
            Institution = s.Institution,
            StartDate = s.StartDate,
            EndDate = s.EndDate
        });

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateStudyRequest request)
    {
        var userId = User.GetUserId();

        var study = new Study
        {
            UserId = userId,
            Title = request.Title,
            Institution = request.Institution,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        await _studyRepository.AddAsync(study);

        return Ok(new StudyResponse
        {
            Id = study.Id,
            UserId = study.UserId,
            Title = study.Title,
            Institution = study.Institution,
            StartDate = study.StartDate,
            EndDate = study.EndDate
        });
    }
}