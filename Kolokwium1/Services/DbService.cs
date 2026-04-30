using Kolokwium1.DTOs;
using Kolokwium1.Exceptions;
using Microsoft.Data.SqlClient;

namespace Kolokwium1.Services;

public class DbService : IDbService
{
    private readonly string _connectionString;

    DbService(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    public async Task<GetMakersDetailsDto> GetMakersDetailsAsync(int makersId)
    {
        var query = """
                    SELECT 
                    m.Id as MakersId,
                    m.Name as MakersName,
                    p.Id as ProductsId,
                    p.Name as ProductsName,
                    p.Description as ProductsDescription,
                    p.StickerPrice as StickerPrice,
                    pt.Id as ProductTypeId,
                    pt.Name as ProductTypeName,
                    v.Code as VendorsCode,
                    v.Name as VendorsName,
                    vp.Amount as VendorsAmount,
                    vp.PricePerUnit as PricePerUnit
                    FROM Makers m 
                    JOIN Prodcuts p ON m.Id = p.MakerId
                    JOIN ProductsType pt ON pt.Id = p.ProductTypeId
                    JOIN VendorProducts vp on p.Id = vp.ProductId
                    JOIN Vendors v on vp.VendorCode = v.Code
                    WHERE m.MakersId = @makersId;
                    """;
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@makersId", makersId);
        
        await using var reader = await command.ExecuteReaderAsync();
        
        GetMakersDetailsDto? result = null;
        
        var ordMakersId = reader.GetOrdinal("MakersId");
        var ordMakersName = reader.GetOrdinal("MakersName");
        var ordProductsId = reader.GetOrdinal("ProductsId");
        var ordProductsName = reader.GetOrdinal("ProductsName");
        var ordProductsDescription = reader.GetOrdinal("ProductsDescription");
        var ordStickerPrice = reader.GetOrdinal("StickerPrice");
        var ordProductTypeId = reader.GetOrdinal("ProductTypeId");
        var ordProductTypeName = reader.GetOrdinal("ProductTypeName");
        var ordVendorsCode = reader.GetOrdinal("VendorsCode");
        var ordVendorsName = reader.GetOrdinal("VendorsName");
        var ordAmount = reader.GetOrdinal("VendorsAmount");
        var ordPricePerUnit = reader.GetOrdinal("PricePerUnit");

        while (await reader.ReadAsync())
        {
            if (result is null)
            {
                result = new GetMakersDetailsDto()
                {
                    MakersId = reader.GetInt32(ordMakersId),
                    MakersName = reader.GetString(ordMakersName),
                    Products = new List<GetProductsDetailsDto>()
                };
                
                var productId = reader.GetInt32(ordProductsId);
                var product = result.Products.FirstOrDefault(x => x.ProductsId.Equals(productId));

                product = new GetProductsDetailsDto()
                {
                    ProductsId = reader.GetInt32(ordProductsId),
                    ProductsName = reader.GetString(ordProductsName),
                    ProductDescription = reader.IsDBNull(ordProductsDescription) ? null : reader.GetString(ordProductsDescription),
                    StickerPrice = reader.GetDecimal(ordStickerPrice),
                    ProductsType = new List<GetProductsTypeDetailsDto>(),
                    Vendors = new List<GetVendorsProductsDetailsDto>()
                };
                result.Products.Add(product);

                product.ProductsType.Add(new GetProductsTypeDetailsDto()
                {
                    ProductsTypeId = reader.GetInt32(ordProductTypeId),
                    ProductsTypeName = reader.GetString(ordProductTypeName)
                });
                
                var vendorsId = reader.GetChar(ordVendorsCode);
                var vendor = product.Vendors.FirstOrDefault(x => x.VendorCode.Equals(vendorsId));

                vendor = new GetVendorsProductsDetailsDto()
                {
                    VendorCode = reader.GetChar(ordVendorsCode),
                    VendorName = reader.GetString(ordVendorsName),
                    Amount = reader.GetInt32(ordAmount),
                    PricePerUnit = reader.GetDecimal(ordPricePerUnit)
                };
                product.Vendors.Add(vendor);
            }
        }
        return result ?? throw new NotFoundException("No makers found");
    }
}