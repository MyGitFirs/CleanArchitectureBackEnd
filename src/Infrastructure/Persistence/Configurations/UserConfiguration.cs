using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(t => t.UserNumber)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(t => t.FirstName).HasMaxLength(200);
            builder.Property(t => t.LastName).HasMaxLength(200);

            builder.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(t => t.Password)
                .IsRequired()
                .HasColumnType("nvarchar(max)"); 

            builder.Property(t => t.Role)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.RefreshToken)
                .HasMaxLength(500); 

            builder.HasIndex(t => t.Email).IsUnique();

            builder.HasOne<Company>()  
                .WithMany()              
                .HasForeignKey(t => t.CompanyID)  
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
