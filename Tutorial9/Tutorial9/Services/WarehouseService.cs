using Microsoft.AspNetCore.Components.Sections;
using Tutorial9.Model_DTOs;
using Tutorial9.Exceptions;
using Tutorial9.Repositories;

namespace Tutorial9.Services;


public class WarehouseService : IWarehouseService
{
    private IWarehouseRepository _warehouseRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<int> FulfillOrderRequest(FulfillOrderRequest fulfillRequest)
    {
        if (fulfillRequest.Amount <= 0)
        {
            throw new BadRequestException("Amount should be greater than 0");
        }
        if (! await _warehouseRepository.IsProductPresent(fulfillRequest.IdProduct))
        {
            throw new NotFoundException("IdProduct not found");
        }
        if (! await _warehouseRepository.IsWarehousePresent(fulfillRequest.IdWarehouse))
        {
            throw new NotFoundException("IdWarehouse not found");
        }
        
        int orderId = await _warehouseRepository.GetOrderId(fulfillRequest.IdProduct, fulfillRequest.Amount);

        if (!await _warehouseRepository.CheckDatesIntegrity(orderId, fulfillRequest.CreatedAt))
        {
            throw new ConflictException("Date of order request is not later than its creation");
        }
        int foreignKey = -1;
        if (await _warehouseRepository.IsOrderFulfilled(orderId))
        {
            throw new BadRequestException("Order has been already fulfilled");
        }
        //Initiating transaction
        try
        {
            await _warehouseRepository.StartTransactionAsync();

            await _warehouseRepository.FulfillOrderAtCurrentDate(orderId);
            //price z Product, Amount (mamy)
            Product product = await _warehouseRepository.GetProduct(fulfillRequest.IdProduct);
            //Console.WriteLine(product.IdProduct + " " + product.Description + " " + product.Name + " " + product.Price);
            decimal totalPrice = product.Price * fulfillRequest.Amount;

            foreignKey = await _warehouseRepository.AddProduct_WarehouseRecord(fulfillRequest.IdWarehouse,
                fulfillRequest.IdProduct,
                orderId, fulfillRequest.Amount, totalPrice);
        }
        catch (ConflictException e)
        {
            await _warehouseRepository.RollbackTransactionAsync();
            throw e;
        }
        catch (InternalServerException e)
        {
            await _warehouseRepository.RollbackTransactionAsync();
            throw e;
        }
        catch (Exception e)
        {
            await _warehouseRepository.RollbackTransactionAsync();
            throw new InternalServerException(e.Message);
        }
        await _warehouseRepository.CommitTransactionAsync();
        return foreignKey;
    }
    
}