using DevHours.CloudNative.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevHours.CloudNative.DataAccess.Configurations
{
    public class BookingsConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(r => r.Id);
        }
    }
}