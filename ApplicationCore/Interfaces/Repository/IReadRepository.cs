using Ardalis.Specification;

namespace RepairMarketPlace.ApplicationCore.Interfaces.Repository
{
    public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
    {
    }
}
