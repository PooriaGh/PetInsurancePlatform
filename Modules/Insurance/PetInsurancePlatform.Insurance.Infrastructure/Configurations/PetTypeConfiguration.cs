using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetInsurancePlatform.Insurance.Domain.Models;

namespace PetInsurancePlatform.Insurance.Infrastructure.Configurations;

internal class PetTypeConfiguration : IEntityTypeConfiguration<PetType>
{
    public void Configure(EntityTypeBuilder<PetType> builder)
    {
        builder
            .HasKey(pt => pt.Id);

        builder
            .Property(pt => pt.Id)
            .ValueGeneratedNever();

        builder
            .Ignore(pt => pt.Diseases);

        builder
            .ComplexProperty(pt => pt.AgeRange);

        builder
            .HasMany<Pet>()
            .WithOne(p => p.PetType)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany<PetTypeDisease>()
            .WithOne(pt => pt.PetType)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
