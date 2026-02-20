using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Infrastructure.Data.Configurations;

public class ParkingSessionConfiguration : IEntityTypeConfiguration<ParkingSession>
{
    public void Configure(EntityTypeBuilder<ParkingSession> builder)
    {
        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.VehicleId)
            .IsRequired();

        builder.Property(ps => ps.EntryTime)
            .IsRequired();

        builder.Property(ps => ps.AmountCharged)
            .HasColumnType("decimal(18,2)");

        builder.Property(ps => ps.CreatedAt)
            .IsRequired();

        builder.HasOne(ps => ps.Vehicle)
            .WithMany()
            .HasForeignKey(ps => ps.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
