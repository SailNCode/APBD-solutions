using System.Transactions;
using Tutorial12.DTOs;
using Tutorial12.Models;
using Tutorial9.Exceptions;

namespace Tutorial12.Services;

public class Service: IService
{
    private TransactionHandler _transactionHandler;
    private ITripsRepository _tripsRepository;
    private IClientsRepository _clientsRepository;

    public Service(ITripsRepository tripsRepository, IClientsRepository clientsRepository, TransactionHandler transactionHandler)
    {
        _tripsRepository = tripsRepository;
        _clientsRepository = clientsRepository;
        _transactionHandler = transactionHandler;
    }   

    public async Task<TripsResponseDTO> GetTripsInfo(int? pageNum, int? pageSize)
    {
        List<Trip> trips = await _tripsRepository.GetTrips();
        trips = trips.OrderByDescending(trip => trip.DateFrom).ToList();
        if (pageNum.HasValue && pageSize.HasValue)
        {
            trips = trips.Skip((pageNum.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
        }
        TripsResponseDTO tripsResponseDto = new TripsResponseDTO()
        {
            Trips = trips.Select(trip => new TripResponseDTO()
            {
                Name = trip.Name,
                Description = trip.Description,
                DateFrom = trip.DateFrom,
                DateTo = trip.DateTo,
                MaxPeople = trip.MaxPeople,
                Countries = trip.IdCountries.Select(country => new CountryResponseDTO()
                {
                    Name = country.Name
                }).ToList(),
                Clients = trip.ClientTrips.Select(clientTrip => new ClientResponseDTO()
                {
                    FirstName = clientTrip.IdClientNavigation.FirstName,
                    LastName = clientTrip.IdClientNavigation.LastName,
                }).ToList()
            }).ToList()
        };
        if (pageNum.HasValue && pageSize.HasValue)
        {
            tripsResponseDto.pageNum = pageNum.Value;
            tripsResponseDto.pageSize = pageSize.Value;
            tripsResponseDto.allPages = (int)Math.Ceiling((double)trips.Count() / pageSize.Value);
        }

        return tripsResponseDto;
    }

    public async Task DeleteClient(int idClient)
    {
        if (!await _clientsRepository.HasClient(idClient))
        {
            throw new NotFoundException("Client with such id does not exist");
        }

        Client client = await _clientsRepository.GetClient(idClient);
        Console.WriteLine(client.ClientTrips.Count);
        if (client.ClientTrips.Count > 0)
        {
            throw new BadRequestException("Client is assigned to trip");
        }

        _clientsRepository.Delete(client);
        await _transactionHandler.SaveChangesAsync();
    }

    public async Task RegisterClientForTrip(AssignClientTripRequestDTO requestDto, int idTrip, DateTime timestamp)
    {
        if (await _clientsRepository.GetClient(requestDto.Pesel) != null)
        {
            throw new BadRequestException("Client with such Pesel already exists");
        }
        // Sprawdzanie 2. warunku nie ma sensu, bo pierwszy jest bardziej generalny, tym samymy zawsze go wyłapie

        Trip? trip = await _tripsRepository.GetTrip(idTrip);
        if (trip == null || trip.DateFrom <= DateTime.Today)
        {
            throw new NotFoundException("Trip does not exists or already started/finished");
        }
        if (requestDto.IdTrip != idTrip)
        {
            throw new BadRequestException("IdTrip from path does not match the one in request");
        }

        if (requestDto.TripName != trip.Name)
        {
            throw new BadRequestException("Trip name of the trip with given id does not match the one in request");
        }

        try
        {
            await _transactionHandler.BeginTransactionAsync();
            Client client = new Client()
            {
                FirstName = requestDto.FirstName,
                LastName = requestDto.LastName,
                Email = requestDto.Email,
                Telephone = requestDto.Telephone,
                Pesel = requestDto.Pesel
            };
            await _clientsRepository.AddClient(client);

            await _transactionHandler.SaveChangesAsync();

            ClientTrip clientTrip = new ClientTrip()
            {
                IdClient = client.IdClient,
                IdTrip = idTrip,
                RegisteredAt = timestamp,
                PaymentDate = requestDto.PaymentDate
            };
            await _tripsRepository.AddClientTrip(clientTrip);
            await _transactionHandler.SaveChangesAsync();
            await _transactionHandler.CommitTransactionAsync();
        }
        catch
        {
            Console.WriteLine("Error while registering client for trip");
            await _transactionHandler.RollbackTransactionAsync();
            throw new InternalServerException("Error while registering client for trip");
        }
    }

}