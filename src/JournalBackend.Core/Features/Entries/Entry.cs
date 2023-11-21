namespace JournalBackend;

public class Entry
{
    public required string Id { get; set; }
    public DateTime Date { get; set; }
    public required string UserId { get; set; }
    public string? Content { get; set; }

}