using AppleWalletPassWithApnsIntegration.Models;
using BL.Abstractions;
using BL.Configurations;
using BL.Dtos;
using BL.Services;
using DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);

builder.Services.AddDbContext(builder.Configuration);
builder.Services.Configure<AppleWalletPassConfig>(builder.Configuration.GetSection("appleWalletConfigurations"));
builder.Services.AddScoped<IPassService, AppleWalletPassService>();

var app = builder.Build();

app.MapPost("/pass/create", async (IPassService passService, PassRequest passRequest) =>
{
//TODO: Добавить automapper    
var result = await passService.CreatePass(new PassDto
{
    UserHashId = passRequest.UserHashId
});

//TODO: Подумать над названием файла (возможно id или имя participant + pass)
return Results.File(result, "application/vnd.apple.pkpasses", "tickets.pkpass");
});

app.Run();