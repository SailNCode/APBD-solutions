using Tutorial12.Data;

namespace Tutorial12;

public class TransactionHandler
{
    private readonly DbContext _context;

    public TransactionHandler(DbContext context)
    {
        this._context = context;
    }

    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}