using Tutorial12.Data;
using Tutorial12.Models;

namespace Tutorial12;

public class TripsRepository: ITripsRepository
{
    public DbContext _dbContext;

    public TripsRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Trip>> GetTrips()
    {
        return _dbContext.Trips.ToList();
    }

    public async Task<Trip> GetTrip(int idTrip)
    {
        return _dbContext.Trips.FirstOrDefault(c => c.IdTrip == idTrip);
    }

    public async Task AddClientTrip(ClientTrip clientTrip)
    {
        await _dbContext.ClientTrips.AddAsync(clientTrip);
    }
}