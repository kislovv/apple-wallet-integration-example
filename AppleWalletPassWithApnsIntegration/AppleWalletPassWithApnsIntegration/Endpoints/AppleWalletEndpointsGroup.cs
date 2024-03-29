using AppleWalletPassWithApnsIntegration.Filters;
using AppleWalletPassWithApnsIntegration.Models;
using AutoMapper;
using BL.Abstractions;
using BL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AppleWalletPassWithApnsIntegration.Endpoints;

internal static class AppleWalletEndpointsGroup
{
    public static void RegisterAppleWalletEndpoints(this IEndpointRouteBuilder routes)
    {
        //inner apple methods
        var apple = routes.MapGroup("/apple");
        
        apple.MapPost("/passes/create",[Authorize(Roles = Roles.User)] async (
                [FromServices]IPassService passService,
                [FromServices] IMapper mapper, 
                [FromBody] PassRequest passRequest) =>
            {
                var result = await passService.CreatePass(mapper.Map<PassDto>(passRequest));
    
                //TODO: Подумать над названием файла (возможно id или имя participant + pass)
                return Results.File(result, "application/vnd.apple.pkpasses", "tickets.pkpass");
            
            }).WithTags("Create apple pass")
            .Produces<FileResult>(StatusCodes.Status201Created, "application/vnd.apple.pkpasses")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation =>
            {
                operation.Description = "Создание apple wallet pass";
                return operation;
            });
        
        apple.MapPut("/passes/update",[Authorize(Roles = Roles.User)] async (
                [FromServices]IPassService passService, [FromServices] IMapper mapper, 
                [FromBody] UpdatePassRequest passRequest, HttpContext context) =>
            {
                await passService.UpdatePass(new UpdatePassDto
                {
                    CardId = passRequest.CardId
                });
            
            }).WithTags("Update apple pass")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation =>
            {
                operation.Description = "Запрос на обновление apple pass. Отправляется push на APN сервер Apple и лишь затем обновление pass";
                return operation;
            });
        
        //Apple wallet integrations without authorization
        var appleWallet = apple.MapGroup("wallet").WithTags("Apple Wallet");;
        
        appleWallet.MapGet("/v1/devices/{deviceId}/registrations/{passTypeId}", 
            async (string deviceId, string passTypeId, HttpContext context,  
                [FromServices] IPassService passService) =>
            {
                if (context.Request.Query.IsNullOrEmpty())
                {
                    return Results.NoContent();
                }
                var lastUpdated = await passService.GetLastUpdatedPasses(deviceId, 
                    DateTimeOffset.Parse(context.Request.Query["previousLastUpdated"]!));
            
                return lastUpdated == null
                    ? Results.NoContent()
                    : Results.Ok(new ListLastUpdatedPassesResponse
                    {
                        SerialNumbers = lastUpdated.SerialNumbers,
                        LastUpdated = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString()
                    });
            });
        
        appleWallet.MapPost("/v1/log", (LogRequest logRequest, ILogger<Program> logger) =>
        {
            foreach (var log in logRequest.Logs)
            {
                logger.LogInformation(log);    
            }
            
            return Results.Ok();
        });

        appleWallet.MapPost("/v1/devices/{deviceId}/registrations/{passTypeId}/{serialNumber}",
        async (
             string deviceId,
             string passTypeId, 
             string serialNumber,
            [FromBody] RegistrationPassRequest request, [FromServices] IPassService passService) =>
        {
            await passService.RegisterPass(new RegisteredPassDto
            {
                PushToken = request.PushToken,
                passTypeId = passTypeId,
                DeviceId = deviceId,
                SerialNumber = serialNumber
            });
            
            return Results.Created();
        }).AddEndpointFilter<AppleWalletEndpointFilter>();;


        appleWallet.MapDelete("/v1/devices/{deviceId}/registrations/{passTypeId}/{serialNumber}",
            async (string deviceId, string passTypeId, string serialNumber,
                [FromServices] IPassService passService) =>
        {
            await passService.UnregisterPass(new UnregisterPassDto
            {
                passTypeId = passTypeId,
                DeviceId = deviceId,
                SerialNumber = serialNumber
            });
            
            return Results.Ok();
        }).AddEndpointFilter<AppleWalletEndpointFilter>();;
        
        appleWallet.MapGet("/v1/passes/{passTypeIdentifier}/{serialNumber}",
            async (string passTypeIdentifier, string serialNumber, 
                [FromServices] IPassService passService) =>
        {
            var result = await passService.GetUpdatedPass(serialNumber);

            //TODO: Подумать над названием файла (возможно id или имя participant + pass)
            return Results.File(result, "application/vnd.apple.pkpasses", "tickets.pkpass");
        }).AddEndpointFilter<AppleWalletEndpointFilter>();;
    }
}