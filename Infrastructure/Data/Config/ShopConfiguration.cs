using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepairMarketPlace.ApplicationCore.Entities;

namespace RepairMarketPlace.Infrastructure.Data.Config
{
    public class ShopConfiguration : IEntityTypeConfiguration<Shop>
    {
        public void Configure(EntityTypeBuilder<Shop> builder)
        {
            builder.HasIndex(x => x.Name);
        }
    }
}
