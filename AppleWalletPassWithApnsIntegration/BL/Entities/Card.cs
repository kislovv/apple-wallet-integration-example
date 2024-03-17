namespace BL.Entities;

public class Card
{
    public long Id { get; set; }
    
    public string UserHashId { get; set; }
    
    public long ParticipantId { get; set; }
    public Participant Participant { get; set; }

    public long PartnerId { get; set; }
    public Partner Partner { get; set; }

    public string PassId { get; set; }
    public AppleWalletPass AppleWalletPass { get; set; }
}