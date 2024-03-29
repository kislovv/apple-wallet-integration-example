namespace BL.Entities;

public class AppleDevice
{
    public string Id { get; set; }
    public string PushToken { get; set; }
    public List<AppleWalletPass> AppleWalletPasses { get; set; }
}