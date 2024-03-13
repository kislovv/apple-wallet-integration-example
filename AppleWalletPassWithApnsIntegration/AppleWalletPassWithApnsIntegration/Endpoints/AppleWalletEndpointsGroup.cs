using AppleWalletPassWithApnsIntegration.Filters;
using AppleWalletPassWithApnsIntegration.Models;
using AutoMapper;
using BL.Abstractions;
using BL.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AppleWalletPassWithApnsIntegration.Endpoints;

public static class AppleWalletEndpointsGroup
{
    public static void RegisterAppleWalletEndpoints(this IEndpointRouteBuilder routes)
    {
        var apple = routes.MapGroup("/apple");

        var appleWallet = apple.MapGroup("wallet").AddEndpointFilter<AppleWalletEndpointFilter>();
        
        apple.MapPost("/passes/create", async (
                [FromServices]IPassService passService,
                [FromServices] IMapper mapper, 
                [FromBody] PassRequest passRequest) =>
        {
            var result = await passService.CreatePass(mapper.Map<PassDto>(passRequest));
    
            //TODO: Подумать над названием файла (возможно id или имя participant + pass)
            return Results.File(result, "application/vnd.apple.pkpasses", "tickets.pkpass");
        }).WithTags("Create apple pass")
            .WithDescription("Создание apple wallet pass")
            .Produces<FileContentHttpResult>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Метод создания pass при первичной регистрации");

        appleWallet.MapPost("/v1/devices/{deviceId}/registrations/{passTypeId}/{serialNumber}",
            async (
                 string deviceId,
                 string passTypeId, 
                 string serialNumber,
                [FromBody] RegistrationPassRequest request) =>
        {
            return Results.Created();
        });
        
        
        appleWallet.MapDelete("/v1/devices/{deviceId}/registrations/{passTypeId}/{serialNumber}",
            async (string deviceId, string passTypeId, string serialNumber) =>
        {
            return Results.Ok();
        }).AddEndpointFilter<AppleWalletEndpointFilter>();
        
        appleWallet.MapGet("/v1/devices/{deviceId}/registrations/{passTypeId}",
            async (string deviceId, string passTypeId,
                [FromQuery] string passesUpdatedSince) =>
        {
            return Results.Ok(new ListLastUpdatedPassesResponse());
        });
        
        
        appleWallet.MapGet("/v1/passes/{passTypeIdentifier}/{serialNumber}",
            async (string passTypeIdentifier, string serialNumber, 
                [FromServices] IPassService passService,[FromServices] IMapper mapper) =>
            {
                //TODO: Добавить метод update 
                var result = await passService.CreatePass(mapper.Map<PassDto>(serialNumber));
    
                //TODO: Подумать над названием файла (возможно id или имя participant + pass)
                return Results.File(result, "application/vnd.apple.pkpasses", "tickets.pkpass");
            });

        appleWallet.WithTags("Apple Wallet");
    }
}