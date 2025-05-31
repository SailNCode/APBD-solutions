using Tutorial12.Models;

namespace Tutorial12;

public interface ITripsRepository
{
    Task<List<Trip>> GetTrips();

    Task<Trip> GetTrip(int idTrip);

    Task AddClientTrip(ClientTrip clientTrip);
}