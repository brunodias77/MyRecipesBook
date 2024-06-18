namespace MRB.Domain.Repositories;

public interface IUnitOfWork
{
    Task<int> CompleteAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
}