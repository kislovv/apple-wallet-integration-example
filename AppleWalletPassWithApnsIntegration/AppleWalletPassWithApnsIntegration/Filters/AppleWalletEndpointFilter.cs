using BL.Configurations;
using Microsoft.Extensions.Options;

namespace AppleWalletPassWithApnsIntegration.Filters;

/// <summary>
/// Фильтр для проверки авторизации от серверов apple
/// </summary>
/// <param name="optionsMonitor"></param>
public class AppleWalletEndpointFilter(IOptionsMonitor<AppleWalletPassConfig> optionsMonitor) : IEndpointFilter
{
    private readonly AppleWalletPassConfig _appleWalletPassConfig = optionsMonitor.CurrentValue;

    /// <inheritdoc />
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