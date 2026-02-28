namespace zoco.Application.DTOs.Addresses;

public class AddressResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Street { get; set; } = null!;
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? ZipCode { get; set; }
}
