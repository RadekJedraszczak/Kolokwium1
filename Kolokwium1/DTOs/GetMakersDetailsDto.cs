namespace Kolokwium1.DTOs;

public class GetMakersDetailsDto
{
    public int MakersId { get; set; }
    public string MakersName { get; set; } = string.Empty;
    public List<GetProductsDetailsDto> Products { get; set; } = [];
}