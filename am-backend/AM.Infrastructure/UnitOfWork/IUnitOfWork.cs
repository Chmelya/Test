namespace AM.Infrastructure.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    void BeginTransaction();
    void Commit();
    void Rollback();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
