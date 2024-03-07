using BL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public class ParticipantEntityConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.ToTable("participants");

        builder.HasOne<Card>(p => p.Card)
            .WithOne(card => card.Participant)
            .HasForeignKey<Card>(c => c.ParticipantId);

        builder.Property(p => p.Balance)
            .HasColumnType("numeric(2)")
            .HasDefaultValue(0.00m);
    }
}