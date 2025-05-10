using Tutorial9.Model_DTOs;

namespace Tutorial9.Repositories;

public interface IWarehouseRepository: ITransactional
{
    Task<bool> IsProductPresent(int productId);
    Task<bool> IsWarehousePresent(int warehouseId);
    Task<bool> IsOrderPresent(int productId, int amount);
    Task<int> GetOrderId(int productId, int amount);
    Task<bool> IsOrderFulfilled(int orderId);
    Task FulfillOrderAtCurrentDate(int orderId);
    Task<Product> GetProduct(int productId);
    Task<int> AddProduct_WarehouseRecord(int warehouseId, int productId, int orderId, int amount, decimal price);
    Task<bool> CheckDatesIntegrity(int orderId, DateTime requestDateTime);
}