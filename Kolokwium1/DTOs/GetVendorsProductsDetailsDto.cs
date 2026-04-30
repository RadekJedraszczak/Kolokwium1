namespace Kolokwium1.DTOs;

public class GetVendorsProductsDetailsDto
{
    public char VendorCode { get; set; }
    public string VendorName { get; set; } = string.Empty;
    public int Amount { get; set; }
    public decimal PricePerUnit { get; set; }
}