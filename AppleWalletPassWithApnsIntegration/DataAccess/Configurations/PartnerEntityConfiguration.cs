using BL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public class PartnerEntityConfiguration: IEntityTypeConfiguration<Partner>
{
    public void Configure(EntityTypeBuilder<Partner> builder)
    {
        builder.HasOne<PartnerSpecific>(p => p.PartnerSpecific)
            .WithOne(specific => specific.Partner)
            .HasForeignKey<PartnerSpecific>(specific => specific.PartnerId);
    }
}