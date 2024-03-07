using BL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public class CardEntityConfigurations : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.HasOne<Pass>(c => c.Pass)
            .WithOne(p => p.Card)
            .HasForeignKey<Pass>(p => p.CardId);
    }
}