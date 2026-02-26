namespace zoco.Application.DTOs.Addresses;

public class UpdateAddressRequest
{
    public string Street { get; set; } = null!;
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? ZipCode { get; set; }
}