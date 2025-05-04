using Microsoft.AspNetCore.Components.Sections;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class TripsService : ITripsService
{
    private readonly string _connectionString =
        "Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;User ID=s28762;Password=oracle12;TrustServerCertificate=True;";

    private ICountriesService countriesService;

    public TripsService(ICountriesService countriesService)
    {
        this.countriesService = countriesService;
    }
    public async Task<bool> IsTripPresent(int tripId)
    {
        await using var con = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand("Select * FROM Trip WHERE IdTrip = @tripId", con);

        cmd.Parameters.AddWithValue("@tripId", tripId);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        return await reader.ReadAsync();
    }
    public async Task<List<TripDTO>> GetTrips()
    {
        await using var con = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(@"SELECT * FROM Trip", con);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        List<TripDTO> trips = new List<TripDTO>();
        while (await reader.ReadAsync())
        {
            TripDTO trip = new TripDTO
            {
                Id = (int) reader["IdTrip"],
                Name = (string) reader["Name"],
                Description = reader["Description"] == DBNull.Value ? null : (string) reader["Description"],
                DateFrom = (DateTime) reader["DateFrom"],
                DateTo = (DateTime) reader["DateTo"],
                MaxPeople = (int) reader["MaxPeople"],
            };
            List<CountryDTO> countries = await countriesService.GetCountriesByTripId(trip.Id);
            trip.Countries = countries.Select(country => country.Name).ToList();
            trips.Add(trip);
        }
        return trips;
    }

    public async Task<List<TripReservationDTO>> GetTripReservationsByClientId(int clientId)
    {
        await using var con = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(
            @"SELECT *
            FROM Trip
            JOIN Client_Trip ct ON Trip.IdTrip = ct.IdTrip
            WHERE ct.IdClient = @clientId",
            con);
        cmd.Parameters.AddWithValue("@clientId", clientId);
        
        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        List<TripReservationDTO> tripReservations = new List<TripReservationDTO>();
        while (await reader.ReadAsync())
        {
            TripReservationDTO tripReservation = new TripReservationDTO()
            {
                Id = (int) reader["IdTrip"],
                Name = (string) reader["Name"],
                Description = (string) reader["Description"],
                DateFrom = (DateTime) reader["DateFrom"],
                DateTo = (DateTime) reader["DateTo"],
                MaxPeople = (int) reader["MaxPeople"],
                RegisteredAt = (int) reader["RegisteredAt"],
                PaymentDate = reader["PaymentDate"] == DBNull.Value ? null : (int) reader["PaymentDate"]
            };
            List<CountryDTO> countries = await countriesService.GetCountriesByTripId(tripReservation.Id);
            tripReservation.Countries = countries.Select(country => country.Name).ToList();
            tripReservations.Add(tripReservation);
        }

        return tripReservations;
    }

    public async Task<bool> IsTripFull(int tripId)
    {
        await using var con = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand("Select MaxPeople FROM Trip WHERE IdTrip = @tripId", con);

        cmd.Parameters.AddWithValue("@tripId", tripId);

        await con.OpenAsync();
        int maxCount = (int) await cmd.ExecuteScalarAsync();
        
        await using var con2 = new SqlConnection(_connectionString);
        await using var cmd2 = new SqlCommand("Select Count(idClient) from Client_Trip WHERE IdTrip = @tripId", con);

        cmd2.Parameters.AddWithValue("@tripId", tripId);

        await con2.OpenAsync();
        int occupiedCount = (int) await cmd.ExecuteScalarAsync();

        return occupiedCount < maxCount;
    }

    public async Task<bool> IsClientRegisteredForTrip(int clientId, int tripId)
    {
        await using var con = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand("Select * FROM Client_Trip WHERE IdClient = @clientId AND IdTrip = @tripId;", con); 
        
        cmd.Parameters.AddWithValue("@clientId", clientId);
        cmd.Parameters.AddWithValue("@tripId", tripId);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        return await reader.ReadAsync();
    }

    public async Task<bool> RegisterClientForTrip(int clientId, int tripId)
    {
        await using var con = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand("INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt) VALUES (@IdClient, @IdTrip, @RegisteredAt)", con);

        cmd.Parameters.AddWithValue("@IdClient", clientId);
        cmd.Parameters.AddWithValue("@IdTrip", tripId);
        
        cmd.Parameters.AddWithValue("@RegisteredAt", DateTime.Now.ToString("yyyyMMdd"));

        await con.OpenAsync();
        int result;
        try
        {
            result = await cmd.ExecuteNonQueryAsync();
        }
        catch (SqlException e)
        {
            return false;
        }

        return result > 0;
    }

    public async Task<bool> RemoveClientFromTrip(int clientId, int tripId)
    {
        await using var con = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand("DELETE FROM Client_Trip WHERE IdClient = @clientId AND IdTrip = @tripId;", con);

        cmd.Parameters.AddWithValue("@clientId", clientId);
        cmd.Parameters.AddWithValue("@tripId", tripId);

        await con.OpenAsync();
        int result;
        try
        {
            result = await cmd.ExecuteNonQueryAsync();
        }
        catch (SqlException e)
        {
            return false;
        }

        return result > 0;
    }
}