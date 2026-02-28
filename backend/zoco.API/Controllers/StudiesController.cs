using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zoco.API.Common;
using zoco.Application.DTOs.Studies;
using zoco.Application.Interfaces.Repositories;
using zoco.Application.Services.Interfaces;
using zoco.Domain.Entities;

namespace zoco.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudiesController : ControllerBase
{
    private readonly IStudyService _studyService;

    public StudiesController(IStudyService studyService)
    {
        _studyService = studyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMine()
    {
        var userId = User.GetUserId();

        var studies = await _studyService.GetByUserIdAsync(userId);

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

    [Authorize(Roles = "Admin")]
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        
        var studies = await _studyService.GetByUserIdAsync(userId);
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
        var isAdmin = User.IsAdmin();

        var study = new Study
        {
            UserId = request.UserId,
            Title = request.Title,
            Institution = request.Institution,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        await _studyService.CreateAsync(userId, isAdmin, request);

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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = User.GetUserId();
        var isAdmin = User.IsAdmin();

        var study = await _studyService.GetByIdAsync(userId, isAdmin, id);
        if (study == null) return NotFound();

        if (!User.IsAdmin() && study.UserId != userId) return Forbid();

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

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateStudyRequest request)
    {
        var userId = User.GetUserId();
        var isAdmin = User.IsAdmin();

        return Ok(await _studyService.UpdateAsync(userId, isAdmin, id, request));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.GetUserId();
        var isAdmin = User.IsAdmin();

        await _studyService.DeleteAsync(userId, isAdmin, id);

        return NoContent();
    }
}