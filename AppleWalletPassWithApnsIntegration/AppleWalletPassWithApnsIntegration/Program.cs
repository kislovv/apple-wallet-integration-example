using System.Reflection;
using System.Text;
using AppleWalletPassWithApnsIntegration.Configurations;
using AppleWalletPassWithApnsIntegration.Endpoints;
using BL.Abstractions;
using BL.Configurations;
using BL.Dtos;
using BL.Services;
using DataAccess;
using dotAPNS.AspNetCore;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);

/*
builder.WebHost.ConfigureKestrel(options =>
{   
    options.ConfigureEndpointDefaults(lo => lo.Protocols = HttpProtocols.Http2);
});
*/

builder.Logging.AddSeq();
builder.Services.AddHttpLogging(o =>
{
    o.LoggingFields = HttpLoggingFields.All;
});

builder.Services.AddDbContext(builder.Configuration);

builder.Services.Configure<AppleWalletPassConfig>(builder.Configuration.GetSection("appleWalletConfigurations"));
builder.Services.Configure<FileProviderConfig>(builder.Configuration.GetSection("azureBlobStorage"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtAuth"));
builder.Services.AddApns();

builder.Services.AddScoped<IPassService, AppleWalletPassService>();
builder.Services.AddScoped<IFileProvider, AzureBlobFileProvider>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped(typeof(IPushService<UpdateAppleWalletPassMessageDto>), typeof(ApplePushService));


builder.Services.AddAutoMapper(expression =>
{
    expression.AddProfile<ApiRequestMapperProfile>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = """
                      JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'
                      """,
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityDefinition("ApplePass", new OpenApiSecurityScheme
    {
        Description = """
                      ApplePass Authorization header using the Token scheme.
                      Enter 'ApplePass' [space] and then your token in the text input below.
                      Example: 'ApplePass 12345abcdef'
                      """,
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApplePass"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApplePass"
                },
                Scheme = "oauth2",
                Name = "ApplePass",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
} );

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>    
    {    
        options.TokenValidationParameters = new TokenValidationParameters    
        {    
            ValidateIssuer = false,    
            ValidateAudience = false,    
            ValidateLifetime = true,    
            ValidateIssuerSigningKey = true,    
            ValidIssuer = builder.Configuration["JwtAuth:Issuer"],    
            ValidAudience = builder.Configuration["JwtAuth:Issuer"],    
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtAuth:Key"]!))    
        };    
    });
builder.Services.AddAuthorization();

//app
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpLogging();

app.RegisterAppleWalletEndpoints();
app.RegisterUserEndpoints();
app.RegisterCardsEndpoints();

app.Run();