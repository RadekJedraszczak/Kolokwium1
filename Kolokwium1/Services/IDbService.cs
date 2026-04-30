using Kolokwium1.DTOs;

namespace Kolokwium1.Services;

public interface IDbService
{
    Task<GetMakersDetailsDto> GetMakersDetailsAsync(int makersId);
}