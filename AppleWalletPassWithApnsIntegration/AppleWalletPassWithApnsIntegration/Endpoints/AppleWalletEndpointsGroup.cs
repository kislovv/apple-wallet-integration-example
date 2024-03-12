using AppleWalletPassWithApnsIntegration.Filters;
using AppleWalletPassWithApnsIntegration.Models;
using AutoMapper;
using BL.Abstractions;
using BL.Dtos;

namespace AppleWalletPassWithApnsIntegration.Endpoints;

public static class AppleWalletEndpointsGroup
{
    public static void RegisterAppleWalletEndpoints(this IEndpointRouteBuilder routes)
    {
        var appleWallet = routes.MapGroup("/apple/wallet");
        
        appleWallet.MapPost("/passes/create", async (IPassService passService, IMapper mapper, PassRequest passRequest) =>
        {
            var result = await passService.CreatePass(mapper.Map<PassDto>(passRequest));
    
            //TODO: Подумать над названием файла (возможно id или имя participant + pass)
            return Results.File(result, "application/vnd.apple.pkpasses", "tickets.pkpass");
        });

        appleWallet.MapPost("/v1/devices/{deviceId}/registrations/{passTypeId}/{serialNumber}",
            async (string deviceId, string passTypeId, string serialNumber, RegistrationPassRequest request) =>
        {
            return Results.Created();
        }).AddEndpointFilter<AppleWalletEndpointFilter>();
        
        
        appleWallet.MapDelete("/v1/devices/{deviceId}/registrations/{passTypeId}/{serialNumber}",
            async (string deviceId, string passTypeId, string serialNumber, RegistrationPassRequest request) =>
        {
            return Results.Ok();
        }).AddEndpointFilter<AppleWalletEndpointFilter>();
        
        appleWallet.MapGet("/v1/devices/{deviceId}/registrations/{passTypeId}",
            async (string deviceId, string passTypeId, string passesUpdatedSince) =>
        {
            return Results.Ok(new ListLastUpdatedPassesResponse());
        }).AddEndpointFilter<AppleWalletEndpointFilter>();
        
        
        appleWallet.MapGet("/v1/passes/{passTypeIdentifier}/{serialNumber}",
            async (string passTypeIdentifier, string serialNumber, IPassService passService, IMapper mapper) =>
            {
                //TODO: Добавить метод update 
                var result = await passService.CreatePass(mapper.Map<PassDto>(serialNumber));
    
                //TODO: Подумать над названием файла (возможно id или имя participant + pass)
                return Results.File(result, "application/vnd.apple.pkpasses", "tickets.pkpass");
            }).AddEndpointFilter<AppleWalletEndpointFilter>();

        appleWallet.WithTags("Apple Wallet");
    }
}