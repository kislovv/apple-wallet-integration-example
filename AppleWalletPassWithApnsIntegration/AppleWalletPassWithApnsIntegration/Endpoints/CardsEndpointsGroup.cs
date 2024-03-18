using AppleWalletPassWithApnsIntegration.Models;
using Microsoft.AspNetCore.Authorization;

namespace AppleWalletPassWithApnsIntegration.Endpoints;

public static class CardsEndpointsGroup
{
    public static void RegisterCardsEndpoints(this IEndpointRouteBuilder routes)
    {
        var cards = routes.MapGroup("Cards");

        cards.MapPost("add", [Authorize](CreateCardRequest request, HttpContent context) =>
        {
            
        });
    }
}