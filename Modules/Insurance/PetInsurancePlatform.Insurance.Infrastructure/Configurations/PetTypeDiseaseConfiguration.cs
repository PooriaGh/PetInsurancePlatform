using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetInsurancePlatform.Insurance.Domain.Models;

namespace PetInsurancePlatform.Insurance.Infrastructure.Configurations;

internal class PetTypeDiseaseConfiguration : IEntityTypeConfiguration<PetTypeDisease>
{
    public void Configure(EntityTypeBuilder<PetTypeDisease> builder)
    {
        builder
            .HasKey(pt => pt.Id);

        builder
            .Property(pt => pt.Id)
            .ValueGeneratedNever();

        builder
            .HasIndex(ptd => new { ptd.PetType, ptd.Disease })
            .IsUnique();

        builder
            .HasOne(ptd => ptd.PetType)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(ptd => ptd.Disease)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
