namespace BL.Entities;

public class Pass
{
    public string PassId { get; set; }
    public List<Device> Devices { get; set; }

    public long CardId { get; set; }
    public Card Card { get; set; }
}