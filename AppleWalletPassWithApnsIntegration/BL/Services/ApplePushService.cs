using System.Net;
using BL.Abstractions;
using BL.Configurations;
using BL.Dtos;
using BL.Exceptions;
using dotAPNS;
using Microsoft.Extensions.Options;

namespace BL.Services;

public class ApplePushService(IOptionsMonitor<AppleWalletPassConfig> appleWalletPassConfigOptions)
    : IPushService<UpdateAppleWalletPassMessageDto>
{
    private readonly AppleWalletPassConfig _appleWalletPassConfig = appleWalletPassConfigOptions.CurrentValue;

    public async Task PushMessage(UpdateAppleWalletPassMessageDto passMessageDto)
    {
        using var httpClient = new HttpClient();
        
        httpClient.DefaultRequestVersion = HttpVersion.Version20;
        httpClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;

        var apn = ApnsClient.CreateUsingJwt(httpClient, new ApnsJwtOptions
        {
            BundleId = _appleWalletPassConfig.ApplicationId,
            CertContent = _appleWalletPassConfig.PushNotificationP8PrivateKey,
            KeyId = _appleWalletPassConfig.PushNotificationP8PrivateKeyId,
            TeamId = _appleWalletPassConfig.TeamIdentifier
        });

        var applePush = new ApplePush(ApplePushType.Background)
            .AddCustomProperty("balance", passMessageDto.NewBalance)
            .AddToken(passMessageDto.PushToken);
        
        if (_appleWalletPassConfig.IsDevelopment)
        {
            applePush.SendToDevelopmentServer();
        }
        
        var apnsResponse = await apn.SendAsync(applePush);
        
        if (!apnsResponse.IsSuccessful)
        {
            throw new BusinessException($"Cant send push notification by apple APNs with new data. ErrorInfo: {apnsResponse.Reason} ");
        }
    }
}