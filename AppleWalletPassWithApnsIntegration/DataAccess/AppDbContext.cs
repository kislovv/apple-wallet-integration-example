using BL.Entities;
using DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Participant> Participants => Set<Participant>();
    
    public DbSet<Card> Cards => Set<Card>();
    public DbSet<AppleWalletPass> AppleWalletPasses => Set<AppleWalletPass>();
    public DbSet<AppleDevice> AppleDevices => Set<AppleDevice>();
    
    public DbSet<Partner> Partners => Set<Partner>();
    public DbSet<AppleWalletPartnerSpecific> AppleWalletPartnerSpecifics => Set<AppleWalletPartnerSpecific>();
    public DbSet<AppleAssociatedStoreApp> AppleAssociatedStoreApps => Set<AppleAssociatedStoreApp>();

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ParticipantEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CardEntityConfigurations());
        modelBuilder.ApplyConfiguration(new PartnerEntityConfiguration());
        modelBuilder.ApplyConfiguration(new AppleWalletPassEntityConfiguration());
        
        SeedingStartedPartner(modelBuilder);
        SeedingStartedPartnerSpecific(modelBuilder);
        SeedingStartedAssociatedStoreApp(modelBuilder);
        
        base.OnModelCreating(modelBuilder);
    }


    private void SeedingStartedPartner(ModelBuilder modelBuilder)
    {
        var partner = new Partner
        {
            Id = 1,
            Name = "Лукоил"
        };
        modelBuilder.Entity<Partner>().HasData(partner);
    }

    private void SeedingStartedPartnerSpecific(ModelBuilder modelBuilder)
    {
        var partnerSpecific = new AppleWalletPartnerSpecific
        {
            Id = 1,
            PartnerId = 1,
            BackgroundColor = "#5bd1e1",
            IconPath = "Intens APP Icon 1x.png",
            LogoPath = "Intens APP Icon 1x.png",
            StripPath = "Intens.png",
            Description = "Интенс APP"
        };
        
        modelBuilder.Entity<AppleWalletPartnerSpecific>().HasData(partnerSpecific);
    }
    
    private void SeedingStartedAssociatedStoreApp(ModelBuilder modelBuilder)
    {
        var appStore = new AppleAssociatedStoreApp
        {
            Id = 1,
            Name = "Интенс APP",
            AppleWalletPartnerSpecificId = 1
        };

        modelBuilder.Entity<AppleAssociatedStoreApp>().HasData(appStore);
    }
}