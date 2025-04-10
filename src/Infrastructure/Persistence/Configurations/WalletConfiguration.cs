using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.Property(w => w.Balance)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(w => w.MonthlyLimit)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(w => w.LastResetDate)
                .IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(w => w.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}