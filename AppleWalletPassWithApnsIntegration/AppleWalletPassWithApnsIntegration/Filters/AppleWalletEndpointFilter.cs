using AppleWalletPassWithApnsIntegration.Models;
using BL.Configurations;
using Microsoft.Extensions.Options;

namespace AppleWalletPassWithApnsIntegration.Filters;

public class AppleWalletEndpointFilter(IOptionsMonitor<AppleWalletPassConfig> optionsMonitor) : IEndpointFilter
{
    private readonly AppleWalletPassConfig _appleWalletPassConfig = optionsMonitor.CurrentValue;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validationHeader = context.HttpContext.Request.Headers.Authorization.ToString();

        if (validationHeader != $"ApplePass {_appleWalletPassConfig.InstanceApiKey}")
        {
            return Results.Unauthorized();
        }
        
        return await next(context);
    }
}