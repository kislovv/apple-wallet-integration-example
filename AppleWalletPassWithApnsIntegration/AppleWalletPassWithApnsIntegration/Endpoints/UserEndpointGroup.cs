using AppleWalletPassWithApnsIntegration.Models;
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
            [FromServices] IUserService userService) =>
        {
            //TODO: AddMap
            var createUser = new User
            {
                Login = request.Login,
                Password = PasswordHasher.Hash(request.Password),
                Role = "user"
            };

            var user = await userService.AddUser(createUser);

            return Results.Ok();

        }).AllowAnonymous().WithOpenApi(operation =>
        {
            operation.Description = "Регистрация пользователя";
            return operation;
        }).Produces(StatusCodes.Status200OK);
        
        userGroup.MapPost("signin", async (LoginRequest request, 
            [FromServices] IUserService userService,[FromServices] IJwtUtils jwtUtils) =>
        {
            //TODO: AddMap
            try
            {
                var user = await userService.LoginUser(new User
                {
                    Login = request.Login,
                    Password = request.Password
                });
                
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