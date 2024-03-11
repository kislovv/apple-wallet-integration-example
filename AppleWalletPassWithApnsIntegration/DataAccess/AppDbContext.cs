using BL.Entities;
using DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Participant> Participants => Set<Participant>();
    
    public DbSet<Card> Cards => Set<Card>();
    public DbSet<Pass> Passes => Set<Pass>();
    public DbSet<Device> Devices => Set<Device>();
    
    public DbSet<Partner> Partners => Set<Partner>();
    public DbSet<PartnerSpecific> PartnerSpecifics => Set<PartnerSpecific>();
    public DbSet<AssociatedStoreApp> AssociatedStoreApps => Set<AssociatedStoreApp>();

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ParticipantEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CardEntityConfigurations());
        modelBuilder.ApplyConfiguration(new PartnerEntityConfiguration());
        
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
        var partnerSpecific = new PartnerSpecific
        {
            Id = 1,
            PartnerId = 1,
            BackgroundColor = "#5bd1e1",
            IconPath = "Intens APP Icon 1x.png",
            LogoPath = "Intens APP Icon 1x.png",
            StripPath = "Intens.png",
            Description = "Интенс APP"
        };
        
        modelBuilder.Entity<PartnerSpecific>().HasData(partnerSpecific);
    }
    
    private void SeedingStartedAssociatedStoreApp(ModelBuilder modelBuilder)
    {
        var appStore = new AssociatedStoreApp
        {
            Id = 1,
            Name = "Интенс APP",
            PartnerSpecificId = 1
        };

        modelBuilder.Entity<AssociatedStoreApp>().HasData(appStore);
    }
}