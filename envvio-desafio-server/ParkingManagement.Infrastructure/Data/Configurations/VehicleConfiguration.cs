using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Infrastructure.Data.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Plate)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(v => v.Plate)
            .IsUnique();

        builder.Property(v => v.Model)
            .HasMaxLength(100);

        builder.Property(v => v.Color)
            .HasMaxLength(50);

        builder.Property(v => v.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(v => v.CreatedAt)
            .IsRequired();
    }
}
