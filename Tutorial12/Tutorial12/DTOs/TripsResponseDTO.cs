namespace Tutorial12.DTOs;

public class TripsResponseDTO
{
    public int? pageNum { get; set; }
    public int? pageSize { get; set; }
    public int? allPages { get; set; }
    public List<TripResponseDTO> Trips { get; set; }
}

public class TripResponseDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public List<CountryResponseDTO> Countries { get; set; }
    public List<ClientResponseDTO> Clients { get; set; }
}

public class CountryResponseDTO
{
    public string Name { get; set; }
}
public class ClientResponseDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}