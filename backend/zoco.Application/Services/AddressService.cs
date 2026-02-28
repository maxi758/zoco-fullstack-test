using zoco.Application.DTOs.Addresses;
using zoco.Application.Interfaces.Repositories;
using zoco.Application.Services.Interfaces;
using zoco.Domain.Entities;

namespace zoco.Application.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;

    public AddressService(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<List<AddressResponse>> GetByUserIdAsync(Guid currentUserId)
    {
        var addresses = await _addressRepository.GetByUserIdAsync(currentUserId);
        return addresses.Select(ToResponse).ToList();
    }

    public async Task<AddressResponse> GetByIdAsync(Guid currentUserId, bool isAdmin, Guid id)
    {
        var address = await _addressRepository.GetByIdAsync(id);
        if (address == null) throw new InvalidOperationException("Dirección no encontrada");

        if (!isAdmin && address.UserId != currentUserId)
            throw new UnauthorizedAccessException("No autorizado");

        return ToResponse(address);
    }

    public async Task<AddressResponse> CreateAsync(Guid currentUserId, bool isAdmin, CreateAddressRequest request)
    {
        var targetUserId = (isAdmin && request.UserId != Guid.Empty)
                       ? request.UserId
                       : currentUserId;

        var address = new Address
        {
            UserId = targetUserId,
            Street = request.Street,
            City = request.City,
            State = request.State,
            Country = request.Country,
            ZipCode = request.ZipCode
        };

        await _addressRepository.AddAsync(address);
        return ToResponse(address);
    }

    public async Task<AddressResponse> UpdateAsync(Guid currentUserId, bool isAdmin, Guid id, UpdateAddressRequest request)
    {
        var address = await _addressRepository.GetByIdAsync(id);
        if (address == null) throw new InvalidOperationException("Dirección no encontrada");

        if (!isAdmin && address.UserId != currentUserId)
            throw new UnauthorizedAccessException("No autorizado");

        address.Street = request.Street;
        address.City = request.City;
        address.State = request.State;
        address.Country = request.Country;
        address.ZipCode = request.ZipCode;

        await _addressRepository.UpdateAsync(address);
        return ToResponse(address);
    }

    public async Task DeleteAsync(Guid currentUserId, bool isAdmin, Guid id)
    {
        var address = await _addressRepository.GetByIdAsync(id);
        if (address == null) throw new InvalidOperationException("Dirección no encontrada");

        if (!isAdmin && address.UserId != currentUserId)
            throw new UnauthorizedAccessException("No autorizado");

        await _addressRepository.DeleteAsync(address);
    }

    private static AddressResponse ToResponse(Address a)
    {
        return new AddressResponse
        {
            Id = a.Id,
            UserId = a.UserId,
            Street = a.Street,
            City = a.City,
            State = a.State,
            Country = a.Country,
            ZipCode = a.ZipCode
        };
    }
}