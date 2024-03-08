namespace BL.Entities;

public class AssociatedStoreApp
{
    public long Id { get; set; }
    public string Name { get; set; }

    public long PartnerSpecificId { get; set; }
    public PartnerSpecific PartnerSpecific { get; set; }
}