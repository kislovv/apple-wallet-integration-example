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
        
        base.OnModelCreating(modelBuilder);
    }
}