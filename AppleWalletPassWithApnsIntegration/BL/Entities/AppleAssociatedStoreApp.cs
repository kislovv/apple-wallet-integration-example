namespace BL.Entities;

public class AppleAssociatedStoreApp
{
    public long Id { get; set; }
    public string Name { get; set; }

    public long AppleWalletPartnerSpecificId { get; set; }
    public AppleWalletPartnerSpecific AppleWalletPartnerSpecific { get; set; }
}