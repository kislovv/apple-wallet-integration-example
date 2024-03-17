using BL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public class AppleWalletPassEntityConfiguration : IEntityTypeConfiguration<AppleWalletPass>
{
    public void Configure(EntityTypeBuilder<AppleWalletPass> builder)
    {
        builder.HasKey(pass => pass.PassId);
    }
}