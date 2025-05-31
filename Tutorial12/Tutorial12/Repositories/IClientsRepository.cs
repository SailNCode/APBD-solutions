using Tutorial12.Models;

namespace Tutorial12;

public interface IClientsRepository
{
    Task<bool> HasClient(int idClient);

    Task<Client> GetClient(int idClient);

    Task<Client> GetClient(string pesel);

    void Delete(Client client);

    Task AddClient(Client client);
}