using BL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public class PartnerEntityConfiguration: IEntityTypeConfiguration<Partner>
{
    public void Configure(EntityTypeBuilder<Partner> builder)
    {
        builder.HasOne<AppleWalletPartnerSpecific>(p => p.PartnerSpecific)
            .WithOne(specific => specific.Partner)
            .HasForeignKey<AppleWalletPartnerSpecific>(specific => specific.PartnerId);
    }
}