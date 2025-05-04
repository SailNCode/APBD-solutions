using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface ITripsService
{
    Task<bool> IsTripPresent(int tripId);
    Task<List<TripDTO>> GetTrips();
    Task<List<TripReservationDTO>> GetTripReservationsByClientId(int clientId);
    Task<bool> RegisterClientForTrip(int clientId, int tripId);
    Task<bool> IsTripFull(int tripId);
    Task<bool> RemoveClientFromTrip(int clientId, int tripId);
    Task<bool> IsClientRegisteredForTrip(int clientId, int tripId);
}