namespace BL.Entities;

public class Partner
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<Card> Cards { get; set; }

    public long PartnerSpecificId { get; set; }
    public PartnerSpecific PartnerSpecific { get; set; }
}