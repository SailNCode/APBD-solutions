using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface ICountriesService
{
    Task<List<CountryDTO>> GetCountriesByTripId(int tripId);
}