using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class KioskConfiguration : IEntityTypeConfiguration<Kiosk>
    {
        public void Configure(EntityTypeBuilder<Kiosk> builder)
        {
            builder.Property(k => k.KioskName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(k => k.Category)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(k => k.Status)
                .IsRequired();

            builder.HasOne<Company>()
                .WithMany()
                .HasForeignKey(k => k.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
