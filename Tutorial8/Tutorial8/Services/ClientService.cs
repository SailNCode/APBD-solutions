using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class ClientService : IClientService
{
    private readonly string _connectionString =
        "Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;User ID=s28762;Password=oracle12;TrustServerCertificate=True;";

    public async Task<bool> IsClientPresent(int clientId)
    {
        await using var con = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand("Select * FROM Client WHERE IdClient = @clientId", con);

        cmd.Parameters.AddWithValue("@clientId", clientId);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        return await reader.ReadAsync();
    }

    public async Task<bool> AddClient(ClientDTO clientDTO)
    {
        await using var con = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand("INSERT INTO CLIENT VALUES (@FirstName, @LastName, @Email, @Telephone, @Pesel);", con);

        cmd.Parameters.AddWithValue("@FirstName", clientDTO.FirstName);
        cmd.Parameters.AddWithValue("@LastName", clientDTO.LastName);
        cmd.Parameters.AddWithValue("@Email", clientDTO.Email);
        cmd.Parameters.AddWithValue("@Telephone", clientDTO.Telephone);
        cmd.Parameters.AddWithValue("@Pesel", clientDTO.Pesel);

        await con.OpenAsync();
        return await cmd.ExecuteNonQueryAsync() > 0;
    }
    

}