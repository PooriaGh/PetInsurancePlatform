using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetInsurancePlatform.Insurance.Domain.Models;

namespace PetInsurancePlatform.Insurance.Infrastructure.Configurations;

internal class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .ValueGeneratedNever();

        builder
           .Ignore(p => p.HasDiseases);

        builder
            .ComplexProperty(p => p.Appearances);

        builder
            .ComplexProperty(p => p.BirthCertificatesPages);

        builder
            .ComplexProperty(p => p.FrontView);

        builder
            .ComplexProperty(p => p.RearView);

        builder
            .ComplexProperty(p => p.RightSideView);

        builder
            .ComplexProperty(p => p.LeftSideView);

        builder
            .ComplexProperty(p => p.HealthCertificate);

        builder
            .HasMany(p => p.Diseases)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
