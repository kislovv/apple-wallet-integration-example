namespace AppleWalletPassWithApnsIntegration.Endpoints;

public static class CardsEndpointsGroup
{
    public static void RegisterCardsEndpoints(this IEndpointRouteBuilder routes)
    {
        var cards = routes.MapGroup("Cards");

        cards.MapPost("add", () => { });
    }
}