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
        
        return serviceCollection.AddDbContext<AppDbContext>(builder =>
        {
            builder.UseNpgsql(configuration["Database:ConnectionString"]);
            builder.UseSnakeCaseNamingConvention();
        });
    }
}