using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class CountriesService : ICountriesService

{
    private readonly string _connectionString =
        "Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;User ID=s28762;Password=oracle12;TrustServerCertificate=True;";
    public async Task<List<CountryDTO>> GetCountriesByTripId(int tripId)
    {
        await using var con = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(
            @"SELECT *
                FROM Country
                JOIN Country_Trip ON Country.IdCountry = Country_Trip.IdCountry
                WHERE Country_Trip.IdTrip = @TripId", con);

        cmd.Parameters.AddWithValue("@TripId", tripId);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        List<CountryDTO> countries = new List<CountryDTO>();
        while (await reader.ReadAsync())
        {
            CountryDTO country = new CountryDTO
            {
                Id = (int)reader["IdCountry"],
                Name = (string)reader["Name"]
            };
            countries.Add(country);
        }

        return countries;
    }
}