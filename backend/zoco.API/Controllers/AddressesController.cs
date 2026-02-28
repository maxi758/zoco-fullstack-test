using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zoco.API.Common;
using zoco.Application.DTOs.Addresses;
using zoco.Application.DTOs.Studies;
using zoco.Application.Services;
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
        return Ok(await _addressService.GetByUserIdAsync(userId));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = User.GetUserId();
        var isAdmin = User.IsAdmin();
        return Ok(await _addressService.GetByIdAsync(userId, isAdmin, id));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var addresses = await _addressService.GetByUserIdAsync(userId);

        var result = addresses.Select(a => new AddressResponse
        {
            Id = a.Id,
            UserId = a.UserId,
            Street = a.Street,
            City = a.City,
            State = a.State,
            Country = a.Country,
            ZipCode = a.ZipCode
        });

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAddressRequest request)
    {
        var userId = User.GetUserId();
        var isAdmin = User.IsAdmin();

        var address = new Address
            {
            UserId = userId,
            Street = request.Street,
            City = request.City,
            State = request.State,
            ZipCode = request.ZipCode,
            Country = request.Country
        };
        await _addressService.CreateAsync(userId, isAdmin, request);
        return Ok( new AddressResponse
        {
            Id = address.Id,
            UserId = address.UserId,
            Street = address.Street,
            City = address.City,
            State = address.State,
            Country = address.Country,
            ZipCode = address.ZipCode
        });
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