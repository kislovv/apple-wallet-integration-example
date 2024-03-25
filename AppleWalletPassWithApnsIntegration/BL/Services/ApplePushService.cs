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
        var applePush = new ApplePush(ApplePushType.Background)
            .AddCustomProperty("balance", passMessageDto.NewBalance)
            .AddToken(passMessageDto.PushToken);
        
        if (_appleWalletPassConfig.IsDevelopment)
        {
            applePush.SendToDevelopmentServer();
        }
        
        var apnsResponse = await apnsService.SendPush(applePush, new ApnsJwtOptions
        {
            BundleId = _appleWalletPassConfig.ApplicationId,
            CertContent = _appleWalletPassConfig.PushNotificationP8PrivateKey,
            KeyId = _appleWalletPassConfig.PushNotificationP8PrivateKeyId,
            TeamId = _appleWalletPassConfig.TeamIdentifier
        });
        
        if (!apnsResponse.IsSuccessful)
        {
            throw new BusinessException($"Cant send push notification by apple APNs with new data. ErrorInfo: {apnsResponse.Reason} ");
        }
    }
}