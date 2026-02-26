using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zoco.API.Common;
using zoco.Application.DTOs.Addresses;
using zoco.Application.Interfaces.Repositories;
using zoco.Domain.Entities;

namespace zoco.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AddressesController : ControllerBase
{
    private readonly IAddressRepository _addressRepository;

    public AddressesController(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetMine()
    {
        var userId = User.GetUserId();

        var addresses = await _addressRepository.GetByUserIdAsync(userId);

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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = User.GetUserId();
        var address = await _addressRepository.GetByIdAsync(id);
        if (address == null) return NotFound();
        if (!User.IsAdmin() && address.UserId != userId) return Forbid();
        return Ok(new AddressResponse
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
            Country = request.Country,
            ZipCode = request.ZipCode
        };

        await _addressRepository.AddAsync(address);

        return Ok(new AddressResponse
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

        var address = await _addressRepository.GetByIdAsync(id);
        if (address == null) return NotFound();

        if (!User.IsAdmin() && address.UserId != userId) return Forbid();

        address.Street = request.Street;
        address.City = request.City;
        address.State = request.State;
        address.Country = request.Country;
        address.ZipCode = request.ZipCode;

        await _addressRepository.UpdateAsync(address);

        return Ok(new AddressResponse
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

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.GetUserId();

        var address = await _addressRepository.GetByIdAsync(id);
        if (address == null) return NotFound();

        if (!User.IsAdmin() && address.UserId != userId) return Forbid();

        await _addressRepository.DeleteAsync(address);

        return NoContent();
    }
}