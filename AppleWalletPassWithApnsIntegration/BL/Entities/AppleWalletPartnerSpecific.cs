namespace BL.Entities;

public class AppleWalletPartnerSpecific
{
    public long Id { get; set; }
    public string BackgroundColor { get; set; }
    public string Description { get; set; }
    public string IconPath { get; set; }
    public string LogoPath { get; set; }
    public string StripPath { get; set; }
    public List<AppleAssociatedStoreApp> AppleAssociatedStoreApps { get; set; }

    public long PartnerId { get; set; }
    public Partner Partner { get; set; }
}