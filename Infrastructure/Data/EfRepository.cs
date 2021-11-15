using Ardalis.Specification.EntityFrameworkCore;
using RepairMarketPlace.ApplicationCore.Interfaces;
using RepairMarketPlace.ApplicationCore.Interfaces.Repository;

namespace RepairMarketPlace.Infrastructure.Data
{
    public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
    {
        public EfRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
