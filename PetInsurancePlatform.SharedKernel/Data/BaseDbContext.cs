using Microsoft.EntityFrameworkCore;

namespace PetInsurancePlatform.SharedKernel.Data;

public class BaseDbContext<TContext>
    : DbContext, IBaseDbContext where TContext : DbContext
{
    protected BaseDbContext(DbContextOptions<TContext> options) : base(options)
    {
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var currentTransaction = Database.CurrentTransaction;

        if (currentTransaction is not null)
        {
            return;
        }

        await Database
            .BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        var currentTransaction = Database.CurrentTransaction;

        if (currentTransaction is null)
        {
            return;
        }

        try
        {
            await currentTransaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await currentTransaction.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await currentTransaction.DisposeAsync();
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        var currentTransaction = Database.CurrentTransaction;

        if (currentTransaction is null)
        {
            return;
        }

        try
        {
            await currentTransaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await currentTransaction.DisposeAsync();
        }
    }
}
