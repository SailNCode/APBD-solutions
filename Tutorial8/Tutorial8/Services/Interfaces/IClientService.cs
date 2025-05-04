using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface IClientService
{
    Task<bool> IsClientPresent(int clientId);
    Task<bool> AddClient(ClientDTO clientDTO);
}