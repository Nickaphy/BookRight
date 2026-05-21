//Fluent API

using BookRight.Domain.Entities.Treatments;
using BookRight.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookRight.Infrastructure.Persistence.Configurations;

/*public class TreatmentConfiguration : IEntityTypeConfiguration<TreatmentType>
{
    public void Configure(EntityTypeBuilder<TreatmentType> builder)
    {
        //Specifices primary key
        builder.HasKey(t => t.Id);

        //Maps "Name" to a collumn sets it non nullable
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        //Same thing as above
        builder.Property(t => t.Duration)
            .IsRequired();

        //Same thing as above but instead of enum and numeric value it stores it as a word
        builder.Property(t => t.NeedsAuthorisation)
            .IsRequired()
            .HasConversion<string>();

        //Same thing as 2 above
        builder.Property(t => t.MaxParticipants)
            .IsRequired();

        // Money is a value object — so it doesnt have its own table, instead we are pushing it onto treatments.
        builder.OwnsOne(t => t.BasePrice, money =>
        {
            money.Property(m => m.Amount)
                .IsRequired();
        });
    }*/
//}
