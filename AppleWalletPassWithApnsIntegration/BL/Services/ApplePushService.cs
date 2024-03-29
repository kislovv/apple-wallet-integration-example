using BL.Abstractions;
using BL.Configurations;
using BL.Dtos;
using BL.Exceptions;
using dotAPNS;
using dotAPNS.AspNetCore;
using Microsoft.Extensions.Options;

namespace BL.Services;

public class ApplePushService(IOptionsMonitor<AppleWalletPassConfig> appleWalletPassConfigOptions, IApnsService apnsService)
    : IPushService<UpdateAppleWalletPassMessageDto>
{
    private readonly AppleWalletPassConfig _appleWalletPassConfig = appleWalletPassConfigOptions.CurrentValue;

    public async Task PushMessage(UpdateAppleWalletPassMessageDto passMessageDto)
    {
        var pushes = passMessageDto.DevicesPushToken.Select(deviceToken =>
        {
            var push = new ApplePush(ApplePushType.Background)
                .AddCustomProperty("balance", passMessageDto.NewBalance)
                .AddToken(deviceToken);
            if (_appleWalletPassConfig.IsDevelopment)
            {
                push.SendToDevelopmentServer();
            }
            return push;
            
        }).ToArray();
        
        var apnsResponses = await apnsService.SendPushes(pushes, new ApnsJwtOptions
        {
            BundleId = _appleWalletPassConfig.PassTypeIdentifier,
            CertContent = _appleWalletPassConfig.PushNotificationP8PrivateKey,
            KeyId = _appleWalletPassConfig.PushNotificationP8PrivateKeyId,
            TeamId = _appleWalletPassConfig.TeamIdentifier
        });
        
        if (!apnsResponses.Any(ar => ar.IsSuccessful))
        {
            throw new BusinessException($"Cant send push notification by apple APNs with new data. ErrorInfo: {string.Join("\n\r", apnsResponses.Where(ar => !ar.IsSuccessful).Select(ar => ar.Reason))}");
        }
    }
}