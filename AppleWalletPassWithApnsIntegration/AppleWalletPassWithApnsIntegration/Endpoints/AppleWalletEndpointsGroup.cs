﻿using AppleWalletPassWithApnsIntegration.Filters;
using AppleWalletPassWithApnsIntegration.Models;
using AutoMapper;
using BL.Abstractions;
using BL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AppleWalletPassWithApnsIntegration.Endpoints;

internal static class AppleWalletEndpointsGroup
{
    public static void RegisterAppleWalletEndpoints(this IEndpointRouteBuilder routes)
    {
        var apple = routes.MapGroup("/apple");

        var appleWallet = apple.MapGroup("wallet").AddEndpointFilter<AppleWalletEndpointFilter>();
        
        apple.MapPost("/passes/create",[Authorize(Roles = Roles.User)] async (
                [FromServices]IPassService passService,
                [FromServices] IMapper mapper, 
                [FromBody] PassRequest passRequest) =>
        {
            var result = await passService.CreatePass(mapper.Map<PassDto>(passRequest));
    
            //TODO: Подумать над названием файла (возможно id или имя participant + pass)
            return Results.File(result, "application/vnd.apple.pkpasses", "tickets.pkpass");
            
        }).WithTags("Create apple pass")
        .Produces<FileContentHttpResult>(StatusCodes.Status201Created, "application/vnd.apple.pkpasses")
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
        });


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
        });
        
        appleWallet.MapGet("/v1/devices/{deviceId}/registrations/{passTypeId}", 
            async (string deviceId, string passTypeId, [FromQuery] DateTimeOffset passesUpdatedSince, 
                [FromServices] IPassService passService) =>
        {
            var lastUpdated = await passService.GetLastUpdatedPasses(deviceId, passesUpdatedSince);
            
        return lastUpdated == null
            ? Results.NoContent()
            : Results.Ok(new ListLastUpdatedPassesResponse
            {
                SerialNumbers = lastUpdated.SerialNumbers,
                LastUpdated = lastUpdated.LastUpdated.UtcTicks
            });
        });
        
        
        appleWallet.MapGet("/v1/passes/{passTypeIdentifier}/{serialNumber}",
            async (string passTypeIdentifier, string serialNumber, 
                [FromServices] IPassService passService) =>
        {
            var result = await passService.GetUpdatedPass(serialNumber);

            //TODO: Подумать над названием файла (возможно id или имя participant + pass)
            return Results.File(result, "application/vnd.apple.pkpasses", "tickets.pkpass");
        });

        appleWallet.WithTags("Apple Wallet");
    }
}