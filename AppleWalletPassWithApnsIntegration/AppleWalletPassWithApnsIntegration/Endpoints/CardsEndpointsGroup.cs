using AppleWalletPassWithApnsIntegration.Models;
using AutoMapper;
using BL.Abstractions;
using BL.Dtos;
using BL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppleWalletPassWithApnsIntegration.Endpoints;

internal static class CardsEndpointsGroup
{
    public static void RegisterCardsEndpoints(this IEndpointRouteBuilder routes)
    {
        var cards = routes.MapGroup("Cards");

        cards.MapPost("add", [Authorize] async (CreateCardRequest request, HttpContext context,
            [FromServices] ICardService cardService, [FromServices] IMapper mapper) =>
        {
            var userId = long.Parse(context.User.FindFirst("id")!.Value);
            var cardDto = mapper.Map<CardDto>(request);
            cardDto.ParticipantId = userId;

            var result = await cardService.CreateCard(cardDto);

            return Results.Ok(result);

        }).WithOpenApi(operation =>
        {
            operation.Summary = "Добавление карты участника";
            return operation;
        }).Produces<Card>();
    }
}