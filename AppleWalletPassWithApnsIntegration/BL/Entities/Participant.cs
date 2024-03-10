namespace BL.Entities;

public class Participant : User
{
    public string Name { get; set; }
    public decimal Balance { get; set; }
    
    public long CardId { get; set; }
    public Card? Card { get; set; }
}