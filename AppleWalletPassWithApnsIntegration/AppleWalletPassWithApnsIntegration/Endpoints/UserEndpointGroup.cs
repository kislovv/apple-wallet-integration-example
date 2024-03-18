﻿using AppleWalletPassWithApnsIntegration.Models;
using AutoMapper;
using BL.Abstractions;
using BL.Entities;
using BL.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppleWalletPassWithApnsIntegration.Endpoints;

public static class UserEndpointGroup
{
    public static void RegisterUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var userGroup = routes.MapGroup("user").WithTags("User methods");

        userGroup.MapPost("signup", async (UserRegistrationRequest request, 
            [FromServices] IUserService userService, [FromServices] IMapper mapper) =>
        {
            
            _ = await userService.AddUser(mapper.Map<User>(request));

            return Results.Ok();

        }).AllowAnonymous().WithOpenApi(operation =>
        {
            operation.Description = "Регистрация пользователя";
            return operation;
        }).Produces(StatusCodes.Status200OK);
        
        userGroup.MapPost("signin", async (LoginRequest request, 
            [FromServices] IUserService userService,
                [FromServices] IJwtUtils jwtUtils, [FromServices] IMapper mapper) =>
        {
            try
            {
                var user = await userService.LoginUser(mapper.Map<User>(request));
                
                return Results.Ok(new LoginResponse
                {
                    Login = user.Login,
                    Token = jwtUtils.GenerateToken(user)
                });
            }
            //TODO: Add logging
            catch(Exception ex)
            {
                return Results.Unauthorized();
            }
        })
        .AllowAnonymous()
        .WithOpenApi(operation =>
        {
            operation.Description = "Аутентификация пользователя";
            return operation;
        })
        .Produces(StatusCodes.Status200OK);
    }
}