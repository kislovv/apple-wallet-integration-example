namespace BL.Configurations;

public class AppleWalletPassConfig
{
    public string WWDRCertificateBase64 { get; set; }
    public string PassTypeIdentifier { get; set; }
    public string PassbookCertificateBase64 { get; set; }
    public string PassbookPassword { get; set; }
    public string TeamIdentifier { get; set; }
    public string OrganizationName { get; set; }
    public string WebServiceUrl { get; set; }
    public string InstanceApiKey { get; set; }
    public bool IsDevelopment { get; set; }
    public string PushNotificationP8PrivateKeyId { get; set; } 
    public string PushNotificationP8PrivateKey { get; set; }
    public string ApplicationId { get; set; }
    
}