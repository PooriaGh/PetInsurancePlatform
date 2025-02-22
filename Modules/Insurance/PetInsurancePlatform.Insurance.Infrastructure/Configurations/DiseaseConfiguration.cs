using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetInsurancePlatform.Insurance.Domain.Models;

namespace PetInsurancePlatform.Insurance.Infrastructure.Configurations;

internal class DiseaseConfiguration : IEntityTypeConfiguration<Disease>
{
    public void Configure(EntityTypeBuilder<Disease> builder)
    {
        builder
            .HasKey(d => d.Id);

        builder
            .Property(d => d.Id)
            .ValueGeneratedNever();

        builder
           .Ignore(d => d.PetTypes);

        builder
            .HasMany<PetTypeDisease>()
            .WithOne(ptd => ptd.Disease)
            .OnDelete(DeleteBehavior.Restrict);

        builder
          .HasOne<Pet>()
          .WithMany(p => p.Diseases)
          .OnDelete(DeleteBehavior.Restrict);
    }
}
