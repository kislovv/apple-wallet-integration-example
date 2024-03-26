using BL.Abstractions;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;

public static class DbRegistrationExt
{
    public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddScoped<ICardRepository, CardRepository>();
        serviceCollection.AddScoped<IParticipantRepository, ParticipantRepository>();
        serviceCollection.AddScoped<IPartnerRepository, PartnerRepository>();
        serviceCollection.AddScoped<IPassRepository, PassRepository>();
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

        var connectionString = configuration["Database:ConnectionString"];
        
        return serviceCollection.AddDbContext<AppDbContext>(builder =>
        {
            builder.UseNpgsql(connectionString);
            builder.UseSnakeCaseNamingConvention();
        });
        
        
        
    }
}