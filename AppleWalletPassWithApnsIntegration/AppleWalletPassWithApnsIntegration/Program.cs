using System.Reflection;
using AppleWalletPassWithApnsIntegration.Configurations;
using AppleWalletPassWithApnsIntegration.Endpoints;
using BL.Abstractions;
using BL.Configurations;
using BL.Services;
using DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);

builder.Services.AddDbContext(builder.Configuration);

builder.Services.Configure<AppleWalletPassConfig>(builder.Configuration.GetSection("appleWalletConfigurations"));
builder.Services.Configure<FileProviderConfig>(builder.Configuration.GetSection("azureBlobStorage"));

builder.Services.AddScoped<IPassService, AppleWalletPassService>();
builder.Services.AddScoped<IFileProvider, AzureBlobFileProvider>();

builder.Services.AddAutoMapper(expression =>
{
    expression.AddProfile<ApiRequestMapperProfile>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
} );

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

//app
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.RegisterAppleWalletEndpoints();
app.Run();