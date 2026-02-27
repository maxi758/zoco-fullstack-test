using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zoco.API.Common;
using zoco.Application.DTOs.Addresses;
using zoco.Application.Services.Interfaces;
using zoco.Domain.Entities;

namespace zoco.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AddressesController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressesController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMine()
    {
        var userId = User.GetUserId();
        return Ok(await _addressService.GetMineAsync(userId));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = User.GetUserId();
        var isAdmin = User.IsAdmin();
        return Ok(await _addressService.GetByIdAsync(userId, isAdmin, id));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAddressRequest request)
    {
        var userId = User.GetUserId();
        var address = new Address
            {
            UserId = userId,
            Street = request.Street,
            City = request.City,
            State = request.State,
            ZipCode = request.ZipCode,
            Country = request.Country
        };
        await _addressService.CreateAsync(userId, request);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateAddressRequest request)
    {
        var userId = User.GetUserId();
        var isAdmin = User.IsAdmin();
        return Ok(await _addressService.UpdateAsync(userId, isAdmin, id, request));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.GetUserId();
        var isAdmin = User.IsAdmin();
        await _addressService.DeleteAsync(userId, isAdmin, id);
        return NoContent();
    }
}