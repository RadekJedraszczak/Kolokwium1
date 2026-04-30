namespace Kolokwium1.DTOs;

public class GetProductsDetailsDto
{
    public int ProductsId { get; set; }
    public string ProductsName { get; set; } = string.Empty;
    public string? ProductDescription { get; set; } = string.Empty;
    public decimal StickerPrice { get; set; }
    public List<GetProductsTypeDetailsDto> ProductsType { get; set; } = [];
    public List<GetVendorsProductsDetailsDto> Vendors { get; set; } = [];
}