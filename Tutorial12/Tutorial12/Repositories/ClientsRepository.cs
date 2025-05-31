using Microsoft.EntityFrameworkCore;
using Tutorial12.Models;
using DbContext = Tutorial12.Data.DbContext;

namespace Tutorial12;

public class ClientsRepository: IClientsRepository
{
    public DbContext _dbContext;

    public ClientsRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> HasClient(int idClient)
    {
        return await _dbContext.Clients.AnyAsync(c => c.IdClient == idClient);
    }

    public async Task<Client> GetClient(int idClient)
    {
        return await _dbContext.Clients.Include(c => c.ClientTrips).FirstOrDefaultAsync(c => c.IdClient == idClient);
    }

    public async Task<Client> GetClient(string pesel)
    {
        return await _dbContext.Clients.FirstOrDefaultAsync(c => c.Pesel == pesel);
    }

    public void Delete(Client client)
    {
        _dbContext.Clients.Remove(client);
    }

    public async Task AddClient(Client client)
    {
        await _dbContext.Clients.AddAsync(client);
    }
}