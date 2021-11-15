using Ardalis.Specification;

namespace RepairMarketPlace.ApplicationCore.Interfaces.Repository
{
    public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
    {
    }
}
