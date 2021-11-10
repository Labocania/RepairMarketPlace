using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepairMarketPlace.ApplicationCore.Entities;

namespace RepairMarketPlace.Infrastructure.Data.Config
{
    public class ComponentTypeConfiguration : IEntityTypeConfiguration<ComponentType>
    {
        public void Configure(EntityTypeBuilder<ComponentType> builder)
        {
            builder.Property(x => x.Name).HasConversion<string>();
        }
    }
}
