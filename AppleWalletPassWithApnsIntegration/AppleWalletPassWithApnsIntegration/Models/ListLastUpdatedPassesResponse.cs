namespace AppleWalletPassWithApnsIntegration.Models;

public class ListLastUpdatedPassesResponse
{
    public List<string> SerialNumbers { get; set; }
    public string LastUpdated { get; set; }
}