using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zoco.API.Common;
using zoco.Application.DTOs.Users;
using zoco.Application.Services.Interfaces;

namespace zoco.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = User.GetUserId();
        return Ok(await _userService.GetMeAsync(userId));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _userService.GetAllAsync());
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _userService.GetByIdAsync(id));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateUserRequest request)
    {
        var currentUserId = User.GetUserId();
        var isAdmin = User.IsAdmin();

        return Ok(await _userService.UpdateAsync(currentUserId, isAdmin, id, request));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }
}