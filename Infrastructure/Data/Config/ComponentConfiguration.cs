using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepairMarketPlace.ApplicationCore.Entities;

namespace RepairMarketPlace.Infrastructure.Data.Config
{
    public class ComponentConfiguration : IEntityTypeConfiguration<Component>
    {
        public void Configure(EntityTypeBuilder<Component> builder)
        {
            builder.Property(x => x.Name).IsUnicode(false);
            builder.Property(x => x.Type).HasConversion<string>();
        }
    }
}
