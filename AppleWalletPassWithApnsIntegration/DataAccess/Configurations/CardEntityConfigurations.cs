using BL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public class CardEntityConfigurations : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.HasOne<AppleWalletPass>(c => c.AppleWalletPass)
            .WithOne(p => p.Card)
            .HasForeignKey<AppleWalletPass>(p => p.CardId);
    }
}