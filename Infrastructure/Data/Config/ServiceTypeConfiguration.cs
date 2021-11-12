using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepairMarketPlace.ApplicationCore.Entities;

namespace RepairMarketPlace.Infrastructure.Data.Config
{
    public class ServiceTypeConfiguration : IEntityTypeConfiguration<ServiceType>
    {
        public void Configure(EntityTypeBuilder<ServiceType> builder)
        {
            builder.HasIndex(x => x.Name);
        }
    }
}
