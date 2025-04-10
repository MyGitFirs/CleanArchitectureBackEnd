using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(c => c.CompanyCode)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(c => c.CompanyName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(c => c.BusinessCategory)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(c => c.CompanyCode).IsUnique();
        }
    }
}