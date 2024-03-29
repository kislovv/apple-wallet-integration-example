namespace BL.Entities;

public class AppleWalletPass
{
    public string PassId { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
    public List<AppleDevice> AppleDevices { get; set; }
    public long CardId { get; set; }
    public Card Card { get; set; }
}