using Tutorial12.DTOs;
using Tutorial12.Models;

namespace Tutorial12.Services;

public interface IService
{
    Task<TripsResponseDTO> GetTripsInfo(int? pageNum, int? pageSize);
    Task DeleteClient(int idClient);
    Task RegisterClientForTrip(AssignClientTripRequestDTO requestDto, int idTrip, DateTime timestamp);
    // Task<Client> GetClient(string pesel);
}