namespace BL.Dtos;

public class LastUpdatedPassesDto
{
    public List<string> SerialNumbers { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
}