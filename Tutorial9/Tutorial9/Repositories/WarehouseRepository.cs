using System.Data.Common;
using Microsoft.Data.SqlClient;
using Tutorial9.Model_DTOs;
using Tutorial9.Exceptions;
using Tutorial9.Services;

namespace Tutorial9.Repositories;

public class WarehouseRepository: TransactionHandler, IWarehouseRepository
{

    public WarehouseRepository(IConfiguration configuration, TransactionHandler transactionHandler): base(configuration)
    {
    }

    public async Task<bool> IsProductPresent(int productId)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var cmd = new SqlCommand("Select 1 FROM Product WHERE IdProduct = @productId", con);

        cmd.Parameters.AddWithValue("@productId", productId);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        return await reader.ReadAsync();
    }
    public async Task<bool> IsWarehousePresent(int warehouseId)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var cmd = new SqlCommand("Select 1 FROM Warehouse WHERE IdWarehouse = @warehouseId", con);

        cmd.Parameters.AddWithValue("@warehouseId", warehouseId);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        return await reader.ReadAsync();
    }

    public async Task<bool> IsOrderPresent(int productId, int amount)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var cmd = new SqlCommand("Select 1 FROM [Order] WHERE IdProduct = @productId AND Amount = @amount", con);

        cmd.Parameters.AddWithValue("@productId", productId);
        cmd.Parameters.AddWithValue("@amount", amount);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        return await reader.ReadAsync();
    }

    public async Task<int> GetOrderId(int productId, int amount)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var cmd = new SqlCommand("Select IdOrder FROM [Order] WHERE IdProduct = @productId AND Amount = @amount", con);

        cmd.Parameters.AddWithValue("@productId", productId);
        cmd.Parameters.AddWithValue("@amount", amount);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return (int)reader["IdOrder"];
        }
        throw new NotFoundException("There is not such order with specific id and amount");
    }

    public async Task<bool> IsOrderFulfilled(int orderId)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var cmd = new SqlCommand("Select 1 FROM [Product_Warehouse] WHERE IdOrder = @orderId", con);

        cmd.Parameters.AddWithValue("@orderId", orderId);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        return await reader.ReadAsync();
    }

    public async Task FulfillOrderAtCurrentDate(int orderId)
    {
        if (!await isFulfilledAtDateTimeNull(orderId))
        {
            throw new ConflictException("Order has already 'FulfilledAt' date");
        }
        
        await using var cmd = new SqlCommand("UPDATE [Order] SET FulfilledAt = GETDATE() WHERE IdOrder = @orderId", TransConnection);
        cmd.Transaction = Transaction as SqlTransaction;
        cmd.Parameters.AddWithValue("@orderId", orderId);

        int nModified = await cmd.ExecuteNonQueryAsync();
        if (nModified != 1)
        {
            throw new InternalServerException("'FulfilledAt' date hasn't been added");
        }

    }

    private async Task<bool> isFulfilledAtDateTimeNull(int orderId)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var cmd = new SqlCommand("Select FulfilledAt FROM [Order] WHERE IdOrder = @orderId", con);

        cmd.Parameters.AddWithValue("@orderId", orderId);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return reader["FulfilledAt"] == DBNull.Value;
        }

        return true;
    }

    public async Task<Product> GetProduct(int productId)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var cmd = new SqlCommand("Select * FROM [Product] WHERE IdProduct = @productId", con);

        cmd.Parameters.AddWithValue("@productId", productId);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        await reader.ReadAsync();
       
        return new Product()
        {
            IdProduct = (int)reader["IdProduct"],
            Name = (string)reader["Name"],
            Description = (string)reader["Description"],
            Price = (decimal)reader["Price"]
        };
    }

    public async Task<int> AddProduct_WarehouseRecord(int warehouseId, int productId, int orderId, int amount, decimal price)
    {
        await using var insComm = new SqlCommand(@"INSERT INTO [Product_Warehouse] VALUES (@warehouseId, @productId, @orderId, @amount, @price, GETDATE()); SELECT SCOPE_IDENTITY()", TransConnection);
        insComm.Transaction = Transaction as SqlTransaction;
        insComm.Parameters.AddWithValue("@warehouseId", warehouseId);
        insComm.Parameters.AddWithValue("@productId", productId);
        insComm.Parameters.AddWithValue("@orderId", orderId);
        insComm.Parameters.AddWithValue("@amount", amount);
        insComm.Parameters.AddWithValue("@price", price);
        
        var result = await insComm.ExecuteScalarAsync();

        if (result == DBNull.Value)
            throw new InternalServerException("'Product_Warehouse' identity not returned");

        return Convert.ToInt32(result);
    }

    public async Task<bool> CheckDatesIntegrity(int orderId, DateTime requestDateTime)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var cmd = new SqlCommand("Select CreatedAt FROM [Order] WHERE IdOrder = @orderId", con);

        cmd.Parameters.AddWithValue("@orderId", orderId);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            var result = reader["CreatedAt"];
            if (result == DBNull.Value)
            {
                return false;
            }

            return ((DateTime)result).CompareTo(requestDateTime) < 0;

        }

        return false;
    }

}